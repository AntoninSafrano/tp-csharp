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
