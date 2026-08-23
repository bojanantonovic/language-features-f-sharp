module language_features_f_sharp.Schleifen

let zeigen () =
    // for-Schleife ueber einen Bereich (Range): Summe der Zahlen 1 bis 5.
    // Die Zaehlvariable "i" ist unveraenderlich, "summe" muss daher explizit mutable sein.
    let mutable summe = 0
    for i in 1..5 do
        summe <- summe + i

    // while-Schleife: verdoppeln, bis ein Grenzwert erreicht ist
    let mutable wert = 1
    while wert < 100 do
        wert <- wert * 2

    // for..in-Schleife: ueber ein Array von Zahlen iterieren
    let zahlen = [| 3; 7; 2; 9; 4 |]
    let mutable groessteZahl = zahlen.[0]
    for zahl in zahlen do
        if zahl > groessteZahl then
            groessteZahl <- zahl

    printfn "%d" summe
    printfn "%d" wert
    printfn "%d" groessteZahl
