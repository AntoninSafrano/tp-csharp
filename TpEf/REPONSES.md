# TP4 : Entity Framework Core, réponses

## 0. Mise en place

**Q2 : `dotnet ef --version`**
L'outil répond « Entity Framework Core .NET Command-line Tools », version 10.0.15.

## 1. Le modèle et la base

**Q5 : qu'a créé la migration ?**
Un dossier `Migrations` avec 3 fichiers (la migration, son Designer et le snapshot du modèle), et le fichier `promo.db` à la racine.

**Q6 : tables, clé étrangère, suppression**
- Deux tables : `Etudiants` et `Notes`.
- Clé étrangère `FK_Notes_Etudiants_EtudiantId` sur `Notes.EtudiantId`, avec `ON DELETE CASCADE` : supprimer un étudiant supprime aussi ses notes.
- Un index `IX_Notes_EtudiantId` est créé.
- Dans le script SQL, `Nom`, `Prenom`, `Promo` et `Matiere` sont `NOT NULL`.
- La table `__EFMigrationsHistory` contient une ligne : `InitialCreate`.

**Pourquoi `Nom` et `Matiere` sont-elles `NOT NULL` ?**
Parce qu'elles sont de type `string` et non `string?`. Avec les types annulables activés, EF Core en déduit que la valeur est obligatoire. Pour autoriser null, il faudrait écrire `string?`.

**Comment EF Core devine-t-il que `EtudiantId` est une clé étrangère ?**
Par convention de nommage : le nom de la navigation `Etudiant`, suivi de `Id`. La classe `Note` a aussi une navigation `Etudiant`, donc EF Core relie les deux tables tout seul.

## 2. Insérer la promo

**Q7 : résultat**
`Avant : Added`, puis `19 lignes écrites` (6 étudiants et 13 notes), puis `Après : Unchanged, Id = 1`. Avant `SaveChanges`, les objets sont seulement suivis par le contexte. `SaveChanges` envoie les `INSERT` dans une transaction, puis la base donne un `Id` à chaque ligne.

**Q8 : seconde exécution**
Rien n'est inséré : les données sont maintenant dans le fichier `promo.db`, donc `db.Etudiants.Any()` renvoie vrai. Le journal ne montre que la requête `SELECT EXISTS (…)`.

## 3. Interroger la base

**Pourquoi Q12 donne-t-elle 0 sans `Include` ? Comment s'appelle le problème qu'on aurait si l'on chargeait les notes de chaque étudiant dans la boucle ?**
>

**Q13 : que se passerait-il si l'on gardait Hamilton, qui n'a aucune note ?**
>

## 4. Modifier, supprimer, faire évoluer

**Pourquoi n'a-t-on appelé aucune méthode `Update` à la question 14 ?**
>

**Après `ExecuteUpdate`, les objets `Note` déjà chargés dans le contexte ont-ils leur nouveau coefficient ? Pourquoi ?**
>
