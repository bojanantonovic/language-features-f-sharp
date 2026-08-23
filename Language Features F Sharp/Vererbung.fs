module Language_Features_F_Sharp.Vererbung

// Basisklasse: "abstract member" deklariert ein Mitglied, "default" liefert die Standardimplementierung,
// die abgeleitete Klassen ueberschreiben koennen (das macht die Klasse hier NICHT abstrakt).
type Tier(name: string) =
    member this.Name = name
    abstract member MachGeraeusch: unit -> string
    default this.MachGeraeusch() = "..."

// Abgeleitete Klasse: "inherit Tier(name)" ruft direkt den Konstruktor der Basisklasse auf.
type Hund(name: string) =
    inherit Tier(name)
    // override: ersetzt die Basisimplementierung von MachGeraeusch
    override this.MachGeraeusch() = "Wuff"

type Katze(name: string) =
    inherit Tier(name)
    override this.MachGeraeusch() = "Miau"

let zeigen () =
    let hund = Hund("Rex")
    let katze = Katze("Minka")

    let hundGeraeusch = hund.MachGeraeusch()
    let katzeGeraeusch = katze.MachGeraeusch()

    printfn "%s" hund.Name
    printfn "%s" hundGeraeusch
    printfn "%s" katze.Name
    printfn "%s" katzeGeraeusch

    // Liste vom abgeleiteten Typ Hund: List-Funktionen nutzen die von Tier geerbte Eigenschaft Name direkt
    let hunde = [ Hund("Rex"); Hund("Bello"); Hund("Ari") ]

    let hundeNamenSortiert =
        hunde
        |> List.sortBy (fun h -> h.Name)
        |> List.map (fun h -> h.Name)

    printfn "%s" (String.concat ", " hundeNamenSortiert)
