module language_features_f_sharp.Methods

// Function with one parameter and a return value: no "return" needed,
// the last expression in the function body is automatically the return value.
let square number = number * number

// Function with two parameters. Automatically "curried" (more on that in FunctionalComposition.fs),
// but can be called here just like an ordinary two-parameter method.
let add a b = a + b

// Function that uses a loop internally (makes the for loop from Loops.fs
// reusable for arbitrary upper bounds)
let sumUpTo limit =
    let mutable sum = 0
    for i in 1..limit do
        sum <- sum + i
    sum

let show () =
    let squared = square 6
    let sumOfTwoNumbers = add 3 4
    let sumUpTo10 = sumUpTo 10

    printfn "%d" squared
    printfn "%d" sumOfTwoNumbers
    printfn "%d" sumUpTo10
