namespace Language_Features_F_Sharp.Features

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks

module F12_Async =

    let private berechneAsync (a: int) (b: int) =
        task {
            do! Task.Delay 1
            return a * b
        }

    let private liesLangsamAsync () =
        task {
            do! Task.Delay 50
            return "aus Datenquelle geladen"
        }

    // ValueTask<T>: vermeidet Heap-Allokation, wenn oft synchron abgeschlossen wird - identischer
    // BCL-Typ, unveraendert aus F# nutzbar.
    let private liesAusCacheAsync (vorhanden: bool) : ValueTask<string> =
        if vorhanden then
            ValueTask<string> "Treffer im Cache"
        else
            ValueTask<string>(liesLangsamAsync ())

    // IAsyncEnumerable<T>: F# hat kein "async yield return" im Kern der Sprache (das TaskSeq-CE
    // dafuer liegt in einer separaten NuGet-Bibliothek). Das naechstliegende Bordmittel ist eine
    // manuelle Implementierung von IAsyncEnumerable<T>/IAsyncEnumerator<T> per Objektausdruck.
    let private zaehleAsync (bis: int) : IAsyncEnumerable<int> =
        { new IAsyncEnumerable<int> with
            member _.GetAsyncEnumerator(_cancellationToken) =
                let mutable aktuell = 0
                { new IAsyncEnumerator<int> with
                    member _.Current = aktuell
                    member _.MoveNextAsync() =
                        ValueTask<bool>(
                            task {
                                if aktuell >= bis then
                                    return false
                                else
                                    do! Task.Delay 1
                                    aktuell <- aktuell + 1
                                    return true
                            })
                    member _.DisposeAsync() = ValueTask.CompletedTask } }

    let private langeOperationAsync (token: CancellationToken) =
        task {
            for _ in 0 .. 99 do
                token.ThrowIfCancellationRequested()
                do! Task.Delay(5, token)
        }

    let runAsync () : Task =
        task {
            // async/await mit Task<T>: F#s "task {}" liest sich fast wortgleich zu C#s async/await,
            // "do!"/"let!" entsprechen "await".
            let! ergebnis = berechneAsync 6 7
            Demo.print "task{} let! (Aequivalent zu async/await Task<T>)" ergebnis

            // Task.Run: Auslagern von CPU-gebundener Arbeit auf den Thread-Pool.
            let! ausgelagert = Task.Run(fun () -> Seq.init 1000 (fun i -> i + 1) |> Seq.sum)
            Demo.print "Task.Run" ausgelagert

            // Task.WhenAll: mehrere Tasks parallel erwarten.
            let! alleErgebnisse = Task.WhenAll(berechneAsync 1 1, berechneAsync 2 2, berechneAsync 3 3)
            Demo.print "Task.WhenAll" (String.Join(",", alleErgebnisse))

            // Task.WhenAny: auf den ersten fertigen Task reagieren.
            let schnellerTask = Task.Delay(10).ContinueWith(fun _ -> "schnell")
            let langsamerTask = Task.Delay(200).ContinueWith(fun _ -> "langsam")
            let! ersterFertig = Task.WhenAny(schnellerTask, langsamerTask)
            let! ersterFertigErgebnis = ersterFertig
            Demo.print "Task.WhenAny" ersterFertigErgebnis

            // Async lokale Funktion: eine verschachtelte "let"-Funktion, die selbst "task {}" nutzt.
            let verdoppleAsync (wert: int) =
                task {
                    do! Task.Delay 1
                    return wert * 2
                }
            let! verdoppelt = verdoppleAsync 21
            Demo.print "async lokale Funktion" verdoppelt

            let! ausCache = liesAusCacheAsync true
            Demo.print "ValueTask<T>" ausCache

            // IAsyncEnumerable<T>: F#s task{}-Builder unterstuetzt (anders als bei einer normalen
            // seq<'T>) kein direktes "for...in...do" ueber IAsyncEnumerable<T> - das manuelle
            // MoveNextAsync/Current/DisposeAsync-Muster per "let!" ist der Ersatz fuer "await foreach".
            let gesammelt = ResizeArray<int>()
            let enumerator = (zaehleAsync 3).GetAsyncEnumerator()
            let mutable weiterMachen = true
            while weiterMachen do
                let! hatWeiter = enumerator.MoveNextAsync()
                if hatWeiter then
                    gesammelt.Add enumerator.Current
                else
                    weiterMachen <- false
            do! enumerator.DisposeAsync()
            Demo.print "manuelles MoveNextAsync (statt await foreach)" (String.Join(",", gesammelt))

            // CancellationToken zum kooperativen Abbrechen - identischer BCL-Typ wie in C#.
            use cts = new CancellationTokenSource()
            cts.CancelAfter(TimeSpan.FromMilliseconds 20.0)
            try
                do! langeOperationAsync cts.Token
            with :? OperationCanceledException ->
                Demo.print "CancellationToken" "Operation abgebrochen"

            // F#-idiomatischer Bonus: das native "async {}"-Workflow ist F#s eigenes, vom Task-Modell
            // unabhaengiges Nebenlaeufigkeitsmodell. Async.AwaitTask/Async.StartAsTask bilden die
            // Bruecke zwischen beiden Welten.
            let asyncWorkflow =
                async {
                    let! wert = berechneAsync 3 3 |> Async.AwaitTask
                    return wert + 1
                }
            let! ausAsyncWorkflow = asyncWorkflow |> Async.StartAsTask
            Demo.print "F#-natives async{} (Bruecke via Async.AwaitTask)" ausAsyncWorkflow
        }
