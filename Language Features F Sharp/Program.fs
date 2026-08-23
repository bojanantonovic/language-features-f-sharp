// Anders als C# (Top-Level Statements, C# 9) benoetigt F# stets einen expliziten Einstiegspunkt:
// eine Funktion "main", markiert mit [<EntryPoint>], die einen int-Exitcode zurueckgibt.
// Ein Naeherungswert an C#s "Top-Level Statements" waeren F#-Skripte (.fsx), die tatsaechlich
// ganz ohne Modul/Funktion auskommen - fuer eine kompilierbare .exe ist [<EntryPoint>] jedoch Pflicht.
module Language_Features_F_Sharp.Program

open System
open System.Threading.Tasks
open Language_Features_F_Sharp.Features

/// Hebt eine synchrone Run-Funktion in dieselbe "unit -> Task"-Form wie die asynchronen Abschnitte,
/// damit Program.fs eine einzige, einheitliche Registrierungsliste fuehren kann.
let private sync (action: unit -> unit) : unit -> Task =
    fun () ->
        action ()
        Task.CompletedTask

/// Registrierung aller Abschnitte als (Id, Titel, Aktion)-Tripel statt einer flachen Abfolge von
/// Demo.section-Aufrufen. Das macht das gezielte Debuggen einzelner Abschnitte moeglich:
///   dotnet run -- --list      listet alle Ids samt Titel auf
///   dotnet run -- 05          fuehrt NUR Abschnitt 05 aus
///   dotnet run -- 05 12       fuehrt mehrere ausgewaehlte Abschnitte aus
///   dotnet run -- Pattern     Freitext-Filter (Teilstring, gross-/kleinschreibungsunabhaengig) auf den Titel
let private sections: (string * string * (unit -> Task)) list =
    [ "01", "01 - Variablen & Typen (let/var, [<Literal>], Nullable, Tupel)", sync F01_VariablesAndTypes.run
      "02", "02 - Operatoren & Kontrollfluss (Option, match, Schleifen, Ranges/Indices)", sync F02_OperatorsAndControlFlow.run
      "03", "03 - Pattern Matching (match-Expression, Record-/Listen-Pattern)", sync F03_PatternMatching.run
      "04", "04 - Klassen, Structs & Records (primary ctor, [<Struct>], Records)", sync F04_ClassesStructsRecords.run
      "05", "05 - Vererbung & Polymorphie (abstract, interfaces, SRTP Generic Math)", sync F05_InheritancePolymorphism.run
      "06", "06 - Generics (Constraints, Kovarianz/Kontravarianz via BCL-Typen)", sync F06_Generics.run
      "07", "07 - Delegates, Events & Lambdas (Closures via ref, lokale Funktionen)", sync F07_DelegatesEventsLambdas.run
      "08", "08 - Collections (Arrays, ResizeArray/Dictionary/HashSet, F#-Listen)", sync F08_Collections.run
      "09", "09 - Sequenzen & LINQ (Seq-Pipeline, query {}, Deferred Execution)", sync F09_Linq.run
      "10", "10 - Strings & Text (Interpolation, Raw Strings, UTF-8-Bytes)", sync F10_Strings.run
      "11", "11 - Exception Handling (Guard when, reraise, use)", sync F11_ExceptionHandling.run
      "12", "12 - Asynchrone Programmierung (task{}, Task, IAsyncEnumerable)", F12_Async.runAsync
      "13", "13 - Iteratoren (seq { yield }, eigene IEnumerable<T>)", sync F13_Iterators.run
      "14", "14 - Span<T>, Memory<T> & stackalloc ([<IsByRefLike>] struct)", sync F14_SpanAndMemory.run
      "15", "15 - Reflection & Attribute (nameof, benutzerdefinierte Attribute)", sync F15_ReflectionAttributes.run
      "16", "16 - Operator Overloading & Konvertierungen", sync F16_OperatorOverloading.run
      "17", "17 - Null-Sicherheit (option statt Nullable Reference Types)", sync F17_NullableReferenceTypes.run
      "18", "18 - Moderne Syntax (Typerweiterungen, Lock, Backing-Field-Property)", sync F18_ModernSyntax.run
      "19", "19 - byref/outref/inref & NativePtr (statt ref/out/in & unsafe)", sync F19_RefOutInAndUnsafe.run
      "20", "20 - Enums & [<Flags>]", sync F20_EnumsAndFlags.run ]

let private printListe () =
    printfn "Verfuegbare Abschnitte (Aufruf per \"dotnet run -- <id-oder-textfilter>...\"):"
    for id, title, _ in sections do
        printfn "  %-4s %s" id title

let private waehleAus (argv: string array) =
    if argv.Length = 0 then
        sections
    else
        sections
        |> List.filter (fun (id, title, _) ->
            argv
            |> Array.exists (fun filter -> id = filter || title.Contains(filter, StringComparison.OrdinalIgnoreCase)))

[<EntryPoint>]
let main argv =
    if argv |> Array.contains "--list" then
        printListe ()
        0
    else
        printfn "F# Sprach-Feature-Demo"
        printfn "Zielrahmenwerk: .NET 10 / F# 10"

        let auswahl = waehleAus argv

        if argv.Length > 0 && List.isEmpty auswahl then
            printfn ""
            printfn "Kein Abschnitt gefunden fuer: %s" (String.Join(", ", argv))
            printListe ()
        else
            // Jeder Abschnitt faengt seine Exceptions selbst ab (s. Demo.section/sectionAsync) -
            // ein Fehler in Abschnitt N bricht die Demo also nicht ab, sondern die restlichen
            // (bereits funktionierenden) Abschnitte laufen trotzdem weiter durch.
            for _, title, action in auswahl do
                (Demo.sectionAsync title action).GetAwaiter().GetResult()

            printfn ""
            printfn "Alle Demos abgeschlossen."

        0
