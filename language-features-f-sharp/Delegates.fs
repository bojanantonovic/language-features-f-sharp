module language_features_f_sharp.Delegates

// Funktionswert: In F# sind Funktionen selbst schon "First-Class-Werte" - ein eigener Delegate-Typ
// wie in C# (RechenOperation) ist normalerweise unnoetig, eine einfache Signatur genuegt.
type RechenOperation = int -> int -> int

// Klasse mit einem Event: benachrichtigt andere Codeteile, wenn sich etwas aendert
type Konto() =
    let mutable guthaben = 0m
    let guthabenGeaendert = Event<decimal>()

    member this.Guthaben = guthaben

    // Event: andere Codeteile abonnieren ueber "Publish"
    [<CLIEvent>]
    member this.GuthabenGeaendert = guthabenGeaendert.Publish

    member this.Einzahlen(betrag: decimal) =
        guthaben <- guthaben + betrag
        guthabenGeaendert.Trigger(guthaben) // loest das Event aus, falls jemand zuhoert

let private addieren a b = a + b

let zeigen () =
    // Funktionswert: einer Funktion zuweisen und wie eine Variable aufrufen
    let addierenFn: RechenOperation = addieren
    let summe = addierenFn 3 4

    // Funktionswert: einem Lambda-Ausdruck zuweisen
    let multiplizieren: RechenOperation = fun a b -> a * b
    let produkt = multiplizieren 3 4

    printfn "%d" summe
    printfn "%d" produkt

    // Liste von Funktionswerten: List.map wendet jeden einzelnen auf dieselben Argumente an
    let operationen: RechenOperation list = [ addieren; (fun a b -> a * b); (fun a b -> a - b) ]

    let ergebnisse = operationen |> List.map (fun operation -> operation 10 3)

    printfn "%s" (String.concat ", " (ergebnisse |> List.map string))

    // Event abonnieren: das Lambda wird aufgerufen, sobald GuthabenGeaendert ausgeloest wird
    let konto = Konto()
    konto.GuthabenGeaendert.Add(fun neuesGuthaben -> printfn "%M" neuesGuthaben)

    konto.Einzahlen(100m)
    konto.Einzahlen(50m)
