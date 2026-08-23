module language_features_f_sharp.Lists

let show () =
    // Array: fixed size, populated directly at declaration (comparable to C#'s string[])
    let colors = [| "Red"; "Green"; "Blue" |]
    let firstColor = colors.[0]
    let colorCount = colors.Length

    // F# list: immutable linked list. "Removing" creates a NEW list,
    // the original stays intact - unlike C#'s mutable List<T>
    // (for a mutable .NET list, see ResizeArray in Collections.fs).
    let shoppingList = [ "Milk"; "Bread"; "Butter" ]
    let withoutBread = shoppingList |> List.filter (fun item -> item <> "Bread")
    let shoppingListCount = withoutBread.Length

    printfn "%s" firstColor
    printfn "%d" colorCount
    printfn "%d" shoppingListCount

    for item in withoutBread do
        printfn "%s" item
