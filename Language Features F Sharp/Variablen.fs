module Language_Features_F_Sharp.Variablen

let zeigen () =
    // Ganzzahl
    let alter = 30

    // Kommazahl: F# leitet den Typ aus dem Literal ab (hier: float)
    let koerpergroesse = 1.78

    // Text
    let name = "Alice"

    // Wahrheitswert: true oder false
    let istAktiv = true

    // Eine Berechnung: das Ergebnis landet zuerst in einem eigenen Binding,
    // nicht direkt in der printfn-Zeile.
    let alterInZehnJahren = alter + 10

    // let bindet einen Namen an einen Wert und ist per Default unveraenderlich - anders als C#s
    // "var", das nur Typinferenz bedeutet, den Wert aber weiterhin veraenderbar laesst.
    // Siehe Unveraenderlichkeit.fs fuer "mutable" und Shadowing.
    printfn "%s" name
    printfn "%d" alter
    printfn "%f" koerpergroesse
    printfn "%b" istAktiv
    printfn "%d" alterInZehnJahren
