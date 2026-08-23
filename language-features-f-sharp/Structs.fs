module language_features_f_sharp.Structs

// [<Struct>]: value type - copied on assignment, unlike a class (reference type),
// where two bindings would share the same instance.
[<Struct>]
type Point(x: int, y: int) =
    member this.X = x
    member this.Y = y

let show () =
    let pointA = Point(1, 2)
    let pointB = pointA // creates a copy, not the same instance

    let pointAX = pointA.X
    let pointBX = pointB.X

    printfn "%d" pointAX
    printfn "%d" pointBX

    // List of structs: List.map computes a new value from each point,
    // the original list remains unchanged
    let points = [ Point(1, 2); Point(-3, 4); Point(5, -1) ]

    let distancesFromOrigin =
        points |> List.map (fun p -> sqrt (float (p.X * p.X + p.Y * p.Y)))

    // List.minBy: find the point with the smallest distance to the origin
    let closestPoint =
        points |> List.minBy (fun p -> sqrt (float (p.X * p.X + p.Y * p.Y)))

    printfn "%s" (String.concat ", " (distancesFromOrigin |> List.map string))
    printfn "%d, %d" closestPoint.X closestPoint.Y
