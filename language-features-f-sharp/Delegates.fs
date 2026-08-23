module language_features_f_sharp.Delegates

// Function value: in F#, functions are already "first-class values" - a dedicated delegate type
// like in C# (CalculationOperation) is normally unnecessary, a plain signature suffices.
type CalculationOperation = int -> int -> int

// Class with an event: notifies other parts of the code when something changes
type Account() =
    let mutable balance = 0m
    let balanceChanged = Event<decimal>()

    member this.Balance = balance

    // Event: other parts of the code subscribe via "Publish"
    [<CLIEvent>]
    member this.BalanceChanged = balanceChanged.Publish

    member this.Deposit(amount: decimal) =
        balance <- balance + amount
        balanceChanged.Trigger(balance) // raises the event, if anyone is listening

let private add a b = a + b

let show () =
    // Function value: assign it to a function and call it like a variable
    let addFn: CalculationOperation = add
    let sum = addFn 3 4

    // Function value: assign it to a lambda expression
    let multiply: CalculationOperation = fun a b -> a * b
    let product = multiply 3 4

    printfn "%d" sum
    printfn "%d" product

    // List of function values: List.map applies each one to the same arguments
    let operations: CalculationOperation list = [ add; (fun a b -> a * b); (fun a b -> a - b) ]

    let results = operations |> List.map (fun operation -> operation 10 3)

    printfn "%s" (String.concat ", " (results |> List.map string))

    // Subscribe to the event: the lambda is called as soon as BalanceChanged is raised
    let account = Account()
    account.BalanceChanged.Add(fun newBalance -> printfn "%M" newBalance)

    account.Deposit(100m)
    account.Deposit(50m)
