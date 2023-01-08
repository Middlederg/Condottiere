using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Condottiere.Core.Negocio;

namespace Condottiere.Core.Model
{
    public class Mapa
    {
        public List<Provincia> ProvinciasDisponibles { get; set; }
        public int UbicacionCondottiere { get; set; }
        public int UbicacionFavorPapa { get; set; }

        public Mapa()
        {
            ProvinciasDisponibles = GameHelper.ObtenerTableroInicial().ToList();
            UbicacionCondottiere = -1;
            UbicacionFavorPapa = -1;
        }

        /// <summary>
        /// Determina si se puede colocar el condottiero en una region para comenzar una batalla por ella
        /// Debe estar disponible, sin ocupar aun
        /// El papa no debe estar en ella
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool CondottieroColocable(Provincia p)
        {
            if (!ProvinciasDisponibles.Contains(p))
                return false;

            if (UbicacionFavorPapa == ProvinciasDisponibles.IndexOf(p))
                return false;

            return true;
        }

        /// <summary>
        /// Determina si se puede colocar el papa en una region tras jugar el obispo
        /// Debe estar disponible, sin ocupar aun
        /// El condottiere no debe estar en ella
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool FavorPapaColocable(Provincia p)
        {
            if (!ProvinciasDisponibles.Contains(p))
                return false;

            if (UbicacionCondottiere == ProvinciasDisponibles.IndexOf(p))
                return false;

            return true;
        }

        /// <summary>
        /// Devuelve lista de regiones donde se puede colocar el Condottiero
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<Provincia> ColocarCondottiero()
        {
            if (UbicacionFavorPapa == -1)
                return ProvinciasDisponibles;
            return ProvinciasDisponibles.Where(x => !x.Equals(ProvinciasDisponibles[UbicacionFavorPapa])).ToList();
        }

        /// <summary>
        /// Devuelve lista de regiones donde se puede colocar el Papa
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<Provincia> ColocarFavorPapa()
        {
            if (UbicacionFavorPapa == -1)
                return ProvinciasDisponibles;
            return ProvinciasDisponibles.Where(x => !x.Equals(ProvinciasDisponibles[UbicacionFavorPapa])).ToList();
        }

    }
}
