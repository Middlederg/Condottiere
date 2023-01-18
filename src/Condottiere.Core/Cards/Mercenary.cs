namespace Condottiere.Core.Cards;

public class Mercenary : Card
{
    public override CardType Type => CardType.Mercenary;

    public int Value { get; }

    public Mercenary(int id, int value) : base(id)
    {
        Value = value;
    }

    public override string Path() => $"mercenary-{Value}.jpg";

    public override string ToString() => $"Mercenary: {Value}";

}
