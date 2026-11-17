namespace Ecole;

public class Promo
{
    private List<Etudiant> _etudiants = new List<Etudiant>();

    public string Nom { get; }

    public Promo(string nom)
    {
        Nom = nom;
    }

    public void Inscrire(Etudiant e)
    {
        _etudiants.Add(e);
    }

    public Etudiant? Chercher(string nom)
    {
        foreach (Etudiant e in _etudiants)
        {
            if (e.Nom == nom)
            {
                return e;
            }
        }
        return null;
    }

    public Etudiant? Major()
    {
        Etudiant? major = null;

        foreach (Etudiant e in _etudiants)
        {
            if (major is null || e.Moyenne() > major.Moyenne())
            {
                major = e;
            }
        }
        return major;
    }

    public double MoyenneGenerale()
    {
        if (_etudiants.Count == 0)
        {
            return 0;
        }

        double somme = 0;
        foreach (Etudiant e in _etudiants)
        {
            somme = somme + e.Moyenne();
        }
        return somme / _etudiants.Count;
    }

    public void AfficherReleve()
    {
        Console.WriteLine($"Relevé de la promo {Nom}");

        foreach (Etudiant e in _etudiants)
        {
            Console.WriteLine($"{e} {Outils.Mention(e.Moyenne())}, moyenne pondérée {e.MoyennePonderee():F2}");
        }

        Console.WriteLine($"Moyenne générale : {MoyenneGenerale():F2}");
    }
}
