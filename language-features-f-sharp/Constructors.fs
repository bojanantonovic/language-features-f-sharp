module language_features_f_sharp.Constructors

// Class with a primary constructor right in the type header: properties are set at creation.
type Book(title: string, pageCount: int) =
    member this.Title = title
    member this.PageCount = pageCount

    // Overloaded (secondary) constructor: calls the primary constructor via "new(...) = Book(...)"
    // and sets a default value for the page count while doing so.
    new(title: string) = Book(title, 0)

let show () =
    let novel = Book("Steppenwolf", 320)
    let unknown = Book("Unknown Book")

    let novelTitle = novel.Title
    let novelPageCount = novel.PageCount

    printfn "%s" novelTitle
    printfn "%d" novelPageCount
    printfn "%s" unknown.Title
    printfn "%d" unknown.PageCount
