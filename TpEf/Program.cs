using Microsoft.EntityFrameworkCore;

// étape 2 : insérer la promo

// using : le contexte est fermé à la fin du bloc
using (var db = new PromoContext())
{
    // Q8 : on insère seulement si la table est vide
    if (!db.Etudiants.Any())
    {
        var promo = Donnees.Promo();

        // on donne les étudiants au contexte (rien n'est encore écrit en base)
        db.Etudiants.AddRange(promo);
        Console.WriteLine($"Avant : {db.Entry(promo[0]).State}");

        // Q7 : SaveChanges écrit tout dans la base et renvoie le nombre de lignes écrites
        int n = db.SaveChanges();
        Console.WriteLine($"{n} lignes écrites");

        // après l'enregistrement, Ada a reçu son Id
        Console.WriteLine($"Après : {db.Entry(promo[0]).State}, Id = {promo[0].Id}");
    }
}

// interroger la base

// un nouveau contexte : il ne connaît encore aucun objet
using (var db = new PromoContext())
{
    // Q9 : les étudiants de M2, triés par nom
    Console.WriteLine("Q9");
    var m2 = db.Etudiants.Where(e => e.Promo == "M2").OrderBy(e => e.Nom);
    Console.WriteLine(m2.ToQueryString());   // affiche le SQL sans exécuter la requête
    foreach (var e in m2.ToList())           // ToList : la requête part ici
        Console.WriteLine($"{e.Nom} {e.Prenom}");

    // Q10 : chaque étudiant et son nombre de notes (projection, sans Include)
    Console.WriteLine("Q10");
    var nbNotes = db.Etudiants
        .Select(e => new { e.Nom, Nb = e.Notes.Count })   // le comptage est fait en SQL
        .ToList();
    foreach (var x in nbNotes)
        Console.WriteLine($"{x.Nom} {x.Nb}");

    // Q11 : la moyenne de chaque matière
    Console.WriteLine("Q11");
    var parMatiere = db.Notes
        .GroupBy(n => n.Matiere)
        .Select(g => new { Matiere = g.Key, Moyenne = g.Average(n => n.Valeur) })
        .ToList();
    foreach (var m in parMatiere)
        Console.WriteLine($"{m.Matiere} {m.Moyenne}");

    // Q12 : e.Notes.Count sans Include, puis avec Include
    Console.WriteLine("Q12 sans Include");
    foreach (var e in db.Etudiants.ToList())          // charge les étudiants sans leurs notes
        Console.WriteLine($"{e.Nom} {e.Notes.Count}");

    Console.WriteLine("Q12 avec include");
    foreach (var e in db.Etudiants.Include(e => e.Notes).ToList())   // charge aussi les notes
        Console.WriteLine($"{e.Nom} {e.Notes.Count}");

    // Q13 : moyenne pondérée des étudiants qui ont au moins une note, de la meilleure à la moins bonne
    Console.WriteLine("Q13");
    var classement = db.Etudiants
        .Where(e => e.Notes.Any())    // on écarte Hamilton (aucune note)
        .Select(e => new
        {
            e.Nom,
            Moyenne = e.Notes.Sum(n => n.Valeur * n.Coef) / e.Notes.Sum(n => n.Coef)
        })
        .OrderByDescending(x => x.Moyenne)
        .ToList();
    foreach (var x in classement)
        Console.WriteLine($"{x.Nom} {x.Moyenne:F2}");   
}

// étape 4 : modifier, supprimer, faire évoluer
// attention : ces changements restent dans promo.db
// pour repartir de zéro : supprimer promo.db puis dotnet ef database update

using (var db = new PromoContext())
{
    // Q14 : Alan Turing passe en M2
    Console.WriteLine("Q14");
    var alan = db.Etudiants.FirstOrDefault(e => e.Nom == "Turing");
    if (alan is not null)
    {
        alan.Promo = "M2";                                     // on change juste la propriété
        Console.WriteLine($"État : {db.Entry(alan).State}");   // Modified
        db.SaveChanges();                                      // UPDATE de la colonne Promo
    }

    // Q15 : on supprime Linus Torvalds, ses notes partent avec lui
    Console.WriteLine("Q15");
    var linus = db.Etudiants.FirstOrDefault(e => e.Nom == "Torvalds");
    if (linus is not null)
    {
        db.Etudiants.Remove(linus);
        db.SaveChanges();                                      // DELETE
    }
    Console.WriteLine($"Notes restantes : {db.Notes.Count()}");

    // Q16 : coef 2 pour toutes les notes de Reseau, en une seule requête
    Console.WriteLine("Q16");
    // on charge la note de Reseau d'Ada avant, pour voir si elle change après
    var noteAda = db.Notes.FirstOrDefault(n => n.Matiere == "Reseau" && n.Etudiant.Nom == "Lovelace");
    // ExecuteUpdate envoie un seul UPDATE tout de suite, sans SaveChanges
    int nb = db.Notes.Where(n => n.Matiere == "Reseau")
                     .ExecuteUpdate(s => s.SetProperty(n => n.Coef, 2));
    Console.WriteLine($"{nb} lignes modifiées");
    // l'objet déjà chargé garde son ancien coef car ExecuteUpdate ne passe pas par le contexte
    Console.WriteLine($"Coef de la note d'Ada en mémoire : {noteAda?.Coef}");

    // Q17 : la colonne Email ajoutée par la migration AjoutEmail
    Console.WriteLine("Q17");
    foreach (var e in db.Etudiants.ToList())
        Console.WriteLine($"{e.Nom} Email = {e.Email ?? "null"}");
}
