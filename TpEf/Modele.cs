// ===== Q3 : les entités =====
// une entité = une classe ordinaire qu'EF Core range dans une table
//   la classe Etudiant -> la table Etudiants
//   une propriété      -> une colonne
//   un objet           -> une ligne
// ce sont des classes avec { get; set; } (pas des records positionnels) :
// EF Core crée l'objet vide, puis remplit ses propriétés une par une

// un étudiant de la promo
public class Etudiant
{
    // la clé primaire : EF Core la reconnaît grâce à son nom "Id" (convention)
    // c'est la base qui lui donne sa valeur (1, 2, 3...) au moment de l'INSERT
    public int Id { get; set; }

    // string sans ? = jamais null, donc la colonne sera NOT NULL
    // = "" donne une valeur de départ, pour éviter l'avertissement du compilateur
    public string Nom { get; set; } = "";
    public string Prenom { get; set; } = "";
    public string Promo { get; set; } = "";

    // la navigation vers ses notes : "un étudiant a plusieurs notes" (relation un-à-plusieurs)
    // ce n'est pas une colonne : EF Core s'en sert pour relier les deux tables
    // = [] : la liste existe toujours (vide au départ), on peut faire Notes.Add tout de suite
    public List<Note> Notes { get; set; } = [];
}

// une note d'un étudiant
public class Note
{
    // la clé primaire de la table Notes
    public int Id { get; set; }

    public string Matiere { get; set; } = "";
    public double Valeur { get; set; }
    public int Coef { get; set; }

    // la clé étrangère : l'Id de l'étudiant à qui appartient la note
    // EF Core la devine grâce à son nom : nom de la navigation (Etudiant) + Id
    public int EtudiantId { get; set; }

    // la navigation inverse : "une note appartient à un étudiant"
    // null! veut dire au compilateur "fais-moi confiance, EF Core la remplira"
    public Etudiant Etudiant { get; set; } = null!;
}
