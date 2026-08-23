module language_features_f_sharp.Extensions

open System

// Typerweiterung: fuegt einem bestehenden Typ (hier: System.String) im Nachhinein neue Mitglieder hinzu,
// ohne dessen Quelltext zu besitzen oder zu aendern (Aequivalent zu C#s Extension-Methoden).
type String with
    member this.Umdrehen() =
        let zeichen = this |> Seq.rev |> Seq.toArray
        String(zeichen)

    member this.IstPalindrom() =
        String.Equals(this, this.Umdrehen(), StringComparison.OrdinalIgnoreCase)

let zeigen () =
    let wort = "Anna"

    // sieht wie ein ganz normaler Methodenaufruf auf string aus, obwohl string selbst nicht
    // veraendert wurde - die Erweiterung muss dafuer sichtbar sein (hier: derselbe Namespace)
    let istPalindrom = wort.IstPalindrom()
    let umgedreht = wort.Umdrehen()

    printfn "%b" istPalindrom
    printfn "%s" umgedreht

    // Liste von Woertern: die Typerweiterungen lassen sich direkt in Lambdas verwenden
    let woerter = [ "Anna"; "Otto"; "Haus"; "Level"; "Baum" ]

    let palindrome = woerter |> List.filter (fun w -> w.IstPalindrom())
    let umgedreheWoerter = woerter |> List.map (fun w -> w.Umdrehen())

    printfn "%s" (String.concat ", " palindrome)
    printfn "%s" (String.concat ", " umgedreheWoerter)
