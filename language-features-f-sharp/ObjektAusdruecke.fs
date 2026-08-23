module language_features_f_sharp.ObjektAusdruecke

open language_features_f_sharp.Interfaces

// Object Expression: implementiert ein Interface (oder eine abstrakte Klasse) direkt an Ort und Stelle,
// ganz ohne eigenen benannten Typ dafuer zu deklarieren. Hat keine direkte Entsprechung in C#
// (am naechsten kommt dort eine anonyme, lokal deklarierte Klasse).
let erzeugeFahrzeug bewegungstext =
    { new IFahrzeug with
        member this.Bewegen() = bewegungstext }

let zeigen () =
    let boot = erzeugeFahrzeug "faehrt auf dem Wasser"

    printfn "%s" (boot.Bewegen())

    // Auch direkt inline, ohne Hilfsfunktion, moeglich - nuetzlich fuer einmalige Interface-Implementierungen
    let flugzeug =
        { new IFahrzeug with
            member this.Bewegen() = "fliegt in der Luft" }

    printfn "%s" (flugzeug.Bewegen())

    let fahrzeuge = [ boot; flugzeug ]
    let bewegungen = fahrzeuge |> List.map (fun f -> f.Bewegen())

    printfn "%s" (String.concat " / " bewegungen)
