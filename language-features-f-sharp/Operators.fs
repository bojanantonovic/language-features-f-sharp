module language_features_f_sharp.Operators

// struct with an overloaded operator: "static member (+)" defines what "+" means for this custom type.
[<Struct>]
type Vector(x: int, y: int) =
    member this.X = x
    member this.Y = y

    // operator +: defines how two Vector values are added
    static member (+)(a: Vector, b: Vector) = Vector(a.X + b.X, a.Y + b.Y)

    override this.ToString() = $"({x}, {y})"

    // Structs/records already have an automatically generated, value-based equality (=) in F# -
    // a custom operator "=" like in C# is therefore normally unnecessary.

let show () =
    let a = Vector(1, 2)
    let b = Vector(3, 4)

    // calls the overloaded operator +, even though Vector is a hand-written type
    let sum = a + b

    // the automatically generated, value-based equality compares X and Y, not the reference
    let areEqual = a = b
    let areNotEqual = a <> b

    printfn "%O" sum
    printfn "%b" areEqual
    printfn "%b" areNotEqual

    // List of vectors: List.reduce repeatedly applies operator + to add them all up
    let vectors = [ Vector(1, 1); Vector(2, 3); Vector(-1, 4) ]

    let total = vectors |> List.reduce (+)

    printfn "%O" total
