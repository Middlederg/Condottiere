using Condottiere.Core.Provinces;

namespace Condottiere.Core;

public static class NameCreator
{
    private static readonly string[] allNames = { "Ricardio", "Vicente", "Emiliano", "Felix",
                               "Verónica", "Montserrat", "Andrea", "Olga", "Nacho", "Domingo",
                               "César", "Agustín", "Hugo", "Tomás", "Rafael", "Donatello", "Miguel Angel",
                        "Leonardo", "Nieves", "Isabel", "Irene", "Mar", "Alicia", "Carla", "Eva",
                        "Lidia", "Aurora", "Celia", "Claudia", "Amparo", "Sebastián", "Samuel" };

    public static IEnumerable<string> Create(int number)
    {
        List<string> namesCopy = allNames.ToList();

        IEnumerable<string> names = Enumerable.Range(0, number).Select(x =>
        {
            string? item = namesCopy.GetRandomItem();
            if (item is null)
            {
                throw new InvalidOperationException("No names available");
            }
            
            namesCopy.Remove(item);
            return item;
        });

        return names;
    }

  

   
}
