using Condottiere.Core.Players;

namespace Condottiere.Core.Provinces;
public class Map
{
    public List<Province> Provinces { get; set; }

    public Map()
    {
        Provinces = MapCreator.Create().ToList();
    }

    public void PlaceCondottiero(int provinceId)
    {
        Province province = SearchProvince(provinceId);
        province.PlaceCondottiero();
    }

    public void PlacePope(int provinceId)
    {
        Province province = SearchProvince(provinceId);
        province.PlacePope();
    }

    public void TakeControl(int provinceId, Player player)
    {
        Province province = SearchProvince(provinceId);
        province.TakeControl(player);
    }

    private Province SearchProvince(int provinceId)
    {
        Province? province = Provinces.FirstOrDefault(p => p.Id == provinceId);
        
        if (province is null)
        {
            throw new InvalidOperationException($"Province with id {provinceId} is not in the map");
        }

        return province;
    }

}
