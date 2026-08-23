namespace Language_Features_F_Sharp.Features

open System
open System.Threading.Tasks

/// Hilfsfunktionen fuer die einheitliche Ausgabe der Demo-Abschnitte, analog zu Demo.cs.
module Demo =

    let private separatorWidth = 90

    let private printHeader (title: string) =
        printfn ""
        printfn "%s" (String('=', separatorWidth))
        printfn "%s" title
        printfn "%s" (String('=', separatorWidth))

    let private printError (title: string) (ex: exn) =
        printfn "  [FEHLER in \"%s\"] %s: %s" title (ex.GetType().Name) ex.Message

    /// Entspricht Demo.Section(string, Action) in C#, faengt aber zusaetzlich Exceptions ab und
    /// meldet sie statt die gesamte Demo abzubrechen. Dadurch bleibt beim Debuggen/Erweitern
    /// einzelner Abschnitte die Ausgabe aller anderen (bereits funktionierenden) Abschnitte sichtbar.
    let section (title: string) (action: unit -> unit) =
        printHeader title
        try
            action ()
        with ex ->
            printError title ex
        printfn ""

    /// Entspricht Demo.SectionAsync(string, Func<Task>) in C#. F# nutzt hier den "task {}"
    /// Computation-Expression-Block aus FSharp.Core, der sich fast identisch zu C#s async/await liest.
    let sectionAsync (title: string) (action: unit -> Task) =
        task {
            printHeader title
            try
                do! action ()
            with ex ->
                printError title ex
            printfn ""
        }

    /// Entspricht Demo.Print(string, object?). "obj" ist F#s Gegenstueck zu "object?": jeder Wert
    /// (auch Werttypen) wird bei Bedarf automatisch in "obj" geboxt.
    let print (label: string) (value: obj) =
        let text = if isNull value then "null" else value.ToString()
        printfn "  %-38s %s" label text
