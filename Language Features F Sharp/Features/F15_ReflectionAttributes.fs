namespace Language_Features_F_Sharp.Features

open System
open System.Reflection

// Benutzerdefiniertes Attribut - identisch zu C#: eine von Attribute abgeleitete Klasse mit
// [<AttributeUsage>].
[<AttributeUsage(AttributeTargets.Class ||| AttributeTargets.Method)>]
type AutorAttribute(name: string) =
    inherit Attribute()
    member _.Name = name

[<Autor "Bojan">]
type Beispielklasse() =
    member val Titel = "Standardtitel" with get, set
    member val Prioritaet = 0 with get, set

    [<Autor "Team">]
    member _.Ausfuehren() =
        printfn "    Beispielklasse.Ausfuehren() wurde per Reflection aufgerufen"

module F15_ReflectionAttributes =

    let run () =
        let typ = typeof<Beispielklasse>

        // nameof: liefert den Namen eines Symbols als String, refactoring-sicher - identisch zu C#.
        Demo.print "nameof Beispielklasse" (nameof Beispielklasse)

        // nameof mit ungebundenem generischen Typ (C# 14, "nameof(List<>)"): F# kennt keine
        // ungebundene generische Typsyntax wie "List<>" - man nutzt eine konkrete Instanziierung;
        // nameof liefert ohnehin in beiden Faellen nur den einfachen Namen ohne Typargumente.
        Demo.print "nameof (List<int>) - konkret statt ungebunden" (nameof (ResizeArray<int>))

        // Attribut per Reflection auslesen - identische BCL-API wie in C#.
        // Ein F#-eigener Referenztyp erlaubt "null" per Default NICHT als gueltigen Wert (compile-
        // seitig abgesichert). GetCustomAttribute<T> kann zur Laufzeit dennoch null liefern - der
        // saubere Weg um das zu pruefen ist daher obj.ReferenceEquals statt "isNull"/Null-Pattern.
        let klassenAttribut = typ.GetCustomAttribute<AutorAttribute>()
        let klassenAttributName = if obj.ReferenceEquals(klassenAttribut, null) then "keins" else klassenAttribut.Name
        Demo.print "Klassen-Attribut per Reflection" klassenAttributName

        let methode = typ.GetMethod "Ausfuehren"
        let methodenAttribut = methode.GetCustomAttribute<AutorAttribute>()
        let methodenAttributName = if obj.ReferenceEquals(methodenAttribut, null) then "keins" else methodenAttribut.Name
        Demo.print "Methoden-Attribut per Reflection" methodenAttributName

        // Properties per Reflection auflisten.
        let propertyNamen = typ.GetProperties() |> Array.map (fun p -> p.Name)
        Demo.print "Type.GetProperties()" (String.Join(",", propertyNamen))

        // Instanz per Reflection erzeugen und Property setzen (Activator.CreateInstance).
        let instanz = Activator.CreateInstance typ :?> Beispielklasse
        typ.GetProperty("Titel").SetValue(instanz, "Per Reflection gesetzt")
        Demo.print "Activator.CreateInstance + SetValue" instanz.Titel

        // Methode per Reflection aufrufen.
        methode.Invoke(instanz, null) |> ignore
