module language_features_f_sharp.Masseinheiten

// Units of Measure: haengt eine Masseinheit an einen Zahlentyp, rein zur Kompilierzeit geprueft -
// zur Laufzeit bleibt es ein normaler float, ohne jeden Overhead. Hat keine Entsprechung in C#.
[<Measure>] type m
[<Measure>] type s
[<Measure>] type km

let geschwindigkeit (strecke: float<m>) (zeit: float<s>) : float<m/s> = strecke / zeit

// Ein Umrechnungsfaktor zwischen zwei Einheiten ist selbst ein Wert mit einer "Verhaeltnis-Einheit"
let kmZuM (strecke: float<km>) : float<m> = strecke * 1000.0<m/km>

let zeigen () =
    let strecke = 100.0<m>
    let zeit = 9.58<s>

    let v = geschwindigkeit strecke zeit
    printfn "%f" v

    // Der Compiler verhindert das versehentliche Mischen inkompatibler Einheiten:
    // "strecke + zeit" waere hier ein Kompilierfehler, "strecke + 50.0<m>" dagegen gueltig.
    let laengereStrecke = strecke + 50.0<m>
    printfn "%f" laengereStrecke

    let marathon = 42.195<km>
    let marathonInMetern = kmZuM marathon
    printfn "%f" marathonInMetern
