# TP4 : Entity Framework Core, réponses

## 0. Mise en place

**Q2 : `dotnet ef --version`**
> Ta réponse :

## 1. Le modèle et la base

**Q5 : qu'a créé la migration ?**
> Ta réponse :

**Q6 : quelles tables, quelle clé étrangère, que deviennent les notes quand on supprime un étudiant ?**
> Ta réponse :

**Question : pourquoi les colonnes `Nom` ou `Matiere` sont-elles `NOT NULL` alors que vous ne l'avez écrit nulle part ?**
> Ta réponse :

**Question : qu'est-ce qui a permis à EF Core de deviner que `EtudiantId` est une clé étrangère ?**
> Ta réponse :

## 2. Insérer la promo

**Q7 : que s'affiche-t-il (avant, nombre de lignes, après) ?**
> Ta réponse :

**Q8 : que se passe-t-il à la seconde exécution, et pourquoi ?**
> Ta réponse :

## 3. Interroger la base

**Question : pourquoi Q12 donne-t-elle 0 sans `Include` ? Comment s'appelle le problème qu'on aurait si l'on chargeait les notes de chaque étudiant dans la boucle ?**
> Ta réponse :

**Question : Q13, que se passerait-il si l'on gardait Hamilton, qui n'a aucune note ?**
> Ta réponse :

## 4. Modifier, supprimer, faire évoluer

**Question : pourquoi n'a-t-on appelé aucune méthode `Update` à la question 14 ?**
> Ta réponse :

**Question : après `ExecuteUpdate`, les objets `Note` déjà chargés dans le contexte ont-ils leur nouveau coefficient ? Pourquoi ?**
> Ta réponse :
