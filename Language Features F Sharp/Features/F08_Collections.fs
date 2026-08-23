namespace Language_Features_F_Sharp.Features

open System
open System.Collections.Generic
open System.Collections.Immutable

module F08_Collections =

    // params IEnumerable<T> (C# 13): F# hat kein "params"-Keyword, akzeptiert dafuer aber ganz
    // natuerlich eine Liste/Sequenz als normalen Parameter - variadische Funktionen im C#-Sinn
    // gibt es nicht, sind aber wegen Tupeln/Listen/Pipelines auch selten noetig.
    let private summiereBeliebigViele (werte: int seq) = Seq.sum werte

    let run () =
        // Arrays: eindimensional, mehrdimensional (rechteckig ueber Array2D) und jagged (Array aus Arrays).
        let eindimensional = [| 1; 2; 3; 4 |]
        let rechteckig = array2D [ [ 1; 2 ]; [ 3; 4 ]; [ 5; 6 ] ]
        let jagged = [| [| 1 |]; [| 2; 3 |]; [| 4; 5; 6 |] |]
        Demo.print "Array (eindimensional)" (String.Join(",", eindimensional))
        Demo.print "Array2D (rechteckig 3x2)" (rechteckig.[2, 1])
        Demo.print "Array (jagged, Zeile 2)" (String.Join(",", jagged.[2]))

        // F#-Listen-Literale mit "@" als nativer Spread-/Verkettungsoperator (Aequivalent zu
        // Collection Expressions mit "..").
        let teilA = [ 1; 2; 3 ]
        let teilB = [ 4; 5 ]
        let kombiniert = teilA @ teilB @ [ 6 ]
        Demo.print "Listenverkettung @ (Aequivalent zu Spread ..)" (String.Join(",", kombiniert))

        // ResizeArray<T> ist F#s Alias fuer List<T>.
        let namen = ResizeArray [ "Alice"; "Bob"; "Carol" ]
        namen.Add "Dave"
        Demo.print "ResizeArray<T> (List<T>)" (String.Join(",", namen))

        // Dictionary<TKey,TValue> via "dict"/direkt als .NET-Typ mit Indexer-Zuweisung.
        let alterNachName = Dictionary<string, int>()
        alterNachName.["Alice"] <- 30
        alterNachName.["Bob"] <- 25
        alterNachName.["Carol"] <- 40
        let alterEintraege = alterNachName |> Seq.map (fun kv -> $"{kv.Key}={kv.Value}")
        Demo.print "Dictionary<K,V>" (String.Join(", ", alterEintraege))

        // HashSet<T>: eindeutige Elemente, Mengenoperationen (mutierend wie in C#).
        let mengeA = HashSet<int> [ 1; 2; 3; 4 ]
        let mengeB = HashSet<int> [ 3; 4; 5; 6 ]
        let schnittmenge = HashSet<int> mengeA
        schnittmenge.IntersectWith mengeB
        Demo.print "HashSet<T> Schnittmenge" (String.Join(",", Seq.sort schnittmenge))

        // Queue<T> (FIFO) und Stack<T> (LIFO) - identische BCL-Typen wie in C#.
        let warteschlange = Queue<string>([ "Erster"; "Zweiter"; "Dritter" ])
        Demo.print "Queue<T>.Dequeue()" (warteschlange.Dequeue())
        let stapel = Stack<string>([ "Unten"; "Mitte"; "Oben" ])
        Demo.print "Stack<T>.Pop()" (stapel.Pop())

        // LinkedList<T>: doppelt verkettete Liste.
        let verkettet = LinkedList<int>([ 1; 2; 3 ])
        verkettet.AddFirst 0 |> ignore
        verkettet.AddLast 4 |> ignore
        Demo.print "LinkedList<T>" (String.Join(",", verkettet))

        // SortedDictionary<TKey,TValue>: haelt Schluessel sortiert.
        let sortiert = SortedDictionary<string, int>(alterNachName)
        Demo.print "SortedDictionary<K,V>" (String.Join(",", sortiert.Keys))

        // Unveraenderliche Collections (System.Collections.Immutable) - direkt aus F# nutzbar.
        let immutableArray = ImmutableArray.Create(1, 2, 3)
        let erweitert = immutableArray.Add 4
        Demo.print "ImmutableArray<T> (Original unveraendert)" (String.Join(",", immutableArray))
        Demo.print "ImmutableArray<T>.Add (neue Instanz)" (String.Join(",", erweitert))

        let immutableList = ImmutableList.Create("a", "b")
        Demo.print "ImmutableList<T>" (String.Join(",", immutableList.Add "c"))

        // F#s idiomatisches Gegenstueck: die eingebauten unveraenderlichen Listen/Maps/Sets sind
        // von Haus aus persistent, ganz ohne einen Namespace wie System.Collections.Immutable.
        let fsharpListe = [ 1; 2; 3 ]
        let fsharpListeErweitert = fsharpListe @ [ 4 ]
        Demo.print "F#-Liste (nativ unveraenderlich, ohne Immutable-Namespace)" (String.Join(",", fsharpListeErweitert))

        // Aequivalent zu params IEnumerable<T>: eine normale seq<int> als Parameter.
        Demo.print "seq<int> als Parameter (Aequivalent zu params)" (summiereBeliebigViele [ 1; 2; 3; 4; 5 ])

        // KeyValuePair-Deconstruction beim Iterieren ueber ein Dictionary - F# unterstuetzt das
        // ebenfalls direkt im "for"-Pattern.
        for KeyValue(name, alter) in alterNachName do
            Demo.print $"Deconstruct KeyValuePair ({name})" alter
