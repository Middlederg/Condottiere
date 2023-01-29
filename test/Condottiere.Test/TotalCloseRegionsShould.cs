using Condottiere.Core;
using Condottiere.Core.Players;
using Condottiere.Core.Provinces;

using FluentAssertions;

using System;
using Xunit;

namespace Condottiere.Test;

public class TotalCloseRegionsShould
{
    private readonly Map map = new();
    private Player NewPlayer() => new(1, "Player 1", Color.Blue, Profile.Human);

    [Fact]
    public void Be_0_when_player_has_no_provinces()
    {
        Player player = NewPlayer();
        player.TotalCloseRegions().Should().Be(0);
    }

    [Fact]
    public void Be_1_when_player_has_only_one_province()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TotalCloseRegions().Should().Be(1);
    }

    [Fact]
    public void Be_1_when_player_has_2_far_way_provinces()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Napoli);
        player.TotalCloseRegions().Should().Be(1);
    }

    [Fact]
    public void Be_1_when_player_has_3_far_way_provinces()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Spoleto);
        player.TotalCloseRegions().Should().Be(1);
    }

    [Fact]
    public void Be_1_when_player_has_4_far_way_provinces()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Venezia);
        player.TakeControl(map.Ancona);
        player.TakeControl(map.Siena);
        player.TakeControl(map.Modena);
        player.TotalCloseRegions().Should().Be(1);
    }

    [Fact]
    public void Be_2_when_player_has_Milano_and_Venezia()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Venezia);
        player.TotalCloseRegions().Should().Be(2);
    }

    [Fact]
    public void Be_2_when_player_has_Spoleto_and_Firenze()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Spoleto);
        player.TakeControl(map.Firenze);
        player.TotalCloseRegions().Should().Be(2);
    }

    [Fact]
    public void Be_2_when_player_has_Milano_Venezia_and_Firenze()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Venezia);
        player.TakeControl(map.Firenze);
        player.TotalCloseRegions().Should().Be(2);
    }

    [Fact]
    public void Be_2_when_player_has_Milano_Roma_and_Siena()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Roma);
        player.TakeControl(map.Milano);
        player.TakeControl(map.Siena);
        player.TotalCloseRegions().Should().Be(2);
    }

    [Fact]
    public void Be_2_when_player_has_Torino_Venezia_Firenze_and_Spoleto()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Torino);
        player.TakeControl(map.Venezia);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Spoleto);
        player.TotalCloseRegions().Should().Be(2);
    }

    [Fact]
    public void Be_2_when_player_has_Milano_Venezia_Firenze_and_Spoleto()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Venezia);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Spoleto);
        player.TotalCloseRegions().Should().Be(2);
    }

    [Fact]
    public void Be_3_when_player_has_Milano_Modena_Firenze()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Modena);
        player.TakeControl(map.Firenze);
        player.TotalCloseRegions().Should().Be(3);
    }

    [Fact]
    public void Be_3_when_player_has_Milano_Venezia_Firenze_Spoleto_and_Siena()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Venezia);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Spoleto);
        player.TakeControl(map.Siena);
        player.TotalCloseRegions().Should().Be(3);
    }

    [Fact]
    public void Be_4_when_player_has_Milano_Venezia_Firenze_Spoleto_Siena_and_Ancona()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Milano);
        player.TakeControl(map.Venezia);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Spoleto);
        player.TakeControl(map.Siena);
        player.TakeControl(map.Ancona);
        player.TotalCloseRegions().Should().Be(4);
    }

    [Fact]
    public void Be_4_when_player_has_Ancona_Spoleto_Firenze_Lucca()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Ancona);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Spoleto);
        player.TakeControl(map.Lucca);
        player.TotalCloseRegions().Should().Be(4);
    }

    [Fact]
    public void Be_5_when_player_has_Bologna_Firenze_Siena_Spoleto_Napoli()
    {
        Player player = NewPlayer();
        player.TakeControl(map.Bologna);
        player.TakeControl(map.Firenze);
        player.TakeControl(map.Siena);
        player.TakeControl(map.Spoleto);
        player.TakeControl(map.Napoli);
        player.TotalCloseRegions().Should().Be(5);
    }

    [Fact]
    public void Be_17_when_player_has_all_provinces()
    {
        Player player = NewPlayer();

        foreach (Province province in map.Provinces)
        {
            player.TakeControl(province);
        }
        
        player.TotalCloseRegions().Should().Be(map.Provinces.Count);
    }
}
