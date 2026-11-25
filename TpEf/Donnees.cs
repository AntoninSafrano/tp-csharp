// ===== fichier fourni par le sujet : les données de la promo =====
// les mêmes étudiants et les mêmes notes qu'aux séances précédentes
// Margaret Hamilton n'a aucune note : c'est volontaire

public static class Donnees
{
    // renvoie les 6 étudiants, chacun avec ses notes déjà rangées dans sa liste Notes
    // [ ... ] : expression de collection, elle crée la List<Etudiant>
    public static List<Etudiant> Promo() =>
    [
        Creer("Lovelace", "Ada", "M1",
              ("Algo", 15, 3), ("BD", 16, 2), ("Reseau", 14, 1)),
        Creer("Turing", "Alan", "M1",
              ("Algo", 9, 3), ("BD", 12, 2), ("Reseau", 11, 1)),
        Creer("Hopper", "Grace", "M2",
              ("Algo", 18, 3), ("BD", 17, 2), ("Reseau", 12, 1)),
        Creer("Torvalds", "Linus", "M1",
              ("Algo", 11, 3), ("Reseau", 16, 1)),
        Creer("Liskov", "Barbara", "M2",
              ("BD", 13, 2), ("Reseau", 15, 1)),
        Creer("Hamilton", "Margaret", "M2"),
    ];

    // fabrique un étudiant et ses notes
    // params : on peut passer autant de notes qu'on veut, séparées par des virgules (ou aucune)
    // chaque note est un tuple (Matiere, Valeur, Coef)
    static Etudiant Creer(string nom, string prenom, string promo,
        params (string Matiere, double Valeur, int Coef)[] notes)
    {
        // initialiseur d'objet : on crée l'étudiant et on remplit ses propriétés
        var e = new Etudiant { Nom = nom, Prenom = prenom, Promo = promo };

        // on déballe chaque tuple en 3 variables, et on ajoute la note à SA liste
        // pas besoin de remplir EtudiantId : EF Core le déduira de la liste au moment d'enregistrer
        foreach (var (m, v, c) in notes)
            e.Notes.Add(new Note { Matiere = m, Valeur = v, Coef = c });

        return e;
    }
}
