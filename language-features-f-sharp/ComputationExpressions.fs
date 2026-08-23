module language_features_f_sharp.ComputationExpressions

// Computation expression: a custom "builder" type defines what "let!"/"return" mean inside
// a { } block - "async {}"/"seq {}" (see Asynchrony.fs, Iterators.fs) are themselves
// just built-in examples of this mechanism. Has no equivalent in C#.
type MaybeBuilder() =
    member _.Bind(value, next) =
        match value with
        | Some x -> next x
        | None -> None

    member _.Return(value) = Some value

let maybe = MaybeBuilder()

let divide numerator denominator =
    if denominator = 0 then None else Some(numerator / denominator)

// Without the computation expression, every intermediate step would have to be checked
// individually via match - "let!" automatically passes a "None" through as soon as any step fails.
let calculation a b c =
    maybe {
        let! firstStage = divide a b
        let! secondStage = divide firstStage c
        return secondStage
    }

let show () =
    let successful = calculation 100 5 2
    let failed = calculation 100 0 2

    printfn "%A" successful
    printfn "%A" failed
