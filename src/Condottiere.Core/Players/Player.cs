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
    public Difficulty Profile { get; }

    public Status? status;

    public Player(int id, string name, Color color, Difficulty profile) : base(id)
    {
        Name = name;
        Color = color;
        Profile = profile;
        Hand = new List<Card>();
        Army = new List<Card>();
        OwnedProvinces = new List<PlayerProvince>();
    }

    public void ResetBattleLines(IEnumerable<Card> newCards, bool isDefending)
    {
        Hand = newCards.ToList();
        status = new Status(isDefending);
        Army = new List<Card>();
    }

    public bool CanPlayMoreCards
    {
        get
        {
            if (status is null)
            {
                return false;
            }
            
            return Hand.Any() && status.CanPlayMoreCards;
        }
    }

    public void Play(Card card)
    {
        Hand.Remove(card);
        Army.Add(card);
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

    /// <summary>
    /// Indica el número de regiones adyacentes que controla el jugador
    /// </summary>
    /// <returns></returns>
    public int NumRegionesAdyacentes()
    {
        List<KeyValuePair<int, int>> lista = new List<KeyValuePair<int, int>>();

        foreach (Province prov in OwnedProvinces)
        {
            //Resto de provincias
            var posiblesAdyacentes = OwnedProvinces.Where(x => !x.Equals(prov)).Select(x => x.Id);

            //Añade ocurrencia con id de provincia y numero de adyacentes
            lista.Add(new KeyValuePair<int, int>(prov.Id, prov.Borders.Intersect(posiblesAdyacentes).Count()));
        }

        //Si cualquier región tiene 3 o más coincidencias, ya hay 4 regiones adyacentes
        if (lista.Any(x => x.Value >= 3))
            return 4;

        //Si cualquier región tiene 2 o más coincidencias, y entre esas regiones son coincidentes, hay 3 regiones unidas por lo menos
        if (lista.Any(x => x.Value >= 2))
        {
            foreach (var item in lista.Where(x => x.Value >= 2))
            {
                Province p = OwnedProvinces.Find(x => x.Id == item.Key); //seleciona la provincia

                var ids = lista.Where(x => x.Key != p.Id).Select(x => x.Key); //coge el resto de ids de provincia
                var posiblesAdyacentes = OwnedProvinces.Where(x => ids.Contains(x.Id)).SelectMany(x => x.Borders);
                if (p.Borders.Intersect(posiblesAdyacentes).Any())
                    return 3;
            }
            return 2;
        }
        return lista.Any(x => x.Value >= 1) ? 1 : 0;
    }

    public int CardsToDraw => 10 + OwnedProvinces.Count - Hand.Count;

    public override string ToString() => Name;

    public bool Owns(int provinceId)
    {
        return OwnedProvinces.Any(x => x.Id == provinceId);
    }

    public void TakeControl(Province province)
    {
        OwnedProvinces.Add(new PlayerProvince(province));
    }
}
