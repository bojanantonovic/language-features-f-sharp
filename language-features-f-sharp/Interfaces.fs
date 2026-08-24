module language_features_f_sharp.Interfaces

// Interface: only specifies WHAT a type must be able to do, not HOW (no common base class needed).
// F# interfaces consist exclusively of abstract members.
type IVehicle =
    abstract member Move: unit -> string

// "interface ... with" implements the interface explicitly (like C#'s explicit interface implementation) -
// the method is therefore only visible through the interface type, not directly on Car.
type Car() =
    interface IVehicle with
        member this.Move() = "drives on the road"

type Bicycle() =
    interface IVehicle with
        member this.Move() = "drives on the bike path"

let show () =
    // List of the interface type: Car and Bicycle have no common base class,
    // but both fulfill the contract of IVehicle.
    let vehicles: IVehicle list = [ Car(); Bicycle() ]

    for vehicle in vehicles do
        let movement = vehicle.Move()
        printfn "%s" movement

    // List.map: turns each vehicle (via the interface) into its movement text
    let movements = vehicles |> List.map (fun vehicle -> vehicle.Move())

    // Type test operator (":?"): evaluates directly to a bool, filters just the cars out of the interface list
    let carCount =
        vehicles
        |> List.filter (fun vehicle -> vehicle :? Car)
        |> List.length

    printfn "%s" (String.concat " / " movements)
    printfn "%d" carCount
