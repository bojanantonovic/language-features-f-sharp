module language_features_f_sharp.Nullbarkeit

let zeigen () =
    // option: F#s eigener, typsicherer Ersatz fuer "null" - ein Wert ist entweder "Some x" oder "None",
    // der Compiler zwingt dazu, beide Faelle zu behandeln (Aequivalent zu C#s Nullable Reference Types).
    let alter: int option = None

    // match statt HasValue/Value: der Compiler prueft, dass wirklich beide Faelle abgedeckt sind
    let hatWert =
        match alter with
        | Some _ -> true
        | None -> false

    printfn "%b" hatWert

    // Option.defaultValue: liefert den Fallback-Wert, wenn die Option None ist (Aequivalent zu ??)
    let alterOderStandard = alter |> Option.defaultValue 18

    printfn "%d" alterOderStandard

    // Option.map: wendet eine Funktion nur an, wenn ein Wert vorhanden ist (Aequivalent zu ?.)
    let name: string option = None
    let nameLaenge = name |> Option.map (fun n -> n.Length)

    printfn "%A" nameLaenge

    let nameGefuellt = Some "Alice"
    let nameLaenge2 = nameGefuellt |> Option.map (fun n -> n.Length)

    printfn "%A" nameLaenge2

    // Liste mit moeglichen Luecken: List.choose behaelt nur die vorhandenen Werte (kombiniert filter+map)
    let zahlenMitLuecken = [ Some 5; None; Some 12; None; Some 8 ]

    let vorhandeneZahlen = zahlenMitLuecken |> List.choose id

    printfn "%s" (String.concat ", " (vorhandeneZahlen |> List.map string))
