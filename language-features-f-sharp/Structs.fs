module language_features_f_sharp.Structs

// [<Struct>]: Werttyp - wird bei Zuweisung kopiert, im Gegensatz zu einer Klasse (Referenztyp),
// bei der zwei Bindings dieselbe Instanz teilen wuerden.
[<Struct>]
type Punkt(x: int, y: int) =
    member this.X = x
    member this.Y = y

let zeigen () =
    let punktA = Punkt(1, 2)
    let punktB = punktA // erzeugt eine Kopie, nicht dieselbe Instanz

    let punktAX = punktA.X
    let punktBX = punktB.X

    printfn "%d" punktAX
    printfn "%d" punktBX

    // Liste von structs: List.map berechnet aus jedem Punkt einen neuen Wert,
    // die urspruengliche Liste bleibt dabei unveraendert
    let punkte = [ Punkt(1, 2); Punkt(-3, 4); Punkt(5, -1) ]

    let entfernungenVomUrsprung =
        punkte |> List.map (fun p -> sqrt (float (p.X * p.X + p.Y * p.Y)))

    // List.minBy: den Punkt mit der kleinsten Entfernung zum Ursprung finden
    let naechsterPunkt =
        punkte |> List.minBy (fun p -> sqrt (float (p.X * p.X + p.Y * p.Y)))

    printfn "%s" (String.concat ", " (entfernungenVomUrsprung |> List.map string))
    printfn "%d, %d" naechsterPunkt.X naechsterPunkt.Y
