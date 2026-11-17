namespace TpCsharp.Tests;

public class MentionTests
{
    [Theory]
    [InlineData(20, "Très bien")]
    [InlineData(16, "Très bien")]
    [InlineData(15.9, "Bien")]
    [InlineData(14, "Bien")]
    [InlineData(12, "Assez bien")]
    [InlineData(10, "Passable")]
    [InlineData(9.99, "Ajourné")]
    [InlineData(0, "Ajourné")]
    public void MentionSelonLaMoyenne(double moyenne, string attendu)
    {
        Assert.Equal(attendu, Outils.Mention(moyenne));
    }

    [Theory]
    [InlineData(-0.5)]
    [InlineData(20.1)]
    public void MoyenneHorsBornesLeveUneException(double moyenne)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Outils.Mention(moyenne));
    }
}
