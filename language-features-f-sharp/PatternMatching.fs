module language_features_f_sharp.PatternMatching

open language_features_f_sharp.Inheritance
open language_features_f_sharp.Structs

let show () =
    let grade = 2

    // match expression with constant patterns (equivalent to a switch expression)
    let rating =
        match grade with
        | 1 -> "Excellent"
        | 2 -> "Good"
        | 3 -> "Satisfactory"
        | _ -> "Unknown"

    printfn "%s" rating

    // Type pattern (":?"): checks the type and binds it to a name at the same time (here: Dog dog)
    let animal: Animal = Dog("Rex")
    let description =
        match animal with
        | :? Dog as dog -> $"Dog named {dog.Name}"
        | :? Cat as cat -> $"Cat named {cat.Name}"
        | _ -> "Unknown animal"

    printfn "%s" description

    // Guard clause ("when"): F# has no relational patterns of its own like C#'s "> 30",
    // but "when" allows arbitrary conditions in the pattern instead.
    let point = Point(1, 2)
    let location =
        match point with
        | p when p.X = 0 && p.Y = 0 -> "Origin"
        | p when p.X > 0 && p.Y > 0 -> "First quadrant"
        | _ -> "Outside the first quadrant"

    printfn "%s" location

    // Combined type and guard pattern in one match
    let value: obj = 42
    match value with
    | :? int as number when number > 10 -> printfn "%d" number
    | _ -> ()

    // List of mixed animals: List.filter with the type test operator (":?") counts only the dogs
    let animals: Animal list = [ Dog("Rex") :> Animal; Cat("Minka") :> Animal; Dog("Bello") :> Animal ]
    let dogCount = animals |> List.filter (fun a -> a :? Dog) |> List.length

    printfn "%d" dogCount
