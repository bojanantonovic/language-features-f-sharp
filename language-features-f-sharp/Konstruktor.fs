module language_features_f_sharp.Konstruktor

// Klasse mit primaerem Konstruktor direkt im Typkopf: Eigenschaften werden bei der Erzeugung gesetzt.
type Buch(titel: string, seitenzahl: int) =
    member this.Titel = titel
    member this.Seitenzahl = seitenzahl

    // Ueberladener (sekundaerer) Konstruktor: ruft ueber "new(...) = Buch(...)" den primaeren
    // Konstruktor auf und setzt dabei einen Standardwert fuer die Seitenzahl.
    new(titel: string) = Buch(titel, 0)

let zeigen () =
    let roman = Buch("Der Steppenwolf", 320)
    let unbekannt = Buch("Unbekanntes Buch")

    let romanTitel = roman.Titel
    let romanSeitenzahl = roman.Seitenzahl

    printfn "%s" romanTitel
    printfn "%d" romanSeitenzahl
    printfn "%s" unbekannt.Titel
    printfn "%d" unbekannt.Seitenzahl
