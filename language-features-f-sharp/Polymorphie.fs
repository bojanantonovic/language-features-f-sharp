module Language_Features_F_Sharp.Polymorphie

open Language_Features_F_Sharp.Vererbung

let zeigen () =
    // Liste vom Basistyp Tier, enthaelt aber Objekte unterschiedlicher abgeleiteter Typen.
    // ":> Tier" (Upcast) macht den gemeinsamen Listentyp explizit.
    let tiere: Tier list = [ Hund("Rex") :> Tier; Katze("Minka") :> Tier; Hund("Bello") :> Tier ]

    // Polymorphie: derselbe Aufruf MachGeraeusch() liefert je nach tatsaechlichem Typ
    // ein anderes Ergebnis - der Compiler kennt hier nur Tier, zur Laufzeit wird aber
    // die passende override-Methode von Hund bzw. Katze ausgefuehrt.
    for tier in tiere do
        let geraeusch = tier.MachGeraeusch()
        printfn "%s" tier.Name
        printfn "%s" geraeusch
