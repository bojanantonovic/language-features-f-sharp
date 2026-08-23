module language_features_f_sharp.Parameters

open language_features_f_sharp.Operators

// byref<...>/outref<...>: F#'s equivalent of C#'s "ref"/"out" - explicit reference parameters, also
// possible on an ordinary "let" function (unlike the optional parameters further below).
let tryDivide numerator denominator (result: outref<int>) =
    if denominator = 0 then
        result <- 0
        false
    else
        result <- numerator / denominator
        true

let doubleValue (value: byref<int>) = value <- value * 2

// inref<...>: the function only gets read access to the value, avoiding a copy
let length (vector: inref<Vector>) = sqrt (float (vector.X * vector.X + vector.Y * vector.Y))

// Optional parameters ("?param") and ParamArray are only allowed on members in F#, so they
// live in a small type here instead of an ordinary "let" function.
type Calculation =
    // Optional parameter: discount does not have to be given at the call site, "defaultArg" then returns the default value
    static member CalculatePrice(price: float, ?discount: float) =
        let d = defaultArg discount 0.0
        price - (price * d)

    // ParamArray: any number of arguments are accepted as an array (equivalent to C#'s "params")
    static member Sum([<System.ParamArray>] numbers: int[]) = Array.sum numbers

let show () =
    // out: the function MUST assign a value to this parameter, intended for extra return values
    let mutable result = 0
    let success = tryDivide 10 2 &result

    printfn "%b" success
    printfn "%d" result

    // ref/byref: the function can read AND change the existing value of the variable
    let mutable number = 5
    doubleValue &number

    printfn "%d" number

    // in/inref: the function only gets read access to the value
    let a = Vector(3, 4)
    let l = length &a

    printfn "%f" l

    // Optional parameter: discount does not have to be given
    let priceWithoutDiscount = Calculation.CalculatePrice(100.0)
    let priceWithDiscount = Calculation.CalculatePrice(100.0, 0.1)

    printfn "%f" priceWithoutDiscount
    printfn "%f" priceWithDiscount

    // ParamArray: any number of arguments are accepted by the method as an array
    let sum = Calculation.Sum(1, 2, 3, 4, 5)

    printfn "%d" sum
