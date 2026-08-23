module language_features_f_sharp.Generics

// Generic function: 'T is a placeholder for any type. F# automatically infers a
// "comparison" constraint for 'T from the use of ">" (no "where" needed).
let greater (a: 'T) (b: 'T) : 'T =
    if a > b then a else b

let show () =
    // The same function "greater" works for int and string,
    // without having to be rewritten for every type.
    let greaterNumber = greater 5 12
    let greaterName = greater "Anna" "Bob"

    printfn "%d" greaterNumber
    printfn "%s" greaterName
