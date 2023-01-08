namespace Condottiere.Core.Cards;

public class Surrender : Card
{
    public override CardType Type => CardType.Action;

    public Surrender(int id) : base(id) { }
}
