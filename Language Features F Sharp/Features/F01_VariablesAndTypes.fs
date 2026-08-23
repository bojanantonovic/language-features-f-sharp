namespace Language_Features_F_Sharp.Features

open System
open System.Collections.Generic

module F01_VariablesAndTypes =

    // const (C#) -> [<Literal>]: eine echte Kompilierzeit-Konstante, vergleichbar mit C#s "const".
    [<Literal>]
    let private PiApprox = 3.14159

    let run () =
        // Typinferenz: "let" leitet den Typ immer aus dem Kontext ab, ein Pendant zu C#s "var"
        // ist daher gar nicht noetig - jede F#-Bindung verhaelt sich wie "var".
        let zahl = 42
        let text = "Hallo"
        Demo.print "let (Typinferenz, int)" zahl
        Demo.print "let (Typinferenz, string)" text

        // Werttypen vs. Referenztypen: dasselbe Verhalten wie in C#.
        let wertTyp = 10
        let mutable kopie = wertTyp
        kopie <- 20
        Demo.print "Werttyp bleibt unveraendert" wertTyp

        // ResizeArray<T> ist F#s Alias fuer System.Collections.Generic.List<T> (Referenztyp).
        let liste = ResizeArray<int> [ 1; 2; 3 ]
        let referenz = liste
        referenz.Add 4
        Demo.print "Referenztyp teilt Zustand" (String.Join(",", liste))

        // let-Bindungen sind implizit "readonly"; jede F#-Variable ist standardmaessig unveraenderlich,
        // "mutable" (siehe oben bei kopie) ist die explizite Ausnahme. const s. [<Literal>] oben.
        Demo.print "[<Literal>] PiApprox" PiApprox

        // Nullable Value Types (Nullable<T>) und die Bruecke zu F#s idiomatischem Option-Typ.
        let optionaleZahl = Nullable<int>()
        let ohneWertAlsDefault = optionaleZahl |> Option.ofNullable |> Option.defaultValue -1
        Demo.print "Nullable<int> ohne Wert -> Option.defaultValue" ohneWertAlsDefault
        let optionaleZahlMitWert = Nullable 7
        Demo.print "Nullable<int>.HasValue / .Value" $"{optionaleZahlMitWert.HasValue} / {optionaleZahlMitWert.Value}"

        // Target-typed new: in F# ueberfluessig, da die Typinferenz den Zieltyp ohnehin immer
        // aus der Deklaration ableitet - es gibt kein Pendant zu C#s "new()".
        let targetTyped: ResizeArray<int> = ResizeArray [ 1; 2; 3 ]
        Demo.print "Typinferenz statt target-typed new" (String.Join(",", targetTyped))

        // default literal -> Unchecked.defaultof<'T>: liefert den Default-Wert eines beliebigen Typs,
        // insbesondere nuetzlich in generischem Code.
        let defaultInt = Unchecked.defaultof<int>
        let defaultString = Unchecked.defaultof<string>
        let defaultStringAnzeige = if isNull defaultString then "null" else defaultString
        Demo.print "Unchecked.defaultof<int/string>" $"{defaultInt} / {defaultStringAnzeige}"

        // Tupel und Deconstruction. F#-Tupel haben - anders als C# 7+ - keine benannten Elemente,
        // die Positionen tragen aber weiterhin Bedeutung.
        let person = ("Alice", 30)
        let (name, alter) = person
        Demo.print "Tuple-Deconstruction" $"{name} ist {alter} Jahre alt"

        // Der projektweite Typalias "Koordinate" aus Aliases.fs (Gegenstueck zum using-Alias in C#).
        let zuerich: Koordinate = (47.3769, 8.5417)
        let (breite, laenge) = zuerich
        Demo.print "Typalias fuer Tupeltyp (Koordinate)" $"{breite}, {laenge}"

        // Discards: ignorieren nicht benoetigte Werte, genau wie in C#.
        let (_, nurAlter) = person
        Demo.print "Discard bei Deconstruction" nurAlter

        // checked/unchecked: F# ist bei den Standardoperatoren *immer* "unchecked" (wie C#s
        // unchecked-Block) und stellt fuer den Gegenfall das Modul "Checked" bereit.
        let ueberlauf = byte (255 + 2)
        Demo.print "unchecked (Standard) Ueberlauf byte(255+2)" ueberlauf
        try
            Checked.byte (255 + 2) |> ignore
        with :? OverflowException as ex ->
            Demo.print "Checked.byte(255+2) wirft" (ex.GetType().Name)

        // Numerische Konvertierung: F# kennt - anders als C# - KEINE impliziten Konvertierungen.
        // Jede Umwandlung (auch eine verlustfreie "Erweiterung") erfolgt explizit ueber Funktionen
        // wie float/byte/int.
        let quelle = 300
        let alsDouble = float quelle
        let alsByte = byte quelle
        Demo.print "explizite Konvertierung int->float (kein implizit in F#)" alsDouble
        Demo.print "explizite Konvertierung int->byte (Verlust)" alsByte
