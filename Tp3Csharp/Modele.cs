// une note lue dans notes.csv
using System.Reflection.Metadata;

// Q2 : une note hors 0-20 leve une ArgumentOutOfRangeException des la creation
// Q3 : le coef doit etre > 0 (methode d aide ThrowIfNegativeOrZero)
public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef)
{
   public double Valeur {get; init;} = Valeur >= 0 && Valeur <= 20 ? Valeur : throw new ArgumentOutOfRangeException(nameof(Valeur), "Note pas comprise entre 0 et 20");
    
    // static car appelee pendant la creation de l objet
    static int Positif(int coef){
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(coef);
        return coef;
    }
    public int Coef { get; init; } = Positif(Coef) ;   
}
// un etudiant avec son code postal (etape 0)
public record Inscrit(string Nom, string Prenom, string Promo, string CodePostal);

// Q17 : Code en string (identifiant, ex 2A004), Population en int? (entier, peut manquer)
public record Commune(string Nom, string Code, int? Population);
