module language_features_f_sharp.Asynchronitaet

// async-Workflow: F#s natives Modell fuer asynchrone Berechnungen (aelter als C#s async/await und
// eines der Vorbilder dafuer) - "let!"/"do!" warten auf ein Ergebnis, ohne den Thread zu blockieren.
let private berechneAsync a b =
    async {
        do! Async.Sleep 100 // simuliert eine dauernde Operation, z. B. einen Netzwerkaufruf
        return a * b
    }

let zeigenAsync () =
    async {
        let! ergebnis = berechneAsync 6 7
        printfn "%d" ergebnis
    }
