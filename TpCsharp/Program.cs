using Ecole;
using Geometrie;

// etape 1 et 2 en commentaire pour que le programme demande pas tout a chaque fois
/*Console.Write("Ton prénom : ");
string? prenom = Console.ReadLine();

Console.Write("Ton année de naissance : ");
string? texteAnnee = Console.ReadLine();

int annee = int.Parse(texteAnnee);

int age = DateTime.Now.Year - annee;

Console.WriteLine($"Bonjour {prenom}, tu as {age} ans !");*/


// 2.2 si on tape rien ReadLine donne null donc on met 0
string? saisie = Console.ReadLine();
Console.WriteLine($"Vous avez tapé {saisie?.Length ?? 0} caractères");
/*
if (saisie is null)
{
    Console.WriteLine($"Vous avez tapé 0 caractères");
    return;
}
else
{
    Console.WriteLine($"Vous avez tapé {saisie.Length} caractères");
}
Console.Write("Ton prénom : ");
string? prenom = Console.ReadLine();

int age;
bool valide;

do
{
    Console.Write("Ton année de naissance : ");
    string? texteAnnee = Console.ReadLine();

    valide = int.TryParse(texteAnnee, out int annee);   

    if (valide)
    {
        age = DateTime.Now.Year - annee;
        valide = age >= 0 && age <= 120;                 
    }
    else
    {
        age = 0;
    }

    if (!valide)
    {
        Console.WriteLine("Année invalide, recommence.");
    }
}
while (!valide);

Console.WriteLine($"Bonjour {prenom}, tu as {age} ans !");
*/

// etape 3

// 3.1 la mention avec un switch
Console.WriteLine(Outils.Mention(17));
Console.WriteLine(Outils.Mention(13));
Console.WriteLine(Outils.Mention(8));

// 3.2 moyenne min et max avec une boucle
List<double> notes = [12, 15.5, 9, 18];
double somme = 0;
double min = notes[0];
double max = notes[0];

foreach (double note in notes)
{
    somme = somme + note;
    if (note < min)
    {
        min = note;
    }
    if (note > max)
    {
        max = note;
    }
}

double moyenne = somme / notes.Count;
Console.WriteLine($"Moyenne : {moyenne:F2} {Outils.Mention(moyenne)}");
Console.WriteLine($"Min : {min} Max : {max}");

// 3.3 un dictionnaire prenom vers ses notes
Dictionary<string, List<double>> dico = new Dictionary<string, List<double>>();
dico["Ada"] = [15, 16];
dico["Alan"] = [9, 12];
dico["Grace"] = [17, 14, 13];

dico["Alan"].Add(14);

foreach (var (nom, notesEtudiant) in dico)
{
    double moy = Outils.Moyenne(notesEtudiant);
    Console.WriteLine($"{nom} : {notesEtudiant.Count} notes, moyenne {moy:F2}, {Outils.Mention(moy)}");
}

// 3.4 dico avec une cle qui existe pas ca plante avec KeyNotFoundException
// Console.WriteLine(dico["Inconnu"]);
// TryGetValue plante pas il renvoie false

if (dico.TryGetValue("Inconnu", out List<double>? notesInconnu))
{
    Console.WriteLine($"Inconnu a {notesInconnu.Count} notes");
}
else
{
    Console.WriteLine("Inconnu n'existe pas");
}

// 3.5 le pattern matching selon le type
Console.WriteLine(Outils.Decrire(4));
Console.WriteLine(Outils.Decrire(7));
Console.WriteLine(Outils.Decrire(""));
Console.WriteLine(Outils.Decrire("bonjour"));
Console.WriteLine(Outils.Decrire(new List<double> { 1, 2, 3 }));
Console.WriteLine(Outils.Decrire(null));
Console.WriteLine(Outils.Decrire(3.14));


// etape 4 les classes Etudiant et Promo
var ada = new Etudiant("Lovelace", "Ada", 4)
{
    Naissance = new DateOnly(1815, 12, 10)
};
ada.AjouterNote(15);
ada.AjouterNote(16);
Console.WriteLine(ada);
ada.Annee = 5;

// la promo avec trois etudiants

var promo = new Promo("M1 Info");
promo.Inscrire(ada);

var alan = new Etudiant("Turing", "Alan", 4);
alan.AjouterNote(9);
alan.AjouterNote(12);
promo.Inscrire(alan);

var grace = new Etudiant("Hopper", "Grace", 4);
grace.AjouterNote(17);
grace.AjouterNote(18);
promo.Inscrire(grace);

promo.AfficherReleve();

// Chercher et Major peuvent renvoyer null donc on verifie
Etudiant? trouve = promo.Chercher("Turing");
if (trouve is not null)
{
    Console.WriteLine($"Trouvé : {trouve.NomComplet}");
}

Etudiant? major = promo.Major();
if (major is not null)
{
    Console.WriteLine($"Major : {major.NomComplet}");
}

// etape 5 heritage et polymorphisme
// Sort utilise CompareTo donc les formes sont triees par aire
List<Forme> formes = [new Cercle(1), new Rectangle(2, 5), new Carre(3), new Cercle(2.5)];
formes.Sort();
foreach (var f in formes)
{
    f.Dessiner();
}

// aire totale et plus grand perimetre avec une boucle
double aireTotale = 0;
Forme plusGrandPerimetre = formes[0];
foreach (var f in formes)
{
    aireTotale = aireTotale + f.Aire();
    if (f.Perimetre() > plusGrandPerimetre.Perimetre())
    {
        plusGrandPerimetre = f;
    }
}
Console.WriteLine($"Aire totale : {aireTotale:F2}");
Console.WriteLine($"Plus grand périmètre : {plusGrandPerimetre.Decrire()}");

// avec override les deux affichent le rayon
Cercle cercle = new Cercle(1);
Forme forme = cercle;
Console.WriteLine(cercle.Decrire());
Console.WriteLine(forme.Decrire());

// etape 6 records et exceptions
// un record compare les valeurs donc note1 == note2 donne true
var note1 = new Note("Lovelace", "Ada", "Algo", 15, 3);
var note2 = new Note("Lovelace", "Ada", "Algo", 15, 3);
Console.WriteLine(note1 == note2);
Console.WriteLine(note1);
// with fait une copie en changeant juste la valeur
var note3 = note1 with { Valeur = 18 };
Console.WriteLine(note3);

// Etudiant est une classe donc == compare les adresses et donne false
var ada2 = new Etudiant("Lovelace", "Ada", 4)
{
    Naissance = new DateOnly(1815, 12, 10)
};
var ada3 = new Etudiant("Lovelace", "Ada", 4)
{
    Naissance = new DateOnly(1815, 12, 10)
};
Console.WriteLine(ada2 == ada3);

// on lit le fichier et on remplit une promo les lignes fausses sont ignorees
try
{
    List<Note> notesFichier = FichierNotes.Lire("notes.csv");

    var promoFichier = new Promo("M1 Info");
    foreach (Note n in notesFichier)
    {
        Etudiant? etudiant = promoFichier.Chercher(n.Nom);
        if (etudiant is null)
        {
            etudiant = new Etudiant(n.Nom, n.Prenom, 4);
            promoFichier.Inscrire(etudiant);
        }
        etudiant.AjouterNote(n.Valeur, n.Coef);
    }

    promoFichier.AfficherReleve();
}
catch (FileNotFoundException)
{
    Console.WriteLine("Le fichier notes.csv est introuvable");
}
