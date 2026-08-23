module Language_Features_F_Sharp.FunktionaleKomposition

// Currying: eine Funktion mit mehreren Parametern ist in Wahrheit eine Kette einstelliger Funktionen,
// jede nimmt EINEN Parameter und liefert eine neue Funktion zurueck (Typ: int -> int -> int).
let addieren a b = a + b

// Partial Application: wird "addieren" nur mit einem Argument aufgerufen, entsteht eine neue,
// spezialisierte Funktion - hier eine, die immer 10 addiert.
let addiere10 = addieren 10

// Funktionskomposition (">>"): verkettet zwei Funktionen zu einer neuen,
// die Ausgabe der ersten wird automatisch zur Eingabe der zweiten.
let verdoppeln x = x * 2
let inkrementieren x = x + 1
let verdoppelnDannInkrementieren = verdoppeln >> inkrementieren

let zeigen () =
    let summe = addieren 3 4
    printfn "%d" summe

    let ergebnis = addiere10 5
    printfn "%d" ergebnis

    let komposition = verdoppelnDannInkrementieren 5
    printfn "%d" komposition

    // Pipe-Operator (|>): schreibt "f x" als "x |> f" - Daten fliessen von links nach rechts,
    // Aufrufketten lesen sich dadurch wie eine Pipeline (praegt den Stil von List.filter/List.map & Co.).
    let ergebnisPerPipe =
        5
        |> verdoppeln
        |> inkrementieren

    printfn "%d" ergebnisPerPipe

    // Lambdas und Funktionen hoeherer Ordnung: List.map nimmt eine Funktion als Argument entgegen
    let quadrateAllerZahlen = [ 1; 2; 3; 4 ] |> List.map (fun x -> x * x)

    printfn "%s" (String.concat ", " (quadrateAllerZahlen |> List.map string))
