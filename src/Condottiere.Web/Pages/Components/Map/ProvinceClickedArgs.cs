using Condottiere.Core;
using Condottiere.Core.Provinces;
using Microsoft.AspNetCore.Components.Web;

namespace Condottiere.Web.Pages.Components.Map.Provinces;

public record ProvinceClickedArgs(MouseEventArgs Args, Province Province) 
{
    public MapPosition Position => new(Args.OffsetX - 10, Args.OffsetY - 10);

    public bool IsForbiden(GameContext gameContext) => Province.IsForbidden(gameContext);
}
