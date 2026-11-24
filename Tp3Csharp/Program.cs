
    // les boites d outils utilisees : Stopwatch, CultureInfo, JsonException, GetFromJsonAsync
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.Json;
    using System.Net.Http.Json;


    // Q14 et Q16 : 3 appels l un apres l autre avec .Result -> environ 3 s
    // .Result marche mais bloque le thread pendant l attente, await est preferable
    var chrono = Stopwatch.StartNew();
    var a1 = OperationLenteAsync("A").Result;
    var b1 = OperationLenteAsync("B").Result;
    var c1 = OperationLenteAsync("C").Result;
    Console.WriteLine($"temps ecoulé : {chrono.ElapsedMilliseconds} ms");

    // Q15 : on lance les 3 d abord puis on les attend ensemble -> environ 1 s
    var chrono2 = Stopwatch.StartNew();
    // sans await : on recupere 3 tickets (Task) qui tournent en meme temps
    var A = OperationLenteAsync("A");
    var B = OperationLenteAsync("B");
    var C = OperationLenteAsync("C");

    // on attend que les 3 soient finis
    await Task.WhenAll(A, B, C);

    Console.WriteLine($"when all: {chrono2.ElapsedMilliseconds} ms");


    // etape 0 : lecture des inscrits (c[3] = le code postal ajoute)
    var inscrits = File.ReadLines("etudiants.csv")
        // saute la ligne d en-tete
        .Skip(1)
        // decoupe chaque ligne aux virgules
        .Select(ligne => ligne.Split(','))
        .Select(c => new Inscrit(c[0], c[1], c[2], c[3]))
        // on fige le resultat dans une vraie liste (le fichier n est lu qu une fois)
        .ToList();

    //on attends 6
    Console.WriteLine($"{inscrits.Count} inscrit");

    // tests de l etape 1.1 : chacune de ces lignes plante exprès (note 24, coef -1)
    //new Note("Lovelance", "Ada", "Algo", 24, 2);

    //new Note("Lovelance", "Ada", "Algo", 24, -1);

    // Q18 : un seul HttpClient pour tout le programme
    // Q23 : pour tester, remplacer l adresse par https://geo.api.gouv.invalid/
    var http = new HttpClient { BaseAddress = new Uri("https://geo.api.gouv.fr/") };

    // Q24 : 3 secondes maximum pour chaque groupe de requetes
    // pour tester, remplacer par TimeSpan.FromMilliseconds(1)
    var delaiMax = TimeSpan.FromSeconds(3);
    using var delai = new CancellationTokenSource();

    // Q19 : 35000 donne Rennes, 29200 donne Brest, 99999 donne une liste vide
    delai.CancelAfter(delaiMax); // le chrono des 3 s demarre ici
    // un code qui existe, un autre qui existe, un qui n existe pas
    foreach (var cp in new[] { "35000", "29200", "99999" })
    {
        Console.WriteLine($"{cp} : {await VilleAsync(cp)}");
    }

    // Q20 et Q21 : la commune de chaque inscrit, toutes les requetes en meme temps
    delai.CancelAfter(delaiMax); // on relance le chrono des 3 s pour ce groupe
    // 1. Select transforme chaque inscrit en ticket (pas de await ici, on lance tout)
    var tickets = inscrits.Select(i => VilleAsync(i.CodePostal));

    // 2. on attend les 6 tickets ensemble, villes[0] va avec inscrits[0], etc.
    var villes = await Task.WhenAll(tickets);

    // 3. on affiche chaque inscrit avec sa ville
    for (int i = 0; i < inscrits.Count; i++)
    {
        var inscrit = inscrits[i];
        Console.WriteLine($"{inscrit.Prenom} {inscrit.Nom} ({inscrit.Promo}) : {villes[i]}");
    }

    // Q22 : on compare la version parallele et la version l une apres l autre
    // mesure faite apres la Q20 : les connexions au serveur sont deja ouvertes,
    // sinon la premiere version paie en plus le temps d ouverture des connexions
    delai.CancelAfter(delaiMax);
    var chronoParallele = Stopwatch.StartNew();
    // les 6 requetes partent ensemble, on attend la plus lente
    await Task.WhenAll(inscrits.Select(i => VilleAsync(i.CodePostal)));
    Console.WriteLine($"requetes en parallele : {chronoParallele.ElapsedMilliseconds} ms");

    delai.CancelAfter(delaiMax);
    var chronoSuite = Stopwatch.StartNew();
    // les 6 requetes une par une : on additionne les temps
    foreach (var inscrit in inscrits)
    {
        await VilleAsync(inscrit.CodePostal);
    }
    Console.WriteLine($"requetes l une apres l autre : {chronoSuite.ElapsedMilliseconds} ms");
    //on attends 13 valide

    // Q9 : on charge promo.json s il existe, sinon on importe le csv et on cree promo.json
    List<Note> notes;

    // File.Exists renvoie true ou false
    if (File.Exists("promo.json"))
    {
        try
        {
            // await : on attend la lecture du disque sans bloquer
            notes = await Stockage.ChargerAsync("promo.json");
        }
        catch(JsonException e)
        {
            // Q10 : json abime -> message clair, on repart du csv et on repare promo.json
            Console.Error.WriteLine("Erreur lecture json :" + e.Message);
            notes =  ChargerNotes("notes.csv");
            await Stockage.SauverAsync("promo.json", notes);
        }

    }
    // pas de promo.json : on lit le csv puis on cree le json pour la prochaine fois
    else
    {
        notes = ChargerNotes("notes.csv");
        await Stockage.SauverAsync("promo.json", notes);
    }
    Console.WriteLine($"{notes.Count} nbres de notes");


    // Q4 : transforme une ligne du csv en Note, FormatException si la ligne est mauvaise
    // une note hors 0-20 est refusee par le record lui meme (ArgumentOutOfRangeException)
    static Note ParserStrict(string ligne, int numero)
    {
        // c[0] nom, c[1] prenom, c[2] matiere, c[3] note, c[4] coef
        var c = ligne.Split(',');
        // porte 1 : le texte doit avoir la bonne forme, sinon FormatException
        if (c.Length != 5) throw new FormatException($"Ligne {numero} champs manquants : {c.Length}");
        // TryParse essaie sans planter, InvariantCulture = le point comme separateur decimal
        if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out var v)) throw new FormatException($"Ligne {numero} note invalide {c[3]}");
        if (!int.TryParse(c[4], out var coef)) throw new FormatException($"Ligne {numero} coefss invalide {c[4]}");
        // porte 2 : le record verifie les valeurs (0-20 et coef > 0)
        return new Note(c[0], c[1], c[2], v, coef);
    }

    // Q5 : lit le csv, garde les notes valides, signale chaque ligne rejetee sur Console.Error
    static List<Note> ChargerNotes(string chemin)
    {
        // le bac vide ou on range les notes valides
        List<Note> notes = new List<Note>();
        int numero = 2;
    foreach (var ligne in File.ReadLines(chemin).Skip(1))
        {
            // si ParserStrict plante, on saute directement au catch
            try{var note = ParserStrict(ligne, numero);
            notes.Add(note);}
            // erreur de forme (champs manquants, note pas numerique)
            catch (FormatException e)
            {
                Console.Error.WriteLine($"messaage :{e.Message} ligne {numero}");
            }
            // erreur de valeur lancee par le record (note 25, coef 0)
            catch(ArgumentOutOfRangeException e){Console.Error.WriteLine($"messaage :{e.Message} ligne {numero}");}
            // hors du try pour avancer a chaque tour
            numero ++;
        }
        return notes;
    }





// simule une operation lente d 1 seconde (donnee par le sujet)
static async Task<string> OperationLenteAsync(string nom)
{
await Task.Delay(1000); // simule 1 seconde d'attente réseau
return $"{nom} terminée";
}

// Q19 : les communes d un code postal, liste vide si aucune (jamais null)
// pas static : elle utilise http et delai de la zone du haut
async Task<List<Commune>> CommunesAsync(string codePostal)
{
    // envoie la requete et transforme la reponse json en liste de Commune
    // delai.Token : la requete s arrete si le delai est depasse
    var communes = await http.GetFromJsonAsync<List<Commune>>($"communes?codePostal={codePostal}", delai.Token);
    // plan B : liste vide si la reponse est null
    return communes ?? [];
}

// Q21, Q23, Q24 : le nom de la commune, ou un texte clair si ca n a pas marche
// chaque requete attrape ses propres erreurs, donc une erreur ne fait pas echouer les autres
async Task<string> VilleAsync(string codePostal)
{
    try
    {
        var communes = await CommunesAsync(codePostal);
        // liste vide -> FirstOrDefault donne null -> ?. evite le plantage -> ?? donne le texte
        return communes.FirstOrDefault()?.Nom ?? "commune inconnue";
    }
    catch (HttpRequestException e)
    {
        // Q23 : adresse fausse ou pas de reseau
        Console.Error.WriteLine($"Service injoignable pour {codePostal} : {e.Message}");
        return "service injoignable";
    }
    catch (TaskCanceledException)
    {
        // Q24 : le delai de 3 secondes est depasse
        Console.Error.WriteLine($"Delai depasse pour {codePostal}");
        return "delai depasse";
    }
}
