module language_features_f_sharp.Recursion

// "rec": makes a function callable within its own body (OFF by default in F#,
// unlike most other languages, where functions automatically know about themselves).
let rec factorial n =
    if n <= 1 then 1
    else n * factorial (n - 1)

// Tail recursion: the recursive call is the LAST action of the function,
// so the compiler turns it into a loop - no stack overflow, even for large n.
// The accumulator is int64, because the sum up to 1 million would overflow an int32.
let sumUpToTailRecursive upperBound =
    let rec loop n accumulator =
        if n > upperBound then accumulator
        else loop (n + 1L) (accumulator + n)
    loop 1L 0L

// Mutual recursion ("and"): two functions call each other,
// both have to be in the same "rec ... and ..." block for that.
let rec isEven n = if n = 0 then true else isOdd (n - 1)
and isOdd n = if n = 0 then false else isEven (n - 1)

let show () =
    let f5 = factorial 5
    printfn "%d" f5

    let sumUpTo1Million = sumUpToTailRecursive 1_000_000L
    printfn "%d" sumUpTo1Million

    printfn "%b" (isEven 10)
    printfn "%b" (isOdd 10)
