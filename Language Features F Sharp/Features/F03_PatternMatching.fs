namespace Language_Features_F_Sharp.Features

open System

/// Entspricht dem "record struct Punkt(int X, int Y)" in C#: ein Werttyp-Record mit Positional Pattern.
[<Struct>]
type Punkt = { X: int; Y: int }

type Bestellung = { Artikel: string; Menge: int; Preis: decimal }

type Kunde = { Stadt: string; LetzteBestellung: Bestellung }

module F03_PatternMatching =

    let run () =
        // match-Ausdruck mit konstanten Patterns (Aequivalent zur switch-Expression).
        let tag = 3
        let tagName =
            match tag with
            | 1 -> "Montag"
            | 2 -> "Dienstag"
            | 3 -> "Mittwoch"
            | _ -> "Unbekannt"
        Demo.print "match mit konstanten Patterns" tagName

        // Relationale Patterns (aktiviert ueber die Active-Pattern-Operatoren "<" usw. gibt es in
        // F# nicht direkt; stattdessen Guard-Klauseln "when") und logische UND/ODER via "when"/"|".
        let temperatur = 17
        let beschreibung =
            match temperatur with
            | t when t < 0 -> "gefroren"
            | t when t >= 0 && t < 15 -> "kalt"
            | t when t >= 15 && t < 25 -> "angenehm"
            | _ -> "heiss"
        Demo.print "Guard-Klauseln (relationale + and-Patterns)" beschreibung

        // Oder-Pattern "|" und Negation ueber "when not" (F# hat kein eingebautes "not"-Pattern).
        let istKeinWochenende =
            match tag with
            | 6 | 7 -> false
            | _ -> true
        Demo.print "Oder-Pattern (statt or/not-Pattern)" istKeinWochenende

        // Type Pattern ueber ":?" innerhalb eines match, samt Variablenbindung und Guard.
        let beliebig: obj = "Text im object"
        match beliebig with
        | :? string as s when s.Length > 5 -> Demo.print "Type Pattern (:? string as s)" s
        | _ -> ()

        // Der Bezeichner-Pattern (entspricht C#s var-Pattern): bindet den Wert immer, ohne Typtest.
        match beliebig with
        | irgendwas -> Demo.print "Bezeichner-Pattern (Aequivalent zu var-Pattern)" irgendwas

        // Positional Pattern: F#-Records unterstuetzen kein Deconstruct-Tupel-Pattern wie C#-Records;
        // stattdessen matcht man direkt auf die benannten Felder (Record Pattern).
        let punkt = { X = 0; Y = 5 }
        let lage =
            match punkt with
            | { X = 0; Y = 0 } -> "Ursprung"
            | { X = 0 } -> "auf der Y-Achse"
            | { Y = 0 } -> "auf der X-Achse"
            | { X = x; Y = y } when x = y -> "auf der Diagonalen"
            | _ -> "irgendwo"
        Demo.print "Record Pattern (Punkt, Aequivalent zu positional pattern)" lage

        // Property Pattern -> in F# ebenfalls ein Record Pattern mit Guard-Klauseln.
        let bestellung = { Artikel = "Buch"; Menge = 3; Preis = 12.50m }
        let versandkosten =
            match bestellung with
            | { Menge = m } when m > 5 -> 0m
            | { Preis = p } when p > 50m -> 0m
            | { Artikel = "Buch" } -> 1.50m
            | _ -> 4.90m
        Demo.print "Record Pattern mit Guard (Bestellung)" versandkosten

        // Verschachteltes Record Pattern + Bindung der Gesamtvariable ueber "as".
        let kunde = { Stadt = "Zuerich"; LetzteBestellung = { Artikel = "Laptop"; Menge = 1; Preis = 999m } }
        match kunde with
        | { Stadt = "Zuerich"; LetzteBestellung = { Preis = p } } as vipKunde when p > 500m ->
            Demo.print "verschachteltes Record Pattern" $"{vipKunde.Stadt} VIP"
        | _ -> ()

        // List Pattern: F# unterstuetzt natives Matching auf Listen/Arrays inkl. Slice-Pattern.
        let zahlen = [ 1; 2; 3; 4; 5 ]
        let listBeschreibung =
            match zahlen with
            | [] -> "leer"
            | [ einzelnes ] -> $"ein Element: {einzelnes}"
            | erstes :: rest when not rest.IsEmpty -> $"erstes={erstes}, restliche={rest.Length}"
            | _ -> "sonstiges"
        Demo.print "Listen-Pattern (cons ::, Aequivalent zu list pattern)" listBeschreibung

        let istEinsBisFuenf =
            match zahlen with
            | 1 :: _ when zahlen |> List.last = 5 -> true
            | _ -> false
        Demo.print "list pattern [1; ..; 5]? (erstes/letztes Element)" istEinsBisFuenf
