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
            Drums => RenderIcon<DrumsIcon>(builder, "w-12 h-12"),
            Heroine => RenderIcon<HeroineIcon>(builder),
            Bishop => RenderIcon<BishopIcon>(builder, "w-12 h-12"),
            Scarecrow => RenderIcon<ScarecrowIcon>(builder, "w-12 h-12"),
            Surrender => RenderIcon<SurrenderIcon>(builder, "w-12 h-12"),
            Mercenary => RenderIcon<MercenaryIcon>(builder),
            SeasonChanger changer => changer.Season == Season.Winter ? RenderIcon<WinterIcon>(builder, "w-12 h-12") : RenderIcon<SpringIcon>(builder, "w-12 h-12"),
            _ => throw new NotImplementedException()
        };
    }

    private bool RenderIcon<T>(RenderTreeBuilder builder, string? style = null) where T : IComponent
    {
        builder.OpenComponent<T>(index++);
        if (style is not null)
        {
            builder.AddAttribute(index++, "Style", style);
        }
        builder.CloseComponent();
        return true;
    }
}
