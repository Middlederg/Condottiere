using System.ComponentModel;

namespace Condottiere.Core.Enums
{
    public enum Estacion
    {
        [Description("")]
        Ninguna = 0,

        [Description("Todos los mercenarios tienen valor 1")]
        Invierno = 1,

        [Description("+3 al mercenario de mayor valor")]
        Primavera = 2
    }
}