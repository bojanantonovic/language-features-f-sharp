module Language_Features_F_Sharp.DiskriminierteVereinigungen

// Discriminated Union: ein Wert ist GENAU EINER von mehreren, klar benannten Faellen -
// F#s zentrales Werkzeug fuer Datenmodellierung, hat keine direkte Entsprechung in C#
// (am naechsten kommen dort geschlossene Klassenhierarchien oder Pattern Matching auf object).
type Form =
    | Kreis of radius: float
    | Rechteck of breite: float * hoehe: float
    | Dreieck of basis: float * hoehe: float

// match zwingt dazu, ALLE Faelle zu behandeln - vergisst man einen, warnt der Compiler.
let flaeche form =
    match form with
    | Kreis radius -> System.Math.PI * radius * radius
    | Rechteck(breite, hoehe) -> breite * hoehe
    | Dreieck(basis, hoehe) -> 0.5 * basis * hoehe

// Single-Case Union: ein "staerker typisiertes" Wrapping um einen primitiven Wert -
// eine Kundennummer laesst sich dadurch nicht mehr versehentlich mit einer beliebigen int vertauschen.
type Kundennummer = Kundennummer of int

let zeigen () =
    let formen = [ Kreis 2.0; Rechteck(3.0, 4.0); Dreieck(6.0, 2.0) ]

    for form in formen do
        printfn "%f" (flaeche form)

    let gesamtflaeche = formen |> List.sumBy flaeche
    printfn "%f" gesamtflaeche

    let kundennummer = Kundennummer 1001
    // Auspacken per Pattern Matching direkt im let-Binding
    let (Kundennummer wert) = kundennummer

    printfn "%d" wert
