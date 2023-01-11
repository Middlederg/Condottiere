using Condottiere.Core.Cards;
using Condottiere.Core.Players;
using Condottiere.Core.Provinces;

namespace Condottiere.Core;

public class Game
{
    public TurnContext Turn { get; }
    public Deck Deck { get; }
    public Map Map { get; }
    
    public Game(GameContext gameContext)
    {
        IEnumerable<string> playerNames = NameCreator.Create(gameContext.PlayerCount);
        Color[] colors = Enum.GetValues<Color>();
        List<Player> players = playerNames.Select((name, index) => new Player(index, name, colors.ElementAt(index), Difficulty.Normal)).ToList();

        Turn = new TurnContext(players);
        Deck = new Deck(gameContext);
        Map = new Map();
    }

    public void PrepareNextBattle(int provinceId)
    {
        Turn.PrepareNextBattle(Deck, provinceId);
    }

}
       
    
