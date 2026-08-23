module Language_Features_F_Sharp.Bedingungen

let zeigen () =
    let alter = 30
    let istAktiv = true

    // if/else als Ausdruck: liefert direkt einen Wert, keine vorher deklarierte Variable noetig
    let status = if istAktiv then "aktiv" else "inaktiv"

    // if/elif/else: mehrere Faelle nacheinander pruefen ("elif" statt "else if")
    let altersgruppe =
        if alter < 18 then "minderjaehrig"
        elif alter < 65 then "erwachsen"
        else "senior"

    printfn "%s" status
    printfn "%s" altersgruppe
