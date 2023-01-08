using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Condottiere.Core.Enums;
using Condottiere.Core.Negocio;

namespace Condottiere.Core.Model
{
    public class Juego
    {
        //Flujo del juego
        //1. jugar carta
        //2. ver si es fin de batalla
        //si --> 	ver si es fin de partida
        //si no --> ver si hay que repartir
        //3. Si no avanzar turno

        public List<Carta> Mazo { get; set; }
        public List<Carta> Descarte { get; set; }
        public List<Jugador> Jugadores { get; set; }
        public Mapa MapaProvincias { get; set; }
        public List<string> Log { get; set; }
        public int Turno { get; set; }
        public int JugadorCondottiere { get; set; }

        public static Estacion EstacionActual;

        public Juego(List<Jugador> jugs)
        {
            Jugadores = jugs;
            ReiniciarJuego();
        }
        
        /// <summary>
        /// Prepara un nueva partida
        /// </summary>
        public void ReiniciarJuego()
        {
            Mazo = General.DesordenarLista(GameHelper.ObtenerMazoInicial().ToList());
            Descarte = new List<Carta>();
            Jugadores.ForEach(x => x.Reiniciar());
            MapaProvincias = new Mapa();
            if (Log == null)
                Log = new List<string>();
            else
                Log.Add("Partida reiniciada");
            EstacionActual = Estacion.Ninguna;
            Turno = GameHelper.r.Next(0, Jugadores.Count);
            JugadorCondottiere = Turno;
            Log.Add("Comienza una nueva partida." + ElTurno().Nombre + " es el jugador inicial");


            RepartirCartas();
        }

        /// <summary>
        /// Devuelve el jugador que tiene el turno
        /// </summary>
        /// <returns></returns>
        public Jugador ElTurno() => Jugadores[Turno];

        /// <summary>
        /// Avanza un turno
        /// Flujo del juego
        /// 1. jugar carta
        /// 2. ver si es fin de batalla
        /// si --> 	ver si es fin de partida
        /// si no --> ver si es hay que repartir
        /// 3. Si no avanzar turno. le tocaría al siguiente que no haya pasado que tenga cartas
        /// </summary>
        public void AvanzaTurno()
        {
            if (!EsFinBatalla())
            { 
                do
                    Turno = (Turno == Jugadores.Count - 1) ? 0 : Turno + 1;
                while (ElTurno().HaPasado || ElTurno().Mano.Count != 0); //Avanza mientras el jugador haya pasado o no tenga cartas
            }
        }

        /// <summary>
        /// Determina si ha terminado la batalla
        /// </summary>
        /// <returns></returns>
        public bool EsFinBatalla()
        {
            //Fin de batalla porque alguien jugó un cerrojo
            if (Jugadores.SelectMany(x => x.Jugada).Any(x => x.EsRendicion()))
                return true;

            //Fin de batalla porque todos han pasado
            if (Jugadores.Count(x => !x.HaPasado) < 1)
                return true;

            //Fin de batalla porque los que no han pasado ya no tienen cartas en la mano
            if (Jugadores.Where(x => !x.HaPasado).SelectMany(x => x.Mano).Count() == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Devuelve coleccion de jugadores con máxima puntuación
        /// </summary>
        /// <returns></returns>
        private List<Jugador> GanadorBatalla()
        {
            int max = Jugadores.Max(x=> x.Puntos(MaxMercenario()));
            return Jugadores.Where(x => x.Puntos(MaxMercenario()) == max).ToList();
        }

        /// <summary>
        /// Determina si hay más de un jugador con la máxima puntuación
        /// </summary>
        /// <returns></returns>
        public bool BatallaEmpatada() => GanadorBatalla().Count > 1;

        //Termina la batalla actual. Parmtros: cartas a conservar del jug 1.
        public void NuevaBatalla(List<Carta> conservarCartas = null)
        {
            //Devolver todas las cartas jugadas al descarte
            Descarte.AddRange(Jugadores.SelectMany(x => x.Jugada));
            Jugadores.ForEach(x => x.Jugada = new List<Carta>());

            //Reinicializar la estación
            EstacionActual = Estacion.Ninguna;
        }

        /// <summary>
        /// Devuelve el jugador al que pasará la ficha de condottiere al finalizar una batalla
        /// </summary>
        /// <returns></returns>
        public Jugador SiguienteCondottiere()
        {
            //Unico jugador con mayor número de cortesanas
            int max = Jugadores.Max(x => x.NumCortesanasJugadas());
            var potenciales = Jugadores.Where(x => x.NumCortesanasJugadas() == max);
            if (potenciales.Count() == 1)
                return potenciales.First();

            //Jugador con más puntos
            if (!BatallaEmpatada())
                return GanadorBatalla().First();

            //Siguiente jugador a la izquierda
            return((JugadorCondottiere == Jugadores.Count - 1) ? Jugadores[0] : Jugadores[JugadorCondottiere + 1]);
        }

        /// <summary>
        /// 4, 5, 6 jugadores -> 5 regiones o 3 adyacentes
        /// 2, 3 jugadores -> 6 regiones o 4 adyacentes
        /// </summary>
        /// <returns></returns>
        public Jugador HayGanadorPartida()
        {
            //Busca si hay algún jugador que haya obtenido el número objetivo de provincias
            int objetivo = Jugadores.Count > 3 ? 5 : 6;
            foreach (Jugador jug in Jugadores)
                if (jug.Provincias.Count >= objetivo)
                    return jug;

            //Busca si hay algún jugador que haya obtenido el número objetivo de provincias adyacentes
            int objetivoAdyacentes = Jugadores.Count > 3 ? 3 : 4;
            foreach (Jugador jug in Jugadores)
                if (jug.NumRegionesAdyacentes() >= objetivoAdyacentes)
                    return jug;

            return null;
        }

        /// <summary>
        /// Se comprueba despues de que los jugadores hayan decidido si descartarse
        /// Si un jugador o ninguno solamente tienen cartas en la mano
        /// </summary>
        /// <returns></returns>
        public bool EsFinDeRonda() => Jugadores.Count(x => x.Mano.Count > 0) <= 1;

        /// <summary>
        /// Reparte el número de cartas correspondiente a cada jugador
        /// </summary>
        public void RepartirCartas()
        {
            //if (Mazo.Count < Jugadores.Select(x=> x.NumCartasRecibir()).Sum())
            List<Carta> lista = new List<Carta>(Mazo);
            lista.AddRange(Descarte);
            Mazo = General.DesordenarLista(lista);
            Jugadores.ForEach(x =>
            {
                int num = x.NumCartasRecibir();
                for (int i = 0; i < num; i++)
                {
                    x.Mano.Add(Mazo[0]);
                    Mazo.Remove(Mazo[0]);
                }
            });
        }

        /// <summary>
        /// Jugador juega carta y afecta al entorno
        /// </summary>
        /// <param name="jug"></param>
        /// <param name="c"></param>
        public void JugarCarta(Carta c, Jugador jug = null)
		{
            if (jug == null) jug = ElTurno();
            Log.Add(jug.JugarCarta(c));
            switch (c.Tipo)
            {
                case TipoCarta.Obispo:
                    int max = MaxMercenario();
                    Enumerable.Range(0, BorrarMaxMercenario()).ToList().ForEach(x => Descarte.Add(new Carta(TipoCarta.Mercenario, max)));
                    break;
                case TipoCarta.Primavera:
                    EstacionActual = Estacion.Primavera;
                    break;
                case TipoCarta.Invierno:
                    EstacionActual = Estacion.Invierno;
                    break;
                case TipoCarta.Rendicion:
                    break;
                case TipoCarta.Espantapajaros:
                    break;
            }
        }

        /// <summary>
        /// Obtiene la carta más alta del tipo mercenario
        /// </summary>
        /// <returns></returns>
        public int MaxMercenario()
        {
            var mercenarios = Jugadores.SelectMany(x => x.Jugada).Where(x => x.EsMercenario());
            return mercenarios == null || !mercenarios.Any() ? 0 : mercenarios.Max(x => x.Valor);
        }

        /// <summary>
        /// Borra los mercenarios más altos
        /// Ocurre cuando alguien juega un obispo
        /// </summary>
        private int BorrarMaxMercenario()
        {
            int quitados = 0;
            foreach (var jugador in Jugadores)
                quitados += jugador.Jugada.RemoveAll(x => x.EsMercenario() && x.Valor == MaxMercenario());
            return quitados;
        }
    }
}
