namespace Language_Features_F_Sharp.Features

open System

// Benutzerdefinierte Exception als vollwertige Klasse (fuer den einfachen Fall kennt F# zusaetzlich
// das leichtgewichtige "exception"-Schluesselwort, z. B. "exception EinfacherFehler of string" -
// hier brauchen wir aber eine eigene Message-Formatierung UND ein zusaetzliches Datenfeld,
// weshalb die klassische Klassenform naeher am C#-Original liegt).
type UngueltigerBetragException(betrag: decimal) =
    inherit Exception($"Ungueltiger Betrag: {betrag}")
    member _.Betrag = betrag

// IDisposable-Implementierung fuer die using/use-Demonstration.
type Ressource(name: string) =
    member _.Name = name
    interface IDisposable with
        member _.Dispose() = printfn "    [Dispose] %s freigegeben" name

module F11_ExceptionHandling =

    let private pruefeBetrag (betrag: decimal) =
        if betrag < 0m then
            raise (UngueltigerBetragException betrag)

    let run () =
        // try/with mit mehreren Faellen (Aequivalent zu mehreren catch-Bloecken) + finally getrennt:
        // F# erlaubt in EINEM try-Block entweder "with" ODER "finally", NICHT beides gleichzeitig
        // wie C#. Fuer beides zusammen wird ein try/finally um ein try/with verschachtelt.
        try
            try
                pruefeBetrag -5m
            with
            | :? UngueltigerBetragException as ex -> Demo.print "with (spezifische Exception)" ex.Message
            | ex -> Demo.print "with (allgemeine Exception)" ex.Message
        finally
            Demo.print "finally (verschachtelt um try/with)" "wird immer ausgefuehrt"

        // Exception-Filter mit "when" - identische Syntax/Idee wie in C#.
        try
            pruefeBetrag -100m
        with
        | :? UngueltigerBetragException as ex when ex.Betrag < -50m ->
            Demo.print "Guard-Klausel (when Betrag < -50)" "stark negativer Betrag abgefangen"

        // throw-Expression (C# 7) -> "raise" ist in F# ohnehin ein ganz normaler Ausdruck vom Typ
        // 'a und daher ueberall einsetzbar, z. B. in Option.defaultWith.
        let eingabe: string option = None
        try
            eingabe
            |> Option.defaultWith (fun () -> raise (ArgumentNullException(nameof eingabe)))
            |> ignore
        with :? ArgumentNullException as ex ->
            Demo.print "raise als Ausdruck (statt throw-Expression)" ex.ParamName

        // Rethrow: "reraise()" erhaelt - wie C#s "throw;" - den urspruenglichen Stacktrace.
        try
            try
                pruefeBetrag -1m
            with :? UngueltigerBetragException ->
                Demo.print "innerer with-Block" "wird weitergereicht (reraise())"
                reraise ()
        with :? UngueltigerBetragException ->
            Demo.print "aeusserer with-Block" "Exception erneut gefangen"

        // "use" innerhalb eines expliziten Klammer-Blocks emuliert C#s using-Statement (Block):
        // Dispose erfolgt am Ende des geklammerten Ausdrucks.
        (
            use ressourceA = new Ressource("Ressource-A")
            Demo.print "use in geklammertem Block (Aequivalent zu using-Block)" ressourceA.Name
        )

        // "use" ohne Block entspricht direkt C#s using-Deklaration (C# 8): Dispose erst am Ende
        // der umschliessenden Funktion/des umschliessenden Scopes.
        use ressourceB = new Ressource("Ressource-B")
        Demo.print "use-Deklaration (Aequivalent zu using-Deklaration)" ressourceB.Name
