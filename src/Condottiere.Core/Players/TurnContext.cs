using Condottiere.Core.Cards;

using System.Linq;

namespace Condottiere.Core.Players;

public class TurnContext
{
    public int Index { get; private set; }

    public List<Player> Players { get; }
    private int currentPlayerIndex;
    public int TotalPlayers => Players.Count;

    public TurnContext(IEnumerable<Player> players)
    {
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

    public IEnumerable<PlayerSummary> GetRanking(GameContext gameContext)
    {
        return Players
            .Select(x => x.ToSummary(gameContext))
            .OrderByDescending(x => x.Points)
            .ToList();
    }

    public PlayerSummary? Winner(GameContext gameContext)
    {
        IEnumerable<PlayerSummary> allPlayers = GetRanking(gameContext);
        int maxPoints = allPlayers.Max(x => x.Points);
        IEnumerable<PlayerSummary> playersWithMaxPoints = allPlayers.Where(x => x.Points == maxPoints);
        return playersWithMaxPoints.Count() == 1 ? playersWithMaxPoints.First() : null;
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

        if (Players.Count(x => x.CanPlayMoreCards) < 2)
            return true;

        return false;
    }


    public bool IsTie(GameContext context) => IsEndOfBattle() && Winner(context) is null;


    /// <summary>
    /// Devuelve el jugador al que pasará la ficha de condottiere al finalizar una batalla
    /// </summary>
    /// <returns></returns>
    public Player SiguienteCondottiere()
    {
        //Unico jugador con mayor número de cortesanas
        int max = Players.Max(x => x.NumCortesanasJugadas());
        var potenciales = Players.Where(x => x.NumCortesanasJugadas() == max);
        if (potenciales.Count() == 1)
            return potenciales.First();

        //Jugador con más puntos
        if (!Tie())
            return GanadorBatalla().First();

        //Siguiente jugador a la izquierda
        return JugadorCondottiere == Players.Count - 1 ? Players[0] : Players[JugadorCondottiere + 1];
    }

    /// <summary>
    /// 4, 5, 6 jugadores -> 5 regiones o 3 adyacentes
    /// 2, 3 jugadores -> 6 regiones o 4 adyacentes
    /// </summary>
    /// <returns></returns>
    public Player HayGanadorPartida()
    {
        //Busca si hay algún jugador que haya obtenido el número objetivo de provincias
        int objetivo = Players.Count > 3 ? 5 : 6;
        foreach (Player jug in Players)
            if (jug.OwnedProvinces.Count >= objetivo)
                return jug;

        //Busca si hay algún jugador que haya obtenido el número objetivo de provincias adyacentes
        int objetivoAdyacentes = Players.Count > 3 ? 3 : 4;
        foreach (Player jug in Players)
            if (jug.NumRegionesAdyacentes() >= objetivoAdyacentes)
                return jug;

        return null;
    }

    /// <summary>
    /// Se comprueba despues de que los jugadores hayan decidido si descartarse
    /// Si un jugador o ninguno solamente tienen cartas en la mano
    /// </summary>
    /// <returns></returns>
    public bool EsFinDeRonda() => Players.Count(x => x.Hand.Count > 0) <= 1;

    public void PrepareNextBattle(Deck deck, IEnumerable<Card> discardPile, int nextBattleProvince)
    {
        Player? owner = Players.FirstOrDefault(x => x.Owns(nextBattleProvince));
        
        foreach (Player player in Players)
        {
            int cardsToDraw = player.CardsToDraw;

            if (!deck.CanDeal(cardsToDraw))
            {
                deck.Incorporate(discardPile);
            }
            
            IEnumerable<Card> newCards = Enumerable.Range(0, player.CardsToDraw).Select(x => deck.Draw());

            bool isDefending = owner is not null && owner.Id == player.Id;
            player.ResetBattleLines(newCards, isDefending);
        }
    }
        
}
