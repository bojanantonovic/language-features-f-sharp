namespace Language_Features_F_Sharp.Features

open System

module F02_OperatorsAndControlFlow =

    let run () =
        // Arithmetische, logische und bitweise Operatoren - identisch zu C#.
        Demo.print "7 / 2 (int) / 7 % 2" $"{7 / 2} / {7 % 2}"
        Demo.print "true && false | true || false" $"{true && false} | {true || false}"
        Demo.print "5 &&& 3 | 5 ^^^ 3 | ~~~5" $"{5 &&& 3} | {5 ^^^ 3} | {~~~5}"
        Demo.print "1 <<< 3 | 8 >>> 2" $"{1 <<< 3} | {8 >>> 2}"

        // F# hat weder "??" noch "?." fuer beliebige Objekte - der idiomatische Ersatz ist der
        // Option-Typ, der Abwesenheit eines Werts bereits im Typsystem abbildet.
        let eingabe: string option = None
        let ergebnis = eingabe |> Option.defaultValue "Standardwert"
        Demo.print "Option.defaultValue (Aequivalent zu ??)" ergebnis
        let mutable eingabeMutable = eingabe
        if eingabeMutable.IsNone then
            eingabeMutable <- Some "jetzt gesetzt"
        Demo.print "??= Aequivalent (mutable Option neu zuweisen)" (eingabeMutable |> Option.defaultValue "?")

        // Option.map ist das Gegenstueck zu "?." (Zugriff nur, wenn ein Wert vorhanden ist).
        let vielleichtNull: ResizeArray<string> option = None
        let anzahlText = vielleichtNull |> Option.map (fun l -> string l.Count) |> Option.defaultValue "null"
        Demo.print "Option.map (Aequivalent zu ?.)" anzahlText
        let jetztVorhanden = Some(ResizeArray [ "a"; "b"; "c" ])
        let indexText = jetztVorhanden |> Option.map (fun l -> l.[1]) |> Option.defaultValue "null"
        Demo.print "Option.map Index-Zugriff (Aequivalent zu ?[])" indexText

        // if/then/else als Ausdruck ersetzt den ternaeren Operator - F# kennt "?:" nicht,
        // braucht ihn aber auch nicht, da if/then/else bereits ein Ausdruck ist.
        let zahl = 5
        let parity = if zahl % 2 = 0 then "gerade" else "ungerade"
        Demo.print "if/then/else als Ausdruck (statt ?:)" parity

        // match-Ausdruck statt klassischem switch-Statement.
        let switchErgebnis =
            match zahl with
            | n when n < 0 -> "negativ"
            | 0 -> "null"
            | _ -> "positiv"
        Demo.print "match-Ausdruck (statt switch)" switchErgebnis

        // Schleifen: for (Range), for..in (foreach), while. F# kennt kein "do-while" (Schleife mit
        // nachgestellter Bedingung) - es muss ueber eine "while"-Schleife mit mutable Flag simuliert werden.
        let mutable summeFor = 0
        for i in 1..5 do
            summeFor <- summeFor + i
        Demo.print "for-Schleife (Range) Summe 1..5" summeFor

        let mutable summeForeach = 0
        for wert in [ 1; 2; 3; 4; 5 ] do
            summeForeach <- summeForeach + wert
        Demo.print "for..in-Schleife (foreach) Summe" summeForeach

        let mutable zaehlerWhile = 0
        let mutable iterationen = 0
        while zaehlerWhile < 3 do
            zaehlerWhile <- zaehlerWhile + 1
            iterationen <- iterationen + 1
        Demo.print "while-Schleife Iterationen" iterationen

        // do-while-Emulation: Bedingung wird erst NACH dem ersten Durchlauf geprueft.
        let mutable zaehlerDoWhile = 0
        let mutable weiter = true
        while weiter do
            zaehlerDoWhile <- zaehlerDoWhile + 1
            weiter <- zaehlerDoWhile < 3
        Demo.print "do-while Emulation (kein natives do-while in F#)" zaehlerDoWhile

        // break/continue: F# kennt beide Schluesselwoerter nicht. Statt "break" verwendet man
        // typischerweise Seq.tryFind/tryFindIndex; statt "continue" filtert man einfach mit if.
        let gefundenBei = [ 0..9 ] |> List.tryFind (fun i -> i = 4) |> Option.defaultValue -1
        Demo.print "Seq.tryFind (Aequivalent zu break)" gefundenBei

        let geradeZahlen =
            [ for i in 0..5 do
                  if i % 2 = 0 then
                      yield i ]
        Demo.print "if-Filter im for (Aequivalent zu continue)" (String.Join(",", geradeZahlen))

        // goto: existiert in F# ueberhaupt nicht (keine Labels/Spruenge). Der idiomatische Ersatz
        // fuer ein wiederholtes "goto"-Sprungziel ist eine (endrekursive) Hilfsfunktion.
        let rec wiederhole schritt =
            if schritt < 3 then wiederhole (schritt + 1) else schritt
        Demo.print "Rekursion (Aequivalent zu goto-Schleife)" (wiederhole 0)

        // Ranges und Indices: F# unterstuetzt seit F# 6 dieselbe Slicing-Syntax wie C# 8,
        // inklusive Index von hinten ueber "^".
        let zahlenArray = [| 10; 20; 30; 40; 50 |]
        Demo.print "Index von hinten zahlenArray[^0]" zahlenArray.[^0]
        Demo.print "Range zahlenArray[1..2]" (String.Join(",", zahlenArray.[1..2]))
        let vonAnfang = String.Join(",", zahlenArray.[..1])
        let vonMitte = String.Join(",", zahlenArray.[2..])
        Demo.print "Range zahlenArray[..1] / zahlenArray[2..]" $"{vonAnfang} / {vonMitte}"
