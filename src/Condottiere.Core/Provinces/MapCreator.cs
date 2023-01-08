namespace Condottiere.Core.Provinces;

public class MapCreator
{
    public static IEnumerable<Province> Create()
    {
        return new List<Province>()
        {
            new Province(1)
            {
                Name = "Torino",
                Description = "Antiguo ducado lombardo. Conquistada en 773 por las tropas de Carlomagno y convertida en francesa, se" +
                "convirtió en los siglos XII y XIII en una ciudad libre, para en 1280 pasar a la casa Saboya. En el siglo XV llegará " +
                "a ser capital de Piamonte",
                Borders = new int[] { 2, 4 }
            },
            new Province(2)
            {
                Name = "Milano",
                Description = "Tradicionalmente bajo el poder de los Viconti, fue uno de los pocos lugares de Europa que no fue alcanzado por la peste negra del " +
                "siglo XIV. Los Visconti y después los Sforza mantenían a su servicio a artistas de la talla de Leonardo da Vinci y Bramante",
                Borders = new int[] { 1, 3, 4, 5, 7, 8 }
            },
            new Province(3)
            {
                Name = "Venezia",
                Description = "",
                Borders = new int[] { 2, 5, 6 }
            },
            new Province(4)
            {
                Name = "Genova",
                Description = "",
                Borders = new int[] { 1, 2, 7 }
            },
            new Province(5)
            {
                Name = "Mantova",
                Description = "",
                Borders = new int[] { 2, 3, 6, 8 }
            },
            new Province(6)
            {
                Name = "Ferrara",
                Description = "",
                Borders = new int[] { 3, 5, 8, 9 }
            },
            new Province(7)
            {
                Name = "Parma",
                Description = "",
                Borders = new int[] { 2, 4, 8, 10 }
            },
            new Province(8)
            {
                Name = "Modena",
                Description = "",
                Borders = new int[] { 2, 4, 5, 6, 9, 10, 11 }
            },
            new Province(9)
            {
                Name = "Bologna",
                Description = "",
                Borders = new int[] { 6, 8, 11, 13 }
            },
            new Province(10)
            {
                Name = "Lucca",
                Description = "",
                Borders = new int[] { 7, 8, 11 }
            },
            new Province(11)
            {
                Name = "Firenze",
                Description = "",
                Borders = new int[] { 8, 9, 10, 12, 13, 14, 16 }
            },
            new Province(12)
            {
                Name = "Siena",
                Description = "",
                Borders = new int[] { 11, 16 }
            },
            new Province(13)
            {
                Name = "Urbino",
                Description = "",
                Borders = new int[] { 9, 11, 14, 15 }
            },
            new Province(14)
            {
                Name = "Spoleto",
                Description = "",
                Borders = new int[] { 11, 13, 15, 16, 17 }
            },
            new Province(15)
            {
                Name = "Ancona",
                Description = "",
                Borders = new int[] { 13, 14, 17 }
            },
            new Province(16)
            {
                Name = "Roma",
                Description = "",
                Borders = new int[] { 11, 14, 17 }
            },
            new Province(17)
            {
                Name = "Napoli",
                Description = "",
                Borders = new int[] { 14, 15, 16 }
            }
        };
    }
}
