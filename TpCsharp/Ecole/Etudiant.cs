namespace Ecole;

public class Etudiant : IEquatable<Etudiant>
{
    private List<double> _notes = new List<double>();
    private List<int> _coefs = new List<int>();
    private int _annee;

    public string Nom { get; }
    public string Prenom { get; }
    public DateOnly Naissance { get; init; }

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

    public string NomComplet => $"{Prenom} {Nom.ToUpper()}";

    public int NbNotes => _notes.Count;

    public IReadOnlyList<double> Notes => _notes;

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

    public bool Equals(Etudiant? autre)
    {
        if (autre is null)
        {
            return false;
        }
        return Nom == autre.Nom && Prenom == autre.Prenom && Naissance == autre.Naissance;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Etudiant);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Nom, Prenom, Naissance);
    }

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
