module language_features_f_sharp.Inheritance

// Base class: "abstract member" declares a member, "default" provides the default implementation,
// which derived classes can override (this does NOT make the class itself abstract).
type Animal(name: string) =
    member this.Name = name
    abstract member MakeSound: unit -> string
    default this.MakeSound() = "..."

// Derived class: "inherit Animal(name)" calls the base class constructor directly.
type Dog(name: string) =
    inherit Animal(name)
    // override: replaces the base implementation of MakeSound
    override this.MakeSound() = "Woof"

type Cat(name: string) =
    inherit Animal(name)
    override this.MakeSound() = "Meow"

let show () =
    let dog = Dog("Rex")
    let cat = Cat("Minka")

    let dogSound = dog.MakeSound()
    let catSound = cat.MakeSound()

    printfn "%s" dog.Name
    printfn "%s" dogSound
    printfn "%s" cat.Name
    printfn "%s" catSound

    // List of the derived type Dog: List functions use the Name property inherited from Animal directly
    let dogs = [ Dog("Rex"); Dog("Bello"); Dog("Ari") ]

    let sortedDogNames =
        dogs
        |> List.sortBy (fun d -> d.Name)
        |> List.map (fun d -> d.Name)

    printfn "%s" (String.concat ", " sortedDogNames)
