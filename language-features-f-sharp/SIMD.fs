module language_features_f_sharp.SIMD

open System.Numerics

// Vector<'T> maps several values at once depending on the CPU (SSE, AVX, AVX-512);
// Vector<float32>.Count only reveals at runtime how many float32 values fit into one vector (e.g. 4, 8, or 16).
let private sumSimd (values: float32[]) =
    let width = Vector<float32>.Count
    let mutable accumulator = Vector<float32>.Zero
    let mutable i = 0

    // As long as a full SIMD block fits, "width" values are added in one operation instead of individually
    while i + width <= values.Length do
        let block = Vector<float32>(values, i)
        accumulator <- accumulator + block
        i <- i + width

    // Vector.Dot with a ones-vector adds up all components of the accumulator into a single scalar
    let mutable sum = Vector.Dot(accumulator, Vector<float32>.One)

    // remaining values that don't fill a full block anymore are added normally (scalar)
    while i < values.Length do
        sum <- sum + values.[i]
        i <- i + 1

    sum

let show () =
    printfn "Hardware acceleration active: %b" Vector.IsHardwareAccelerated
    printfn "Width of a float32 vector on this CPU: %d" Vector<float32>.Count

    let values = [| for n in 1 .. 17 -> float32 n |]
    let sum = sumSimd values

    printfn "%f" sum

    // Vector3 is a fixed 3-component vector (e.g. for 3D coordinates) with overloaded operators
    let a = Vector3(1.0f, 2.0f, 3.0f)
    let b = Vector3(4.0f, 5.0f, 6.0f)

    let vectorSum = a + b
    let dotProduct = Vector3.Dot(a, b)

    printfn "%O" vectorSum
    printfn "%f" dotProduct
