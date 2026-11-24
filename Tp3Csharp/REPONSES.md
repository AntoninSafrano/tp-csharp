# TP3 : réponses

## 1. Exceptions

**Pourquoi intercepter `FormatException` et `ArgumentOutOfRangeException` plutôt que `Exception` ?**
On attrape seulement les erreurs qu'on sait traiter (ligne mal formée, note hors limites). `Exception` attraperait aussi les bugs et les erreurs imprévues, et les ferait passer pour une simple ligne invalide.

**`double.TryParse` ou `double.Parse` dans un `try` ?**
`TryParse`. Une note mal écrite est un cas prévu, un `if` suffit et ne coûte rien. Ça permet aussi de lancer notre propre `FormatException` avec le numéro de ligne et le texte fautif.

**`throw;` ou `throw e;` ?**
`throw;` relance l'erreur en gardant la pile d'appels d'origine. `throw e;` la remet à zéro à partir du `catch`, et on ne sait plus où l'erreur est née. Il faut utiliser `throw;`.

## 2. JSON

**Q8 : comment s'écrivent les propriétés dans `promo.json` ?**
En camelCase (`nom`, `valeur`…), à cause de `JsonSerializerDefaults.Web` qui suit la convention du Web (JavaScript).

**Q10 : `promo.json` abîmé**
`Deserialize` lance une `JsonException`. On l'attrape, on affiche un message, on relit `notes.csv` et on réécrit `promo.json`.

**Q11 : une note à 30 dans `promo.json`**
Le JSON est valide, mais le record `Note` refuse la valeur et lance une `ArgumentOutOfRangeException`. C'est voulu, aucune note invalide n'entre dans le programme. En revanche, cette exception n'est pas une `JsonException`, donc elle n'est pas attrapée et le programme s'arrête.

**Quand `Deserialize` renvoie-t-il `null` ?**
Quand le fichier contient le JSON `null`. On le traite avec `?? new List<Note>()` pour toujours renvoyer une liste.

**Pourquoi `Path.Combine` ?**
Il met le bon séparateur selon le système (`/` sous Linux et dans Docker, `\` sous Windows). Un chemin écrit à la main peut ne pas fonctionner ailleurs.

## 3. Async

**Q13 : un `await` supprimé**
Avertissement CS4014 : l'appel n'est pas attendu et le programme continue avant la fin de l'écriture. Le fichier peut être incomplet, et une erreur passerait inaperçue.

**Q14-15 : mesures**
Environ 3000 ms l'un après l'autre, environ 1000 ms avec `Task.WhenAll` : les trois attentes se font en même temps.

**Q16 : `.Result` à la place de `await`**
Ça fonctionne (environ 3 s), mais `.Result` bloque le thread pendant l'attente. Dans une application graphique ou un serveur, ça fige le programme et peut provoquer un blocage complet (deadlock). Les erreurs arrivent aussi emballées dans une `AggregateException`.

## 4. API geo.api.gouv.fr

**Q17 : type de `Population`**
`int?` : c'est un nombre entier d'habitants, et il peut manquer dans la réponse. `Code` est en `string`, car c'est un identifiant (ex. `2A004`).

**Q19 : code postal 99999**
L'API renvoie une liste vide `[]`. La méthode renvoie `communes ?? []`, donc jamais `null`.

**Q22 : parallèle ou l'un après l'autre**
Environ 35 ms en parallèle contre environ 150 ms l'un après l'autre. La première vague en parallèle est plus lente, car elle ouvre plusieurs connexions au serveur.

**Q23 : adresse invalide**
`HttpRequestException`. Elle est attrapée dans `VilleAsync`, qui affiche « service injoignable ».

**Q24 : délai dépassé**
`TaskCanceledException`. Elle est attrapée dans `VilleAsync`, qui affiche « délai dépassé ».

**Pourquoi un seul `HttpClient` ?**
Chaque `HttpClient` ouvre ses propres connexions. En créer un par requête gaspille des connexions et peut finir par les épuiser. Un seul client réutilise les connexions déjà ouvertes.

**Q21 : pourquoi une erreur ne doit pas faire échouer les autres requêtes ?**
Un étudiant sans commune ne doit pas empêcher d'afficher les autres. Chaque requête passe par `VilleAsync`, qui attrape ses propres erreurs et renvoie un texte. `Task.WhenAll` ne reçoit donc jamais d'exception.
