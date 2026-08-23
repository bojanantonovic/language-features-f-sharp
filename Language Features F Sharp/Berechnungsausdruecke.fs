module Language_Features_F_Sharp.Berechnungsausdruecke

// Computation Expression: ein eigener "Builder"-Typ legt fest, was "let!"/"return" innerhalb
// eines { }-Blocks bedeuten - "async {}"/"seq {}" (siehe Asynchronitaet.fs, Iteratoren.fs) sind
// selbst nur eingebaute Beispiele dieses Mechanismus. Hat keine Entsprechung in C#.
type MaybeBuilder() =
    member _.Bind(wert, weiter) =
        match wert with
        | Some x -> weiter x
        | None -> None

    member _.Return(wert) = Some wert

let maybe = MaybeBuilder()

let dividieren zaehler nenner =
    if nenner = 0 then None else Some(zaehler / nenner)

// Ohne die Computation Expression muesste jeder Zwischenschritt einzeln per match geprueft werden -
// "let!" reicht ein "None" automatisch durch, sobald irgendein Schritt fehlschlaegt.
let berechnung a b c =
    maybe {
        let! ersteStufe = dividieren a b
        let! zweiteStufe = dividieren ersteStufe c
        return zweiteStufe
    }

let zeigen () =
    let erfolgreich = berechnung 100 5 2
    let fehlgeschlagen = berechnung 100 0 2

    printfn "%A" erfolgreich
    printfn "%A" fehlgeschlagen
