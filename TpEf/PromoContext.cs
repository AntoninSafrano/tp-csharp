// ===== Q4 : le contexte =====
// le contexte, c'est la porte d'entrée vers la base de données
// on passe par lui pour lire (LINQ), ajouter, modifier, supprimer, puis enregistrer (SaveChanges)

using Microsoft.EntityFrameworkCore;      // DbContext, DbSet, UseSqlite
using Microsoft.Extensions.Logging;       // LogLevel, pour afficher le SQL

// héritage (séance 1) : notre contexte EST un DbContext, il récupère tout son fonctionnement
public class PromoContext : DbContext
{
    // un DbSet = une table : on peut l'interroger avec LINQ, comme une liste
    // => Set<Etudiant>() : propriété calculée qui demande la table au contexte
    public DbSet<Etudiant> Etudiants => Set<Etudiant>();
    public DbSet<Note> Notes => Set<Note>();

    // override (séance 1) : on redéfinit la méthode du parent pour configurer NOTRE base
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        // UseSqlite : la base est le fichier promo.db, à la racine du projet
        => options.UseSqlite("Data Source=promo.db")
                  // LogTo : affiche dans la console chaque requête SQL envoyée à la base
                  // (on pourra mettre cette ligne en commentaire pour alléger l'affichage)
                  .LogTo(Console.WriteLine, LogLevel.Information);
}
