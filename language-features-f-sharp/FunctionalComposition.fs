module language_features_f_sharp.FunctionalComposition

// Currying: a function with several parameters is really a chain of single-argument functions,
// each takes ONE parameter and returns a new function (type: int -> int -> int).
let add a b = a + b

// Partial application: calling "add" with only one argument creates a new,
// specialized function - here one that always adds 10.
let add10 = add 10

// Function composition (">>"): chains two functions into a new one,
// the output of the first automatically becomes the input of the second.
let double x = x * 2
let increment x = x + 1
let doubleThenIncrement = double >> increment

let show () =
    let sum = add 3 4
    printfn "%d" sum

    let result = add10 5
    printfn "%d" result

    let composition = doubleThenIncrement 5
    printfn "%d" composition

    // Pipe operator (|>): writes "f x" as "x |> f" - data flows from left to right,
    // making call chains read like a pipeline (shapes the style of List.filter/List.map & co.).
    let resultViaPipe =
        5
        |> double
        |> increment

    printfn "%d" resultViaPipe

    // Lambdas and higher-order functions: List.map takes a function as an argument
    let squaresOfAllNumbers = [ 1; 2; 3; 4 ] |> List.map (fun x -> x * x)

    printfn "%s" (String.concat ", " (squaresOfAllNumbers |> List.map string))
