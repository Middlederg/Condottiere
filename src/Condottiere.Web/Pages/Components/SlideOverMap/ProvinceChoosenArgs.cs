using Condottiere.Core.Provinces;
using Condottiere.Web.Pages.Components.Map.Provinces;

using Microsoft.AspNetCore.Components.Web;

namespace Condottiere.Web.Pages.Components.SlideOverMap;

public record ProvinceChoosenArgs
{
    public MouseEventArgs MouseArgs { get; set; }
    public Province Province { get; set; }
    public MapSelectionOption Option { get; }
    public ProvinceChoosenArgs(ProvinceClickedArgs args, MapSelectionOption option)
    {
        Province = args.Province;
        MouseArgs = args.MouseArgs;
        Option = option;
    }
    public MapPosition Position => new(MouseArgs.OffsetX - 10, MouseArgs.OffsetY - 10);

}
