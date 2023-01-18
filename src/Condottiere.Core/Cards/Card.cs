using Condottiere.Core.Cards;

namespace Condottiere.Core;

public abstract class Card : Entity<int>
{
    public abstract CardType Type { get; }

    public virtual string Path() => $"{GetType().Name.ToLower()}.jpg";

    protected Card(int id) : base(id) { }

    public T As<T>() where T : Card
    {
        if (this is not T parsed)
        {
            throw new ArgumentException($"{this} can not be converted to {typeof(T)}");
        }
        
        return parsed;
    }
}
