module Language_Features_F_Sharp.AktiveMuster

// Active Pattern: verpackt beliebige Erkennungslogik in ein eigenes, wiederverwendbares Pattern -
// "Gerade"/"Ungerade" lassen sich danach genauso in einem match verwenden wie eingebaute Patterns.
// Hat keine Entsprechung in C#.
let (|Gerade|Ungerade|) n =
    if n % 2 = 0 then Gerade else Ungerade

let beschreibeZahl n =
    match n with
    | Gerade -> $"{n} ist gerade"
    | Ungerade -> $"{n} ist ungerade"

// Partielles Active Pattern (Rueckgabe als option): passt nur, wenn die Erkennung tatsaechlich zutrifft,
// andernfalls faellt der match auf den naechsten Fall durch.
let (|PositiveZahl|_|) n = if n > 0 then Some n else None

let beschreibeVorzeichen n =
    match n with
    | PositiveZahl wert -> $"positiv: {wert}"
    | 0 -> "null"
    | _ -> "negativ"

// Parametrisiertes Active Pattern: nimmt zusaetzlich zum gematchten Wert weitere Argumente entgegen
let (|VielfachesVon|_|) teiler wert =
    if wert % teiler = 0 then Some() else None

let zeigen () =
    printfn "%s" (beschreibeZahl 4)
    printfn "%s" (beschreibeZahl 7)

    printfn "%s" (beschreibeVorzeichen 5)
    printfn "%s" (beschreibeVorzeichen -5)
    printfn "%s" (beschreibeVorzeichen 0)

    let zahlen = [ 3; 5; 9; 10; 14; 15 ]
    let vielfacheVon5 =
        zahlen |> List.filter (function VielfachesVon 5 -> true | _ -> false)

    printfn "%s" (String.concat ", " (vielfacheVon5 |> List.map string))
