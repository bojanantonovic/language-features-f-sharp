namespace Language_Features_F_Sharp.Features

open System

type Profil() =
    member val Spitzname: string option = None with get, set

module F17_NullableReferenceTypes =

    // Nullable Reference Types (<Nullable>enable</Nullable>) sind ein rein C#-seitiger Compiler-
    // Check auf TOP von weiterhin nullbaren Referenztypen. F#s idiomatischer, staerkerer Ersatz
    // bildet Abwesenheit direkt im Typsystem ab: "string" bedeutet in F# schon immer "ein string
    // (nicht null erwartet)", "string option" bedeutet "vielleicht kein Wert". Seit F# 9 gibt es
    // zusaetzlich experimentelle, C#-aehnliche Nullness-Checks fuer echte Interop-Faelle ueber das
    // Compiler-Flag "--checknulls" (dort auch mit "T | null"-Typsyntax) - fuer eigenen F#-Code
    // bleibt "option" aber der uebliche und hier verwendete Weg.
    let private formatiereName (pflichtName: string) (optionalerZweitname: string option) =
        let zweitnameTeil = optionalerZweitname |> Option.map (fun z -> $" \"{z}\"") |> Option.defaultValue ""
        $"{pflichtName}{zweitnameTeil}"

    // ArgumentNullException.ThrowIfNull nutzt in C# [CallerArgumentExpression], damit der
    // Parametername automatisch am Aufrufort ermittelt wird - der F#-Compiler wertet dieses
    // C#-spezifische Attribut NICHT aus, daher muss der Name hier explizit per nameof mitgegeben
    // werden (sonst bliebe ex.ParamName null).
    let private validiereNichtNull (wert: string) = ArgumentNullException.ThrowIfNull(wert, nameof wert)

    let private ermittleWert () : string option = Some "garantierter Wert"

    let run () =
        Demo.print "string vs. string option (statt nicht-nullable/nullable)" (formatiereName "Alice" None)

        // Option.map ist bereits in F02 als Aequivalent zu "?." eingefuehrt - hier erneut im
        // direkten NRT-Vergleich.
        let eventuellNichts: string option = None
        let laengeText = eventuellNichts |> Option.map (fun s -> string s.Length) |> Option.defaultValue "null"
        Demo.print "Option.map (Aequivalent zu ?.)" laengeText

        // ArgumentNullException.ThrowIfNull: identische BCL-API, direkt aus F# nutzbar (fuer echte
        // Interop-Faelle, in denen ein "null" ankommen kann, z. B. aus C#-Code).
        try
            validiereNichtNull null
        with :? ArgumentNullException as ex ->
            Demo.print "ArgumentNullException.ThrowIfNull" ex.ParamName

        // Der F#-Ersatz fuer den Null-forgiving Operator (!) ist "Option.get": eine bewusste
        // Behauptung "ich weiss, dass ein Wert da ist" - mit demselben Laufzeitrisiko wie C#s "!",
        // falls die Annahme falsch war (dann eine Exception statt stillschweigendem null).
        let moeglicherweiseNichts = ermittleWert ()
        let garantiert = moeglicherweiseNichts |> Option.get
        Demo.print "Option.get (Aequivalent zum Null-forgiving Operator !)" garantiert

        // Null-conditional Assignment (C# 14, "?. ="): F# hat keine eigene Syntax dafuer, aber
        // Option.iter fuehrt eine Aktion nur aus, wenn tatsaechlich ein Wert (bzw. Objekt) vorhanden ist.
        let profil: Profil option = Some(Profil())
        profil |> Option.iter (fun p -> p.Spitzname <- Some "Ali")
        let spitznameText = profil |> Option.bind (fun p -> p.Spitzname) |> Option.defaultValue "null"
        Demo.print "Option.iter (Aequivalent zu ?. =)" spitznameText

        let keinProfil: Profil option = None
        keinProfil |> Option.iter (fun p -> p.Spitzname <- Some "wird nie gesetzt")
        Demo.print "Option.iter auf None" (if keinProfil.IsNone then "keine NullReferenceException" else "unerwartet")
