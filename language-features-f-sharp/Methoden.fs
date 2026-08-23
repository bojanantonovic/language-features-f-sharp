module language_features_f_sharp.Methoden

// Funktion mit einem Parameter und Rueckgabewert: kein "return" noetig,
// der letzte Ausdruck im Funktionskoerper ist automatisch der Rueckgabewert.
let quadrieren zahl = zahl * zahl

// Funktion mit zwei Parametern. Ist automatisch "curried" (mehr dazu in FunktionaleKomposition.fs),
// laesst sich hier aber genau wie eine gewoehnliche Zwei-Parameter-Methode aufrufen.
let addieren a b = a + b

// Funktion, die intern eine Schleife nutzt (macht die for-Schleife aus Schleifen.fs
// fuer beliebige Obergrenzen wiederverwendbar)
let summeVon1Bis grenze =
    let mutable summe = 0
    for i in 1..grenze do
        summe <- summe + i
    summe

let zeigen () =
    let quadrat = quadrieren 6
    let summeZweierZahlen = addieren 3 4
    let summeBis10 = summeVon1Bis 10

    printfn "%d" quadrat
    printfn "%d" summeZweierZahlen
    printfn "%d" summeBis10
