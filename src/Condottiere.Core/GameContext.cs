using Condottiere.Core.Cards;

using System.Linq;

namespace Condottiere.Core;

public record SpringOptions(bool UseSpring = false, int HighestMercenaryBonus = 3);
public record BishopOptions(bool UseOldEditionRules = false)
{
    public int TotalCards() => UseOldEditionRules ? 3 : 6;
}

public class GameContext
{
    public SpringOptions SpringOptions { get; }
    public BishopOptions BishopOptions { get; }

    public GameContext(SpringOptions springOptions, BishopOptions bishopOptions)
    {
        SpringOptions = springOptions;
        BishopOptions = bishopOptions;
        CurrentSeason = Season.None;
        playedMercenaries = new List<Mercenary>();
    }

    public void CleanBattle()
    {
        playedMercenaries = new List<Mercenary>();
        CurrentSeason = Season.None;
    }
    
    public Season CurrentSeason { get; private set; }
    public bool IsWinter() => CurrentSeason == Season.Winter;

    private List<Mercenary> playedMercenaries;
    public Mercenary? HighestMercenary => playedMercenaries.OrderByDescending(m => m.Value).FirstOrDefault();

    public void CardPlayed(Card card, Mercenary? witdrawedMercenary)
    {
        if (card is Mercenary mercenary)
        {
            playedMercenaries.Add(mercenary);
            return;
        }

        if (card is SeasonChanger seasonChanger)
        {
            CurrentSeason = seasonChanger.Season;
            return;
        }

        if (card is Scarecrow && witdrawedMercenary is not null)
        {
            playedMercenaries.Remove(witdrawedMercenary);
        }
    }
}
