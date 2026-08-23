module language_features_f_sharp.Tuples

// Tuple as a function's return value: return several values together,
// without needing a dedicated record for it.
let minMax (numbers: int list) = (List.min numbers, List.max numbers)

let show () =
    // Tuple: bundle several values into a single value
    let person = ("Alice", 30)

    printfn "%s" (fst person)
    printfn "%d" (snd person)

    // Deconstruction: unpack the elements of a tuple directly into their own names when binding
    let firstName, years = person

    printfn "%s" firstName
    printfn "%d" years

    // Tuple as a function's return value, unpacked directly
    let minimum, maximum = minMax [ 5; 12; 3; 8; 21; 4 ]

    printfn "%d" minimum
    printfn "%d" maximum

    // List of tuples: List functions work just like on any other type
    let people = [ "Alice", 30; "Bob", 25; "Carol", 40 ]

    let namesOver28 =
        people
        |> List.filter (fun (_, age) -> age > 28)
        |> List.map fst

    printfn "%s" (String.concat ", " namesOver28)
