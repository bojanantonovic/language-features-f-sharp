module language_features_f_sharp.Conditions

let show () =
    let age = 30
    let isActive = true

    // if/else as an expression: yields a value directly, no pre-declared variable needed
    let status = if isActive then "active" else "inactive"

    // if/elif/else: check several cases in sequence ("elif" instead of "else if")
    let ageGroup =
        if age < 18 then "minor"
        elif age < 65 then "adult"
        else "senior"

    printfn "%s" status
    printfn "%s" ageGroup
