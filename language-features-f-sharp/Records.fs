module language_features_f_sharp.Records

// record: F#'s native, compact syntax for an immutable data type with automatic
// structural equality (C#'s "record" was inspired by this F# concept).
type Address = { Street: string; City: string }

let show () =
    let address1 = { Street = "Bahnhofstrasse 1"; City = "Zurich" }
    let address2 = { Street = "Bahnhofstrasse 1"; City = "Zurich" }

    // records compare their values, not the reference (unlike a regular class)
    let areEqual = address1 = address2

    // "with" expression: creates a new copy with one field changed,
    // the original record remains unchanged
    let address3 = { address1 with City = "Bern" }

    printfn "%b" areEqual
    printfn "%s" address1.City
    printfn "%s" address3.City
    printfn "%A" address1

    // List of records: List functions work on them just like on int in Collections.fs
    let addresses =
        [ { Street = "Bahnhofstrasse 1"; City = "Zurich" }
          { Street = "Marktgasse 5"; City = "Bern" }
          { Street = "Seestrasse 12"; City = "Zurich" } ]

    // filter + map: filter first, then pull just the street out of the record
    let zurichStreets =
        addresses
        |> List.filter (fun address -> address.City = "Zurich")
        |> List.map (fun address -> address.Street)

    // groupBy: groups the addresses by a field of the record
    let addressesByCity = addresses |> List.groupBy (fun address -> address.City)

    printfn "%s" (String.concat ", " zurichStreets)

    for city, group in addressesByCity do
        printfn "%s: %d" city (List.length group)
