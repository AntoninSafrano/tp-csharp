// la mention reprise de la seance 1
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
}
