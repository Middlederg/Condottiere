using Condottiere.Core;
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
            Color.Yellow => "#ff0",
            Color.Blue => "#00f",
            Color.Gray => "#888",
            Color.Red => "#f00",
            Color.Pink => "#f0f",
            Color.Green => "#0f0",
            _ => "#fff",
        };
    }

    public static string ToTailwindText(this Color color) => $"text-{color.ToTailwindColor()}-700";
    public static string ToTailwindBox(this Color color) => $"text-{color.ToTailwindColor()}-700 bg-{color.ToTailwindColor()}-50 border-{color.ToTailwindColor()}-200";
    public static string ToTailwindColor(this Color color)
    {
        return color switch
        {
            Color.Yellow => "yellow",
            Color.Blue => "sky",
            Color.Gray => "gray",
            Color.Red => "red",
            Color.Pink => "purple",
            Color.Green => "emerald",
            _ => "",
        };
    }

    public static string ToTailwindColor(this Card card)
    {
        return card.Type switch
        {
            CardType.Mercenary => "gray-700",
            CardType.Special => "teal-700",
            CardType.Action => "gray-500",
            _ => "",
        };
    }
}
