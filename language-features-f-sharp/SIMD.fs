module language_features_f_sharp.SIMD

open System.Numerics

// Vector<'T> bildet je nach CPU (SSE, AVX, AVX-512) mehrere Werte gleichzeitig ab;
// Vector<float32>.Count verraet erst zur Laufzeit, wie viele float32-Werte in einen Vektor passen (z. B. 4, 8 oder 16).
let private summeSimd (werte: float32[]) =
    let breite = Vector<float32>.Count
    let mutable akkumulator = Vector<float32>.Zero
    let mutable i = 0

    // Solange ein voller SIMD-Block passt, werden "breite" Werte in einer Operation addiert statt einzeln
    while i + breite <= werte.Length do
        let block = Vector<float32>(werte, i)
        akkumulator <- akkumulator + block
        i <- i + breite

    // Vector.Dot mit einem Einsvektor addiert alle Komponenten des Akkumulators zu einem einzelnen Skalar
    let mutable summe = Vector.Dot(akkumulator, Vector<float32>.One)

    // uebrig gebliebene Werte, die keinen vollen Block mehr fuellen, werden normal (skalar) addiert
    while i < werte.Length do
        summe <- summe + werte.[i]
        i <- i + 1

    summe

let zeigen () =
    printfn "Hardware-Beschleunigung aktiv: %b" Vector.IsHardwareAccelerated
    printfn "Breite eines float32-Vektors auf dieser CPU: %d" Vector<float32>.Count

    let werte = [| for n in 1 .. 17 -> float32 n |]
    let summe = summeSimd werte

    printfn "%f" summe

    // Vector3 ist ein fester 3-Komponenten-Vektor (z. B. fuer 3D-Koordinaten) mit ueberladenen Operatoren
    let a = Vector3(1.0f, 2.0f, 3.0f)
    let b = Vector3(4.0f, 5.0f, 6.0f)

    let summeVektoren = a + b
    let skalarprodukt = Vector3.Dot(a, b)

    printfn "%O" summeVektoren
    printfn "%f" skalarprodukt
