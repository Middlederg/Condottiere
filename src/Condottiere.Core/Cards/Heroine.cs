namespace Condottiere.Core.Cards;

public class Heroine : Card
{
    public override CardType Type => CardType.Special;

    public int Value => 10;

    public Heroine(int id) : base(id) { }
}

