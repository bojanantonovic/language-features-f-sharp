module language_features_f_sharp.Tupel

// Tuple als Rueckgabewert einer Funktion: mehrere Werte zusammen zurueckgeben,
// ohne dafuer einen eigenen Record zu brauchen.
let minMax (zahlen: int list) = (List.min zahlen, List.max zahlen)

let zeigen () =
    // Tuple: mehrere Werte in einem einzigen Wert buendeln
    let person = ("Alice", 30)

    printfn "%s" (fst person)
    printfn "%d" (snd person)

    // Deconstruction: die Elemente eines Tuples direkt beim Binden in eigene Namen entpacken
    let vorname, jahre = person

    printfn "%s" vorname
    printfn "%d" jahre

    // Tuple als Rueckgabewert einer Funktion, direkt entpackt
    let minimum, maximum = minMax [ 5; 12; 3; 8; 21; 4 ]

    printfn "%d" minimum
    printfn "%d" maximum

    // Liste von Tuples: List-Funktionen funktionieren genauso wie bei jedem anderen Typ
    let personen = [ "Alice", 30; "Bob", 25; "Carol", 40 ]

    let namenUeber28 =
        personen
        |> List.filter (fun (_, alter) -> alter > 28)
        |> List.map fst

    printfn "%s" (String.concat ", " namenUeber28)
