namespace Geometrie;

public class Rectangle : Forme
{
    public double Largeur { get; }
    public double Hauteur { get; }

    public Rectangle(double largeur, double hauteur) : this("Rectangle", largeur, hauteur)
    {
    }

    protected Rectangle(string nom, double largeur, double hauteur) : base(nom)
    {
        if (largeur <= 0 || hauteur <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(largeur), "Les dimensions doivent être strictement positives");
        }
        Largeur = largeur;
        Hauteur = hauteur;
    }

    public override double Aire()
    {
        return Largeur * Hauteur;
    }

    public override double Perimetre()
    {
        return 2 * (Largeur + Hauteur);
    }
}
