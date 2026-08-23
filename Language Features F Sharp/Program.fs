module Language_Features_F_Sharp.Program

open Language_Features_F_Sharp

[<EntryPoint>]
let main _ =
    printfn "F# Sprach-Feature-Demo"
    printfn ""

    Variablen.zeigen ()
    Bedingungen.zeigen ()
    Schleifen.zeigen ()
    Methoden.zeigen ()
    Listen.zeigen ()
    Klassen.zeigen ()
    Konstruktor.zeigen ()
    Vererbung.zeigen ()
    Polymorphie.zeigen ()
    Interfaces.zeigen ()
    Generics.zeigen ()
    Woerterbuch.zeigen ()
    Strings.zeigen ()
    Exceptions.zeigen ()
    Asynchronitaet.zeigenAsync () |> Async.RunSynchronously
    Collections.zeigen ()
    Enums.zeigen ()
    Structs.zeigen ()
    Records.zeigen ()
    Delegates.zeigen ()
    Extensions.zeigen ()
    Attribut.zeigen ()
    Polymorphismus.zeigen ()
    Tupel.zeigen ()
    Nullbarkeit.zeigen ()
    MusterAbgleich.zeigen ()
    Indexer.zeigen ()
    Operatoren.zeigen ()
    Iteratoren.zeigen ()
    Parameter.zeigen ()
    Rekursion.zeigen ()
    Unveraenderlichkeit.zeigen ()
    FunktionaleKomposition.zeigen ()
    DiskriminierteVereinigungen.zeigen ()
    AktiveMuster.zeigen ()
    Masseinheiten.zeigen ()
    Berechnungsausdruecke.zeigen ()
    ObjektAusdruecke.zeigen ()

    0
