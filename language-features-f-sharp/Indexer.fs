module language_features_f_sharp.Indexer

open System.Collections.Generic

// Custom class with an indexer: "member this.Item" with get/set allows access with square brackets
// like an array, even though a Dictionary holds the data internally.
type WeeklySchedule() =
    let appointmentsByDay = Dictionary<string, string>()

    member this.Item
        with get (day: string) =
            match appointmentsByDay.TryGetValue(day) with
            | true, appointment -> appointment
            | false, _ -> "free"
        and set (day: string) (value: string) = appointmentsByDay.[day] <- value

let show () =
    let schedule = WeeklySchedule()

    // Assignment via the indexer, just like an array
    schedule.["Monday"] <- "Dentist"
    schedule.["Wednesday"] <- "Meeting"

    // Reading via the indexer
    let mondayAppointment = schedule.["Monday"]
    let tuesdayAppointment = schedule.["Tuesday"] // no entry present -> "free"

    printfn "%s" mondayAppointment
    printfn "%s" tuesdayAppointment

    // List of days: List.map uses the indexer to look up the appointment for each day
    let days = [ "Monday"; "Tuesday"; "Wednesday" ]

    let schedulePlan = days |> List.map (fun day -> $"{day}: {schedule.[day]}")

    printfn "%s" (String.concat " | " schedulePlan)
