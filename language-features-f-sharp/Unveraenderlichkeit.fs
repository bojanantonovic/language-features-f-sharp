module language_features_f_sharp.Unveraenderlichkeit

let zeigen () =
    // let bindet einen Namen unveraenderlich an einen Wert - eine erneute Zuweisung ("wert <- 2")
    // waere hier ein Kompilierfehler.
    let wert = 1

    // Shadowing: "let wert = ..." erzeugt ein NEUES, unabhaengiges Binding mit demselben Namen,
    // das ab hier das alte verdeckt - anders als eine Zuweisung veraendert es das alte nicht.
    let wert = wert + 1

    printfn "%d" wert

    // "mutable" macht ein Binding explizit veraenderlich - erkennbar sowohl an der Deklaration
    // als auch an jeder Stelle, an der "<-" verwendet wird.
    let mutable zaehler = 0
    zaehler <- zaehler + 1
    zaehler <- zaehler + 1

    printfn "%d" zaehler

    // Records/Listen sind standardmaessig unveraenderlich: "aendern" erzeugt immer eine Kopie
    // mit dem geaenderten Feld/Element, das Original bleibt bestehen (siehe Records.fs, Listen.fs).
    let liste1 = [ 1; 2; 3 ]
    let liste2 = 0 :: liste1

    printfn "%d" (List.length liste1)
    printfn "%d" (List.length liste2)
