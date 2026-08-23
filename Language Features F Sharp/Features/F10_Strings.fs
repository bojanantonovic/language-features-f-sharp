namespace Language_Features_F_Sharp.Features

// F# unterstuetzt seit F# 6 fuer eine begrenzte, eingebaute Auswahl an BCL-Konvertierungen
// (u. a. Array -> Span<T>/ReadOnlySpan<T>) doch eine implizite op_Implicit-Anwendung; das ist die
// Ausnahme von der Regel "F# kennt keine impliziten Konvertierungen" aus F01/F16 und wird hier
// bewusst genutzt (daher die Warnung FS3391 unterdrueckt).
#nowarn "3391"

open System
open System.Text

module F10_Strings =

    let run () =
        // String-Interpolation mit Format- und Alignment-Spezifizierern: F# unterstuetzt seit F# 5
        // exakt dieselbe Syntax "{ausdruck,ausrichtung:format}" wie C#.
        let name = "Zuerich"
        let einwohner = 434335
        Demo.print "String-Interpolation" $"{name,-10} hat {einwohner:N0} Einwohner"

        // Verbatim-String (@"..."): identische Syntax wie in C#, Escape-Sequenzen werden ignoriert.
        let pfad = @"C:\Projekte\LanguageFeatures\Program.fs"
        Demo.print "Verbatim-String" pfad

        // Raw String Literals ("""..."""): kein Escaping von Anfuehrungszeichen noetig, genau wie
        // in C# 11. EIN Unterschied bleibt: C# entfernt automatisch die gemeinsame Einrueckung aller
        // Zeilen (basierend auf der Position der schliessenden """); F# tut das NICHT - die
        // Einrueckung ist Teil des Werts und muss bei Bedarf manuell entfernt werden.
        let jsonRoh =
            """
            {
              "name": "Alice",
              "aktiv": true
            }
            """
        let jsonOhneEinrueckung =
            jsonRoh.Split('\n')
            |> Array.map (fun zeile -> zeile.Trim())
            |> Array.filter (fun zeile -> zeile.Length > 0)
            |> String.concat " "
        Demo.print "Raw String Literal (manuell dedented, s. Kommentar)" jsonOhneEinrueckung

        // Interpoliertes Raw String Literal: wie in C# wird die Anzahl "$" bestimmt, wie viele
        // geschweifte Klammern fuer eine Interpolation noetig sind.
        let wert = 42
        let interpoliertRoh =
            $$"""
            Ein JSON-Objekt: { "wert": {{wert}} }
            """
        Demo.print "interpoliertes Raw String Literal" (interpoliertRoh.Trim())

        // UTF-8 String Literals ("..."u8): F# kennt dieses Suffix nicht. Der Ersatz ist der
        // klassische Weg ueber Encoding.UTF8.GetBytes, der ebenfalls in eine ReadOnlySpan<byte> passt.
        let utf8Bytes: ReadOnlySpan<byte> = Encoding.UTF8.GetBytes "Hallo"
        Demo.print "Encoding.UTF8.GetBytes (statt u8-Suffix)" utf8Bytes.Length

        // Neue Escape-Sequenz \e (C# 13) fuer ESCAPE (0x1B): F# kennt dieses Kuerzel nicht,
        // wohl aber den allgemeinen Unicode-Escape \u001B mit demselben Ergebnis.
        let escapeZeichen = "\u001B"
        Demo.print "\\u001B Escape-Sequenz (statt \\e)" (int escapeZeichen.[0])

        // StringBuilder fuer effiziente, mutable Stringverkettung - identischer BCL-Typ.
        let builder = StringBuilder()
        builder.Append("Teil1").Append('-').Append("Teil2").AppendLine().Append "Zeile2" |> ignore
        Demo.print "StringBuilder" (builder.ToString().Replace("\n", " / "))

        // Nuetzliche string-Methoden - direkt aus dem BCL, ohne F#-spezifischen Unterschied.
        let csv = " Alice , Bob ,Carol "
        let teile = csv.Split(',') |> Array.map (fun t -> t.Trim())
        Demo.print "Split + Trim" (String.Join("|", teile))
        Demo.print "String.Join" (String.Join(" & ", teile))
        let enthaelt = name.Contains "ric"
        let startet = name.StartsWith "Zue"
        let endet = name.EndsWith "ich"
        Demo.print "Contains / StartsWith / EndsWith" $"{enthaelt} / {startet} / {endet}"
