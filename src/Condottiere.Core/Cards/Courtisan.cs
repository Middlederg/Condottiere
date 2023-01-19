namespace Condottiere.Core.Cards;

public class Courtisan : Card, IValuableCard
{
    public override CardType Type => CardType.Special;

    public int Value => 1;

    public Courtisan(int id) : base(id) { }
}

