namespace Language_Features_F_Sharp.Features

open System

type Mitarbeiter =
    { Name: string; Abteilung: string; Gehalt: decimal }
    override this.ToString() = $"Mitarbeiter {{ Name = {this.Name}, Abteilung = {this.Abteilung}, Gehalt = {this.Gehalt} }}"

module F09_Linq =

    let private belegschaft =
        [| { Name = "Alice"; Abteilung = "IT"; Gehalt = 9500m }
           { Name = "Bob"; Abteilung = "IT"; Gehalt = 8200m }
           { Name = "Carol"; Abteilung = "Verkauf"; Gehalt = 7100m }
           { Name = "Dave"; Abteilung = "Verkauf"; Gehalt = 7800m }
           { Name = "Eve"; Abteilung = "HR"; Gehalt = 6600m } |]

    let run () =
        // Method-Syntax -> das Seq-Modul (LINQ-Aequivalent): Where/Select/OrderBy als Pipeline
        // ueber den "|>"-Operator statt Punktnotation.
        let itMitarbeiterNamen =
            belegschaft
            |> Seq.filter (fun m -> m.Abteilung = "IT")
            |> Seq.sortByDescending (fun m -> m.Gehalt)
            |> Seq.map (fun m -> m.Name)
        Demo.print "Seq-Pipeline (filter/sortByDescending/map)" (String.Join(",", itMitarbeiterNamen))

        // Query-Syntax: F# bietet mit dem "query {}"-Computation-Expression-Block ein direktes
        // Pendant zu C#s from/where/orderby/select.
        let verkaufNamen =
            query {
                for m in belegschaft do
                    where (m.Abteilung = "Verkauf")
                    sortBy m.Gehalt
                    select m.Name
            }
        Demo.print "query {} (Aequivalent zu from/where/orderby/select)" (String.Join(",", verkaufNamen))

        // Deferred Execution: seq<'T>-Pipelines werden - genau wie IEnumerable<T> in C# - erst bei
        // der Iteration ausgewertet.
        let query1 = belegschaft |> Seq.filter (fun m -> m.Gehalt > 7000m)
        Demo.print "Deferred Execution vor Materialisierung" "Sequenz noch nicht ausgewertet"
        let materialisiert = query1 |> Seq.toList
        Demo.print "Deferred Execution nach Seq.toList" materialisiert.Length

        // Seq.groupBy.
        let proAbteilung = belegschaft |> Seq.groupBy (fun m -> m.Abteilung)
        for (abteilung, gruppe) in proAbteilung do
            let mitarbeiterNamen = gruppe |> Seq.map (fun m -> m.Name)
            Demo.print $"Seq.groupBy ({abteilung})" (String.Join(",", mitarbeiterNamen))

        // Aggregation: Sum, Average, Max, Min, Length/Count, fold (Aequivalent zu Aggregate).
        Demo.print "Seq.sumBy Gehalt" (belegschaft |> Seq.sumBy (fun m -> m.Gehalt))
        Demo.print "Seq.averageBy Gehalt" (belegschaft |> Seq.averageBy (fun m -> m.Gehalt))
        let maxGehalt = belegschaft |> Seq.map (fun m -> m.Gehalt) |> Seq.max
        let minGehalt = belegschaft |> Seq.map (fun m -> m.Gehalt) |> Seq.min
        Demo.print "Seq.max / Seq.min Gehalt" $"{maxGehalt} / {minGehalt}"
        let gesamtsummeUeberFold = belegschaft |> Seq.fold (fun summe m -> summe + m.Gehalt) 0m
        Demo.print "Seq.fold (Aequivalent zu Aggregate)" gesamtsummeUeberFold

        // Any / All / First(OrDefault) -> Seq.exists / Seq.forall / Seq.tryFind.
        Demo.print "Seq.exists (Gehalt > 9000)" (belegschaft |> Seq.exists (fun m -> m.Gehalt > 9000m))
        Demo.print "Seq.forall (Gehalt > 1000)" (belegschaft |> Seq.forall (fun m -> m.Gehalt > 1000m))
        let hrMitarbeiter = belegschaft |> Seq.tryFind (fun m -> m.Abteilung = "HR")
        Demo.print "Seq.tryFind (Aequivalent zu FirstOrDefault)" (hrMitarbeiter |> Option.map box |> Option.defaultValue null)

        // Seq.map2/join-aehnliche Verknuepfung zweier Sequenzen ueber einen gemeinsamen Schluessel.
        let abteilungsKuerzel =
            [ "IT", "IT"
              "Verkauf", "VK"
              "HR", "PE" ]
            |> Map.ofList
        let mitKuerzel =
            belegschaft
            |> Seq.map (fun m -> $"{m.Name} ({abteilungsKuerzel.[m.Abteilung]})")
        Demo.print "Join zweier Sequenzen (Map-Lookup)" (String.Join(", ", mitKuerzel))

        // Seq.toDictionary-Aequivalent: dict/Dictionary aus einer Sequenz.
        let nachName = belegschaft |> Seq.map (fun m -> m.Name, m) |> dict
        Demo.print "dict (Aequivalent zu ToDictionary)" (nachName.["Bob"].Gehalt)

        // Seq.chunkBySize (Aequivalent zu Chunk).
        let bloecke = Seq.init 7 (fun i -> i + 1) |> Seq.chunkBySize 3
        let bloeckeText = bloecke |> Seq.map (fun b -> String.Join(",", b))
        Demo.print "Seq.chunkBySize(3) (Aequivalent zu Chunk)" (String.Join(" | ", bloeckeText))

        // Seq.map2 (Aequivalent zu Zip).
        let namenSeq = belegschaft |> Seq.map (fun m -> m.Name)
        let gehaelterSeq = belegschaft |> Seq.map (fun m -> m.Gehalt)
        let zipResultat = Seq.map2 (fun n g -> $"{n}:{g}") namenSeq gehaelterSeq
        Demo.print "Seq.map2 (Aequivalent zu Zip)" (String.Join(", ", zipResultat))
