using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Condottiere.Core.Enums;

namespace Condottiere.Core.Model
{
    public class Jugador
	{
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Carta> Mano { get; set; }
        public List<Carta> Jugada { get; set; }
        public List<Provincia> Provincias { get; set; }
        public Color ColorJugador { get; set; }
        public Dificultad Perfil { get; set; }

        //Indica si el jugador está participando en la batalla actual
        public bool HaPasado { get; set; }

        public Jugador(int id, string nombre, Color color, Dificultad perfil = Dificultad.Normal)
        {
            Id = id;
            Nombre = nombre;
            Reiniciar();
            ColorJugador = color;
            Perfil = perfil;
        }

        public Jugador(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
            Perfil = Dificultad.Normal;
            Reiniciar();
            ColorJugador = Color.Rojo;
        }

        /// <summary>
        /// Reinicia el jugador
        /// </summary>
        public void Reiniciar()
		{
            Mano = new List<Carta>();
            Jugada = new List<Carta>();
            Provincias = new List<Provincia>();
		}

        /// <summary>
        /// Elimina la carta de la mano y la coloca en las jugadas
        /// </summary>
        /// <param name="c">Carta jugada</param>
        /// <returns>Frase para el log</returns>
        public string JugarCarta(Carta c)
		{
			Mano.Remove(c);
			Jugada.Add(c);
            return ToString() + " juega " + c.ToString();
        }
			
		public void GanaProvincia(Provincia p)    
		{
			Provincias.Add(p);
		}

        /// <summary>
        /// Número de mercenarios que le quedan al jugador en la mano
        /// </summary>
        /// <returns></returns>
        public int NumMercenariosMano() => Mano.Count(x => x.EsMercenario());

        /// <summary>
        /// Número de mercenarios que ha jugado
        /// </summary>
        /// <returns></returns>
        public int NumMercenariosJugados() => Jugada.Count(x => x.EsMercenario());

        /// <summary>
        /// Numero de cortesanas que ha jugado
        /// </summary>
        /// <returns></returns>
        public int NumCortesanasJugadas() => Jugada.Count(x => x.EsCortesana());

        /// <summary>
        /// Devuelve la puntuación actual de ese jugador
        /// </summary>
        /// <param name="valorMaximoMercenario">Valor del mercenario máximo en juego</param>
        /// <returns></returns>
        public int Puntos(int valorMaximoMercenario)
        {
            int puntos = 0;

            //Valor de los mercenarios
            if (Juego.EstacionActual.Equals(Estacion.Invierno))
                puntos += NumMercenariosJugados();
            else
                puntos += (from x in Jugada
                          where x.EsMercenario()
                          select x.Valor).Sum();

            //Si hay un tambor, el valor de los mercenarios se duplica
            if (Jugada.Any(x => x.EsTambor()))
                puntos = puntos * 2;

            //Si hay algún mercenario igual al valor máximo de los mercenarios, se suma 3
            if (Juego.EstacionActual.Equals(Estacion.Primavera) && valorMaximoMercenario != 0 && Jugada.Any(x => x.EsMercenario() && x.Valor == valorMaximoMercenario))
                puntos += 3 * Jugada.Count(x => x.EsMercenario() && x.Valor == valorMaximoMercenario);

            //Sumamos las cortesanas y las heroinas
            puntos +=   (from x in Jugada
                        where !x.EsMercenario()
                        select x.Valor).Sum();

            return puntos;
        }

        public void Recuperar(Carta card)
        {
            if (!Jugada.Any(x => x.Equals(card)))
                throw new ArgumentException($"Carta {card.ToString()} no ha sido jugada por {ToString()}. No se puede recuperar");
            Jugada.Remove(card);
            Mano.Add(card);
        }

        /// <summary>
        /// Indica el número de regiones adyacentes que controla el jugador
        /// </summary>
        /// <returns></returns>
        public int NumRegionesAdyacentes()
        {
            List<KeyValuePair<int, int>> lista = new List<KeyValuePair<int, int>>();

            foreach (Provincia prov in Provincias)
            {
                //Resto de provincias
                var posiblesAdyacentes = Provincias.Where(x => !x.Equals(prov)).Select(x=> x.Id);

                //Añade ocurrencia con id de provincia y numero de adyacentes
                lista.Add(new KeyValuePair<int, int>(prov.Id, prov.Adyacentes.Intersect(posiblesAdyacentes).Count()));
            }

            //Si cualquier región tiene 3 o más coincidencias, ya hay 4 regiones adyacentes
            if (lista.Any(x => x.Value >= 3))
                return 4;

            //Si cualquier región tiene 2 o más coincidencias, y entre esas regiones son coincidentes, hay 3 regiones unidas por lo menos
            if (lista.Any(x => x.Value >= 2))
            {
                foreach (var item in lista.Where(x => x.Value >= 2))
                {
                    Provincia p = Provincias.Find(x => x.Id == item.Key); //seleciona la provincia

                    var ids = lista.Where(x => x.Key != p.Id).Select(x => x.Key); //coge el resto de ids de provincia
                    var posiblesAdyacentes = Provincias.Where(x => ids.Contains(x.Id)).SelectMany(x => x.Adyacentes);
                    if (p.Adyacentes.Intersect(posiblesAdyacentes).Any())
                        return 3;
                }
                return 2;
            }
            return lista.Any(x => x.Value >= 1) ? 1 : 0;
        }

        /// <summary>
        /// Descarta todas las cartas de la mano 
        /// (Solo cuando al final de una batalla no se tienen mercenarios)
        /// </summary>
        public List<Carta> DescartarMano()
        {
            if (NumMercenariosMano() > 0)
                throw new Exception(ToString() + " no se puede descartar porque tiene mercenarios");

            List<Carta> alDescarte = Mano;
            Mano = new List<Carta>();
            return alDescarte;
        }

        /// <summary>
        /// Número de cartas que recibe el jugador tras el final de ronda
        /// </summary>
        /// <returns></returns>
        public int NumCartasRecibir() => 10 + Provincias.Count - Mano.Count;

        public override string ToString() => Nombre;
        public string Titulo(int valorMaxmercenario) => $"{Nombre} - {Puntos(valorMaxmercenario)} puntos";

        public override bool Equals(System.Object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                throw new ArgumentOutOfRangeException(GetType().ToString() + " no puede ser nulo");
            Jugador o = obj as Jugador;
            return Id == o.Id;
        }

        public bool Equals(Jugador o)
        {
            if (o is null)
                throw new ArgumentNullException("Objeto " + GetType().ToString() + " es nulo");
            return o.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


	}
}
