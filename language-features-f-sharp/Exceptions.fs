module language_features_f_sharp.Exceptions

// Custom exception type: "exception" defines an F#-native exception type
// (equivalent to a class derived from Exception in C#) including a payload (string).
exception InvalidAgeException of string

// throws the custom exception when the given value is not domain-valid
let checkAge age =
    if age < 0 then
        raise (InvalidAgeException $"Age must not be negative: {age}")

let safeDivide numerator denominator =
    try
        try
            let result = numerator / denominator
            result
        with :? System.DivideByZeroException ->
            // "with" catches the exception instead of letting the program crash
            printfn "Error: division by 0 is not allowed."
            0
    finally
        // finally always runs, whether or not an exception occurred
        printfn "safeDivide was called."

let show () =
    let resultOk = safeDivide 10 2
    let resultError = safeDivide 10 0

    printfn "%d" resultOk
    printfn "%d" resultError

    try
        checkAge -5
    with InvalidAgeException message ->
        // catches only our own exception specifically, not just any exception
        printfn "%s" message
