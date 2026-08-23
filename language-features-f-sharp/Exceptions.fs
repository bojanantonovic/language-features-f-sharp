module Language_Features_F_Sharp.Exceptions

// Eigener Exception-Typ: "exception" definiert einen F#-eigenen Ausnahmetyp
// (Aequivalent zu einer von Exception abgeleiteten Klasse in C#) inklusive einer Nutzlast (string).
exception UngueltigesAlterException of string

// wirft die eigene Exception, wenn der uebergebene Wert fachlich ungueltig ist
let pruefeAlter alter =
    if alter < 0 then
        raise (UngueltigesAlterException $"Alter darf nicht negativ sein: {alter}")

let sicherDividieren zaehler nenner =
    try
        try
            let ergebnis = zaehler / nenner
            ergebnis
        with :? System.DivideByZeroException ->
            // "with" faengt die Exception ab, statt das Programm abstuerzen zu lassen
            printfn "Fehler: Division durch 0 ist nicht erlaubt."
            0
    finally
        // finally laeuft immer, egal ob eine Exception aufgetreten ist oder nicht
        printfn "sicherDividieren wurde aufgerufen."

let zeigen () =
    let ergebnisOk = sicherDividieren 10 2
    let ergebnisFehler = sicherDividieren 10 0

    printfn "%d" ergebnisOk
    printfn "%d" ergebnisFehler

    try
        pruefeAlter -5
    with UngueltigesAlterException nachricht ->
        // faengt gezielt nur unsere eigene Exception ab, nicht jede beliebige Exception
        printfn "%s" nachricht
