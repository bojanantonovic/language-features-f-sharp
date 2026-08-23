module language_features_f_sharp.Loops

let show () =
    // for loop over a range: sum of the numbers 1 to 5.
    // The counter "i" is immutable, so "sum" has to be explicitly mutable.
    let mutable sum = 0
    for i in 1..5 do
        sum <- sum + i

    // while loop: doubling until a threshold is reached
    let mutable value = 1
    while value < 100 do
        value <- value * 2

    // for..in loop: iterate over an array of numbers
    let numbers = [| 3; 7; 2; 9; 4 |]
    let mutable largestNumber = numbers.[0]
    for number in numbers do
        if number > largestNumber then
            largestNumber <- number

    printfn "%d" sum
    printfn "%d" value
    printfn "%d" largestNumber
