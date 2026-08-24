module language_features_f_sharp.Attributes

open System.Reflection

// Custom attribute: inherits from Attribute, can then be applied to classes or methods
[<System.AttributeUsage(System.AttributeTargets.Method)>]
type AuthorAttribute(name: string) =
    inherit System.Attribute()
    member this.Name = name

// Attributes can be applied to class members in F# just like in C#
type Demo() =
    [<Author("Alice")>]
    member this.Show() = ()

    [<Author("Bob")>]
    member this.HelperMethod() = ()

let rec show () =
    // nameof: returns the identifier name as a string, without hardcoding it -
    // if the function name changes, a wrong string is caught immediately at compile time.
    let ownName = nameof show
    printfn "%s" ownName

    // Reflection: the attribute is read from the method at runtime, not used at compile time
    let t = typeof<Demo>
    let method = t.GetMethod("Show")
    let attribute = method.GetCustomAttribute<AuthorAttribute>()

    // Option.ofObj: converts a possibly-null reference from a .NET API into an option
    // (equivalent to C#'s "?." / "??").
    let authorName = attribute |> Option.ofObj |> Option.map (fun a -> a.Name) |> Option.defaultValue "unknown"

    printfn "%s" authorName

    // Reflection returns all methods of the class, Array.choose filters out those with an author attribute
    let allMethods = t.GetMethods(BindingFlags.Public ||| BindingFlags.Instance)

    let authorsByMethod =
        allMethods
        |> Array.choose (fun m ->
            m.GetCustomAttribute<AuthorAttribute>()
            |> Option.ofObj
            |> Option.map (fun a -> $"{m.Name}: {a.Name}"))
        |> List.ofArray

    printfn "%s" (String.concat " | " authorsByMethod)
