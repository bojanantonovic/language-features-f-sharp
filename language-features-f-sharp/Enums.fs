module language_features_f_sharp.Enums

// Enum: named set of fixed int values. F# enums - unlike discriminated unions - are always
// bound to an underlying numeric type (see DiscriminatedUnions.fs for the F#-native alternative).
type Weekday =
    | Monday = 0
    | Tuesday = 1
    | Wednesday = 2
    | Thursday = 3
    | Friday = 4
    | Saturday = 5
    | Sunday = 6

let show () =
    let today = Weekday.Wednesday

    // match on an enum value: more compact than a switch statement, yields a value directly
    let kind =
        match today with
        | Weekday.Saturday | Weekday.Sunday -> "weekend"
        | _ -> "weekday"

    printfn "%A" today
    printfn "%s" kind

    // System.Enum.GetValues returns all values of the enum, Seq/List functions work on it just the same
    let allDays = System.Enum.GetValues<Weekday>() |> List.ofArray

    let weekendDays =
        allDays |> List.filter (fun day -> day = Weekday.Saturday || day = Weekday.Sunday)

    let weekdayCount =
        allDays
        |> List.filter (fun day -> day <> Weekday.Saturday && day <> Weekday.Sunday)
        |> List.length

    printfn "%s" (String.concat ", " (weekendDays |> List.map string))
    printfn "%d" weekdayCount
