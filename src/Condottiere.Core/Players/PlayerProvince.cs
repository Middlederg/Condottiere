using Condottiere.Core.Provinces;

namespace Condottiere.Core.Players;

public record PlayerProvince 
{
    public int Id { get; }
    public string Name { get; }
    public int[] Borders { get; }
    
    public PlayerProvince(Province province)
    {
        Id = province.Id;
        Name = province.Name;
        Borders = province.Borders;
    }
}