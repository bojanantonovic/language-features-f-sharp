namespace Language_Features_F_Sharp.Features

// Siehe F10: F# wendet op_Implicit fuer eine kleine, eingebaute Auswahl an BCL-Konvertierungen
// (u. a. Array -> Span<T>/ReadOnlySpan<T>, aber NICHT Array -> Memory<T>) automatisch an - hier
// bewusst genutzt, daher die Warnung FS3391 unterdrueckt.
#nowarn "3391"
// NativePtr-Operationen wie stackalloc gelten als potenziell nicht verifizierbarer IL-Code -
// bewusst in Kauf genommen, analog zu C#s "unsafe"/AllowUnsafeBlocks.
#nowarn "9"

open System
open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop

// ref struct (C#): [<Struct>] kombiniert mit [<IsByRefLike>] (aus System.Runtime.CompilerServices,
// da FSharp.Core kein eigenes Pendant definiert) erzeugt denselben "lebt garantiert nur auf dem
// Stack"-Typ wie C#s "ref struct" und darf daher ein Span<T>-Feld enthalten.
[<Struct; IsByRefLike>]
type ZeichenPuffer(daten: Span<char>) =
    member _.Daten = daten
    member this.GrossSchreiben() =
        for i in 0 .. this.Daten.Length - 1 do
            this.Daten.[i] <- Char.ToUpperInvariant this.Daten.[i]

module F14_SpanAndMemory =

    let run () =
        // stackalloc-Ersatz: F# hat - anders als C# - KEIN eingebautes "stackalloc", das direkt ein
        // Span<T> liefert. NativePtr.stackalloc + der Span<T>-Konstruktor aus rohem Zeiger + Laenge
        // bilden dasselbe nach. WICHTIG: das muss direkt hier (im aufrufenden Funktionskoerper)
        // stehen - ausgelagert in eine eigene (auch "inline" markierte) Hilfsfunktion wird der
        // Stack-Speicher nicht zuverlaessig ueber die Aufrufgrenze hinweg gueltig gehalten.
        let zeiger = NativePtr.stackalloc<int> 5
        let stapelSpeicher = Span<int>(NativePtr.toVoidPtr zeiger, 5)
        for i in 0 .. stapelSpeicher.Length - 1 do
            stapelSpeicher.[i] <- i * i
        Demo.print "NativePtr.stackalloc + Span<T> (Ersatz fuer stackalloc)" (String.Join(",", stapelSpeicher.ToArray()))

        // First-class Span-Konvertierungen: Array wird implizit zu Span<T>/ReadOnlySpan<T>.
        let quellArray = [| 10; 20; 30; 40; 50 |]
        let arraySpan: Span<int> = quellArray
        let nurLesenSpan: ReadOnlySpan<int> = quellArray
        Demo.print "implizite Array->Span<T> Konvertierung" arraySpan.Length

        // Slicing ohne Kopieren der Daten.
        let mittlererAbschnitt = arraySpan.Slice(1, 3)
        mittlererAbschnitt.[0] <- 999
        Demo.print "Span-Slice teilt Speicher mit Original" (String.Join(",", quellArray))
        Demo.print "ReadOnlySpan<T> aus Array" (String.Join(",", nurLesenSpan.ToArray()))

        // Span<T> ueber Text (string.AsSpan) fuer allokationsarme Verarbeitung.
        let text = "Hallo Span-Welt"
        let textSpan: ReadOnlySpan<char> = text.AsSpan()
        let ersteWortSpan = textSpan.Slice(0, 5)
        Demo.print "string.AsSpan + Slicing" (ersteWortSpan.ToString())

        // ref struct als Wrapper um Span<char>: kann nur auf dem Stack existieren.
        let mutableText = [| 'h'; 'a'; 'l'; 'l'; 'o' |]
        let puffer = ZeichenPuffer(Span<char> mutableText)
        puffer.GrossSchreiben()
        Demo.print "[<IsByRefLike>] struct ZeichenPuffer" (String(mutableText))

        // Memory<T>: Heap-faehiges Pendant zu Span<T>, u. a. fuer async-Kontexte nutzbar. Anders als
        // bei Span<T>/ReadOnlySpan<T> gehoert Array->Memory<T> NICHT zur impliziten Konvertierungs-
        // Whitelist von F# - der Konstruktor muss hier explizit aufgerufen werden.
        let speicherBereich = Memory<int>([| 1; 2; 3; 4; 5 |])
        let teilBereich = speicherBereich.Slice(1, 3)
        Demo.print "Memory<T> Slice (expliziter Konstruktor)" (String.Join(",", teilBereich.ToArray()))
