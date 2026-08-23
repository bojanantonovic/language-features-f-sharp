module language_features_f_sharp.ActivePatterns

// Active pattern: wraps arbitrary recognition logic in its own, reusable pattern -
// "Even"/"Odd" can then be used in a match just like a built-in pattern.
// Has no equivalent in C#.
let (|Even|Odd|) n =
    if n % 2 = 0 then Even else Odd

let describeNumber n =
    match n with
    | Even -> $"{n} is even"
    | Odd -> $"{n} is odd"

// Partial active pattern (returns an option): only matches when the recognition actually applies,
// otherwise the match falls through to the next case.
let (|PositiveNumber|_|) n = if n > 0 then Some n else None

let describeSign n =
    match n with
    | PositiveNumber value -> $"positive: {value}"
    | 0 -> "zero"
    | _ -> "negative"

// Parameterized active pattern: takes additional arguments besides the matched value
let (|MultipleOf|_|) divisor value =
    if value % divisor = 0 then Some() else None

let show () =
    printfn "%s" (describeNumber 4)
    printfn "%s" (describeNumber 7)

    printfn "%s" (describeSign 5)
    printfn "%s" (describeSign -5)
    printfn "%s" (describeSign 0)

    let numbers = [ 3; 5; 9; 10; 14; 15 ]
    let multiplesOf5 =
        numbers |> List.filter (function MultipleOf 5 -> true | _ -> false)

    printfn "%s" (String.concat ", " (multiplesOf5 |> List.map string))
