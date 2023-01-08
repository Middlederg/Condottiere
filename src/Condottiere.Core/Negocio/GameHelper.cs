using Condottiere.Core.Provinces;

namespace Condottiere.Core;

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

  

   
}
