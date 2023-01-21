using Condottiere.Core.Players;

namespace Condottiere.Core.Provinces;
public class Map
{
    public List<Province> Provinces { get; set; }
    
    private Province Get(int id) => Provinces.First(p => p.Id == id);

    public Map()
    {
        Provinces = MapCreator.Create().ToList();
    }

    public Province? GetBattleProvince()
    {
        Province? province = Provinces.SingleOrDefault(x => x.HasCondottiero);
        return province;
    }

    public Province? GetPopeProvince()
    {
        Province? province = Provinces.SingleOrDefault(x => x.HasPope);
        return province;
    }

    public void PlaceCondottiero(int provinceId, MapPosition position)
    {
        Province province = SearchProvince(provinceId);
        province.PlaceCondottiero(position);
    }

    public void PlacePope(int provinceId, MapPosition position)
    {
        Province province = SearchProvince(provinceId);
        province.PlacePope(position);
    }

    public void TakeControl(Province province, Player player)
    {
        //Province province = SearchProvince(province.Id);
        province.TakeControl(player);
        player.TakeControl(province);
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

    public Province Torino => Get(1);
    public Province Milano => Get(2);
    public Province Venezia => Get(3);
    public Province Genova => Get(4);
    public Province Mantova => Get(5);
    public Province Ferrara => Get(6);
    public Province Parma => Get(7);
    public Province Modena => Get(8);
    public Province Bologna => Get(9);
    public Province Lucca => Get(10);
    public Province Firenze => Get(11);
    public Province Siena => Get(12);
    public Province Urbino => Get(13);
    public Province Spoleto => Get(14);
    public Province Ancona => Get(15);
    public Province Roma => Get(16);
    public Province Napoli => Get(17);
}

public record MapPosition(double X, double Y);