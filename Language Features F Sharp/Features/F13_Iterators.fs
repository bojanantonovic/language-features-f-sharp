namespace Language_Features_F_Sharp.Features

open System
open System.Collections
open System.Collections.Generic

// Eigene Collection mit klassischem IEnumerable<T>-Interface: F#s "seq {}"-Ausdruck kompiliert
// intern bereits zu einer Zustandsmaschine (Aequivalent zu C#s yield-return-Iterator-Methoden)
// und wird hier einfach als Datenquelle fuer die Interface-Implementierung genutzt.
type Fibonacci(anzahl: int) =
    let werte: int64 seq =
        seq {
            let mutable a = 0L
            let mutable b = 1L
            for _ in 1..anzahl do
                yield a
                let naechstes = a + b
                a <- b
                b <- naechstes
        }

    interface IEnumerable<int64> with
        member _.GetEnumerator() = werte.GetEnumerator()

    interface IEnumerable with
        member _.GetEnumerator() = (werte :> IEnumerable).GetEnumerator()

module F13_Iterators =

    // "seq {}" mit yield: erzeugt Werte "on demand" (lazy) - direktes Pendant zu "yield return".
    let private quadratzahlen (bis: int) =
        seq {
            for i in 1..bis do
                yield i * i
        }

    // Unendliche Sequenz dank "seq { while true do ... }"; wird erst durch Seq.take begrenzt.
    let private unendlicheGeradzahlen () =
        seq {
            let mutable wert = 0
            let mutable weiter = true
            while weiter do
                yield wert
                wert <- wert + 2
                // F# kennt kein explizites "yield break"-Schluesselwort - die Iteration endet
                // stattdessen einfach dadurch, dass die Schleifenbedingung "weiter" false wird.
                if wert > 1_000_000 then
                    weiter <- false
        }

    let run () =
        Demo.print "seq { yield } (Quadratzahlen bis 5)" (String.Join(",", quadratzahlen 5))

        let geradzahlen = unendlicheGeradzahlen () |> Seq.take 5
        Demo.print "unendliche seq + Seq.take(5)" (String.Join(",", geradzahlen))

        let fibonacci = Fibonacci 8
        Demo.print "eigene IEnumerable<T> (Fibonacci)" (String.Join(",", fibonacci))

        // for..in funktioniert dank IEnumerable<T> genauso wie bei eingebauten Collections.
        let mutable summe = 0
        for zahl in fibonacci do
            summe <- summe + int zahl
        Demo.print "for..in ueber eigene Collection" summe
