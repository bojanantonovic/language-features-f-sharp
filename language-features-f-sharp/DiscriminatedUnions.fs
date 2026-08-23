module language_features_f_sharp.DiscriminatedUnions

// Discriminated union: a value is EXACTLY ONE of several, clearly named cases -
// F#'s central tool for data modeling, has no direct equivalent in C#
// (the closest there are closed class hierarchies or pattern matching on object).
type Shape =
    | Circle of radius: float
    | Rectangle of width: float * height: float
    | Triangle of baseLength: float * height: float

// match forces ALL cases to be handled - if one is forgotten, the compiler warns.
let area shape =
    match shape with
    | Circle radius -> System.Math.PI * radius * radius
    | Rectangle(width, height) -> width * height
    | Triangle(baseLength, height) -> 0.5 * baseLength * height

// Single-case union: a "more strongly typed" wrapper around a primitive value -
// a customer number can no longer be accidentally swapped for an arbitrary int.
type CustomerNumber = CustomerNumber of int

let show () =
    let shapes = [ Circle 2.0; Rectangle(3.0, 4.0); Triangle(6.0, 2.0) ]

    for shape in shapes do
        printfn "%f" (area shape)

    let totalArea = shapes |> List.sumBy area
    printfn "%f" totalArea

    let customerNumber = CustomerNumber 1001
    // Unpacking via pattern matching directly in the let binding
    let (CustomerNumber value) = customerNumber

    printfn "%d" value
