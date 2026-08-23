module language_features_f_sharp.Attribut

open System.Reflection

// Eigenes Attribut: erbt von Attribute, kann anschliessend auf Klassen oder Methoden angewendet werden
[<System.AttributeUsage(System.AttributeTargets.Method)>]
type AutorAttribute(name: string) =
    inherit System.Attribute()
    member this.Name = name

// Attribute lassen sich in F# auf Klassenmitglieder genauso anwenden wie in C#
type Demo() =
    [<Autor("Alice")>]
    member this.Zeigen() = ()

    [<Autor("Bob")>]
    member this.Hilfsmethode() = ()

let rec zeigen () =
    // nameof: liefert den Bezeichnernamen als String, ohne ihn hart zu codieren -
    // aendert sich der Funktionsname, faellt ein falscher String sofort beim Kompilieren auf.
    let eigenerName = nameof zeigen
    printfn "%s" eigenerName

    // Reflection: das Attribut wird zur Laufzeit von der Methode ausgelesen, nicht schon beim Kompilieren verwendet
    let typ = typeof<Demo>
    let methode = typ.GetMethod("Zeigen")
    let attribut = methode.GetCustomAttribute<AutorAttribute>()

    let autorName = if isNull (box attribut) then "unbekannt" else attribut.Name

    printfn "%s" autorName

    // Reflection liefert alle Methoden der Klasse, Array.choose filtert daraus die mit einem Autor-Attribut
    let alleMethoden = typ.GetMethods(BindingFlags.Public ||| BindingFlags.Instance)

    let autorenProMethode =
        alleMethoden
        |> Array.choose (fun m ->
            let a = m.GetCustomAttribute<AutorAttribute>()
            if isNull (box a) then None else Some $"{m.Name}: {a.Name}")
        |> List.ofArray

    printfn "%s" (String.concat " | " autorenProMethode)
