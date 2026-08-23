namespace Language_Features_F_Sharp.Features

open System
open System.Collections.Generic

// Primary Constructor: F#-Klassen haben IMMER einen primaeren Konstruktor direkt im Typkopf -
// das ist der Normalfall, nicht ein Sonderfall wie in C# 12. Die Parameter (inhaber, startguthaben)
// sind im gesamten Klassenkoerper sichtbar.
type Konto(inhaber: string, startguthaben: decimal) =
    static let mutable erstellteKonten = 0

    // "let" auf Klassenebene entspricht einem private readonly Feld, hier zugleich mit Seiteneffekt
    // (Zaehler erhoehen) initialisiert - vergleichbar mit C#s "_kontonummer = ++_erstellteKonten".
    let kontonummer =
        erstellteKonten <- erstellteKonten + 1
        erstellteKonten

    let mutable guthaben = startguthaben

    member _.Inhaber = inhaber
    member _.Guthaben = guthaben
    static member AnzahlKonten = erstellteKonten

    member _.Einzahlen(betrag: decimal) =
        if betrag <= 0m then
            raise (ArgumentOutOfRangeException(nameof betrag))
        guthaben <- guthaben + betrag

    // Override + Expression-Member (F# kennt keine separate "expression-bodied member"-Syntax,
    // jedes Member ist ohnehin ein Ausdruck).
    override _.ToString() = $"Konto #{kontonummer} von {inhaber}: {guthaben:C}"

// "field"-Keyword (C# 14): F# braucht dafuer kein Sonderkeyword, da jede Property ueber "let mutable"
// jederzeit ein eigenes Backing-Field deklarieren kann - das ist der uebliche F#-Weg fuer Validierung
// in einem Setter.
type Temperatur() =
    let mutable celsius = 0.0

    member _.Celsius
        with get () = celsius
        and set value =
            celsius <-
                if value < -273.15 then
                    raise (ArgumentOutOfRangeException(nameof value, "Unterhalb des absoluten Nullpunkts"))
                else
                    value

// Required Members (C# 11) -> F#-Records verlangen ohnehin IMMER alle Felder bei der Konstruktion
// (kein Feld darf implizit weggelassen werden); das entspricht "required" bereits im Normalfall.
type Registrierung =
    { Email: string
      Benutzername: string
      Datum: DateOnly }

// readonly struct -> [<Struct>] Record: ein reiner Werttyp, dessen Felder unveraenderlich sind.
[<Struct>]
type Geldbetrag =
    { Betrag: decimal
      Waehrung: string }
    override this.ToString() = $"{this.Betrag} {this.Waehrung}"

// Indexer: das "Item"-Member mit Get-Zugriff erzeugt array-aehnlichen Zugriff wie in C#.
type Woche() =
    let tage = [| "Mo"; "Di"; "Mi"; "Do"; "Fr"; "Sa"; "So" |]
    member _.Item
        with get (index: int) = tage.[index]
    member _.Count = tage.Length

// record class -> F#-Record (Referenztyp) mit automatischer Werte-Gleichheit, Deconstruction
// und ToString bereits als Sprachkern, nicht als Zusatzfeature wie in C#.
type Adresse =
    { Strasse: string; Stadt: string }
    // Ueberschreibt F#s mehrzeilige Standard-Strukturformatierung fuer Records durch eine
    // einzeilige Darstellung, wie sie C#s Record.ToString() liefert.
    override this.ToString() = $"Adresse {{ Strasse = {this.Strasse}, Stadt = {this.Stadt} }}"

// record struct (C# 10) -> [<Struct>] vor einem Record.
[<Struct>]
type Vektor2D =
    { X: float; Y: float }
    member this.Laenge = sqrt ((this.X * this.X) + (this.Y * this.Y))

// Partial class + partial method: F# kennt WEDER partial classes NOCH partial methods - ein Typ
// muss vollstaendig an einer Stelle deklariert werden. Der naechstliegende Ersatz ist eine
// Typerweiterung ("type X with member ...") in einer separaten Datei/an anderer Stelle, die aber
// nur zusaetzliche Member, keine zusaetzlichen Felder ergaenzen kann.
type Logger() =
    let eintraege = ResizeArray<string>()
    member _.Eintraege: IReadOnlyList<string> = eintraege
    member this.Log(nachricht: string) =
        eintraege.Add nachricht
        this.OnLog nachricht
    // Ersatz fuer "partial void OnLog(...)": eine virtuelle Methode mit leerer Standardimplementierung,
    // die bei Bedarf ueberschrieben wird (F# hat keine no-op-Hook-Methoden wie partial methods).
    abstract member OnLog: string -> unit
    default _.OnLog(nachricht: string) = printfn "    [Logger] %s" nachricht

module F04_ClassesStructsRecords =

    let run () =
        let konto = Konto("Alice", 100m)
        konto.Einzahlen 50m
        Demo.print "Primary Constructor (Konto)" konto
        Demo.print "statisches Feld AnzahlKonten" Konto.AnzahlKonten

        let temperatur = Temperatur(Celsius = 20.5)
        Demo.print "Backing-Field via let mutable (Temperatur.Celsius)" temperatur.Celsius
        try
            temperatur.Celsius <- -300.0
        with :? ArgumentOutOfRangeException as ex ->
            Demo.print "Setter-Validierung schlaegt fehl" ex.Message

        let registrierung =
            { Email = "alice@example.com"
              Benutzername = "alice"
              Datum = DateOnly.FromDateTime DateTime.Today }
        Demo.print "Record erzwingt alle Felder (statt required)" $"{registrierung.Benutzername} ({registrierung.Email}), {registrierung.Datum}"

        let betrag = { Betrag = 19.99m; Waehrung = "CHF" }
        Demo.print "[<Struct>] Record (readonly struct)" betrag

        let woche = Woche()
        Demo.print "Indexer woche[2]" woche.[2]

        let adresse1 = { Strasse = "Bahnhofstrasse 1"; Stadt = "Zuerich" }
        let adresse2 = { Strasse = "Bahnhofstrasse 1"; Stadt = "Zuerich" }
        Demo.print "record Werte-Gleichheit" (adresse1 = adresse2)
        let adresse3 = { adresse1 with Stadt = "Bern" }
        Demo.print "with-Expression" adresse3
        let { Strasse = strasse; Stadt = stadt } = adresse3
        Demo.print "record Deconstruction" $"{strasse}, {stadt}"

        let vektor = { X = 3.0; Y = 4.0 }
        Demo.print "[<Struct>] record (Vektor2D.Laenge)" vektor.Laenge

        let logger = Logger()
        logger.Log "Anwendung gestartet"
        Demo.print "Typerweiterungs-Ersatz fuer partial class/method" logger.Eintraege.Count
