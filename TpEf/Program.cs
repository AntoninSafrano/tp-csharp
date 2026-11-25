// TP4 : Entity Framework Core

using Microsoft.EntityFrameworkCore;

// étape 2 : insérer la promo (Q7 et Q8)

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

// étape 3 : interroger la base (Q9 à Q13)

// un nouveau contexte : il ne connaît encore aucun objet
using (var db = new PromoContext())
{
    // Q9 : les étudiants de M2, triés par nom
    Console.WriteLine("\n===== Q9 =====");
    var m2 = db.Etudiants.Where(e => e.Promo == "M2").OrderBy(e => e.Nom);
    Console.WriteLine(m2.ToQueryString());   // affiche le SQL sans exécuter la requête
    foreach (var e in m2.ToList())           // ToList : la requête part ici
        Console.WriteLine($"{e.Nom} {e.Prenom}");

    // Q10 : chaque étudiant et son nombre de notes (projection, sans Include)
    Console.WriteLine("\n===== Q10 =====");
    var nbNotes = db.Etudiants
        .Select(e => new { e.Nom, Nb = e.Notes.Count })   // le comptage est fait en SQL
        .ToList();
    foreach (var x in nbNotes)
        Console.WriteLine($"{x.Nom} {x.Nb}");

    // Q11 : la moyenne de chaque matière
    Console.WriteLine("\n===== Q11 =====");
    var parMatiere = db.Notes
        .GroupBy(n => n.Matiere)
        .Select(g => new { Matiere = g.Key, Moyenne = g.Average(n => n.Valeur) })
        .ToList();
    foreach (var m in parMatiere)
        Console.WriteLine($"{m.Matiere} {m.Moyenne}");

    // Q12 : e.Notes.Count sans Include, puis avec Include
    Console.WriteLine("\n===== Q12 sans Include =====");
    foreach (var e in db.Etudiants.ToList())          // charge les étudiants SANS leurs notes
        Console.WriteLine($"{e.Nom} {e.Notes.Count}");

    Console.WriteLine("\n===== Q12 avec Include =====");
    foreach (var e in db.Etudiants.Include(e => e.Notes).ToList())   // charge AUSSI les notes
        Console.WriteLine($"{e.Nom} {e.Notes.Count}");

    // Q13 : moyenne pondérée des étudiants qui ont au moins une note, de la meilleure à la moins bonne
    Console.WriteLine("\n===== Q13 =====");
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
        Console.WriteLine($"{x.Nom} {x.Moyenne:F2}");   // F2 : deux décimales
}
