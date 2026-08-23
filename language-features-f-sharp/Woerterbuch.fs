module language_features_f_sharp.Woerterbuch

open System.Collections.Generic

let zeigen () =
    // Dictionary (aus dem .NET BCL, veraenderlich): speichert Werte unter einem eindeutigen Schluessel.
    // F#s eigene, unveraenderliche Alternative "Map" wird in Collections.fs gezeigt.
    let alterNachName = Dictionary<string, int>()
    alterNachName.["Alice"] <- 30
    alterNachName.["Bob"] <- 25
    alterNachName.Add("Carol", 40)

    // Zugriff ueber den Schluessel
    let aliceAlter = alterNachName.["Alice"]

    // Sicherer Zugriff: TryGetValue liefert (bool * int) statt bei fehlendem Schluessel
    // eine Exception zu werfen - in F# direkt als Tuple entpackbar, ganz ohne "out".
    let gefunden, daveAlter = alterNachName.TryGetValue("Dave")

    let anzahlEintraege = alterNachName.Count

    printfn "%d" aliceAlter
    printfn "%b" gefunden
    printfn "%d" daveAlter
    printfn "%d" anzahlEintraege

    for eintrag in alterNachName do
        printfn "%s" eintrag.Key
        printfn "%d" eintrag.Value

    // Seq-Pipeline auf einem Dictionary: iteriert ueber KeyValuePair<string, int>-Eintraege
    let namenUeber28 =
        alterNachName
        |> Seq.filter (fun eintrag -> eintrag.Value > 28)
        |> Seq.map (fun eintrag -> eintrag.Key)
        |> List.ofSeq

    // Seq.maxBy: den Namen mit dem hoechsten Alter finden
    let aeltesterName =
        alterNachName
        |> Seq.maxBy (fun eintrag -> eintrag.Value)
        |> fun eintrag -> eintrag.Key

    printfn "%s" (String.concat ", " namenUeber28)
    printfn "%s" aeltesterName
