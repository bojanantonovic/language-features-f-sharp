namespace Language_Features_F_Sharp.Features

open System

// Eigener Delegate-Typ: F# kann - genau wie C# - einen echten .NET-Delegate-Typ deklarieren.
type RechenOperation = delegate of int * int -> int

// EventArgs-Ableitung fuer ein benutzerdefiniertes Event.
type PreisAenderungEventArgs(alterPreis: decimal, neuerPreis: decimal) =
    inherit EventArgs()
    member _.AlterPreis = alterPreis
    member _.NeuerPreis = neuerPreis

type Aktie(symbol: string) =
    let preisGeaendert = Event<EventHandler<PreisAenderungEventArgs>, PreisAenderungEventArgs>()
    let mutable preis = 0m

    member _.Symbol = symbol

    // [<CLIEvent>] macht daraus ein echtes, CLS-konformes .NET-Event (add_/remove_-Accessoren),
    // exakt wie "event EventHandler<T>" in C#.
    [<CLIEvent>]
    member _.PreisGeaendert = preisGeaendert.Publish

    member this.Preis
        with get () = preis
        and set value =
            if value <> preis then
                let alterPreis = preis
                preis <- value
                preisGeaendert.Trigger(this, PreisAenderungEventArgs(alterPreis, value))

module F07_DelegatesEventsLambdas =

    let private aktiePreisGeaendert: EventHandler<PreisAenderungEventArgs> =
        EventHandler<PreisAenderungEventArgs>(fun _sender e ->
            let differenz = e.NeuerPreis - e.AlterPreis
            printfn "    [Handler] Aenderung um %s" (differenz.ToString "+0.00;-0.00"))

    let run () =
        // Eigener Delegate-Typ, instanziiert per Lambda; Aufruf via .Invoke (oder Kurzform).
        let addieren = RechenOperation(fun a b -> a + b)
        let multiplizieren = RechenOperation(fun a b -> a * b)
        Demo.print "eigener Delegate-Typ (Lambda)" (addieren.Invoke(3, 4))
        Demo.print "eigener Delegate-Typ (Lambda, static-aehnlich)" (multiplizieren.Invoke(3, 4))

        // Multicast-Delegate: F# ueberlaedt "+=" nicht fuer Delegates (das ist reine C#-Compiler-
        // Syntax) - die Kombination erfolgt explizit ueber Delegate.Combine.
        let ziel1 = Action<string>(fun s -> printfn "    [1] %s" s)
        let ziel2 = Action<string>(fun s -> printfn "    [2] %s" s)
        let mehrfach = Delegate.Combine(ziel1, ziel2) :?> Action<string>
        Demo.print "Delegate.Combine (Aequivalent zu +=, 2 Ziele)" (mehrfach.GetInvocationList().Length)
        mehrfach.Invoke "Hallo Multicast"

        // Func, Action, Predicate aus dem Framework funktionieren unveraendert aus F#.
        let quadrat = Func<int, int>(fun x -> x * x)
        let istGerade = Predicate<int>(fun x -> x % 2 = 0)
        Demo.print "Func<int,int>" (quadrat.Invoke 5)
        Demo.print "Predicate<int>" (istGerade.Invoke 4)

        // Lokale Funktion vs. Lambda + Closures: EIN wichtiger Unterschied zu C#: F# erlaubt es
        // NICHT, eine "let mutable"-Variable in einer Closure einzufangen (Compilerfehler). Um wie
        // in C# eine spaeter geaenderte Variable in der Closure sichtbar zu machen, braucht man eine
        // Referenzzelle ("ref").
        let basiswert = ref 10
        let lokaleAddition x = x + basiswert.Value
        let lambdaAddition = fun x -> x + basiswert.Value
        basiswert.Value <- 100
        Demo.print "lokale Funktion (Closure ueber ref, spaeter gelesen)" (lokaleAddition 1)
        Demo.print "Lambda-Closure (Closure ueber ref, spaeter gelesen)" (lambdaAddition 1)

        // Events: Abonnieren via Lambda (.Add, F#-Kurzform ohne sender) und via AddHandler
        // (vollstaendige EventHandler<T>-Signatur mit sender, wie in C#).
        let aktie = Aktie "MSFT"
        aktie.PreisGeaendert.Add(fun e -> printfn "%s" $"    [Event] {aktie.Symbol}: {e.AlterPreis:C} -> {e.NeuerPreis:C}")
        aktie.PreisGeaendert.AddHandler aktiePreisGeaendert
        aktie.Preis <- 415.25m
        Demo.print "Event ausgeloest fuer" aktie.Symbol
