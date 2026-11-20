namespace Ecole;

// exception quand une ligne du fichier est fausse on garde le numero de la ligne
public class NoteInvalideException : Exception
{
    public int Ligne { get; }

    public NoteInvalideException(string message, int ligne) : base(message)
    {
        Ligne = ligne;
    }
}
