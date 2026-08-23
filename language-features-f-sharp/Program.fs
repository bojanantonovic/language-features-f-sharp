module language_features_f_sharp.Program

open language_features_f_sharp

[<EntryPoint>]
let main _ =
    printfn "F# Language Feature Demo"
    printfn ""

    Variables.show ()
    Conditions.show ()
    Loops.show ()
    Methods.show ()
    Lists.show ()
    Classes.show ()
    Constructors.show ()
    Inheritance.show ()
    Polymorphism.show ()
    Interfaces.show ()
    Generics.show ()
    Dictionary.show ()
    Strings.show ()
    Exceptions.show ()
    Asynchrony.showAsync () |> Async.RunSynchronously
    Collections.show ()
    Enums.show ()
    Structs.show ()
    Records.show ()
    Delegates.show ()
    Extensions.show ()
    Attributes.show ()
    PolymorphismViaFunctions.show ()
    Tuples.show ()
    Nullability.show ()
    PatternMatching.show ()
    Indexer.show ()
    Operators.show ()
    Iterators.show ()
    Parameters.show ()
    Recursion.show ()
    Immutability.show ()
    FunctionalComposition.show ()
    DiscriminatedUnions.show ()
    ActivePatterns.show ()
    UnitsOfMeasure.show ()
    ComputationExpressions.show ()
    ObjectExpressions.show ()
    SIMD.show ()

    0
