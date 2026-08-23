module language_features_f_sharp.Immutability

let show () =
    // let binds a name to a value immutably - reassigning it ("value <- 2")
    // would be a compile error here.
    let value = 1

    // Shadowing: "let value = ..." creates a NEW, independent binding with the same name,
    // which shadows the old one from here on - unlike an assignment, it does not change the old one.
    let value = value + 1

    printfn "%d" value

    // "mutable" makes a binding explicitly changeable - visible both at the declaration
    // and at every place where "<-" is used.
    let mutable counter = 0
    counter <- counter + 1
    counter <- counter + 1

    printfn "%d" counter

    // Records/lists are immutable by default: "changing" always creates a copy
    // with the changed field/element, the original remains intact (see Records.fs, Lists.fs).
    let list1 = [ 1; 2; 3 ]
    let list2 = 0 :: list1

    printfn "%d" (List.length list1)
    printfn "%d" (List.length list2)
