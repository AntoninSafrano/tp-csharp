namespace Geometrie;

// la classe mere de toutes les formes on peut pas faire new Forme
public abstract class Forme : IDessinable, IComparable<Forme>
{
    public string Nom { get; }

    protected Forme(string nom)
    {
        Nom = nom;
    }

    // chaque forme doit calculer son aire et son perimetre
    public abstract double Aire();

    public abstract double Perimetre();

    // virtual donc les filles peuvent la redefinir avec override
    public virtual string Decrire()
    {
        return $"{Nom} : aire {Aire():F2}, périmètre {Perimetre():F2}";
    }

    // pour trier les formes par aire
    public int CompareTo(Forme? autre)
    {
        if (autre is null)
        {
            return 1;
        }
        return Aire().CompareTo(autre.Aire());
    }

    public void Dessiner()
    {
        Console.WriteLine(Decrire());
    }
}
