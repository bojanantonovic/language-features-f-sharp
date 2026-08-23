module language_features_f_sharp.Polymorphism

open language_features_f_sharp.Inheritance

let show () =
    // List of the base type Animal, but containing objects of different derived types.
    // ":> Animal" (upcast) makes the common list type explicit.
    let animals: Animal list = [ Dog("Rex") :> Animal; Cat("Minka") :> Animal; Dog("Bello") :> Animal ]

    // Polymorphism: the same call to MakeSound() yields a different result depending on the
    // actual type - the compiler only knows Animal here, but at runtime the matching
    // override method of Dog or Cat is executed.
    for animal in animals do
        let sound = animal.MakeSound()
        printfn "%s" animal.Name
        printfn "%s" sound
