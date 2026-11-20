namespace Ecole;

// une promo avec sa liste d etudiants
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

    // cherche par nom et renvoie null si on trouve pas
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

    // l etudiant avec la meilleure moyenne et null si la promo est vide
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

    // moyenne des moyennes et 0 si la promo est vide
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

    // affiche chaque etudiant avec sa mention puis la moyenne generale
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
