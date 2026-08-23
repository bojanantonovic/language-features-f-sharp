module language_features_f_sharp.Dictionary

open System.Collections.Generic

let show () =
    // Dictionary (from the .NET BCL, mutable): stores values under a unique key.
    // F#'s own immutable alternative "Map" is shown in Collections.fs.
    let ageByName = Dictionary<string, int>()
    ageByName.["Alice"] <- 30
    ageByName.["Bob"] <- 25
    ageByName.Add("Carol", 40)

    // Access via the key
    let aliceAge = ageByName.["Alice"]

    // Safe access: TryGetValue returns (bool * int) instead of throwing an exception
    // for a missing key - in F# directly unpackable as a tuple, with no "out" needed.
    let found, daveAge = ageByName.TryGetValue("Dave")

    let entryCount = ageByName.Count

    printfn "%d" aliceAge
    printfn "%b" found
    printfn "%d" daveAge
    printfn "%d" entryCount

    for entry in ageByName do
        printfn "%s" entry.Key
        printfn "%d" entry.Value

    // Seq pipeline on a Dictionary: iterates over KeyValuePair<string, int> entries
    let namesOver28 =
        ageByName
        |> Seq.filter (fun entry -> entry.Value > 28)
        |> Seq.map (fun entry -> entry.Key)
        |> List.ofSeq

    // Seq.maxBy: find the name with the highest age
    let oldestName =
        ageByName
        |> Seq.maxBy (fun entry -> entry.Value)
        |> fun entry -> entry.Key

    printfn "%s" (String.concat ", " namesOver28)
    printfn "%s" oldestName
