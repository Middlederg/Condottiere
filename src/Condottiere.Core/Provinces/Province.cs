using Condottiere.Core.Players;

namespace Condottiere.Core.Provinces;

public class Province : Entity<int>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int[] Borders { get; init; }
    public required MapPosition Position { get; init; }

    public Province(int id) : base(id) { }

    public ProvinceOwner? Owner { get; private set; }
    public bool HasOwner => Owner is not null;
    public MapPosition? PopePosition { get; private set; }
    public bool HasPope => PopePosition is not null;
    public MapPosition? CondottieroPosition { get; private set; }
    public bool HasCondottiero => CondottieroPosition is not null;

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
        CondottieroPosition = null;
    }

    public void PlacePope(MapPosition position)
    {
        if (Owner is not null)
        {
            throw new InvalidOperationException($"Pope can not be placed in {Name}, because is owned by {Owner.Color}");
        }

        if (HasPope)
        {
            throw new InvalidOperationException($"Pope can not be placed in {Name} to when condottiere is in it");
        }

        PopePosition = position;
    }

    public void PlaceCondottiero(MapPosition position)
    {
        if (HasPope)
        {
            throw new InvalidOperationException($"Condottiero can not be placed in {Name} when pope is in it");
        }

        CondottieroPosition = position;
    }

    public bool IsForbidden(GameContext gameContext)
    {
        if (HasPope || HasCondottiero)
        {
            return true;
        }

        if (gameContext.WithSieges)
        {
            return false;
        }

        return HasOwner;
    }
}

public record ProvinceOwner(int PlayerId, Color Color);

