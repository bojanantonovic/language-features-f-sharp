module language_features_f_sharp.Indexer

open System.Collections.Generic

// Eigene Klasse mit Indexer: "member this.Item" mit get/set erlaubt Zugriff mit eckigen Klammern
// wie bei einem Array, obwohl intern ein Dictionary die Daten haelt.
type Wochenplan() =
    let termineNachTag = Dictionary<string, string>()

    member this.Item
        with get (tag: string) =
            match termineNachTag.TryGetValue(tag) with
            | true, termin -> termin
            | false, _ -> "frei"
        and set (tag: string) (wert: string) = termineNachTag.[tag] <- wert

let zeigen () =
    let plan = Wochenplan()

    // Zuweisung ueber den Indexer, genau wie bei einem Array
    plan.["Montag"] <- "Zahnarzt"
    plan.["Mittwoch"] <- "Meeting"

    // Lesen ueber den Indexer
    let montagTermin = plan.["Montag"]
    let dienstagTermin = plan.["Dienstag"] // kein Eintrag vorhanden -> "frei"

    printfn "%s" montagTermin
    printfn "%s" dienstagTermin

    // Liste von Tagen: List.map nutzt den Indexer, um pro Tag den Termin abzufragen
    let tage = [ "Montag"; "Dienstag"; "Mittwoch" ]

    let terminplan = tage |> List.map (fun tag -> $"{tag}: {plan.[tag]}")

    printfn "%s" (String.concat " | " terminplan)
