using Condottiere.Core.Players;

namespace Condottiere.Core;

public class Game
{
    //Flujo del juego
    //1. jugar carta
    //2. ver si es fin de batalla
    //si --> 	ver si es fin de partida
    //si no --> ver si hay que repartir
    //3. Si no avanzar turno

    public List<Card> Deck { get; set; }
    public List<Card> DiscardPile { get; set; }
    public Provinces.Map Map { get; set; }
    public List<string> Log { get; set; }
    public int Turn { get; set; }
    public int JugadorCondottiere { get; set; }

    public Game(List<Player> jugs)
    {
        Players = jugs;
        ReiniciarJuego();
    }

    /// <summary>
    /// Prepara un nueva partida
    /// </summary>
    public void ReiniciarJuego()
    {
        Deck = GameHelper.ObtenerMazoInicial().ToList().DesordenarLista();
        DiscardPile = new List<Card>();
        Players.ForEach(x => x.Reset());
        Map = new Map();
        if (Log == null)
            Log = new List<string>();
        else
            Log.Add("Partida reiniciada");
        EstacionActual = Season.None;
        Turn = GameHelper.r.Next(0, Players.Count);
        JugadorCondottiere = Turn;
        Log.Add("Comienza una nueva partida." + ElTurno().Name + " es el jugador inicial");


        RepartirCartas();
    }

    /// <summary>
    /// Reparte el número de cartas correspondiente a cada jugador
    /// </summary>
    public void RepartirCartas()
    {
        //if (Mazo.Count < Jugadores.Select(x=> x.NumCartasRecibir()).Sum())
        List<Card> lista = new List<Card>(Deck);
        lista.AddRange(DiscardPile);
        Deck = lista.Shuffle();
        Players.ForEach(x =>
        {
            int num = x.CardsToDraw();
            for (int i = 0; i < num; i++)
            {
                x.Hand.Add(Deck[0]);
                Deck.Remove(Deck[0]);
            }
        });
    }
}
