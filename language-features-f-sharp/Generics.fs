module language_features_f_sharp.Generics

// Generische Funktion: 'T ist ein Platzhalter fuer einen beliebigen Typ. F# leitet aus der
// Verwendung von ">" automatisch einen "comparison"-Constraint fuer 'T ab (kein "where" noetig).
let groesser (a: 'T) (b: 'T) : 'T =
    if a > b then a else b

let zeigen () =
    // Dieselbe Funktion "groesser" funktioniert fuer int und string,
    // ohne dass sie fuer jeden Typ neu geschrieben werden muss.
    let groessereZahl = groesser 5 12
    let groessererName = groesser "Anna" "Bob"

    printfn "%d" groessereZahl
    printfn "%s" groessererName
