module language_features_f_sharp.UnitsOfMeasure

// Units of measure: attaches a unit to a numeric type, checked purely at compile time -
// at runtime it remains a normal float, with no overhead at all. Has no equivalent in C#.
[<Measure>] type m
[<Measure>] type s
[<Measure>] type km

let speed (distance: float<m>) (time: float<s>) : float<m/s> = distance / time

// A conversion factor between two units is itself a value with a "ratio unit"
let kmToM (distance: float<km>) : float<m> = distance * 1000.0<m/km>

let show () =
    let distance = 100.0<m>
    let time = 9.58<s>

    let v = speed distance time
    printfn "%f" v

    // The compiler prevents accidentally mixing incompatible units:
    // "distance + time" would be a compile error here, "distance + 50.0<m>" is valid though.
    let longerDistance = distance + 50.0<m>
    printfn "%f" longerDistance

    let marathon = 42.195<km>
    let marathonInMeters = kmToM marathon
    printfn "%f" marathonInMeters
