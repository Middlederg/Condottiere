using Condottiere.Core.Cards;

using System.Linq;

namespace Condottiere.Core.Players;

public class TurnContext
{
    public int Index { get; private set; }

    public List<Player> Players { get; }
    public IEnumerable<Player> Opponents => Players.Where(x => x.Profile != Difficulty.Human);
    public Player MainPlayer => Players.First(x => x.Profile == Difficulty.Human);
    public bool IsMainPlayerTurn => CurrentPlayer == MainPlayer;

    private int currentPlayerIndex;
    public int TotalPlayers => Players.Count;
    public bool IsCurrentPlayerWinning(GameContext gameContext) => BattleWinner(gameContext) == CurrentPlayer;

    public TurnContext(IEnumerable<Player> players)
    {
        if (players.Count(x => x.Profile == Difficulty.Human) != 1)
        {
            throw new ArgumentException("There must be exactly one human player");
        }
        
        Players = players.ToList();
        currentPlayerIndex = 0;
        Index = 0;
    }

    private int NextPlayerIndex => currentPlayerIndex >= TotalPlayers - 1 ? 0 : currentPlayerIndex + 1;

    public Player NextPlayer => Players.ElementAt(NextPlayerIndex);
    public Player CurrentPlayer => Players.ElementAt(currentPlayerIndex);

    private void MoveToNextPlayer()
    {
        currentPlayerIndex = NextPlayerIndex;
        Index++;
    }

    public IEnumerable<PlayerSummary> GetBattleRanking(GameContext gameContext)
    {
        return Players
            .Select(x => x.ToSummary(gameContext))
            .OrderByDescending(x => x.Points)
            .ToList();
    }

    public Player? BattleWinner(GameContext gameContext)
    {
        IEnumerable<PlayerSummary> allPlayers = GetBattleRanking(gameContext);
        int maxPoints = allPlayers.Max(x => x.Points);
        IEnumerable<PlayerSummary> playersWithMaxPoints = allPlayers.Where(x => x.Points == maxPoints);
        return playersWithMaxPoints.Count() == 1 ? Players.First(x => x.Id == playersWithMaxPoints.First().Id) : null;
    }

    public void NextTurn()
    {
        if (!IsEndOfBattle())
        {
            do
            {
                MoveToNextPlayer();
            }
            while (!CurrentPlayer.CanPlayMoreCards);
        }
    }

    public bool IsEndOfBattle()
    {
        if (Players.SelectMany(x => x.Army).IsPlayed<Surrender>())
            return true;

        if (!Players.Any(x => x.CanPlayMoreCards))
            return true;

        return false;
    }


    public bool IsTie(GameContext context) => IsEndOfBattle() && BattleWinner(context) is null;

    public Player NextBattleChooser(GameContext gameContext)
    {
        int maximumCourtisan = Players.Max(x => x.Army.CountOf<Courtisan>());
        IEnumerable<Player> potentials = Players.Where(x => x.Army.CountOf<Courtisan>() == maximumCourtisan);
        if (potentials.Count() == 1)
        {
            return potentials.First();
        }

        if (!IsTie(gameContext))
        {
            return Players.First(x => x.Id == BattleWinner(gameContext)!.Id);
        }

        return Players[NextPlayerIndex];
    }


    public IEnumerable<Player> GetAllWinners(GameContext gameContext)
    {
        return SearchForWinners().Distinct().ToList();
        
        IEnumerable<Player> SearchForWinners()
        {
            foreach (Player player in Players)
            {
                if (player.OwnedProvinces.Count >= gameContext.GetProvincesTarget)
                {
                    yield return player;
                }

                if (player.OwnedProvinces.Count >= gameContext.GetNearProvincesTarget)
                {
                    yield return player;
                }
            }
        }
    }

    public bool IsGameEnd(GameContext gameContext) => GetAllWinners(gameContext).Any();

    public bool IsEndOfRound() => Players.Count(x => x.Hand.Count > 0) <= 1;

    public void PrepareNextBattle(Deck deck, int nextBattleProvince)
    {
        Player? owner = Players.FirstOrDefault(x => x.Owns(nextBattleProvince));
        
        foreach (Player player in Players)
        {
            int cardsToDraw = player.CardsToDraw;

            if (!deck.CanDeal(cardsToDraw))
            {
                deck.IncorporateDiscardPile();
            }
            
            IEnumerable<Card> newCards = Enumerable.Range(0, player.CardsToDraw).Select(x => deck.Draw());

            bool isDefending = owner is not null && owner.Id == player.Id;
            deck.Discard(player.Army);
            player.ResetBattleLines(newCards, isDefending);
        }
    } 
}
