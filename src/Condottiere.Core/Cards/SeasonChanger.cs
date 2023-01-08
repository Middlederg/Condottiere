namespace Condottiere.Core.Cards;

public class SeasonChanger : Card
{
    public override CardType Type => CardType.Action;

    public Season Season { get; }

    private SeasonChanger(int id, Season season) : base(id) 
    {
        Season = season;
    }
    public static SeasonChanger Winter(int id) => new(id, Season.Winter);
    public static SeasonChanger Spring(int id) => new(id, Season.Spring);
}