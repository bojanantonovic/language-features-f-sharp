module language_features_f_sharp.Collections

let show () =
    let numbers = [ 5; 12; 3; 8; 21; 4 ]

    // List.filter: filters elements that satisfy a condition
    let evenNumbers = numbers |> List.filter (fun number -> number % 2 = 0)

    // List.map: transforms each element
    let doubled = numbers |> List.map (fun number -> number * 2)

    // List.sort: sorts ascending, without mutating the original list
    let sorted = numbers |> List.sort

    // Aggregate functions return a single value directly instead of a list
    let sum = numbers |> List.sum
    let average = numbers |> List.averageBy float
    let largestNumber = numbers |> List.max

    printfn "%s" (String.concat ", " (evenNumbers |> List.map string))
    printfn "%s" (String.concat ", " (doubled |> List.map string))
    printfn "%s" (String.concat ", " (sorted |> List.map string))
    printfn "%d" sum
    printfn "%f" average
    printfn "%d" largestNumber

    // Map: F#'s immutable alternative to Dictionary (see Dictionary.fs), internally a balanced tree
    let ageByName = Map.ofList [ "Alice", 30; "Bob", 25 ]
    let ageByNameExtended = ageByName |> Map.add "Carol" 40

    // Set: immutable collection without duplicates
    let uniqueNumbers = Set.ofList [ 1; 2; 2; 3; 3; 3 ]

    printfn "%d" ageByNameExtended.["Carol"]
    printfn "%d" uniqueNumbers.Count
