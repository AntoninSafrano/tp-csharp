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

