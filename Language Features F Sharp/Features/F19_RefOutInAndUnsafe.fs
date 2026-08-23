namespace Language_Features_F_Sharp.Features

// NativePtr-Zeigeroperationen gelten als potenziell nicht verifizierbarer IL-Code - bewusst in
// Kauf genommen, analog zu C#s "unsafe"/AllowUnsafeBlocks.
#nowarn "9"

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

module F19_RefOutInAndUnsafe =

    // ref-Parameter: "byref<'T>" uebergibt eine Variable per Referenz, Aenderungen wirken sich auf
    // den Aufrufer aus - direktes Pendant zu C#s "ref".
    let private verdopple (wert: byref<int>) = wert <- wert * 2

    // out-Parameter: "outref<'T>" - die Funktion muss den Wert vor der Rueckkehr setzen.
    let private versucheParsen (text: string) (ergebnis: outref<int>) : bool = Int32.TryParse(text, &ergebnis)

    // in-Parameter: "inref<'T>" - per Referenz uebergeben (keine Kopie), aber schreibgeschuetzt.
    // Wiederverwendung des Bruch-Typs aus F16, genau wie im C#-Original.
    let private betrag (bruch: inref<Bruch>) = abs (double bruch.Zaehler / double bruch.Nenner)

    // ref-Rueckgabewert: liefert eine Referenz auf ein Array-Element statt einer Kopie. WICHTIG:
    // F# erlaubt es NICHT, eine byref-typisierte Funktion per if/match direkt mit einem "raise"-
    // Zweig zu mischen (raise laesst sich nicht generisch auf byref<'T> instanziieren, Fehler
    // FS0001). Der Ersatz: die Validierung VORHER in einer eigenen, nicht-byref-typisierten
    // Funktion erledigen, sodass die byref-Funktion selbst nur noch einen einzigen, unbedingten
    // "&ausdruck" als Ergebnis hat.
    let private pruefeGeradeVorhanden (werte: int[]) =
        if werte |> Array.forall (fun w -> w % 2 <> 0) then
            raise (InvalidOperationException "Keine gerade Zahl gefunden")

    let private findeErstesGeradesElement (werte: int[]) : byref<int> =
        pruefeGeradeVorhanden werte
        let index = werte |> Array.findIndex (fun w -> w % 2 = 0)
        &werte.[index]

    let run () =
        let mutable zahl = 21
        verdopple &zahl
        Demo.print "byref-Parameter (Aequivalent zu ref)" zahl

        let mutable geparst = 0
        let erfolgreich = versucheParsen "123" &geparst
        Demo.print "outref-Parameter (Aequivalent zu out, TryParse)" (if erfolgreich then geparst else -1)

        let bruch = Bruch(-3, 4)
        Demo.print "inref-Parameter (Aequivalent zu in, readonly Referenz)" (betrag &bruch)

        let werte = [| 1; 3; 4; 5 |]
        let mutable referenz = &findeErstesGeradesElement werte
        referenz <- 100
        Demo.print "byref-Rueckgabe + byref lokale Variable" (String.Join(",", werte))

        // readonly ref lokale Variable ("inref<'T>"): verhindert Schreibzugriff ueber diese Referenz.
        let schreibgeschuetzt: inref<int> = &werte.[0]
        Demo.print "inref lokale Variable (readonly ref local)" schreibgeschuetzt

        // unsafe-Zeigerarithmetik: F# hat kein "unsafe"-Schluesselwort und auch kein "fixed"-
        // Statement zum Pinnen eines Arrays - der Ersatz ist GCHandle.Alloc(..., Pinned) kombiniert
        // mit dem NativePtr-Modul fuer die eigentliche Zeigerarithmetik.
        let array = [| 10; 20; 30 |]
        let handle = GCHandle.Alloc(array, GCHandleType.Pinned)
        try
            let zeiger = NativePtr.ofNativeInt<int> (handle.AddrOfPinnedObject())
            NativePtr.set (NativePtr.add zeiger 1) 0 999
            let a = NativePtr.get zeiger 0
            let b = NativePtr.get zeiger 1
            let c = NativePtr.get zeiger 2
            Demo.print "GCHandle (Pinned) + NativePtr (statt fixed/unsafe)" $"{a}, {b}, {c}"
        finally
            handle.Free()

        // sizeof<'T>: in F# ein ganz normaler generischer Operator aus FSharp.Core - anders als in
        // C# ist dafuer KEIN unsafe-Kontext noetig.
        Demo.print "sizeof<double> (kein unsafe-Kontext noetig)" sizeof<double>

        // stackalloc mit rohem Zeiger (klassische unsafe-Variante, Gegenstueck zu Span<T> aus F14):
        // muss - wie in F14 gezeigt - direkt hier im Funktionskoerper stehen, nicht in einer eigenen
        // Hilfsfunktion, damit der Stack-Speicher gueltig bleibt.
        let stapelZeiger = NativePtr.stackalloc<int> 3
        NativePtr.set stapelZeiger 0 1
        NativePtr.set stapelZeiger 1 2
        NativePtr.set stapelZeiger 2 3
        let s0 = NativePtr.get stapelZeiger 0
        let s1 = NativePtr.get stapelZeiger 1
        let s2 = NativePtr.get stapelZeiger 2
        Demo.print "NativePtr.stackalloc mit Zeiger" $"{s0},{s1},{s2}"
