// fichier fourni : les 6 étudiants et leurs notes
// Margaret Hamilton n'a aucune note (c'est voulu)

public static class Donnees
{
    // renvoie la liste des étudiants avec leurs notes
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

    // crée un étudiant et ajoute ses notes dans sa liste
    // params : on peut donner autant de notes qu'on veut (ou aucune)
    static Etudiant Creer(string nom, string prenom, string promo,
        params (string Matiere, double Valeur, int Coef)[] notes)
    {
        var e = new Etudiant { Nom = nom, Prenom = prenom, Promo = promo };
        foreach (var (m, v, c) in notes)
            e.Notes.Add(new Note { Matiere = m, Valeur = v, Coef = c });
        return e;
    }
}
