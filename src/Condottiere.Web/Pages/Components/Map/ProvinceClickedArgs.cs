using Condottiere.Core;
using Condottiere.Core.Provinces;

using Microsoft.AspNetCore.Components.Web;

namespace Condottiere.Web.Pages.Components.Map.Provinces;

public record ProvinceClickedArgs(MouseEventArgs MouseArgs, Province Province) 
{
    public bool IsForbiden(GameContext gameContext) => Province.IsForbidden(gameContext);
}
