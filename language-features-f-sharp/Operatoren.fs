module Language_Features_F_Sharp.Operatoren

// struct mit ueberladenem Operator: "static member (+)" legt fest, was "+" fuer diesen eigenen Typ bedeutet.
[<Struct>]
type Vektor(x: int, y: int) =
    member this.X = x
    member this.Y = y

    // operator +: legt fest, wie zwei Vektor-Werte addiert werden
    static member (+)(a: Vektor, b: Vektor) = Vektor(a.X + b.X, a.Y + b.Y)

    override this.ToString() = $"({x}, {y})"

    // Structs/Records haben in F# bereits eine automatisch generierte, werte-basierte Gleichheit (=) -
    // ein eigener operator "=" wie in C# ist daher normalerweise unnoetig.

let zeigen () =
    let a = Vektor(1, 2)
    let b = Vektor(3, 4)

    // ruft den ueberladenen operator + auf, obwohl Vektor ein selbst geschriebener Typ ist
    let summe = a + b

    // die automatisch generierte, werte-basierte Gleichheit vergleicht X und Y, nicht die Referenz
    let sindGleich = a = b
    let sindUngleich = a <> b

    printfn "%O" summe
    printfn "%b" sindGleich
    printfn "%b" sindUngleich

    // Liste von Vektoren: List.reduce wendet wiederholt operator + an, um alle zu addieren
    let vektoren = [ Vektor(1, 1); Vektor(2, 3); Vektor(-1, 4) ]

    let gesamtsumme = vektoren |> List.reduce (+)

    printfn "%O" gesamtsumme
