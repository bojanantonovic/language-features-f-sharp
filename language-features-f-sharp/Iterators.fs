module language_features_f_sharp.Iterators

// seq { yield ... } produces the values only one at a time, as they are actually requested -
// the function itself does not run through to completion immediately (equivalent to C#'s "yield return").
let evenNumbersUpTo upperBound =
    seq {
        for number in 0..upperBound do
            if number % 2 = 0 then
                yield number
    }

// Custom iterator over a list: returns the elements in reverse order
let reversed (values: string list) =
    seq {
        for index in (values.Length - 1) .. -1 .. 0 do
            yield values.[index]
    }

let show () =
    for number in evenNumbersUpTo 10 do
        printfn "%d" number

    // The result of an iterator can be processed like any other seq<'T> with List/Seq functions
    let evenNumbers = evenNumbersUpTo 10 |> List.ofSeq
    let sum = evenNumbers |> List.sum

    printfn "%d" sum

    let names = [ "Alice"; "Bob"; "Carol" ]

    for name in reversed names do
        printfn "%s" name
