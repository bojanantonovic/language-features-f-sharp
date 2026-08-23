module language_features_f_sharp.Classes

// Custom type with mutable properties: "member val" creates an auto-implemented
// property with a getter AND setter (comparable to C#'s "{ get; set; }").
type Person() =
    member val Name = "" with get, set
    member val Age = 0 with get, set

let show () =
    // Create an object and set its properties one by one
    let alice = Person()
    alice.Name <- "Alice"
    alice.Age <- 30

    // Read properties, the result lands in its own binding
    let aliceName = alice.Name
    let aliceAge = alice.Age

    // Properties can also be set directly at creation via named arguments
    let bob = Person(Name = "Bob", Age = 25)

    printfn "%s" aliceName
    printfn "%d" aliceAge
    printfn "%s" bob.Name
    printfn "%d" bob.Age

    // List of objects: List functions work on custom types too, not just primitive types
    let carol = Person(Name = "Carol", Age = 40)
    let people = [ alice; bob; carol ]

    // List.maxBy: find the person with the highest age
    let oldest = people |> List.maxBy (fun person -> person.Age)

    // List.averageBy: average of a number derived from the objects
    let averageAge = people |> List.averageBy (fun person -> float person.Age)

    printfn "%s" oldest.Name
    printfn "%f" averageAge
