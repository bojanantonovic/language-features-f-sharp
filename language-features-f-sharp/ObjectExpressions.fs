module language_features_f_sharp.ObjectExpressions

open language_features_f_sharp.Interfaces

// Object expression: implements an interface (or an abstract class) right on the spot,
// without declaring a dedicated named type for it. Has no direct equivalent in C#
// (the closest there is an anonymous, locally declared class).
let createVehicle movementText =
    { new IVehicle with
        member this.Move() = movementText }

let show () =
    let boat = createVehicle "drives on the water"

    printfn "%s" (boat.Move())

    // Also possible directly inline, without a helper function - useful for one-off interface implementations
    let plane =
        { new IVehicle with
            member this.Move() = "flies in the air" }

    printfn "%s" (plane.Move())

    let vehicles = [ boat; plane ]
    let movements = vehicles |> List.map (fun v -> v.Move())

    printfn "%s" (String.concat " / " movements)
