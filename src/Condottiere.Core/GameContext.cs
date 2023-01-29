using Condottiere.Core.Cards;

using System.Linq;

namespace Condottiere.Core;

public record SpringOptions(bool UseSpring = false, int HighestMercenaryBonus = 3);
public record BishopOptions(bool UseOldEditionRules = false)
{
    public int TotalCards() => UseOldEditionRules ? 3 : 6;
}

public record VictoryOptions(int Provinces, int NearProvinces);

public class GameContext
{
    public const int InitialHand = 2;

    public int PlayerCount { get; }
    public bool WithSieges { get; }
    public SpringOptions SpringOptions { get; }
    public BishopOptions BishopOptions { get; }

    public Dictionary<int, VictoryOptions> VictoryOptions = new()
    {
        {2, new VictoryOptions(6, 4)},
        {3, new VictoryOptions(6, 4)},
        {4, new VictoryOptions(5, 3)},
        {5, new VictoryOptions(5, 3)},
        {6, new VictoryOptions(5, 3)},
    };

    public GameContext(int totalPlayers, bool withSieges, SpringOptions springOptions, BishopOptions bishopOptions)
    {
        PlayerCount = totalPlayers;
        WithSieges = withSieges;
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

    public int GetProvincesTarget => VictoryOptions[PlayerCount].Provinces;
    public int GetNearProvincesTarget => VictoryOptions[PlayerCount].NearProvinces;
}
       
    
