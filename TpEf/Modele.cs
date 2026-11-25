// Q3 : les entités (une classe = une table, une propriété = une colonne)

// un étudiant
public class Etudiant
{
    // clé primaire (reconnue grâce au nom Id)
    public int Id { get; set; }

    // string sans ? : jamais null, donc colonne NOT NULL
    public string Nom { get; set; } = "";
    public string Prenom { get; set; } = "";
    public string Promo { get; set; } = "";

    // ses notes (un étudiant a plusieurs notes)
    public List<Note> Notes { get; set; } = [];
}

// une note
public class Note
{
    // clé primaire
    public int Id { get; set; }

    public string Matiere { get; set; } = "";
    public double Valeur { get; set; }
    public int Coef { get; set; }

    // clé étrangère : l'Id de l'étudiant de cette note
    public int EtudiantId { get; set; }

    // l'étudiant de cette note (rempli par EF Core)
    public Etudiant Etudiant { get; set; } = null!;
}
