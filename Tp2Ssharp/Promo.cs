// la promo de la seance 1 reecrite avec une seule expression linq par methode
public class Promo(string nom, IEnumerable<Note> notes)
{
    // on fige les notes une fois pour toutes avec ToList
    private readonly List<Note> _notes = notes.ToList();

    public string Nom { get; } = nom;

    // les prenoms sans doublon dans l ordre du fichier
    public IEnumerable<string> Etudiants() => _notes.Select(n => n.Prenom).Distinct();

    // moyenne ponderee de l etudiant et 0 si il a aucune note
    // sinon ca ferait 0 divise par 0 et ca donne NaN
    public double Moyenne(string prenom)
    {
        var sesNotes = _notes.Where(n => n.Prenom == prenom).ToList();
        return sesNotes.Count == 0 ? 0 : sesNotes.Sum(n => n.Valeur * n.Coef) / sesNotes.Sum(n => n.Coef);
    }

    // moyenne des moyennes des etudiants et 0 si la promo est vide
    public double MoyenneGenerale() => Etudiants().Select(Moyenne).DefaultIfEmpty(0).Average();

    // prenom du meilleur et null si la promo est vide
    public string? Major() => Etudiants().MaxBy(Moyenne);

    // prenom et moyenne du meilleur au moins bon
    public IEnumerable<(string Prenom, double Moyenne)> Classement() =>
        Etudiants().Select(p => (Prenom: p, Moyenne: Moyenne(p))).OrderByDescending(e => e.Moyenne);

    // pour chaque matiere le nombre de notes au dessus de 10
    public IReadOnlyDictionary<string, int> NbAdmisParMatiere() =>
        _notes.GroupBy(n => n.Matiere).ToDictionary(g => g.Key, g => g.Count(n => n.Valeur >= 10));

    // une ligne par etudiant dans l ordre du classement
    public IEnumerable<string> Releve() =>
        Classement().Select((e, i) => $"{i + 1}. {e.Prenom} {e.Moyenne:F2} {Outils.Mention(e.Moyenne)}");
}
