using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Condottiere.Core.Enums;
using Condottiere.Core.Model;

namespace Condottiere.Core.Negocio
{
    public static class GameHelper
    {
        public static Random r = new Random();

        private static string[] nombres = { "Ricardio", "Vicente", "Emiliano", "Felix",
                                   "Verónica", "Montserrat", "Andrea", "Olga", "Nacho", "Domingo",
                                   "César", "Agustín", "Hugo", "Tomás", "Rafael", "Donatello", "Miguel Angel",
                            "Leonardo", "Nieves", "Isabel", "Irene", "Mar", "Alicia", "Carla", "Eva",
                            "Lidia", "Aurora", "Celia", "Claudia", "Amparo", "Sebastián", "Samuel" };

        /// <summary>
        /// Obtiene lista de nombres aleatorios
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNombres(int numero)
        {
            var copiaNombres = nombres.ToList();
            foreach (int i in Enumerable.Range(0, numero))
            {
                string nombre = General.ElementoAleatorio(copiaNombres);
                copiaNombres.Remove(nombre);
                yield return nombre;
            }
        }

        public static IEnumerable<Carta> ObtenerMazoInicial()
		{
            foreach (int i in Enumerable.Range(0, 10))
                yield return new Carta(TipoCarta.Mercenario, 1);

            foreach (int i in Enumerable.Range(0, 8))
            {
                yield return new Carta(TipoCarta.Mercenario, 2);
                yield return new Carta(TipoCarta.Mercenario, 3);
                yield return new Carta(TipoCarta.Mercenario, 4);
                yield return new Carta(TipoCarta.Mercenario, 5);
                yield return new Carta(TipoCarta.Mercenario, 6);
                yield return new Carta(TipoCarta.Mercenario, 10);
            }

            foreach (int i in Enumerable.Range(0, 12))
                yield return new Carta(TipoCarta.Cortesana, 1);

            foreach (int i in Enumerable.Range(0, 3))
            {
                yield return new Carta(TipoCarta.Heroina, 10);
                yield return new Carta(TipoCarta.Primavera);
                yield return new Carta(TipoCarta.Invierno);
                yield return new Carta(TipoCarta.Rendicion);
            }

            foreach (int i in Enumerable.Range(0, 6))
            {
                yield return new Carta(TipoCarta.Tambor);
                yield return new Carta(TipoCarta.Obispo);
            }

            foreach (int i in Enumerable.Range(0, 16))
                yield return new Carta(TipoCarta.Espantapajaros);
		}

        public static IEnumerable<Provincia> ObtenerTableroInicial()
        {
            string[] regiones = { "Torino","Milano","Venezia","Genova","Mantova","Ferrara","Parma","Modena",
            "Bologna","Lucca","Firenze","Siena","Urbino","Spoleto","Ancona","Roma","Napoli"};


            string[] desc = { "Torino","Milano","Venezia","Genova","Mantova","Ferrara","Parma","Modena",
            "Bologna","Lucca","Firenze","Siena","Urbino","Spoleto","Ancona","Roma","Napoli"};

            desc[0] = "Antiguo ducado lombardo. Conquistada en 773 por las tropas de Carlomagno y convertida en francesa, se" +
            "convirtió en los siglos XII y XIII en una ciudad libre, para en 1280 pasar a la casa Saboya. En el siglo XV llegará " +
            "a ser capital de Piamonte";
            desc[1] = "Tradicionalmente bajo el poder de los Viconti, fue uno de los pocos lugares de Europa que no fue alcanzado por la peste negra del " +
                "siglo XIV. Los Visconti y después los Sforza mantenían a su servicio a artistas de la talla de Leonardo da Vinci y Bramante";

            List<int[]> ady = new List<int[]>()
            {
                new int[] {2,4},                            //Torino
                new int[] { 1, 3, 4, 5, 7, 8 } ,            //Milano   
                new int[] { 2, 5, 6 },                      //Venezia
                new int[] { 1, 2, 7 },                      //Genova
                new int[] { 2, 3, 6, 8 },                   //Mantova
                new int[] { 3, 5, 8, 9 },                   //Ferrara
                new int[] { 2, 4, 8, 10 },                  //Parma
                new int[] { 2, 4, 5, 6, 9, 10, 11 },        //Modena
                new int[] { 6, 8, 11, 13 },                 //Bologna
                new int[] { 7, 8, 11 },                     //Luca
                new int[] { 8, 9, 10, 12, 13, 14, 16 },     //Firenze
                new int[] { 11, 16 },                       //Siena
                new int[] { 9, 11, 14, 15 },                //Urbino
                new int[] { 11, 13, 15, 16, 17 },           //Spoleto
                new int[] { 13, 14, 17 },                   //Ancona
                new int[] { 11, 12, 14, 17 },               //Roma
                new int[] { 14, 15, 16 }                    //Napoli
            };

            for (int i = 0; i < regiones.Length; i++)
                yield return new Provincia(i + 1, regiones[i], desc[i], ady[i]);
        }
    }
}
