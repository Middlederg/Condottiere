using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.ComponentModel;
using System.Text;

namespace Condottiere.Core.Negocio
{
    public static class General
    {
        private static Random r = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Numero aleatorio entre dos números, incluidos ambos
        /// </summary>
        /// <param name="inf">Número inferior</param>
        /// <param name="sup">Número superior</param>
        /// <returns></returns>
        public static int NumAleatorio(int inf, int sup) => r.Next(inf, sup + 1);

        public static T ElementoAleatorio<T>(IEnumerable<T> lista)
        {
            if (lista == null || !lista.Any()) return default(T);
            return lista.ElementAt(NumAleatorio(0, lista.Count() - 1));
        }

        public static Image ObtenerRecurso(string nombre) => (Image)Properties.Resources.ResourceManager.GetObject(nombre);

        public static List<T> DesordenarLista<T>(this List<T> lista)
        {
            List<T> mazoAuxiliar = new List<T>();

            int pasadas = lista.Count();
            for (int i = 0; i < pasadas; i++)
            {
                int pos = r.Next(0, lista.Count);
                T o = lista[pos];
                mazoAuxiliar.Add(o);
                lista.Remove(o);
            }
            return mazoAuxiliar;
        }

        public static string Descripcion(this Enum value)
        {
            Type type = value.GetType();
            DescriptionAttribute atributo = Attribute.GetCustomAttribute(type.GetField(Enum.GetName(type, value)),
                               typeof(DescriptionAttribute)) as DescriptionAttribute;
            return atributo?.Description ?? value.ToString();
        }

        /// <summary>
        /// Devuelve lista T de posibles valores de una enumeración
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetEnumList<T>() => new List<T>((T[])Enum.GetValues(typeof(T)));

        /// <summary>
        /// Devuelve lista strings de posibles valores de una enumeración
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<string> GetEnumListString<T>() => GetEnumList<T>().Select(x => x.ToString());
    }
}
