using Ecole;

namespace TpCsharp.Tests;

public class EtudiantTests
{
    [Fact]
    public void MoyenneSansNoteVautZero()
    {
        var etudiant = new Etudiant("Lovelace", "Ada", 4);

        Assert.Equal(0, etudiant.Moyenne());
    }

    [Fact]
    public void MoyenneDePlusieursNotes()
    {
        var etudiant = new Etudiant("Lovelace", "Ada", 4);
        etudiant.AjouterNote(12);
        etudiant.AjouterNote(15);
        etudiant.AjouterNote(18);

        Assert.Equal(15, etudiant.Moyenne());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(20.5)]
    [InlineData(25)]
    public void NoteHorsBornesLeveUneException(double note)
    {
        var etudiant = new Etudiant("Lovelace", "Ada", 4);

        Assert.Throws<ArgumentOutOfRangeException>(() => etudiant.AjouterNote(note));
    }

    [Fact]
    public void NomCompletAvecNomEnMajuscules()
    {
        var etudiant = new Etudiant("Lovelace", "Ada", 4);

        Assert.Equal("Ada LOVELACE", etudiant.NomComplet);
    }

    [Fact]
    public void MoyennePondereeTientCompteDesCoefs()
    {
        var etudiant = new Etudiant("Turing", "Alan", 4);
        etudiant.AjouterNote(9, 3);
        etudiant.AjouterNote(12, 2);

        Assert.Equal(10.2, etudiant.MoyennePonderee(), 2);
    }

    [Fact]
    public void DeuxEtudiantsIdentiquesSontEgaux()
    {
        var a = new Etudiant("Lovelace", "Ada", 4) { Naissance = new DateOnly(1815, 12, 10) };
        var b = new Etudiant("Lovelace", "Ada", 5) { Naissance = new DateOnly(1815, 12, 10) };

        Assert.True(a.Equals(b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
        Assert.False(a == b);
    }
}
