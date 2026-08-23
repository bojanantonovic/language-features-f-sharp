namespace Language_Features_F_Sharp.Features

open System
open System.Collections.Generic
open System.Linq
open System.Threading

// System.Threading.Lock (C# 13): derselbe BCL-Typ ist auch aus F# nutzbar. ABER: Lock.EnterScope()
// liefert einen "ref struct"-Scope, der Dispose() nur nach dem C#-spezifischen "pattern-based
// dispose"-Muster (Duck-Typing ueber eine Methode namens Dispose, OHNE IDisposable zu implementieren)
// anbietet. F#s "use"-Bindung verlangt eine ECHTE IDisposable-Implementierung und akzeptiert das
// Pattern-based Dispose nicht - der Ersatz ist ein manuelles try/finally mit explizitem .Dispose().
type Zaehlwerk() =
    let sperre = Lock()
    let mutable wert = 0

    member _.Erhoehen() =
        let scope = sperre.EnterScope()
        try
            wert <- wert + 1
        finally
            scope.Dispose()

    member _.Wert =
        let scope = sperre.EnterScope()
        try
            wert
        finally
            scope.Dispose()

// Partial Properties (C# 13): F# kennt - wie schon partial classes/methods in F04 - kein
// Deklaration/Implementierung-Splitting. Der Ersatz ist erneut eine ganz normale Property mit
// explizitem Backing-Field (s. F04, "field"-Keyword-Ersatz).
type Konfiguration() =
    let mutable modus = "Standard"
    member _.Modus
        with get () = modus
        and set value = modus <- value

// Typerweiterungen auf FREMDEN (nicht in dieser Datei definierten) Typen muessen - anders als
// Erweiterungen auf eigenen Typen wie Zaehlwerk/Konfiguration oben - in einem eigenen Modul stehen
// (Compilerfehler FS0644 sonst).
module private Erweiterungen =

    // Klassische Extension Method (C# 3): F# nennt das "Typerweiterung" ("type ... with") und
    // braucht dafuer keinen speziellen "this"-Parameter wie C# - die Syntax ist dieselbe wie bei
    // normalen Type-Members.
    type System.String with
        member this.Umdrehen() = String(this.ToCharArray() |> Array.rev)

    // Extension Members (C# 14, gruppierte "extension<T>(...) { }"-Bloecke): ein einzelner
    // F#-Typerweiterungsblock ("type X with member ... member ...") buendelt ohnehin schon mehrere
    // Member fuer denselben Typ - ein eigenes Gruppierungs-Sprachfeature ist daher gar nicht noetig.
    // WICHTIG: F# kann - anders als C# - KEINE Erweiterungen fuer VARIANTE generische BCL-Interfaces
    // wie IEnumerable<out T> deklarieren (Compilerfehler FS0957: die Varianz-Annotation passt nicht
    // zur Typerweiterung). Der Ersatz ist eine Erweiterung des konkreten, nicht-varianten Typs
    // List<'T> (F#s "ResizeArray<'T>" ist nur ein Typalias dafuer und laesst sich, genau wie
    // "seq<'T>", ebenfalls nicht direkt erweitern - FS0964).
    type List<'T> with
        member this.IstLeer = not (this.Any())
        member this.ZweitesOderStandard() = this.Skip(1).FirstOrDefault()

    type System.Int32 with
        member this.IstPrimzahl =
            if this < 2 then
                false
            else
                let mutable istPrim = true
                let mutable teiler = 2
                while istPrim && teiler * teiler <= this do
                    if this % teiler = 0 then
                        istPrim <- false
                    teiler <- teiler + 1
                istPrim

module F18_ModernSyntax =
    open Erweiterungen

    let run () =
        // Namespace-Deklarationen in F# sind IMMER "file-scoped" (nur "namespace X", ohne
        // geschweifte Klammern) - ein Block-Namespace wie in klassischem C# existiert in F# gar
        // nicht erst, es gibt also auch keine gesonderte "file-scoped"-Variante davon.
        Demo.print "namespace X (immer file-scoped, s. Dateianfang)" typeof<Konfiguration>.Namespace

        // Typalias aus Aliases.fs (Aequivalent zu global using + using-Alias).
        let namen: StringListe = StringListe [ "Alice"; "Bob" ]
        Demo.print "Typalias (StringListe, Aequivalent zu global using-Alias)" (String.Join(",", namen))

        Demo.print "klassische Typerweiterung (Umdrehen)" ("fsharp".Umdrehen())

        // Nutzung der Typerweiterungen (instanzbezogen und primitive-typbezogen).
        Demo.print "Typerweiterung (IstLeer)" (ResizeArray<int>()).IstLeer
        Demo.print "Typerweiterung (ZweitesOderStandard)" ((ResizeArray [ 1; 2; 3 ]).ZweitesOderStandard())
        Demo.print "Typerweiterung auf int (IstPrimzahl)" (17).IstPrimzahl

        let zaehlwerk = Zaehlwerk()
        System.Threading.Tasks.Parallel.For(0, 1000, (fun _ -> zaehlwerk.Erhoehen())) |> ignore
        Demo.print "System.Threading.Lock (try/finally statt use)" zaehlwerk.Wert

        let konfiguration = Konfiguration()
        Demo.print "Property mit Backing-Field (statt partial property)" konfiguration.Modus
        konfiguration.Modus <- "Erweitert"
        Demo.print "nach Zuweisung" konfiguration.Modus
