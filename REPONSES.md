# TP — Introduction à C# : réponses
dotnet. 10.0 est visé 
oui :     <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
ca aevolué avec les nouvelles version de dotnet ils font un cahce avec une seule pile 

▪ Pourquoi c1.X et s1.X n’ont-ils pas la même valeur ?

car y'en a un c'est une class donc ca fait une copie vers la meme adresse memoire donc si la valeur dans celle ci change ca changera aussi contrairement ou la struct qui fait une simple copie de la valeur 

▪ Que donnerait a == b en Java avec deux String ? Pourquoi C# répond-il différemment ?

en java ca compare les addresses memoires donc c'est false tandis qu'en c# ca compare les valeurs on obtient true 

Que signifie le suffixe m ? Dans quel type de programme le choix entre double et decimal est-il
important ?

m signifie que la valeur est decimal et non pas binaire par exemple dans les programmes de banques touchant à la finances par exemple 



etape 4

ToString est virtual dans object, quel mot clé pour la redefinir ? que se passe t il si on l'oublie ?

faut mettre override. si on l'oublie le compilateur met un warning (CS0114) qui dit que ca cache la methode de object au lieu de la redefinir, et comme on a TreatWarningsAsErrors ca compile plus. et meme sans ca, Console.WriteLine(ada) appelerait le ToString de object et afficherait juste "Ecole.Etudiant"

lire ada._notes dans Program.cs, message du compilateur ?

erreur CS0122 : 'Etudiant._notes' est inaccessible en raison de son niveau de protection. c'est parce que _notes est private donc on peut y acceder que dans la classe Etudiant

pourquoi pas exposer List<double> en public ? proposer Notes en IReadOnlyList<double>

si on met la liste en public n'importe qui peut faire ada.Notes.Add(50) ou Clear() et ca contourne la verif entre 0 et 20 de AjouterNote. avec public IReadOnlyList<double> Notes => _notes; on peut lire les notes (Count, notes[0], foreach) mais pas les modifier, faut passer par AjouterNote

4.2.3 visibilité : on enleve public devant class Promo, ca compile toujours ?

oui ca compile toujours. par defaut une classe c'est internal, ca veut dire visible dans tout le projet (l'assembly) mais pas depuis un autre projet. ca poserait probleme avec un projet de tests separé genre TpCsharp.Tests qui pourrait plus utiliser Promo (sauf avec InternalsVisibleTo)


etape 5

5.1.4 on met sealed sur Carre et on essaie d'en deriver une classe quelle erreur

erreur CS0509 cannot derive from sealed type Carre sealed ca bloque l'heritage plus personne peut heriter de Carre comme final en java

5.1.5 pour Carre j'appelle le constructeur parent avec base mais je passe aussi le nom Carré sinon il s'appelait Rectangle dans l'affichage

5.2 les formes s'affichent par aire croissante

oui Sort utilise CompareTo qui compare les aires donc on a cercle 3.14 puis carré 9 puis rectangle 10 puis cercle 19.63

le texte de Cercle contient le rayon

oui parce que Cercle redefinit Decrire avec override et reutilise base.Decrire() puis rajoute le rayon et comme Dessiner appelle Decrire c'est la version de Cercle qui est appelée meme si la liste est une List de Forme

aire totale et plus grand perimetre avec une boucle

aire totale 41.78 et le plus grand perimetre c'est le cercle de rayon 2.5 avec 15.71

5.3.1 on enleve virtual de Decrire dans Forme que dit le compilateur

erreur CS0506 Cercle.Decrire() cannot override inherited member Forme.Decrire() because it is not marked virtual abstract or override on peut override que si la methode du parent est virtual abstract ou deja override

5.3.3 avec new les deux lignes affichent la meme chose

non c.Decrire() affiche le rayon mais f.Decrire() non
avec override la methode est choisie a l'execution selon le vrai type de l'objet donc un Cercle range dans une variable Forme appelle quand meme la version de Cercle c'est le polymorphisme
avec new c'est du masquage la methode est choisie a la compilation selon le type de la variable donc avec f qui est une Forme ca appelle la version de Forme
en java toutes les methodes sont virtuelles par defaut donc les deux lignes afficheraient le rayon comme avec override en c# il faut mettre virtual explicitement

etape 6

6.1 deux notes identiques comparées avec == ca donne true parce qu'un record compare les valeurs de tous ses champs pas l'adresse
l'affichage donne Note { Nom = Lovelace, Prenom = Ada, Matiere = Algo, Valeur = 15, Coef = 3 } le ToString est generé tout seul
with fait une copie en changeant juste les champs qu'on veut l'original change pas

== entre deux Etudiant avec les memes données

false parce que Etudiant c'est une classe normale donc == compare les references et c'est deux objets differents en memoire
meme avec IEquatable que j'ai rajouté apres Equals donne true mais == reste false vu que j'ai pas redefini l'operateur ==

6.4 resultat deux erreurs sur Console.Error ligne 5 abc c'est pas un nombre et ligne 7 la note 25 est pas entre 0 et 20 puis le programme continue et affiche le relevé des 3 etudiants

6.5 si le fichier existe pas new StreamReader leve FileNotFoundException je l'attrape dans Program et j'affiche Le fichier notes.csv est introuvable

6.6 bonus moyenne ponderée

je l'ai mise dans Etudiant parce que c'est lui qui a les notes faut juste garder le coef de chaque note donc j'ai rajouté une liste _coefs a coté de _notes et AjouterNote prend un coef en plus avec 1 par defaut comme ca l'ancien code marche toujours
MoyennePonderee fait somme des note x coef divisé par la somme des coefs
Alan a 9 coef 3 et 12 coef 2 ca fait 51/5 = 10.20 alors que la moyenne simple fait 10.50

pour aller plus loin

tests xunit
j'ai fait la solution Tp.slnx avec TpCsharp et TpCsharp.Tests et dotnet test passe 18 tests
moyenne sans note moyenne de plusieurs notes note hors bornes avec Assert.Throws NomComplet et Mention avec Theory et InlineData plus la moyenne ponderée et l'egalité
Theory avec InlineData ca lance le meme test plusieurs fois avec des valeurs differentes au lieu d'ecrire un Fact par cas

struct Point2D
readonly struct donc on peut pas modifier X ou Y apres la creation faut faire une copie avec with
p2 = p1 puis on change p2 et p1 bouge pas car c'est une struct donc copie par valeur
pareil pour Centre de Forme quand je recupere rond.Centre j'ai une copie si je la modifie le centre du cercle change pas

IEquatable
Equals compare Nom Prenom et Naissance j'ai aussi redefini Equals(object?) et GetHashCode avec HashCode.Combine sur les memes champs
faut redefinir GetHashCode sinon le HashSet range les deux objets a des endroits differents et les voit comme differents
dans le HashSet j'ajoute deux Ada identiques et Alan ca donne 2 elements donc le doublon est bien detecté
