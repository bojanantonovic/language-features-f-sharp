module language_features_f_sharp.PolymorphismViaFunctions

open language_features_f_sharp.Inheritance

let show () =
    // Function value pointing at the (polymorphic) MakeSound method
    let soundOf (animal: Animal) = animal.MakeSound()

    let dog = Dog("Rex") :> Animal
    let cat = Cat("Minka") :> Animal

    // The same function value calls a different implementation depending on the actual type -
    // the polymorphism from Inheritance.fs/Polymorphism.fs works just the same through a function value.
    let dogSound = soundOf dog
    let catSound = soundOf cat

    printfn "%s" dogSound
    printfn "%s" catSound

    // Function values can also carry different behavior directly,
    // with no inheritance at all - a second, independent form of "interchangeable behavior".
    let louderVersion (animal: Animal) = animal.MakeSound().ToUpper() + "!!!"
    let louderDog = louderVersion dog

    printfn "%s" louderDog
