
Console.Write("Ton prénom kijkj : ");
string? prenom = Console.ReadLine();

Console.Write("Ton année de naissance : ");
string? texteAnnee = Console.ReadLine();

int annee = int.Parse(texteAnnee);

int age = DateTime.Now.Year - annee;

Console.WriteLine($"Bonjour {prenom}, tu as {age} ans !");