namespace Language_Features_F_Sharp.Features

open System
open System.Collections.Generic

// Generische Klasse. F# kennt keinen direkten Entsprechung zu C#s "notnull"-Constraint;
// generische F#-Typparameter sind ohnehin nicht auf Nullable<T> anwendbar und lassen sich
// stattdessen ueber Constraints wie "equality"/"comparison"/eigene Interfaces einschraenken.
type Auffangbox<'T>() =
    let elemente = ResizeArray<'T>()
    member _.Hinzufuegen(element: 'T) = elemente.Add element
    member _.Elemente: IReadOnlyList<'T> = elemente

type Zaehler() =
    member val Wert = 0 with get, set
    override this.ToString() = $"Zaehler({this.Wert})"

module F06_Generics =

    // Generische Methode mit new()-Constraint: F# druckt dies als expliziten SRTP-Constraint aus.
    let erzeuge<'T when 'T: (new: unit -> 'T)> () = new 'T()

    // Constraint IEquatable<'T> - ein Interface-Constraint, direktes Pendant zu C#s "where T : IEquatable<T>".
    let gleicherWert<'T when 'T :> IEquatable<'T>> (a: 'T) (b: 'T) = a.Equals b

    let run () =
        let box = Auffangbox<string>()
        box.Hinzufuegen "Eins"
        box.Hinzufuegen "Zwei"
        Demo.print "generische Klasse Auffangbox<'T>" (String.Join(",", box.Elemente))

        let neuerZaehler = erzeuge<Zaehler> ()
        Demo.print "generische Funktion mit new()-Constraint" neuerZaehler

        Demo.print "Constraint IEquatable<'T>" (gleicherWert 5 5)

        // Kovarianz/Kontravarianz: F# erlaubt es nicht, EIGENE generische Interfaces als
        // kovariant/kontravariant zu deklarieren (kein "out"/"in" wie in C#). Selbst fuer bereits
        // kovariante .NET-BCL-Typen wie IEnumerable<out T> wendet F#s Upcast-Operator ":>" die
        // CLR-Varianz NICHT automatisch an (anders als C#) - man muss sie elementweise nachbilden.
        let kreisSequenz: Kreis seq = Seq.singleton (Kreis 1.0)
        let figurSequenz: Figur seq = kreisSequenz |> Seq.map (fun k -> k :> Figur)
        Demo.print "Kovarianz-Ersatz (elementweises Upcasting statt automatischer Varianz)" ((Seq.head figurSequenz).Flaeche())

        // Ebenso fuer Kontravarianz (IComparer<in T>): statt einer automatischen Konvertierung
        // schreibt man einen expliziten Adapter, der die Argumente hochcastet.
        let figurComparer =
            { new IComparer<Figur> with
                member _.Compare(a, b) = compare (a.Flaeche()) (b.Flaeche()) }
        let kreisComparer =
            { new IComparer<Kreis> with
                member _.Compare(a, b) = figurComparer.Compare(a :> Figur, b :> Figur) }
        Demo.print "Kontravarianz-Ersatz (expliziter Adapter statt IComparer<in T>)" (kreisComparer.Compare(Kreis 1.0, Kreis 2.0))

        // Funktionstyp mit mehreren "Typparametern": in F# ist das schlicht eine Funktion aus
        // mehreren Argumenten - kein generisches Delegate wie Func<int,int,int> noetig.
        let addieren: int -> int -> int = fun a b -> a + b
        Demo.print "Funktionstyp int -> int -> int (statt Func<int,int,int>)" (addieren 3 4)
