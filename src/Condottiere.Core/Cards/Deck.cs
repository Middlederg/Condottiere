namespace Condottiere.Core.Cards;

public class Deck
{
    private List<Card> cards;
    private List<Card> discardPile;
    public bool IsEmpty() => !cards.Any();

    public Deck(GameContext gameContext)
    {
        cards = Randomizer.Shuffle(Create(gameContext).ToList()).ToList();
        discardPile = new List<Card>();
    }

    public Card Draw()
    {
        if (IsEmpty())
        {
            throw new Exception("Can not draw card because deck is empty");
        }
        
        Card card = cards.First();
        cards.Remove(card);
        return card;
    }

    public bool CanDeal(int amount)
    {
        return cards.Count >= amount;
    }
    
    public void IncorporateDiscardPile()
    {
        IEnumerable<Card> suffledDiscardPile = Randomizer.Shuffle(discardPile);
        cards = cards.Concat(suffledDiscardPile).ToList();
    }

    public void ToDiscard(IEnumerable<Card> cardsToDiscard)
    {
        discardPile = discardPile.Concat(cardsToDiscard).ToList();
    }

    private IEnumerable<Card> Create(GameContext gameContext)
    {
        int index = 0;

        foreach (int i in Enumerable.Range(0, 10))
            yield return new Mercenary(index++, 1);

        foreach (int i in Enumerable.Range(0, 8))
        {
            yield return new Mercenary(index++, 2);
            yield return new Mercenary(index++, 3);
            yield return new Mercenary(index++, 4);
            yield return new Mercenary(index++, 5);
            yield return new Mercenary(index++, 6);
            yield return new Mercenary(index++, 10);
        }

        foreach (int i in Enumerable.Range(0, 12))
            yield return new Courtisan(index++);

        foreach (int i in Enumerable.Range(0, 3))
        {
            yield return new Heroine(index++);

            if (gameContext.SpringOptions.UseSpring)
                yield return SeasonChanger.Spring(index++);
            yield return SeasonChanger.Winter(index++);
            yield return new Surrender(index++);
        }

        foreach (int i in Enumerable.Range(0, gameContext.BishopOptions.TotalCards()))
        {
            yield return new Drums(index++);
            yield return new Bishop(index++);
        }

        foreach (int i in Enumerable.Range(0, 16))
            yield return new Scarecrow(index++);
    }
}
