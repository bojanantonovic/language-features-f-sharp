namespace Language_Features_F_Sharp.Features

open System

// Interface mit Default Interface Method (C# 8): F# unterstuetzt seit F# 4.1 ebenfalls Default-
// Implementierungen in Interfaces, ueber "abstract"+"default" in einer expliziten
// "interface ... with"-Definition.
type IFigur =
    abstract member Flaeche: unit -> float
    abstract member Beschreibung: unit -> string

type IFarbig =
    abstract member Farbe: string

// Abstrakte Basisklasse mit virtueller Methode.
[<AbstractClass>]
type Figur() =
    abstract member Flaeche: unit -> float
    abstract member Zusammenfassung: unit -> string
    default this.Zusammenfassung() = $"{this.GetType().Name}: {this.Flaeche():F2}"

    interface IFigur with
        member this.Flaeche() = this.Flaeche()
        // Default-Implementierung des Interface-Members ueber die virtuelle Basismethode.
        member this.Beschreibung() = $"Figur mit Flaeche {this.Flaeche():F2}"

type Kreis(radius: float) =
    inherit Figur()
    let mutable farbe = "unbekannt"
    member _.Radius = radius
    member _.Farbe
        with get () = farbe
        and set value = farbe <- value
    override _.Flaeche() = Math.PI * radius * radius
    // override + Aufruf der Basisimplementierung via "base".
    override this.Zusammenfassung() = base.Zusammenfassung() + $" (Farbe: {this.Farbe})"
    interface IFarbig with
        member this.Farbe = this.Farbe

// "sealed" ist in F# der Normalfall: Klassen sind bereits implizit versiegelt, sobald sie keine
// "abstract"-Member enthalten - ein explizites Schluesselwort wie in C# ist nicht noetig.
type Quadrat(seite: float) =
    inherit Figur()
    member _.Seite = seite
    override _.Flaeche() = seite * seite

// Kovariante Rueckgabetypen (C# 9): F# erlaubt in Overrides KEINE abgeleiteten Rueckgabetypen -
// die Signatur muss exakt der Basisdeklaration entsprechen. Der Ersatz ist eine eigene Methode
// mit praezisem Rueckgabetyp neben der Basis-Factory-Methode.
[<AbstractClass>]
type FigurFabrik() =
    abstract member Erzeuge: unit -> Figur

type KreisFabrik() =
    inherit FigurFabrik()
    override _.Erzeuge() : Figur = Kreis(1.0)
    // Zusaetzliche, praezise typisierte Methode als Ersatz fuer den kovarianten Rueckgabetyp.
    member _.ErzeugeKreis() : Kreis = Kreis(1.0)

// Static Abstract Members in Interfaces (C# 11, "Generic Math"): F# hat kein direktes Sprachfeature
// dafuer, erreicht dasselbe Ziel aber ueber Statically Resolved Type Parameters (SRTP) mit
// "^T" und einem "member"-Constraint - komplett ohne Interface, rein strukturell/duck-typed.
[<Struct>]
type Geld = { Betrag: decimal }
    with
    static member Addiere(a: Geld, b: Geld) = { Betrag = a.Betrag + b.Betrag }

module F05_InheritancePolymorphism =

    // SRTP-Constraint "^T : (static member Addiere : ^T * ^T -> ^T)" - der F#-Ersatz fuer
    // "static abstract members in interfaces" (Generic Math).
    let inline summiereGenerisch<'T when 'T: (static member Addiere: 'T * 'T -> 'T)> (a: 'T) (b: 'T) =
        'T.Addiere(a, b)

    let run () =
        let kreis = Kreis(2.0)
        kreis.Farbe <- "rot"
        let figuren: Figur array = [| kreis; Quadrat(3.0) |]

        // Polymorphismus: die konkrete Implementierung wird zur Laufzeit gewaehlt.
        for figur in figuren do
            Demo.print "Polymorphie: Zusammenfassung" (figur.Zusammenfassung())

        // Default Interface Method wird ueber das Interface aufgerufen.
        let alsInterface = figuren.[1] :> IFigur
        Demo.print "Default Interface Method" (alsInterface.Beschreibung())

        // Type Pattern (":?") als Aequivalent zu is/as.
        for figur in figuren do
            match box figur with
            | :? IFarbig as farbig -> Demo.print "Type Pattern auf Interface" $"{figur.GetType().Name} ist {farbig.Farbe}"
            | _ -> ()
            // ":?" prueft nur den Typ (Aequivalent zu C#s "as", das bei Fehlschlag null liefert -
            // in F# nutzt man dafuer typischerweise direkt das bool-Pattern statt einer Null-Kopie).
            Demo.print "Type-Test :? (Aequivalent zu as)" (if figur :? Kreis then "ist Kreis" else "kein Kreis")

        // Praezise typisierte Fabrikmethode als Ersatz fuer kovarianten Rueckgabetyp.
        let erzeugterKreis = KreisFabrik().ErzeugeKreis()
        Demo.print "praeziser Rueckgabetyp (statt kovariant)" erzeugterKreis.Radius

        // Generic Math ueber SRTP statt static abstract interface member.
        let summe = summiereGenerisch { Betrag = 10.50m } { Betrag = 4.50m }
        Demo.print "SRTP statt static abstract interface member" summe.Betrag
