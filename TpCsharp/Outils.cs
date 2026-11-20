// des petites fonctions de l etape 3
public static class Outils
{
    // la mention selon la moyenne et une exception si on est pas entre 0 et 20
    public static string Mention(double moyenne) => moyenne switch
    {
        < 0 or > 20 => throw new ArgumentOutOfRangeException(nameof(moyenne)),
        >= 16 => "Très bien",
        >= 14 => "Bien",
        >= 12 => "Assez bien",
        >= 10 => "Passable",
        _ => "Ajourné"
    };

    // moyenne simple avec une boucle et 0 si la liste est vide
    public static double Moyenne(List<double> notes)
    {
        if (notes.Count == 0)
        {
            return 0;
        }

        double somme = 0;
        foreach (double note in notes)
        {
            somme = somme + note;
        }
        return somme / notes.Count;
    }

    // decrit n importe quoi selon son type avec le pattern matching
    public static string Decrire(object? o) => o switch
    {
        null => "null",
        int n when n % 2 == 0 => "entier pair",
        int => "entier impair",
        string { Length: 0 } => "chaîne vide",
        string s => $"chaîne de {s.Length} caractères",
        List<double> liste => $"liste de {liste.Count} éléments",
        _ => "autre"
    };
}
