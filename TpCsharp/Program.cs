/*Console.Write("Ton prénom : ");
string? prenom = Console.ReadLine();

Console.Write("Ton année de naissance : ");
string? texteAnnee = Console.ReadLine();

int annee = int.Parse(texteAnnee);

int age = DateTime.Now.Year - annee;

Console.WriteLine($"Bonjour {prenom}, tu as {age} ans !");*/


string? saisie = Console.ReadLine();
Console.WriteLine($"Vous avez tapé {saisie?.Length?? 0} caractères");
/*
if (saisie is null)
{
    Console.WriteLine($"Vous avez tapé 0 caractères");
    return;
}
else
{
    Console.WriteLine($"Vous avez tapé {saisie.Length} caractères");
}
Console.Write("Ton prénom : ");
string? prenom = Console.ReadLine();

int age;
bool valide;

do
{
    Console.Write("Ton année de naissance : ");
    string? texteAnnee = Console.ReadLine();

    valide = int.TryParse(texteAnnee, out int annee);   

    if (valide)
    {
        age = DateTime.Now.Year - annee;
        valide = age >= 0 && age <= 120;                 
    }
    else
    {
        age = 0;
    }

    if (!valide)
    {
        Console.WriteLine("Année invalide, recommence.");
    }
}
while (!valide);

Console.WriteLine($"Bonjour {prenom}, tu as {age} ans !");
*/




{
    Console.WriteLine(" 3.1 Mention ");
    Console.WriteLine($"17   -> {Outils.Mention(17)}");
    Console.WriteLine($"14   -> {Outils.Mention(14)}");
    Console.WriteLine($"12.5 -> {Outils.Mention(12.5)}");
    Console.WriteLine($"10   -> {Outils.Mention(10)}");
    Console.WriteLine($"8    -> {Outils.Mention(8)}");

    try
    {
        Outils.Mention(25);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine($"25   -> exception : {e.Message}");
    }
}

{
    Console.WriteLine();
    Console.WriteLine("--- 3.2 Liste de notes ---");

    List<double> notes = [12, 15.5, 9, 18];  

    double somme = 0;
    double min = notes[0];   
    double max = notes[0];

    foreach (double note in notes)
    {
        somme += note;
        if (note < min) min = note;
        if (note > max) max = note;
    }

    double moyenne = somme / notes.Count;

    Console.WriteLine($"Moyenne : {moyenne:F2} ({Outils.Mention(moyenne)})");
    Console.WriteLine($"Min : {min}, Max : {max}");
}


Console.WriteLine();
Console.WriteLine("--- 3.3 Dictionnaire étudiants -> notes ---");

Dictionary<string, List<double>> dico = new()
{
    ["Ada"] = [15, 16],
    ["Alan"] = [9, 12],
    ["Grace"] = [17, 14, 13],
};

dico["Alan"].Add(14);   

foreach (var (nom, notesEtudiant) in dico)
{
    double moy = Outils.Moyenne(notesEtudiant);
    Console.WriteLine($"{nom} : {notesEtudiant.Count} notes, moyenne {moy:F2}, {Outils.Mention(moy)}");
}


{
    Console.WriteLine();
    Console.WriteLine("--- 3.4 Clé inconnue ---");

    try
    {
        List<double> n = dico["Inconnu"];
    }
    catch (KeyNotFoundException e)
    {
        Console.WriteLine($"dico[\"Inconnu\"] -> {e.GetType().Name} : {e.Message}");
    }

    if (dico.TryGetValue("Inconnu", out List<double>? notesTrouvees))
    {
        Console.WriteLine($"Trouvé : {notesTrouvees.Count} notes");
    }
    else
    {
        Console.WriteLine("TryGetValue -> \"Inconnu\" n'est pas dans le dictionnaire");
    }
}

{
    Console.WriteLine();
    Console.WriteLine("--- 3.5 Decrire ---");

    object?[] valeurs = [4, 7, "", "bonjour", new List<double> { 1, 2, 3 }, null, 3.14];

    foreach (object? v in valeurs)
    {

        string texte = v switch
        {
            null => "null",
            string s => $"\"{s}\"",
            List<double> l => $"[{string.Join(", ", l)}]",
            _ => v.ToString() ?? ""
        };
        Console.WriteLine($"{texte,-12} -> {Outils.Decrire(v)}");
    }
}
