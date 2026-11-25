TP4 Entity Framework Core réponses

Etape 0 Mise en place

Q2

L'outil répond Entity Framework Core NET Command-line Tools en version 10.0.15.

Etape 1 Le modèle et la base

Q5

La migration crée un dossier Migrations avec 3 fichiers.

Elle crée aussi le fichier promo.db à la racine du projet.

Q6

Deux tables sont créées Etudiants et Notes.

La clé étrangère est la colonne EtudiantId de la table Notes.

Elle est en suppression en cascade donc supprimer un étudiant supprime aussi ses notes.

Un index est créé sur EtudiantId.

Les colonnes Nom Prenom Promo et Matiere sont obligatoires en NOT NULL.

Une table d'historique des migrations contient une ligne InitialCreate.

Pourquoi Nom et Matiere sont NOT NULL

Elles sont de type string sans point d'interrogation.

EF Core les considère donc comme obligatoires.

Pour accepter une valeur vide il faudrait ajouter un point d'interrogation après string.

Comment EF Core devine que EtudiantId est une clé étrangère

C'est une convention de nommage.

Le nom est celui de la navigation Etudiant suivi de Id.

EF Core fait donc le lien tout seul entre les deux tables.

Etape 2 Insérer la promo

Q7

Le programme affiche Avant Added puis 19 lignes écrites puis Après Unchanged avec Id 1.

Il y a 19 lignes car il y a 6 étudiants et 13 notes.

Avant SaveChanges les objets sont seulement suivis par le contexte.

SaveChanges écrit tout dans la base et la base donne un Id à chaque ligne.

Q8

A la seconde exécution rien n'est inséré.

Les données sont déjà dans promo.db donc Any renvoie vrai.

On voit seulement une requête SELECT EXISTS dans le journal.

Etape 3 Interroger la base

Q9

Le résultat est Hamilton Margaret puis Hopper Grace puis Liskov Barbara.

Le SQL fait un WHERE sur la promo M2 et un ORDER BY sur le nom.

Q10

Le résultat est Lovelace 3 Turing 3 Hopper 3 Torvalds 2 Liskov 2 Hamilton 0.

Le SQL compte les notes avec une sous requête SELECT COUNT.

Les notes ne sont pas chargées.

Q11

Le résultat est Algo 13,25 BD 14,5 Reseau 13,6.

Le SQL contient GROUP BY et AVG donc le calcul est fait par la base.

Seulement 3 lignes reviennent.

Q12

Sans Include tout le monde a 0 note.

Avec Include on a les vrais nombres 3 3 3 2 2 0.

Le second SQL contient un LEFT JOIN sur la table Notes.

Q13

Le résultat est Hopper 16,67 Lovelace 15,17 Liskov 13,67 Torvalds 12,25 Turing 10,33.

Hamilton n'apparaît pas.

Pourquoi Q12 donne 0 sans Include et quel problème aurait on en chargeant les notes dans la boucle

Sans Include EF Core charge seulement la table Etudiants donc les listes de notes restent vides.

Include ajoute un LEFT JOIN pour charger les notes en même temps.

Charger les notes dans la boucle ferait une requête pour les étudiants plus une requête par étudiant.

C'est le problème N plus 1.

Que se passerait il si on gardait Hamilton

Hamilton n'a aucune note donc sa somme de coefficients vaut 0.

On diviserait par 0 et le SQL renverrait NULL.

EF Core ne peut pas mettre NULL dans un double donc le programme plante avec une InvalidOperationException.

C'est pour ça qu'on garde seulement les étudiants qui ont au moins une note.

Etape 4 Modifier, supprimer, faire évoluer

Q14

Avant SaveChanges l'état d'Alan est Modified.

Le journal montre un UPDATE sur la table Etudiants qui change seulement la colonne Promo.

Q15

Il reste 11 notes.

Linus avait 2 notes et elles sont supprimées avec lui grâce à la suppression en cascade.

Q16

4 lignes sont modifiées.

Ce sont les notes de Reseau d'Ada Alan Grace et Barbara.

Pas besoin de SaveChanges car ExecuteUpdate s'exécute tout de suite.

Q17

La migration AjoutEmail contient seulement un ajout de colonne.

Les étudiants sont toujours là et leur Email est vide car la colonne accepte les valeurs nulles.

Pourquoi on n'appelle aucune méthode Update à la question 14

Le contexte suit les objets qu'il a chargés.

Au moment de SaveChanges il voit que la promo d'Alan a changé.

Il génère alors tout seul un UPDATE sur cette seule colonne.

Après ExecuteUpdate les notes déjà chargées ont elles leur nouveau coefficient et pourquoi

Non elles gardent l'ancien coefficient.

ExecuteUpdate envoie la modification directement à la base sans passer par le contexte.

Les objets déjà en mémoire ne sont donc pas mis à jour.

Dans le programme la note de Reseau d'Ada chargée avant garde un coefficient de 1 alors que la base contient 2.
