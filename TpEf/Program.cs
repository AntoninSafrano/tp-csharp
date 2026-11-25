// ===== TP4 : Entity Framework Core =====

using Microsoft.EntityFrameworkCore;

// ===== étape 2 : insérer la promo (Q7 et Q8) =====

// using : le contexte est fermé (Dispose) à la fin du bloc, comme à la séance 3
using (var db = new PromoContext())
{
    // Any() : "la table Etudiants contient-elle au moins une ligne ?"
    // traduit en SQL par SELECT EXISTS (...) : on n'insère la promo que si la base est vide
    if (!db.Etudiants.Any())
    {
        var promo = Donnees.Promo();

        // AddRange : on CONFIE les 6 étudiants (et leurs notes) au contexte
        // rien n'est encore écrit dans la base !
        db.Etudiants.AddRange(promo);

        // l'état de suivi d'Ada aux yeux du contexte (change tracking)
        Console.WriteLine($"Avant : {db.Entry(promo[0]).State}");

        // À TOI (Q7) : remplace le 0 par l'appel qui ENREGISTRE vraiment dans la base
        // indice : c'est la méthode du contexte vue en cours ; elle renvoie le nombre de lignes écrites
        int n = 0;

        Console.WriteLine($"{n} lignes écrites");
        Console.WriteLine($"Après : {db.Entry(promo[0]).State}, Id = {promo[0].Id}");
    }
}
