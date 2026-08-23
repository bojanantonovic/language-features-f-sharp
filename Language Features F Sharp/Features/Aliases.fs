namespace Language_Features_F_Sharp.Features

// Typalias / Type Abbreviation: das F#-Gegenstueck zu den globalen using-Aliasen aus
// GlobalUsings.cs im C#-Geschwisterprojekt ("global using Koordinate = (double, double);").
// F# kennt kein "global using", aber ein Typalias, das direkt im gemeinsamen Namespace steht,
// ist projektweit ohne zusaetzliches "open" sichtbar - dasselbe Ergebnis wie in C#.

/// Alias fuer ein Tupel aus Breiten- und Laengengrad, analog zu "Koordinate" in C#.
type Koordinate = float * float

/// Alias fuer eine generische Liste, analog zu "StringListe" in C#.
type StringListe = System.Collections.Generic.List<string>
