using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condottiere.Core.Model
{	
	public class Provincia
	{
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int[] Adyacentes { get; set; }
		
		public Provincia(int ident, string nombre, string desc, int[] ady)
		{
			Id = ident;
			Nombre = nombre;
			Descripcion = desc;
            Adyacentes = ady;
		}

        public string Ruta() => Nombre;

        public override string ToString() => Nombre;

        public override bool Equals(Object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                throw new ArgumentOutOfRangeException(GetType().ToString() + " no puede ser nulo");
            Provincia o = obj as Provincia;
            return Id == o.Id;
        }

        public bool Equals(Provincia o)
        {
            if (o is null)
                throw new ArgumentNullException("Objeto " + GetType().ToString() + " es nulo");
            return o.Id == Id;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
