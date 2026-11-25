// Q4 : le contexte = la porte d'entrée vers la base

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PromoContext : DbContext
{
    // les deux tables
    public DbSet<Etudiant> Etudiants => Set<Etudiant>();
    public DbSet<Note> Notes => Set<Note>();

    // la base est le fichier promo.db, et on affiche le SQL dans la console
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=promo.db")
                  .LogTo(Console.WriteLine, LogLevel.Information);
}
