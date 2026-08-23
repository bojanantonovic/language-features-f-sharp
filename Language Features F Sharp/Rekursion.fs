module Language_Features_F_Sharp.Rekursion

// "rec": macht eine Funktion innerhalb ihres eigenen Koerpers aufrufbar (in F# per Default AUS,
// anders als in den meisten anderen Sprachen, in denen Funktionen sich automatisch selbst kennen).
let rec fakultaet n =
    if n <= 1 then 1
    else n * fakultaet (n - 1)

// Endrekursion (tail recursion): der rekursive Aufruf ist die LETZTE Aktion der Funktion,
// der Compiler wandelt ihn dadurch in eine Schleife um - kein Stack-Overflow, selbst bei grossen n.
// Der Akkumulator ist int64, weil die Summe bis 1 Million einen int32 ueberlaufen wuerde.
let summeBisTailrekursiv obergrenze =
    let rec schleife n akkumulator =
        if n > obergrenze then akkumulator
        else schleife (n + 1L) (akkumulator + n)
    schleife 1L 0L

// Wechselseitige Rekursion ("and"): zwei Funktionen rufen sich gegenseitig auf,
// beide muessen dafuer im selben "rec ... and ..."-Block stehen.
let rec istGerade n = if n = 0 then true else istUngerade (n - 1)
and istUngerade n = if n = 0 then false else istGerade (n - 1)

let zeigen () =
    let f5 = fakultaet 5
    printfn "%d" f5

    let summeBis1Million = summeBisTailrekursiv 1_000_000L
    printfn "%d" summeBis1Million

    printfn "%b" (istGerade 10)
    printfn "%b" (istUngerade 10)
