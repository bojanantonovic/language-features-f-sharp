module Language_Features_F_Sharp.Parameter

open Language_Features_F_Sharp.Operatoren

// byref<...>/outref<...>: F#s Aequivalent zu C#s "ref"/"out" - explizite Referenzparameter, auch auf
// einer gewoehnlichen "let"-Funktion moeglich (anders als optionale Parameter weiter unten).
let tryDividieren zaehler nenner (ergebnis: outref<int>) =
    if nenner = 0 then
        ergebnis <- 0
        false
    else
        ergebnis <- zaehler / nenner
        true

let verdoppeln (wert: byref<int>) = wert <- wert * 2

// inref<...>: die Funktion bekommt den Wert nur zum Lesen, eine Kopie wird dabei vermieden
let laenge (vektor: inref<Vektor>) = sqrt (float (vektor.X * vektor.X + vektor.Y * vektor.Y))

// Optionale Parameter ("?param") und ParamArray sind in F# nur auf Members erlaubt, daher stecken
// sie hier in einem kleinen Typ statt in einer gewoehnlichen "let"-Funktion.
type Berechnung =
    // Optionaler Parameter: rabatt muss beim Aufruf nicht angegeben werden, "defaultArg" liefert dann den Standardwert
    static member PreisBerechnen(preis: float, ?rabatt: float) =
        let r = defaultArg rabatt 0.0
        preis - (preis * r)

    // ParamArray: beliebig viele Argumente werden als Array entgegengenommen (Aequivalent zu C#s "params")
    static member Summiere([<System.ParamArray>] zahlen: int[]) = Array.sum zahlen

let zeigen () =
    // out: die Funktion MUSS diesem Parameter einen Wert zuweisen, gedacht fuer zusaetzliche Rueckgabewerte
    let mutable ergebnis = 0
    let erfolg = tryDividieren 10 2 &ergebnis

    printfn "%b" erfolg
    printfn "%d" ergebnis

    // ref/byref: die Funktion kann den bestehenden Wert der Variablen lesen UND aendern
    let mutable zahl = 5
    verdoppeln &zahl

    printfn "%d" zahl

    // in/inref: die Funktion bekommt den Wert nur zum Lesen
    let a = Vektor(3, 4)
    let l = laenge &a

    printfn "%f" l

    // Optionaler Parameter: rabatt muss nicht angegeben werden
    let preisOhneRabatt = Berechnung.PreisBerechnen(100.0)
    let preisMitRabatt = Berechnung.PreisBerechnen(100.0, 0.1)

    printfn "%f" preisOhneRabatt
    printfn "%f" preisMitRabatt

    // ParamArray: beliebig viele Argumente werden von der Methode als Array entgegengenommen
    let summe = Berechnung.Summiere(1, 2, 3, 4, 5)

    printfn "%d" summe
