using Condottiere.Core.Players;
using Condottiere.Core.Provinces;

namespace Condottiere.Web;

public static class ColorExtensions
{
    public static string ToHtml(this Province province)
    {
        if (province.Owner is null)
            return "#fff";

        return province.Owner.ToHtml();
    }

    public static string ToHtml(this Player player) => player.Color.ToHtml();
    public static string ToHtml(this ProvinceOwner owner) => owner.Color.ToHtml();

    public static string ToHtml(this Color color)
    {
        return color switch
        {
            Core.Players.Color.Yellow => "#ff0",
            Core.Players.Color.Blue => "#00f",
            Core.Players.Color.Gray => "#888",
            Core.Players.Color.Red => "#f00",
            Core.Players.Color.Pink => "#f0f",
            Core.Players.Color.Green => "#0f0",
            _ => "#fff",
        };
    }

}
