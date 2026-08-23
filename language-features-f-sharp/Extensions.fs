module language_features_f_sharp.Extensions

open System

// Type extension: adds new members to an existing type (here: System.String) after the fact,
// without owning or modifying its source code (equivalent to C#'s extension methods).
type String with
    member this.Reverse() =
        let chars = this |> Seq.rev |> Seq.toArray
        String(chars)

    member this.IsPalindrome() =
        String.Equals(this, this.Reverse(), StringComparison.OrdinalIgnoreCase)

let show () =
    let word = "Anna"

    // looks like a completely normal method call on string, even though string itself was not
    // modified - the extension has to be visible for this (here: same namespace)
    let isPalindrome = word.IsPalindrome()
    let reversed = word.Reverse()

    printfn "%b" isPalindrome
    printfn "%s" reversed

    // List of words: the type extensions can be used directly in lambdas
    let words = [ "Anna"; "Otto"; "Haus"; "Level"; "Baum" ]

    let palindromes = words |> List.filter (fun w -> w.IsPalindrome())
    let reversedWords = words |> List.map (fun w -> w.Reverse())

    printfn "%s" (String.concat ", " palindromes)
    printfn "%s" (String.concat ", " reversedWords)
