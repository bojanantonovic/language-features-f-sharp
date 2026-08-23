module Language_Features_F_Sharp.Collections

let zeigen () =
    let zahlen = [ 5; 12; 3; 8; 21; 4 ]

    // List.filter: filtert Elemente, die eine Bedingung erfuellen
    let geradeZahlen = zahlen |> List.filter (fun zahl -> zahl % 2 = 0)

    // List.map: wandelt jedes Element um
    let verdoppelt = zahlen |> List.map (fun zahl -> zahl * 2)

    // List.sort: sortiert aufsteigend, ohne die urspruengliche Liste zu veraendern
    let sortiert = zahlen |> List.sort

    // Aggregat-Funktionen liefern direkt einen einzelnen Wert statt einer Liste
    let summe = zahlen |> List.sum
    let durchschnitt = zahlen |> List.averageBy float
    let groessteZahl = zahlen |> List.max

    printfn "%s" (String.concat ", " (geradeZahlen |> List.map string))
    printfn "%s" (String.concat ", " (verdoppelt |> List.map string))
    printfn "%s" (String.concat ", " (sortiert |> List.map string))
    printfn "%d" summe
    printfn "%f" durchschnitt
    printfn "%d" groessteZahl

    // Map: F#s unveraenderliche Alternative zu Dictionary (siehe Woerterbuch.fs), intern ein balancierter Baum
    let alterNachName = Map.ofList [ "Alice", 30; "Bob", 25 ]
    let alterNachNameErweitert = alterNachName |> Map.add "Carol" 40

    // Set: unveraenderliche Menge ohne Duplikate
    let einzigartigeZahlen = Set.ofList [ 1; 2; 2; 3; 3; 3 ]

    printfn "%d" alterNachNameErweitert.["Carol"]
    printfn "%d" einzigartigeZahlen.Count
