using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Condottiere.Core;
using System;
using Condottiere.Core.Cards;
using Condottiere.Web.Shared.Icons.Cards;

namespace Condottiere.Web.Pages.Components.Cards;

public class CardIconRenderer : ComponentBase
{
    [Parameter]
    public Card? Card { get; set; }

    [Parameter]
    public string Style { get; set; } = "w-8 w-8 md:w-12 md:h-12";

    private static int index = 0;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Card is null)
        {
            return;
        }

        _ = Card switch
        {
            Courtisan => RenderIcon<CourtisanIcon>(builder),
            Drums => RenderIcon<DrumsIcon>(builder),
            Heroine => RenderIcon<HeroineIcon>(builder),
            Bishop => RenderIcon<BishopIcon>(builder),
            Scarecrow => RenderIcon<ScarecrowIcon>(builder),
            Surrender => RenderIcon<SurrenderIcon>(builder),
            Mercenary mercenary => RenderIcon<MercenaryIcon>(builder, mercenary.Value == 10),
            SeasonChanger changer => changer.Season == Season.Winter ? RenderIcon<WinterIcon>(builder) : RenderIcon<SpringIcon>(builder),
            _ => throw new NotImplementedException()
        };
    }

    private bool RenderIcon<T>(RenderTreeBuilder builder, bool isStrong = false) where T : IComponent
    {
        builder.OpenComponent<T>(index++);
        if (Style is not null)
        {
            builder.AddAttribute(index++, "Style", Style);
        }

        if (isStrong)
        {
            builder.AddAttribute(index++, "IsStrong", isStrong);
        }
        
        builder.CloseComponent();
        return true;
    }
}
