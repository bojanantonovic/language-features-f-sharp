module language_features_f_sharp.Iteratoren

// seq { yield ... } erzeugt die Werte erst nacheinander, wenn sie tatsaechlich abgefragt werden -
// die Funktion selbst laeuft nicht sofort komplett durch (Aequivalent zu C#s "yield return").
let geradeZahlenBis obergrenze =
    seq {
        for zahl in 0..obergrenze do
            if zahl % 2 = 0 then
                yield zahl
    }

// Eigener Iterator ueber eine Liste: liefert die Elemente in umgekehrter Reihenfolge
let rueckwaerts (werte: string list) =
    seq {
        for index in (werte.Length - 1) .. -1 .. 0 do
            yield werte.[index]
    }

let zeigen () =
    for zahl in geradeZahlenBis 10 do
        printfn "%d" zahl

    // Das Ergebnis eines Iterators laesst sich wie jede andere seq<'T> mit List-/Seq-Funktionen weiterverarbeiten
    let geradeZahlen = geradeZahlenBis 10 |> List.ofSeq
    let summe = geradeZahlen |> List.sum

    printfn "%d" summe

    let namen = [ "Alice"; "Bob"; "Carol" ]

    for name in rueckwaerts namen do
        printfn "%s" name
