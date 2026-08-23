module language_features_f_sharp.Variables

let show () =
    // Integer
    let age = 30

    // Floating-point number: F# infers the type from the literal (here: float)
    let height = 1.78

    // Text
    let name = "Alice"

    // Boolean: true or false
    let isActive = true

    // A calculation: the result lands in its own binding first,
    // not directly in the printfn line.
    let ageInTenYears = age + 10

    // let binds a name to a value and is immutable by default - unlike C#'s
    // "var", which only means type inference while the value stays mutable.
    // See Immutability.fs for "mutable" and shadowing.
    printfn "%s" name
    printfn "%d" age
    printfn "%f" height
    printfn "%b" isActive
    printfn "%d" ageInTenYears
