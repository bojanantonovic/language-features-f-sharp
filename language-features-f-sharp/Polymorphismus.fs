module language_features_f_sharp.Polymorphismus

open language_features_f_sharp.Vererbung

let zeigen () =
    // Funktionswert, der auf die (polymorphe) Methode MachGeraeusch zeigt
    let geraeuschVon (tier: Tier) = tier.MachGeraeusch()

    let hund = Hund("Rex") :> Tier
    let katze = Katze("Minka") :> Tier

    // Derselbe Funktionswert ruft je nach tatsaechlichem Typ eine andere Implementierung auf -
    // die Polymorphie aus Vererbung.fs/Polymorphie.fs wirkt genauso durch einen Funktionswert hindurch.
    let hundGeraeusch = geraeuschVon hund
    let katzeGeraeusch = geraeuschVon katze

    printfn "%s" hundGeraeusch
    printfn "%s" katzeGeraeusch

    // Funktionswerte koennen aber auch direkt unterschiedliches Verhalten tragen,
    // ganz ohne Vererbung - eine zweite, unabhaengige Form von "austauschbarem Verhalten".
    let lauteVersion (tier: Tier) = tier.MachGeraeusch().ToUpper() + "!!!"
    let lauterHund = lauteVersion hund

    printfn "%s" lauterHund
