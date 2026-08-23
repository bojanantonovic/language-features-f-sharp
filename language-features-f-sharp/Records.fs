module language_features_f_sharp.Records

// record: F#s native, kompakte Syntax fuer einen unveraenderlichen Datentyp mit automatischer
// struktureller Gleichheit (C#s "record" wurde von diesem F#-Konzept inspiriert).
type Adresse = { Strasse: string; Stadt: string }

let zeigen () =
    let adresse1 = { Strasse = "Bahnhofstrasse 1"; Stadt = "Zuerich" }
    let adresse2 = { Strasse = "Bahnhofstrasse 1"; Stadt = "Zuerich" }

    // records vergleichen ihre Werte, nicht die Referenz (anders als eine normale Klasse)
    let sindGleich = adresse1 = adresse2

    // "with"-Ausdruck: erzeugt eine neue Kopie mit einem geaenderten Feld,
    // das urspruengliche Record bleibt dabei unveraendert
    let adresse3 = { adresse1 with Stadt = "Bern" }

    printfn "%b" sindGleich
    printfn "%s" adresse1.Stadt
    printfn "%s" adresse3.Stadt
    printfn "%A" adresse1

    // Liste von records: List-Funktionen funktionieren auf ihnen genauso wie in Collections.fs auf int
    let adressen =
        [ { Strasse = "Bahnhofstrasse 1"; Stadt = "Zuerich" }
          { Strasse = "Marktgasse 5"; Stadt = "Bern" }
          { Strasse = "Seestrasse 12"; Stadt = "Zuerich" } ]

    // filter + map: erst filtern, dann nur die Strasse aus dem Record herausziehen
    let zuercherStrassen =
        adressen
        |> List.filter (fun adresse -> adresse.Stadt = "Zuerich")
        |> List.map (fun adresse -> adresse.Strasse)

    // groupBy: gruppiert die Adressen anhand eines Feldes des Records
    let adressenProStadt = adressen |> List.groupBy (fun adresse -> adresse.Stadt)

    printfn "%s" (String.concat ", " zuercherStrassen)

    for stadt, gruppe in adressenProStadt do
        printfn "%s: %d" stadt (List.length gruppe)
