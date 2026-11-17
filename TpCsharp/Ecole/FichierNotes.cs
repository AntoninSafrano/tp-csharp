using System.Globalization;

namespace Ecole;

public static class FichierNotes
{
    public static Note LireLigne(string ligne, int numero)
    {
        string[] champs = ligne.Split(',');

        if (champs.Length != 5)
        {
            throw new NoteInvalideException($"il faut 5 champs, il y en a {champs.Length}", numero);
        }

        if (!double.TryParse(champs[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double valeur))
        {
            throw new NoteInvalideException($"\"{champs[3]}\" n'est pas un nombre", numero);
        }

        if (valeur < 0 || valeur > 20)
        {
            throw new NoteInvalideException($"la note {valeur} n'est pas entre 0 et 20", numero);
        }

        if (!int.TryParse(champs[4], out int coef))
        {
            throw new NoteInvalideException($"\"{champs[4]}\" n'est pas un coefficient valide", numero);
        }

        return new Note(champs[0], champs[1], champs[2], valeur, coef);
    }

    public static List<Note> Lire(string chemin)
    {
        List<Note> notes = new List<Note>();

        using StreamReader lecteur = new StreamReader(chemin);

        lecteur.ReadLine();
        int numero = 1;

        string? ligne = lecteur.ReadLine();
        while (ligne is not null)
        {
            numero = numero + 1;
            try
            {
                notes.Add(LireLigne(ligne, numero));
            }
            catch (NoteInvalideException e)
            {
                Console.Error.WriteLine($"Ligne {e.Ligne} ignorée : {e.Message}");
            }
            ligne = lecteur.ReadLine();
        }

        return notes;
    }
}
