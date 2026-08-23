module Language_Features_F_Sharp.MusterAbgleich

open Language_Features_F_Sharp.Vererbung
open Language_Features_F_Sharp.Structs

let zeigen () =
    let note = 2

    // match-Ausdruck mit konstanten Patterns (Aequivalent zur switch-Expression)
    let bewertung =
        match note with
        | 1 -> "Sehr gut"
        | 2 -> "Gut"
        | 3 -> "Befriedigend"
        | _ -> "Unbekannt"

    printfn "%s" bewertung

    // Type-Pattern (":?"): prueft gleichzeitig den Typ und bindet ihn an einen Namen (hier: Hund hund)
    let tier: Tier = Hund("Rex")
    let beschreibung =
        match tier with
        | :? Hund as hund -> $"Hund namens {hund.Name}"
        | :? Katze as katze -> $"Katze namens {katze.Name}"
        | _ -> "Unbekanntes Tier"

    printfn "%s" beschreibung

    // Guard-Klausel ("when"): F# hat keine eigenen relationalen Patterns wie C#s "> 30",
    // dafuer aber "when" fuer beliebige Bedingungen im Pattern.
    let punkt = Punkt(1, 2)
    let lage =
        match punkt with
        | p when p.X = 0 && p.Y = 0 -> "Ursprung"
        | p when p.X > 0 && p.Y > 0 -> "Erster Quadrant"
        | _ -> "Ausserhalb des ersten Quadranten"

    printfn "%s" lage

    // Kombiniertes Type- und Guard-Pattern in einem match
    let wert: obj = 42
    match wert with
    | :? int as zahl when zahl > 10 -> printfn "%d" zahl
    | _ -> ()

    // Liste gemischter Tiere: List.filter mit Type-Pattern in der Lambda zaehlt nur die Hunde
    let tiere: Tier list = [ Hund("Rex") :> Tier; Katze("Minka") :> Tier; Hund("Bello") :> Tier ]
    let anzahlHunde = tiere |> List.filter (function :? Hund -> true | _ -> false) |> List.length

    printfn "%d" anzahlHunde
