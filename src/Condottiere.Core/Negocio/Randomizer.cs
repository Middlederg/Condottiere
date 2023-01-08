namespace Condottiere.Core;

public static class Randomizer
{
    private static readonly Random random = new(DateTime.Now.Millisecond);
    private static int GetRandomNumber(int inf, int sup) => random.Next(inf, sup + 1);
    public static T? GetRandomItem<T>(this IEnumerable<T> lista)
    {
        if (lista == null || !lista.Any())
            return default;

        return lista.ElementAt(GetRandomNumber(0, lista.Count() - 1));
    }
    public static IEnumerable<T> Shuffle<T>(this List<T> source)
    {
        List<T> result = new();

        int totalItems = source.Count;
        for (int i = 0; i < totalItems; i++)
        {
            int pos = random.Next(0, source.Count);
            T o = source[pos];
            result.Add(o);
            source.Remove(o);
        }
        return result;
    }
}
