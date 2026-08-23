module language_features_f_sharp.Strings

let show () =
    let name = "Alice"
    let age = 30
    let price = 19.999

    // String interpolation: insert values directly into the string, with $ before the quotes
    let greeting = $"Hello, {name}!"

    // Calculations are also possible inside {}
    let ageInfo = $"{name} is {age} years old, in 10 years {age + 10}."

    // Formatting inside {}: %.2f rounds to 2 decimal places (printf format specifier instead of a .NET format string)
    let priceText = $"Price: %.2f{price}"

    printfn "%s" greeting
    printfn "%s" ageInfo
    printfn "%s" priceText

    // Raw/verbatim string (@"..."): escape characters like \ are not interpreted, useful for paths
    let path = @"C:\Data\Alice\Notes.txt"

    // Split: breaks a string into several parts at a separator
    let csv = "Apple, Pear , Cherry"
    let parts = csv.Split(',')

    // Trim removes whitespace at the start/end of each part
    let trimmedParts = parts |> Array.map (fun part -> part.Trim())

    // String.concat: joins several parts back into one string
    let joined = String.concat " | " trimmedParts

    // Checks for substrings
    let containsPear = joined.Contains("Pear")
    let startsWithApple = joined.StartsWith("Apple")

    printfn "%s" path
    printfn "%s" joined
    printfn "%b" containsPear
    printfn "%b" startsWithApple
