using Condottiere.Core.Cards;
using Condottiere.Core.Players;
using Condottiere.Core.Provinces;

namespace Condottiere.Core;

public class Game
{
    public TurnContext Turn { get; }
    public Deck Deck { get; }
    public Map Map { get; }
    
    public Game(GameContext gameContext, string playerName = "player-1")
    {
        IEnumerable<string> playerNames = NameCreator.Create(gameContext.PlayerCount- 1);
        Color[] colors = Enum.GetValues<Color>();
        List<Player> players = playerNames.Select((name, index) => new Player(index, name, colors.ElementAt(index), Difficulty.Normal)).ToList();
        players.Add(new Player(gameContext.PlayerCount - 1, playerName, colors.ElementAt(gameContext.PlayerCount - 1), Difficulty.Human));
        Turn = new TurnContext(players);
        Deck = new Deck(gameContext);
        Map = new Map();
    }

    public void PrepareNextBattle(int provinceId)
    {
        Turn.PrepareNextBattle(Deck, provinceId);
    }

}
       
    
