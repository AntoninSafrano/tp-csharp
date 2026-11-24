    using System.Text.Json;
    // etape 2 : sauvegarde et chargement des notes en json
    public static class Stockage
    {
    // options partagees : noms en camelCase (Web) et json indente
    private static readonly JsonSerializerOptions Options =
    new(JsonSerializerDefaults.Web) { WriteIndented = true };
    // Serialize = objet -> texte json, Deserialize = texte json -> objet (?? si null)
    public static void Sauver(string chemin, List<Note> notes) => File.WriteAllText(chemin, JsonSerializer.Serialize(notes, Options));
    public static List<Note> Charger(string chemin) => JsonSerializer.Deserialize<List<Note>>( File.ReadAllText(chemin) , Options ) ?? new List<Note>();

     // etape 3 : versions async, le await porte sur l acces au disque
     public static async Task SauverAsync(string chemin, List<Note> notes) => await File.WriteAllTextAsync(chemin, JsonSerializer.Serialize(notes, Options));
    public static async Task<List<Note>> ChargerAsync(string chemin) => JsonSerializer.Deserialize<List<Note>>(await File.ReadAllTextAsync(chemin) , Options ) ?? new List<Note>();
    public record Commune(string Nom, string Code, int? Population);
    
    }