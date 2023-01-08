using Condottiere.Core.Players;

namespace Condottiere.Core.Provinces;

public class Province : Entity<int>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int[] Borders { get; init; }

    public Province(int id) : base(id) { }

    public ProvinceOwner? Owner { get; private set; }
    public bool HasPope { get; private set; }
    public bool HasCondottiere { get; private set; }

    public string Path() => Name;

    public override string ToString() => Name;

    public void TakeControl(Player player)
    {
        if (Owner is not null)
        {
            throw new InvalidOperationException($"Province {Name} can not be taken, because is already owned by {Owner.Color}");
        }

        if (HasPope)
        {
            throw new InvalidOperationException($"Province {Name} can not be taken when pope is in it");
        }

        Owner = new ProvinceOwner(player.Id, player.Color);
        HasCondottiere = false;
    }

    public void PlacePope()
    {
        if (Owner is not null)
        {
            throw new InvalidOperationException($"Pope can not be placed in {Name}, because is owned by {Owner.Color}");
        }

        if (HasCondottiere)
        {
            throw new InvalidOperationException($"Pope can not be placed in {Name} to when condottiere is in it");
        }

        HasPope = true;
    }

    public void PlaceCondottiero()
    {
        if (HasPope)
        {
            throw new InvalidOperationException($"Condottiero can not be placed in {Name} when pope is in it");
        }

        HasCondottiere = true;
    }
}

public record ProvinceOwner(int PlayerId, Color Color);

