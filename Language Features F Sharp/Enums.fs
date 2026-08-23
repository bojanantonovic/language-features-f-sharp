module Language_Features_F_Sharp.Enums

// Enum: benannte Menge fester int-Werte. F#-Enums sind - anders als Discriminated Unions - immer
// an einen Basis-Zahlentyp gebunden (siehe DiskriminierteVereinigungen.fs fuer die F#-eigene Alternative).
type Wochentag =
    | Montag = 0
    | Dienstag = 1
    | Mittwoch = 2
    | Donnerstag = 3
    | Freitag = 4
    | Samstag = 5
    | Sonntag = 6

let zeigen () =
    let heute = Wochentag.Mittwoch

    // match auf einem Enum-Wert: kompakter als ein switch-Statement, liefert direkt einen Wert
    let art =
        match heute with
        | Wochentag.Samstag | Wochentag.Sonntag -> "Wochenende"
        | _ -> "Werktag"

    printfn "%A" heute
    printfn "%s" art

    // System.Enum.GetValues liefert alle Werte des Enums, Seq-/List-Funktionen wirken genauso darauf
    let alleTage = System.Enum.GetValues<Wochentag>() |> List.ofArray

    let wochenendTage =
        alleTage |> List.filter (fun tag -> tag = Wochentag.Samstag || tag = Wochentag.Sonntag)

    let anzahlWerktage =
        alleTage
        |> List.filter (fun tag -> tag <> Wochentag.Samstag && tag <> Wochentag.Sonntag)
        |> List.length

    printfn "%s" (String.concat ", " (wochenendTage |> List.map string))
    printfn "%d" anzahlWerktage
