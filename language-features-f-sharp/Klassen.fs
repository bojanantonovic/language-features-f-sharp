module Language_Features_F_Sharp.Klassen

// Eigener Typ mit veraenderlichen Eigenschaften: "member val" erzeugt eine auto-implementierte
// Property mit Getter UND Setter (vergleichbar mit C#s "{ get; set; }").
type Person() =
    member val Name = "" with get, set
    member val Alter = 0 with get, set

let zeigen () =
    // Objekt erzeugen und Eigenschaften einzeln setzen
    let alice = Person()
    alice.Name <- "Alice"
    alice.Alter <- 30

    // Eigenschaften auslesen, Ergebnis landet in einem eigenen Binding
    let aliceName = alice.Name
    let aliceAlter = alice.Alter

    // Eigenschaften lassen sich auch direkt beim Erzeugen ueber benannte Argumente setzen
    let bob = Person(Name = "Bob", Alter = 25)

    printfn "%s" aliceName
    printfn "%d" aliceAlter
    printfn "%s" bob.Name
    printfn "%d" bob.Alter

    // Liste von Objekten: List-Funktionen wirken auch auf eigene Typen, nicht nur auf primitiven Typen
    let carol = Person(Name = "Carol", Alter = 40)
    let personen = [ alice; bob; carol ]

    // List.maxBy: die Person mit dem hoechsten Alter finden
    let aelteste = personen |> List.maxBy (fun person -> person.Alter)

    // List.averageBy: Durchschnitt aus einer aus den Objekten abgeleiteten Zahl
    let durchschnittsalter = personen |> List.averageBy (fun person -> float person.Alter)

    printfn "%s" aelteste.Name
    printfn "%f" durchschnittsalter
