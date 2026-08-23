module language_features_f_sharp.Listen

let zeigen () =
    // Array: feste Groesse, direkt bei der Deklaration befuellt (vergleichbar mit C#s string[])
    let farben = [| "Rot"; "Gruen"; "Blau" |]
    let ersteFarbe = farben.[0]
    let anzahlFarben = farben.Length

    // F#-Liste: unveraenderliche verkettete Liste. "Entfernen" erzeugt eine NEUE Liste,
    // die urspruengliche bleibt bestehen - anders als C#s veraenderliche List<T>
    // (fuer eine veraenderliche .NET-Liste siehe ResizeArray in Collections.fs).
    let einkaufsliste = [ "Milch"; "Brot"; "Butter" ]
    let ohneBrot = einkaufsliste |> List.filter (fun artikel -> artikel <> "Brot")
    let anzahlEinkaufsliste = ohneBrot.Length

    printfn "%s" ersteFarbe
    printfn "%d" anzahlFarben
    printfn "%d" anzahlEinkaufsliste

    for artikel in ohneBrot do
        printfn "%s" artikel
