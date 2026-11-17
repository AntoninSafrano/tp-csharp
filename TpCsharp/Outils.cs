public static class Outils
{
    public static string Mention(double moyenne) => moyenne switch
    {
        < 0 or > 20 => throw new ArgumentOutOfRangeException(nameof(moyenne)),
        >= 16 => "Très bien",
        >= 14 => "Bien",
        >= 12 => "Assez bien",
        >= 10 => "Passable",
        _ => "Ajourné"
    };

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
