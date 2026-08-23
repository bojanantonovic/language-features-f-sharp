module language_features_f_sharp.Asynchrony

// async workflow: F#'s native model for asynchronous computations (older than C#'s async/await and
// one of its inspirations) - "let!"/"do!" wait for a result without blocking the thread.
let private calculateAsync a b =
    async {
        do! Async.Sleep 100 // simulates a long-running operation, e.g. a network call
        return a * b
    }

let showAsync () =
    async {
        let! result = calculateAsync 6 7
        printfn "%d" result
    }
