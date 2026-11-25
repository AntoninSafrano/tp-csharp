TP4 Entity Framework Core réponses


0. Mise en place

Q2
L'outil répond Entity Framework Core .NET Command-line Tools en version 10.0.15.


1. Le modèle et la base

Q5
La migration crée un dossier Migrations avec 3 fichiers et le fichier promo.db à la racine du projet.

Q6
Deux tables sont créées Etudiants et Notes.
La clé étrangère FK_Notes_Etudiants_EtudiantId est sur la colonne EtudiantId de Notes.
Elle est en ON DELETE CASCADE donc supprimer un étudiant supprime aussi ses notes.
Un index IX_Notes_EtudiantId est créé.
Les colonnes Nom Prenom Promo et Matiere sont NOT NULL.
La table __EFMigrationsHistory contient une ligne InitialCreate.

Pourquoi Nom et Matiere sont NOT NULL
Elles sont de type string et pas string? donc EF Core les considère obligatoires.
Pour accepter null il faudrait écrire string?.

Comment EF Core devine que EtudiantId est une clé étrangère
C'est une convention de nommage.
Le nom est celui de la navigation Etudiant suivi de Id donc EF Core fait le lien tout seul.


2. Insérer la promo

Q7
Le programme affiche Avant Added puis 19 lignes écrites puis Après Unchanged avec Id 1.
19 lignes car il y a 6 étudiants et 13 notes.
Avant SaveChanges les objets sont seulement suivis par le contexte.
SaveChanges écrit tout dans la base et la base donne un Id à chaque ligne.

Q8
A la seconde exécution rien n'est inséré.
Les données sont déjà dans promo.db donc Any renvoie vrai.
On voit seulement la requête SELECT EXISTS dans le journal.


3. Interroger la base

Q9
Résultat Hamilton Margaret puis Hopper Grace puis Liskov Barbara.
Le SQL fait un WHERE sur la promo M2 et un ORDER BY sur le nom.

Q10
Résultat Lovelace 3 Turing 3 Hopper 3 Torvalds 2 Liskov 2 Hamilton 0.
Le SQL compte les notes avec une sous requête SELECT COUNT.
Les notes ne sont pas chargées.

Q11
Résultat Algo 13,25 BD 14,5 Reseau 13,6.
Le SQL contient GROUP BY et AVG donc le calcul est fait par la base.
Seulement 3 lignes reviennent.

Q12
Sans Include tout le monde a 0 note.
Avec Include on a les vrais nombres 3 3 3 2 2 0.
Le second SQL contient un LEFT JOIN sur Notes.

Q13
Résultat Hopper 16,67 Lovelace 15,17 Liskov 13,67 Torvalds 12,25 Turing 10,33.
Hamilton n'apparaît pas.

Pourquoi Q12 donne 0 sans Include et quel problème aurait on en chargeant les notes dans la boucle
Sans Include EF Core charge seulement la table Etudiants donc les listes de notes restent vides.
Include ajoute un LEFT JOIN pour charger les notes en même temps.
Charger les notes dans la boucle ferait une requête pour les étudiants plus une requête par étudiant.
C'est le problème N+1.

Que se passerait il si on gardait Hamilton
Hamilton n'a aucune note donc sa somme de coefficients vaut 0.
On diviserait par 0 et le SQL renverrait NULL.
EF Core ne peut pas mettre NULL dans un double donc le programme plante avec une InvalidOperationException.
C'est pour ça qu'on garde seulement les étudiants qui ont au moins une note.


4. Modifier, supprimer, faire évoluer

Pourquoi on n'appelle aucune méthode Update à la question 14


Après ExecuteUpdate les notes déjà chargées ont elles leur nouveau coefficient et pourquoi

