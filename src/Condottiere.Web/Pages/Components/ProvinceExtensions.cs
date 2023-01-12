using Condottiere.Core.Provinces;

namespace Condottiere.Web.Pages.Components;

public static class ProvinceExtensions
{
    public static string Color(this Province province)
    {
        if (province.Owner is null)
        {
            return "#fff";
        }

        return province.Owner.Color switch
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

    public static string Opacity(this Province province)
    {
        if (province.Owner is null)
        {
            return "0.01";
        }
        return "1";
    }
}
