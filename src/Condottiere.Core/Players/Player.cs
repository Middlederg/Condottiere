using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Condottiere.Core.Cards;
using Condottiere.Core.Provinces;

namespace Condottiere.Core.Players;

public class Player : Entity<int>
{
    public string Name { get; }
    public List<Card> Hand { get; private set; }
    public List<Card> Army { get; private set; }
    public List<PlayerProvince> OwnedProvinces { get; private set; }
    public Color Color { get; }
    public Profile Profile { get; }

    public Status? status;

    public Player(int id, string name, Color color, Profile profile) : base(id)
    {
        Name = name;
        Color = color;
        Profile = profile;
        Hand = new List<Card>();
        Army = new List<Card>();
        OwnedProvinces = new List<PlayerProvince>();
    }

    public void ResetBattleLines(bool isDefending)
    {
        status = new Status(isDefending);
        Army = new List<Card>();
    }

    public void NewRound(IEnumerable<Card> newCards)
    {
        Hand = newCards.ToList();
    }

    //public bool HasPassed => status?.HasPassed ?? false;
    public bool CanPlayMoreCards
    {
        get
        {
            if (status is null)
            {
                return false;
            }
            
            return Hand.Any() && !status.HasPassed;
        }
    }

    public void Play(Card card)
    {
        Hand.Remove(card);
        Army.Add(card);
        status?.Play();
    }

    public void Pass()
    {
        status?.Pass();
    }
    
    public int Points(GameContext gameContext)
    {
        IEnumerable<Mercenary> mercenaries = Army.GetOfType<Mercenary>();

        int points = gameContext.IsWinter() ? mercenaries.Count() : mercenaries.Sum(x => x!.Value);

        if (Army.Any(x => x is Drums))
            points *= 2;

        if (gameContext.SpringOptions.UseSpring && gameContext.CurrentSeason == Season.Spring && gameContext.HighestMercenary is not null)
        {
            int HighestMercenariesCount = mercenaries.Count(x => x.Value == gameContext.HighestMercenary.Value);
            points = HighestMercenariesCount * gameContext.SpringOptions.HighestMercenaryBonus;
        }

        points += Army.GetOfType<Heroine>().Sum(x => x.Value);
        points += Army.GetOfType<Courtisan>().Sum(x => x.Value);

        return points;
    }

    public PlayerSummary ToSummary(GameContext gameContext) => new(Id, Name, Color, Points(gameContext));

    public int TotalCloseRegions()
    {
        if (OwnedProvinces.Count == 0 || OwnedProvinces.Count == 1)
        {
            return OwnedProvinces.Count;
        }

        IEnumerable<int> allBorders = OwnedProvinces.SelectMany(x => x.Borders);

        var frontiers = new List<int>();
        
        foreach (PlayerProvince province in OwnedProvinces)
        {
            bool isFrontier = allBorders.Contains(province.Id);
            if (isFrontier)
            {
                frontiers.Add(province.Id);
            }
        }

        return frontiers.Distinct() + 1;
    }

    public int CardsToDraw => GameContext.InitialHand + OwnedProvinces.Count - Hand.Count;

    public override string ToString() => Name;

    public bool Owns(int provinceId)
    {
        return OwnedProvinces.Any(x => x.Id == provinceId);
    }

    public void TakeControl(Province province)
    {
        OwnedProvinces.Add(new PlayerProvince(province));
    }

    public bool IsAboutToWin(GameContext gameContext, Province? battleProvince)
    {
        if (!CanPlayMoreCards)
        {
            return false;
        }

        return false; //TODO
    }

    public Card? Choose() => Hand.GetRandomItem();
}
    
