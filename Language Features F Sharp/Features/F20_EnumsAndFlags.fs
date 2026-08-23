namespace Language_Features_F_Sharp.Features

open System

// Einfaches Enum mit explizitem Basistyp: F#-Enums verlangen - anders als C# - fuer JEDEN Fall
// einen expliziten Wert (kein automatisches Hochzaehlen).
type Wochentag =
    | Montag = 0uy
    | Dienstag = 1uy
    | Mittwoch = 2uy
    | Donnerstag = 3uy
    | Freitag = 4uy
    | Samstag = 5uy
    | Sonntag = 6uy

// [<Flags>]-Enum: Werte sind Zweierpotenzen und lassen sich per Bitwise-OR kombinieren. F#-Enum-
// Werte muessen konstante Literale sein - ein Fall kann daher nicht per "|||" auf andere, im selben
// Enum deklarierte Faelle verweisen (anders als C#s "Alle = Lesen | Schreiben | ..."); der
// kombinierte Wert wird stattdessen direkt als Literal (1+2+4+8=15) angegeben.
[<Flags>]
type Berechtigung =
    | Keine = 0
    | Lesen = 1
    | Schreiben = 2
    | Ausfuehren = 4
    | Loeschen = 8
    | Alle = 15

module F20_EnumsAndFlags =

    let run () =
        let heute = Wochentag.Mittwoch
        Demo.print "Enum-Wert" heute
        Demo.print "Enum -> byte (underlying byte)" (byte heute)

        // Pattern Matching auf Enum-Werten - funktioniert direkt mit match.
        let istWochenende =
            match heute with
            | Wochentag.Samstag
            | Wochentag.Sonntag -> true
            | _ -> false
        Demo.print "match auf Enum (statt switch-Expression)" istWochenende

        // [<Flags>]: Kombinieren, Pruefen und Entfernen von Bit-Flags - dieselben Operatoren/APIs wie C#.
        let mutable rechte = Berechtigung.Lesen ||| Berechtigung.Schreiben
        Demo.print "Flags kombiniert (Bitwise-OR |||)" rechte
        Demo.print "HasFlag(Schreiben)" (rechte.HasFlag Berechtigung.Schreiben)
        Demo.print "HasFlag(Loeschen)" (rechte.HasFlag Berechtigung.Loeschen)

        rechte <- rechte ||| Berechtigung.Ausfuehren
        rechte <- rechte &&& ~~~Berechtigung.Lesen
        Demo.print "Flags erweitert & entfernt" rechte

        // Enum.GetValues<T>() / Enum.GetNames<T>() - identische generische BCL-APIs.
        Demo.print "Enum.GetValues<T>()" (String.Join(",", Enum.GetValues<Wochentag>()))

        // Enum.Parse<T> / Enum.TryParse<T>.
        let geparst = Enum.Parse<Wochentag> "Freitag"
        Demo.print "Enum.Parse<T>" geparst
        Demo.print "Enum.TryParse<T> (ungueltig)" (Enum.TryParse<Wochentag>("Ungueltig") |> fst)
