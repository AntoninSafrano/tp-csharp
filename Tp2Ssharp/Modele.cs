// une note lue dans notes.csv
public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef);

// un etudiant lu dans etudiants.csv
public record Inscrit(string Nom, string Prenom, string Promo);

// toutes les notes d un etudiant dans une seule liste pour le 3.3
public record Bulletin(string Prenom, List<double> Valeurs);
