module Language_Features_F_Sharp.Strings

let zeigen () =
    let name = "Alice"
    let alter = 30
    let preis = 19.999

    // String-Interpolation: Werte direkt im String einsetzen, mit $ vor den Anfuehrungszeichen
    let begruessung = $"Hallo, {name}!"

    // Auch Berechnungen sind innerhalb von {} moeglich
    let altersinfo = $"{name} ist {alter} Jahre alt, in 10 Jahren {alter + 10}."

    // Formatierung innerhalb von {}: %.2f rundet auf 2 Nachkommastellen (printf-Formatangabe statt .NET-Formatstring)
    let preisText = $"Preis: %.2f{preis}"

    printfn "%s" begruessung
    printfn "%s" altersinfo
    printfn "%s" preisText

    // Raw/Verbatim-String (@"..."): Escape-Zeichen wie \ werden nicht interpretiert, nuetzlich fuer Pfade
    let pfad = @"C:\Daten\Alice\Notizen.txt"

    // Split: zerlegt einen String an einem Trennzeichen in mehrere Teile
    let csv = "Apfel, Birne , Kirsche"
    let teile = csv.Split(',')

    // Trim entfernt Leerzeichen am Anfang/Ende jedes Teils
    let getrimmteTeile = teile |> Array.map (fun teil -> teil.Trim())

    // String.concat: fuegt mehrere Teile wieder zu einem String zusammen
    let zusammengefuegt = String.concat " | " getrimmteTeile

    // Pruefungen auf Teilstrings
    let enthaeltBirne = zusammengefuegt.Contains("Birne")
    let startetMitApfel = zusammengefuegt.StartsWith("Apfel")

    printfn "%s" pfad
    printfn "%s" zusammengefuegt
    printfn "%b" enthaeltBirne
    printfn "%b" startetMitApfel
