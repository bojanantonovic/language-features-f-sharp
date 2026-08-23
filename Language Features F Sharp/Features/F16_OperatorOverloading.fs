namespace Language_Features_F_Sharp.Features

open System

// Operator Overloading + benutzerdefinierte implizite/explizite Konvertierungen.
// Bewusst eine Klasse statt [<Struct>] (anders als C#s "readonly struct"): F#-Structs duerfen im
// primaeren Konstruktor keine eigene Logik (hier: die Nenner-!=0-Pruefung) ausfuehren, da der
// vom Runtime bereitgestellte parameterlose Default-Konstruktor eines Structs diese umgehen wuerde
// (der Compiler lehnt das mit FS0901 explizit ab). Ein "echtes", validiertes Struct waere in F# nur
// ueber private Record-Felder + eine separate Smart-Constructor-Funktion moeglich (s. [<Struct>]-
// Einsatz bereits in F04); fuer den Fokus hier (Operatoren) reicht die Klasse.
type Bruch(zaehler: int, nennerRoh: int) =
    let nenner = if nennerRoh = 0 then raise (DivideByZeroException()) else nennerRoh

    member _.Zaehler = zaehler
    member _.Nenner = nenner

    static member (+) (a: Bruch, b: Bruch) =
        Bruch((a.Zaehler * b.Nenner) + (b.Zaehler * a.Nenner), a.Nenner * b.Nenner)

    static member (-) (a: Bruch, b: Bruch) =
        Bruch((a.Zaehler * b.Nenner) - (b.Zaehler * a.Nenner), a.Nenner * b.Nenner)

    // F# ueberlaedt "=="/"!=" nicht separat wie C#: die Operatoren "="/"<>" SIND bereits die
    // anpassbare (Struktur-)Gleichheit - man ueberschreibt einfach Equals/GetHashCode wie hier,
    // ganz ohne eigene "operator =="-Deklaration.
    member this.ErgibtGleichenWert(other: Bruch) = this.Zaehler * other.Nenner = other.Zaehler * this.Nenner

    override this.Equals(obj) =
        match obj with
        | :? Bruch as other -> this.ErgibtGleichenWert other
        | _ -> false

    override _.GetHashCode() = HashCode.Combine(zaehler, nenner)

    // Implizite Konvertierung: jede int-Zahl kann verlustfrei zu Bruch werden. WICHTIG: F# wendet
    // benutzerdefinierte op_Implicit-Konvertierungen (anders als C#) NIE automatisch an (s. F01/F10/
    // F14) - der Aufruf muss immer explizit erfolgen.
    static member op_Implicit(ganzzahl: int) : Bruch = Bruch(ganzzahl, 1)

    // Explizite Konvertierung: Bruch -> double kann Praezision verlieren. F# hat keine eigene
    // "(T)wert"-Cast-Syntax fuer benutzerdefinierte Typen - auch hier ruft man op_Explicit als
    // normale statische Methode auf.
    static member op_Explicit(b: Bruch) : double = double b.Zaehler / double b.Nenner

    override _.ToString() = $"{zaehler}/{nenner}"

module F16_OperatorOverloading =

    let run () =
        let einHalb = Bruch(1, 2)
        let einDrittel = Bruch(1, 3)

        Demo.print "operator + (Bruch)" (einHalb + einDrittel)
        Demo.print "operator - (Bruch)" (einHalb - einDrittel)
        let gleich = einHalb = Bruch(2, 4)
        let ungleich = einHalb <> einDrittel
        Demo.print "= / <> (Aequivalent zu ==/!=)" $"{gleich} / {ungleich}"

        // Implizite Konvertierung muss in F# explizit aufgerufen werden.
        let ausInt: Bruch = Bruch.op_Implicit 5
        Demo.print "Bruch.op_Implicit (statt automatischer Konvertierung)" ausInt

        // Explizite Konvertierung ebenso als expliziter Methodenaufruf.
        let alsDouble = Bruch.op_Explicit einHalb
        Demo.print "Bruch.op_Explicit (statt (double)-Cast)" alsDouble
