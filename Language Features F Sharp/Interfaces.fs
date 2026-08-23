module Language_Features_F_Sharp.Interfaces

// Interface: legt nur fest, WAS ein Typ koennen muss, nicht WIE (keine gemeinsame Basisklasse noetig).
// F#-Interfaces bestehen ausschliesslich aus abstrakten Mitgliedern.
type IFahrzeug =
    abstract member Bewegen: unit -> string

// "interface ... with" implementiert das Interface explizit (wie C#s explizite Interface-Implementierung) -
// die Methode ist daher nur ueber den Interface-Typ sichtbar, nicht direkt auf Auto.
type Auto() =
    interface IFahrzeug with
        member this.Bewegen() = "faehrt auf der Strasse"

type Fahrrad() =
    interface IFahrzeug with
        member this.Bewegen() = "faehrt auf dem Radweg"

let zeigen () =
    // Liste vom Interface-Typ: Auto und Fahrrad haben keine gemeinsame Basisklasse,
    // erfuellen aber beide den Vertrag von IFahrzeug.
    let fahrzeuge: IFahrzeug list = [ Auto(); Fahrrad() ]

    for fahrzeug in fahrzeuge do
        let bewegung = fahrzeug.Bewegen()
        printfn "%s" bewegung

    // List.map: wandelt jedes Fahrzeug (ueber das Interface) in seinen Bewegungstext um
    let bewegungen = fahrzeuge |> List.map (fun fahrzeug -> fahrzeug.Bewegen())

    // Type-Pattern ueber ":?": filtert aus der Interface-Liste gezielt nur die Autos heraus
    let anzahlAutos =
        fahrzeuge
        |> List.filter (function :? Auto -> true | _ -> false)
        |> List.length

    printfn "%s" (String.concat " / " bewegungen)
    printfn "%d" anzahlAutos
