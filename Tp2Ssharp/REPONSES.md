# TP séance 2 délégués lambdas et LINQ : réponses

etape 0

0.3.1 on lit avec File.ReadLines on saute l'en tete avec Skip(1) on passe chaque ligne dans Parser avec Select et on enleve les null
ca donne 13 notes et 6 inscrits la ligne de Barbara en Algo avec abc est ecartée

0.3.2 avec Where(n => n is not null) le type reste IEnumerable<Note?> le compilateur sait pas que le Where a enlevé les null donc apres il faudrait mettre des ! partout pour faire n.Valeur
avec OfType<Note>() on a direct un IEnumerable<Note> sans le ? parce que OfType garde que ce qui est vraiment une Note et null c'est pas une Note donc c'est plus pratique plus de warning nullable

0.3.3 pareil pour etudiants.csv Skip(1) puis un Select qui fait le Split puis un Select qui fait new Inscrit pas besoin de filtre vu que toutes les lignes sont bonnes

0.3.4 bonus avec Select((ligne, i) => …) on a l'index i commence a 0 et compte apres le Skip donc le numero de ligne dans le fichier c'est i + 2 (un pour l'en tete un parce que ca commence a 0)
j'ai gardé le numero avec la note dans un tuple et j'affiche les lignes null avec un foreach comme ca y a pas de Console.Error dans le Select
ca affiche Ligne 14 invalide

etape 1

1.1 Filtrer c'est mon propre Where avec un foreach et un if (critere(n)) le seul endroit ou on fait une boucle
avec EstEnAlgo 4 notes avec la lambda < 10 1 note avec la lambda Ada >= 15 2 notes
EstEnAlgo et n => n.Matiere == "Algo" font pareil l'un a un nom l'autre non

1.1.3 la lambda rangée dans une variable a le type Func<Note, bool> (prend une Note rend un bool)
var c = n => n.Valeur >= 15; compile pas erreur CS8917 impossible de déduire le type délégué
avec var le compilateur doit deviner le type mais rien dans la ligne dit que n est une Note donc il peut pas
dans Filtrer(notes, n => …) ca marche parce qu'il lit le type Func<Note, bool> dans la signature de Filtrer

1.2 Func<Note, double> points = n => n.Valeur * n.Coef donne 45 pour Ada en Algo (15 x 3)
Action<Note> afficher affiche Ada · Algo · 15 et renvoie rien c'est pour ca que c'est une Action et pas une Func
AuDessusDe(seuil) renvoie une lambda Filtrer(notes, AuDessusDe(14)) donne 7 notes
pour le tri j'ai copié la liste avec new List<Note>(notes) parce que Sort modifie la liste puis Sort avec une comparaison par matiere puis par valeur decroissante et RemoveAll pour enlever Reseau il reste 8 notes et notes a toujours 13 notes

1.3 je pensais 10 puis 6 et c'est bien 10 puis 6
la lambda capture la variable seuil pas sa valeur au moment ou on l'ecrit donc quand seuil passe a 15 la lambda voit 15
en java ca compile pas une variable capturée doit etre effectivement final donc seuil = 15 est interdit
pour garder 12 il faut capturer une variable que personne change par exemple AuDessusDe(12) chaque appel a son propre parametre seuil donc meme si on change seuil apres ca reste 12 (ca donne toujours 10)

etape 2

2.1 Algo triées par valeur decroissante Grace 18 Ada 15 Linus 11 Alan 9
matieres sans doublon triées Algo BD Reseau avec Select Distinct Order
sous 10 en type anonyme { Prenom = Alan, Valeur = 9 } et en tuple (Alan, 9)
avec deux OrderBy de suite c'est le dernier qui gagne donc c'est trié par prenom (Ada Ada Ada Alan…) le premier tri sert juste a departager parce que le tri est stable
avec ThenBy ca marche Algo d'abord Grace 18 Ada 15 Linus 11 Alan 9 puis BD etc
top 3 Grace 18 Grace 17 Ada 16 puis de la 4e a la 6e avec Skip(3).Take(3) Linus 16 Ada 15 Barbara 15 (Linus est avant Ada car le tri est stable et dans le fichier Ada BD 16 est avant Linus)

2.2 13 notes 1 sous 10 (Alan en Algo) moyenne simple 13,77
Max donne juste 18 alors que MaxBy donne la note entiere Grace Algo 18 donc avec MaxBy on sait qui a la note
moyenne ponderée de toutes les notes 13,72 (somme valeur x coef divisé par somme des coefs)
Aggregate et string.Join donnent pareil Ada, Alan, Grace, Linus, Barbara je prefere string.Join c'est plus lisible et Aggregate recree une string a chaque fois

2.3 FirstOrDefault pour Linus en BD renvoie null car il a pas de note en BD
First plante avec InvalidOperationException Sequence contains no matching element
Single pour Ada en Algo marche (une seule note) mais Single sur toutes les notes d'Ada plante InvalidOperationException Sequence contains more than one matching element car Ada a 3 notes Single veut exactement un element
Any < 10 donne true (Alan 9) All pour BD >= 12 donne true (Ada 16 Alan 12 Grace 17 Barbara 13 Linus a pas de note en BD)
All sur une sequence vide renvoie true y a aucun element qui contredit la condition

etape 3

3.1 un groupe c'est un IGrouping<string, Note> il a une Key (la matiere) et c'est lui meme une sequence de notes
stats par matiere triées par moyenne decroissante BD 4 notes 14,50 Reseau 5 notes 13,60 Algo 4 notes 13,25 avec min max et le major Grace en Algo et en BD Linus en Reseau
moyennes ponderées dans un Dictionary avec ToDictionary Grace 16,67 Ada 15,17 Barbara 13,67 Linus 12,25 Alan 10,33
CountBy donne direct des paires prenom nombre ([Ada, 3]…) avec GroupBy il faut faire Select(g => (g.Key, g.Count())) derriere meme resultat CountBy est plus court

3.2 jointure sur le nom et le prenom avec un tuple comme clé (i.Nom, i.Prenom) ca donne 13 lignes
moyenne par promo M1 13,00 M2 15,00
Margaret apparait pas car Join c'est une jointure interne elle a aucune note donc elle a pas de correspondance et elle disparait
je la retrouve avec Where et !notes.Any(…) puis avec GroupJoin (elle a 0 notes dans son groupe) et avec LeftJoin de .NET 10 (sa note est null)
ToLookup fait un index prenom vers notes pour Grace 3 notes Algo 18 BD 17 Reseau 12 pour un prenom absent comme Margaret ca plante pas ca renvoie une sequence vide

3.3 bulletins avec GroupBy puis Select new Bulletin
SelectMany met tout a plat ca donne un IEnumerable<double> avec les 13 valeurs alors que Select donne un IEnumerable<List<double>> donc 5 listes une par etudiant
la deuxieme forme SelectMany(b => b.Valeurs, (b, v) => (b.Prenom, v)) donne les paires (Ada, 15) (Ada, 16)…

etape 4

4.1 ca affiche 6 puis 7 puis 8 / 7
6 au debut il y a 6 notes >= 15
7 car la requete bonnes est une recette elle se recalcule a chaque Count et voit la note de Margaret 19 qu'on vient d'ajouter
8 / 7 bonnes voit aussi la 2eme note de Margaret (20) alors que figees a été calculée avec ToList avant l'ajout donc elle bouge plus
je l'ai fait sur une copie de la liste pour pas ajouter Margaret dans les vraies notes

4.2 avec un affichage dans le Where et deux Count() chaque note est testée 2 fois 26 tests pour 13 notes
avec ToList() d'abord chaque note est testée une seule fois 13 tests et apres on lit juste la liste

4.3 Count() > 0 teste les 13 notes alors que Any() s'arrete au premier qui marche donc 1 seul test (Ada Algo 15 est la premiere)

4.4 Where avant ou apres OrderBy le resultat change pas (les 6 memes notes dans le meme ordre)
mais avec Where avant seulement 6 notes passent par le tri et avec Where apres les 13 passent par le tri donc faut filtrer tot
l'affichage dans le Where je l'ai retiré apres l'experience il est en commentaire dans Program.cs

etape 5

5.1 chaque methode de Promo fait une seule expression avec =>
si l'etudiant a aucune note et qu'on divise les deux Sum ca fait 0 / 0 et avec des double ca plante pas ca donne NaN
pour l'eviter Moyenne renvoie 0 si l'etudiant a pas de note et MoyenneGenerale utilise DefaultIfEmpty(0) pour une promo vide sinon Average plante
Moyenne c'est la seule methode avec deux lignes au lieu d'un => parce qu'il faut verifier qu'il y a des notes avant de diviser

5.2 Releve donne 1. Grace 16,67 Très bien 2. Ada 15,17 Bien 3. Barbara 13,67 Assez bien 4. Linus 12,25 Assez bien 5. Alan 10,33 Passable
avec Classement puis Select((e, i) => …) pour avoir le rang et Mention de la seance 1

5.3 comparaison avec la seance 1
la Promo de la seance 1 fait environ 70 lignes avec des foreach et des variables somme major etc celle en LINQ fait une ligne par methode
c'est plus lisible on lit ce qu'on veut (les etudiants leur moyenne le plus grand) au lieu de comment le calculer
et c'est plus facile a modifier par exemple changer le critere des admis c'est juste changer le >= 10
par contre faut connaitre les operateurs et pour un truc compliqué une boucle peut etre plus claire

5.4 oui Classement() est recalculée a chaque appel car elle renvoie un IEnumerable donc c'est differé et Releve ou un foreach la refont a chaque fois et chaque Moyenne refait un Where sur toutes les notes
ici c'est pas grave il y a 13 notes et les notes changent pas (on a fait ToList dans le constructeur)
faudrait la mettre en cache si y avait beaucoup de notes ou si on l'appelait souvent par exemple la calculer une fois avec ToList dans un champ
