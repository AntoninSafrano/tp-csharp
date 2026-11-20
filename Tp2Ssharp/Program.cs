using System.Globalization;

// etape 0 lecture des fichiers

// on garde le numero de chaque ligne avec la note ou null si la ligne est pas bonne
var lignes = File.ReadLines("notes.csv")
    .Skip(1)
    .Select((ligne, i) => (Numero: i + 2, Note: Parser(ligne)))
    .ToList();

// OfType enleve les null et donne direct des Note sans le point d interrogation
var notes = lignes.Select(l => l.Note).OfType<Note>().ToList();

// bonus 0 on affiche les lignes pas valides sur la sortie d erreur
foreach (var l in lignes.Where(l => l.Note is null))
{
    Console.Error.WriteLine($"Ligne {l.Numero} invalide");
}

// les inscrits on suppose que toutes les lignes sont bonnes
var inscrits = File.ReadLines("etudiants.csv")
    .Skip(1)
    .Select(ligne => ligne.Split(','))
    .Select(c => new Inscrit(c[0], c[1], c[2]))
    .ToList();

Console.WriteLine($"{notes.Count} notes et {inscrits.Count} inscrits");


// etape 1 delegues et lambdas

// 1.1 le meme filtre avec une methode nommee puis des lambdas
Console.WriteLine($"Notes en Algo : {Filtrer(notes, EstEnAlgo).Count}");
Console.WriteLine($"Notes sous 10 : {Filtrer(notes, n => n.Valeur < 10).Count}");

// avec var ca compile pas CS8917 le compilateur peut pas deviner le type de n
Func<Note, bool> bonnesNotesAda = n => n.Prenom == "Ada" && n.Valeur >= 15;
Console.WriteLine($"Notes d'Ada >= 15 : {Filtrer(notes, bonnesNotesAda).Count}");

// 1.2 Func renvoie une valeur et Action renvoie rien
Func<Note, double> points = n => n.Valeur * n.Coef;
Action<Note> afficher = n => Console.WriteLine(Court(n));
Console.WriteLine($"Points de la première note : {points(notes[0])}");
afficher(notes[0]);

// AuDessusDe fabrique une lambda avec le seuil qu on lui donne
Console.WriteLine($"Notes >= 14 : {Filtrer(notes, AuDessusDe(14)).Count}");

// on trie une copie pour pas toucher a notes
// meme matiere on compare les valeurs a l envers sinon on compare les matieres
var copie = new List<Note>(notes);
copie.Sort((a, b) => a.Matiere == b.Matiere ? b.Valeur.CompareTo(a.Valeur) : string.CompareOrdinal(a.Matiere, b.Matiere));
copie.RemoveAll(n => n.Matiere == "Reseau");
Console.WriteLine("Copie triée sans Réseau :");
copie.ForEach(afficher);

// 1.3 la lambda capture la variable seuil et pas sa valeur
double seuil = 12;
Func<Note, bool> admis = n => n.Valeur >= seuil;
Console.WriteLine($"Admis avec seuil 12 : {Filtrer(notes, admis).Count}");
seuil = 15;
Console.WriteLine($"Admis avec seuil 15 : {Filtrer(notes, admis).Count}");

// pour garder 12 on passe par AuDessusDe qui capture son propre parametre
Func<Note, bool> admisA12 = AuDessusDe(12);
seuil = 18;
Console.WriteLine($"Admis à 12 même si seuil change : {Filtrer(notes, admisA12).Count}");


// etape 2 premieres requetes plus de boucle pour calculer

// 2.1 filtrer projeter trier
var algo = notes.Where(n => n.Matiere == "Algo").OrderByDescending(n => n.Valeur);
Console.WriteLine($"Algo : {string.Join(", ", algo.Select(Court))}");

var matieres = notes.Select(n => n.Matiere).Distinct().Order();
Console.WriteLine($"Matières : {string.Join(", ", matieres)}");

var sousDixAnonyme = notes.Where(n => n.Valeur < 10).Select(n => new { n.Prenom, n.Valeur });
var sousDixTuple = notes.Where(n => n.Valeur < 10).Select(n => (n.Prenom, n.Valeur));
Console.WriteLine($"Sous 10 : {string.Join(", ", sousDixAnonyme)} et {string.Join(", ", sousDixTuple)}");

// deux OrderBy de suite le dernier gagne donc c est trie par prenom d abord
var mauvaisClassement = notes.OrderBy(n => n.Matiere).OrderByDescending(n => n.Valeur).OrderBy(n => n.Prenom);
var classement = notes
    .OrderBy(n => n.Matiere)
    .ThenByDescending(n => n.Valeur)
    .ThenBy(n => n.Prenom);
Console.WriteLine($"Avec deux OrderBy : {string.Join(", ", mauvaisClassement.Take(4).Select(Court))}");
Console.WriteLine($"Avec ThenBy : {string.Join(", ", classement.Take(4).Select(Court))}");

var parValeur = notes.OrderByDescending(n => n.Valeur);
Console.WriteLine($"Top 3 : {string.Join(", ", parValeur.Take(3).Select(Court))}");
Console.WriteLine($"4e à 6e : {string.Join(", ", parValeur.Skip(3).Take(3).Select(Court))}");

// 2.2 agreger
Console.WriteLine($"{notes.Count()} notes dont {notes.Count(n => n.Valeur < 10)} sous 10 moyenne {notes.Average(n => n.Valeur):F2}");

// Max donne juste le nombre et MaxBy donne la note entiere
Console.WriteLine($"Max : {notes.Max(n => n.Valeur)} MaxBy : {Court(notes.MaxBy(n => n.Valeur)!)}");
// ponderee c est la somme des valeur fois coef divisee par la somme des coefs
Console.WriteLine($"Moyenne pondérée : {notes.Sum(n => n.Valeur * n.Coef) / notes.Sum(n => n.Coef):F2}");

var prenoms = notes.Select(n => n.Prenom).Distinct();
Console.WriteLine(prenoms.Aggregate((acc, p) => acc + ", " + p));
Console.WriteLine(string.Join(", ", prenoms));

// 2.3 chercher et tester
var linusBd = notes.FirstOrDefault(n => n.Prenom == "Linus" && n.Matiere == "BD");
Console.WriteLine($"Linus en BD : {(linusBd is null ? "null" : Court(linusBd))}");
try
{
    notes.First(n => n.Prenom == "Linus" && n.Matiere == "BD");
}
catch (InvalidOperationException e)
{
    Console.WriteLine($"First : {e.Message}");
}

Console.WriteLine($"Ada en Algo : {Court(notes.Single(n => n.Prenom == "Ada" && n.Matiere == "Algo"))}");
try
{
    notes.Single(n => n.Prenom == "Ada");
}
catch (InvalidOperationException e)
{
    Console.WriteLine($"Single : {e.Message}");
}

Console.WriteLine($"Une note sous 10 : {notes.Any(n => n.Valeur < 10)}");
Console.WriteLine($"Tous au moins 12 en BD : {notes.Where(n => n.Matiere == "BD").All(n => n.Valeur >= 12)}");
Console.WriteLine($"All sur une liste vide : {new List<Note>().All(n => n.Valeur > 100)}");


// etape 3 regrouper et joindre

// 3.1 chaque groupe est un IGrouping avec une cle et ses notes
foreach (var groupe in notes.GroupBy(n => n.Matiere))
{
    Console.WriteLine($"{groupe.Key} : {string.Join(", ", groupe.Select(Court))}");
}

var stats = notes
    .GroupBy(n => n.Matiere)
    .Select(g => new
    {
        Matiere = g.Key,
        Nb = g.Count(),
        Moyenne = g.Average(n => n.Valeur),
        Min = g.Min(n => n.Valeur),
        Max = g.Max(n => n.Valeur),
        Major = g.MaxBy(n => n.Valeur)!.Prenom
    })
    .OrderByDescending(s => s.Moyenne);
foreach (var s in stats)
{
    Console.WriteLine($"{s.Matiere} : {s.Nb} notes moyenne {s.Moyenne:F2} min {s.Min} max {s.Max} major {s.Major}");
}

var moyennes = notes
    .GroupBy(n => n.Prenom)
    .ToDictionary(g => g.Key, g => g.Sum(n => n.Valeur * n.Coef) / g.Sum(n => n.Coef));
Console.WriteLine(string.Join(", ", moyennes.OrderByDescending(m => m.Value).Select(m => $"{m.Key} {m.Value:F2}")));

// CountBy fait le GroupBy et le Count en une fois
var avecCountBy = notes.CountBy(n => n.Prenom);
var avecGroupBy = notes.GroupBy(n => n.Prenom).Select(g => (g.Key, Nb: g.Count()));
Console.WriteLine($"CountBy : {string.Join(", ", avecCountBy)}");
Console.WriteLine($"GroupBy : {string.Join(", ", avecGroupBy)}");

// 3.2 la cle c est le nom et le prenom ensemble dans un tuple
var releve = inscrits.Join(notes,
    i => (i.Nom, i.Prenom),
    n => (n.Nom, n.Prenom),
    (i, n) => new { n.Prenom, i.Promo, n.Matiere, n.Valeur });
Console.WriteLine($"Relevé : {releve.Count()} lignes");

var parPromo = releve
    .GroupBy(r => r.Promo)
    .Select(g => $"{g.Key} {g.Average(r => r.Valeur):F2}")
    .Order();
Console.WriteLine($"Moyenne par promo : {string.Join(", ", parPromo)}");

// margaret a aucune note donc la jointure interne la fait disparaitre
var sansNoteAny = inscrits.Where(i => !notes.Any(n => n.Nom == i.Nom && n.Prenom == i.Prenom));
var sansNoteGroupJoin = inscrits
    .GroupJoin(notes, i => (i.Nom, i.Prenom), n => (n.Nom, n.Prenom), (i, sesNotes) => new { i.Prenom, Nb = sesNotes.Count() })
    .Where(x => x.Nb == 0);
var sansNoteLeftJoin = inscrits
    .LeftJoin(notes, i => (i.Nom, i.Prenom), n => (n.Nom, n.Prenom), (i, n) => new { i.Prenom, Note = n })
    .Where(x => x.Note is null);
Console.WriteLine($"Sans note : {string.Join(", ", sansNoteAny.Select(i => i.Prenom))} / {string.Join(", ", sansNoteGroupJoin.Select(x => x.Prenom))} / {string.Join(", ", sansNoteLeftJoin.Select(x => x.Prenom))}");

// ToLookup donne une sequence vide pour un prenom qui existe pas
var index = notes.ToLookup(n => n.Prenom);
Console.WriteLine($"Notes de Grace : {string.Join(", ", index["Grace"].Select(Court))}");
Console.WriteLine($"Notes de Margaret : {index["Margaret"].Count()}");

// 3.3 SelectMany met tout a plat alors que Select donne une liste de listes
var bulletins = notes
    .GroupBy(n => n.Prenom)
    .Select(g => new Bulletin(g.Key, g.Select(n => n.Valeur).ToList()))
    .ToList();
IEnumerable<double> aPlat = bulletins.SelectMany(b => b.Valeurs);
IEnumerable<List<double>> pasAPlat = bulletins.Select(b => b.Valeurs);
var paires = bulletins.SelectMany(b => b.Valeurs, (b, v) => (b.Prenom, v));
Console.WriteLine($"A plat : {string.Join(", ", aPlat)} ({pasAPlat.Count()} listes avec Select)");
Console.WriteLine($"Paires : {string.Join(", ", paires.Take(4))}");


// etape 4 evaluation differee
// on travaille sur une copie pour pas ajouter margaret dans les vraies notes
var notes4 = new List<Note>(notes);
var bonnes = notes4.Where(n => n.Valeur >= 15);
Console.WriteLine(bonnes.Count());
notes4.Add(new Note("Hamilton", "Margaret", "Algo", 19, 3));
Console.WriteLine(bonnes.Count());
var figees = notes4.Where(n => n.Valeur >= 15).ToList();
notes4.Add(new Note("Hamilton", "Margaret", "BD", 20, 2));
Console.WriteLine(bonnes.Count() + " / " + figees.Count);

// experience 4.2 a 4.4 avec un affichage dans le Where
// on l a retiree une fois finie comme demande les resultats sont dans REPONSES.md
// var testees = notes.Where(n => { Console.WriteLine($"test de {n.Prenom}"); return n.Valeur >= 15; });
// Console.WriteLine(testees.Count());
// Console.WriteLine(testees.Count());
// Console.WriteLine(testees.ToList().Count);
// Console.WriteLine(testees.Count() > 0);
// Console.WriteLine(testees.Any());
// Console.WriteLine(notes.Where(n => n.Valeur >= 15).OrderBy(n => n.Valeur).Count());
// Console.WriteLine(notes.OrderBy(n => n.Valeur).Where(n => n.Valeur >= 15).Count());


// etape 5 la classe Promo reecrite en linq
var promo = new Promo("ESIEE-IT", notes);
Console.WriteLine($"Étudiants : {string.Join(", ", promo.Etudiants())}");
Console.WriteLine($"Moyenne générale : {promo.MoyenneGenerale():F2} major {promo.Major()}");
Console.WriteLine($"Moyenne de Margaret : {promo.Moyenne("Margaret")}");
Console.WriteLine($"Admis par matière : {string.Join(", ", promo.NbAdmisParMatiere())}");
foreach (var ligne in promo.Releve())
{
    Console.WriteLine(ligne);
}


// les fonctions

// transforme une ligne du csv en note et renvoie null si la ligne est pas bonne
static Note? Parser(string ligne)
{
    var c = ligne.Split(',');
    if (c.Length != 5) return null;
    if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out var v)) return null;
    if (v < 0 || v > 20) return null;
    if (!int.TryParse(c[4], out var coef)) return null;
    return new Note(c[0], c[1], c[2], v, coef);
}

// mon propre Where avec une boucle le seul endroit ou on a le droit
static List<Note> Filtrer(List<Note> source, Func<Note, bool> critere)
{
    var resultat = new List<Note>();
    foreach (Note n in source)
    {
        if (critere(n))
        {
            resultat.Add(n);
        }
    }
    return resultat;
}

static bool EstEnAlgo(Note n) => n.Matiere == "Algo";

// une methode qui renvoie une lambda
static Func<Note, bool> AuDessusDe(double seuil) => n => n.Valeur >= seuil;

// affichage court d une note genre Ada · Algo · 15
static string Court(Note n) => $"{n.Prenom} · {n.Matiere} · {n.Valeur}";
