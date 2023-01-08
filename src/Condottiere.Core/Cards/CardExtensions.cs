namespace Condottiere.Core;

public static class CardExtensions
{
    public static IEnumerable<T> GetOfType<T>(this IEnumerable<Card> cards) where T : Card
    {
        return cards.Where(x => x is T)
            .Select(x => x.As<T>());
    }
    
    public static bool IsPlayed<T>(this IEnumerable<Card> cards) where T : Card
    {
        return cards.Any(x => x is T);
    }
}
