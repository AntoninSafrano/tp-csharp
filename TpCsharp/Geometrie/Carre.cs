namespace Geometrie;

// un carre c est un rectangle avec deux cotes pareils sealed donc on peut plus en heriter
public sealed class Carre : Rectangle
{
    public Carre(double cote) : base("Carré", cote, cote)
    {
    }
}
