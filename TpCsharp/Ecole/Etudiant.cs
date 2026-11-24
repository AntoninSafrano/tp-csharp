namespace Ecole;

// un etudiant avec ses notes et ses coefs
public class Etudiant
{
    // les notes sont privees on passe par AjouterNote pour les verifier
    private List<double> _notes = new List<double>();
    private List<int> _coefs = new List<int>();
    private int _annee;

    public string Nom { get; }
    public string Prenom { get; }
    // init donc on peut la donner juste a la creation
    public DateOnly Naissance { get; init; }

    // annee entre 1 et 5 sinon exception
    public int Annee
    {
        get { return _annee; }
        set
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "L'année doit être entre 1 et 5");
            }
            _annee = value;
        }
    }

    // proprietes calculees
    public string NomComplet => $"{Prenom} {Nom.ToUpper()}";

    public int NbNotes => _notes.Count;

    // on peut lire les notes mais pas les modifier
    public IReadOnlyList<double> Notes => _notes;

    // le nom et le prenom peuvent pas etre vides
    public Etudiant(string nom, string prenom, int annee)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            throw new ArgumentException("Le nom ne peut pas être vide");
        }
        if (string.IsNullOrWhiteSpace(prenom))
        {
            throw new ArgumentException("Le prénom ne peut pas être vide");
        }

        Nom = nom;
        Prenom = prenom;
        Annee = annee;
    }

    // la note doit etre entre 0 et 20 et le coef positif
    public void AjouterNote(double note, int coef = 1)
    {
        if (note < 0 || note > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(note), "La note doit être entre 0 et 20");
        }
        if (coef <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coef), "Le coefficient doit être positif");
        }
        _notes.Add(note);
        _coefs.Add(coef);
    }

    // moyenne simple et 0 si pas de note
    public double Moyenne()
    {
        if (_notes.Count == 0)
        {
            return 0;
        }

        double somme = 0;
        foreach (double note in _notes)
        {
            somme = somme + note;
        }
        return somme / _notes.Count;
    }

    // somme des note fois coef divisee par la somme des coefs
    public double MoyennePonderee()
    {
        if (_notes.Count == 0)
        {
            return 0;
        }

        double somme = 0;
        int totalCoefs = 0;
        for (int i = 0; i < _notes.Count; i++)
        {
            somme = somme + _notes[i] * _coefs[i];
            totalCoefs = totalCoefs + _coefs[i];
        }
        return somme / totalCoefs;
    }

    // affiche le prenom le nom en majuscule le niveau et la moyenne
    public override string ToString()
    {
        string niveau;
        if (Annee <= 3)
        {
            niveau = $"L{Annee}";
        }
        else
        {
            niveau = $"M{Annee - 3}";
        }

        return $"{NomComplet} ({niveau}) : {Moyenne():F2}";
    }
}
