module language_features_f_sharp.Nullability

let show () =
    // option: F#'s own, type-safe replacement for "null" - a value is either "Some x" or "None",
    // the compiler forces both cases to be handled (equivalent to C#'s nullable reference types).
    let age: int option = None

    // match instead of HasValue/Value: the compiler checks that both cases are really covered
    let hasValue =
        match age with
        | Some _ -> true
        | None -> false

    printfn "%b" hasValue

    // Option.defaultValue: returns the fallback value if the option is None (equivalent to ??)
    let ageOrDefault = age |> Option.defaultValue 18

    printfn "%d" ageOrDefault

    // Option.map: applies a function only if a value is present (equivalent to ?.)
    let name: string option = None
    let nameLength = name |> Option.map (fun n -> n.Length)

    printfn "%A" nameLength

    let filledName = Some "Alice"
    let nameLength2 = filledName |> Option.map (fun n -> n.Length)

    printfn "%A" nameLength2

    // List with possible gaps: List.choose keeps only the present values (combines filter+map)
    let numbersWithGaps = [ Some 5; None; Some 12; None; Some 8 ]

    let presentNumbers = numbersWithGaps |> List.choose id

    printfn "%s" (String.concat ", " (presentNumbers |> List.map string))
