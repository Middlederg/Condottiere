using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Condottiere.Core.Enums;
using Condottiere.Core.Negocio;

namespace Condottiere.Core.Model
{
    public class Carta
    {
        public TipoCarta Tipo { get; set; }
        public int Valor { get; set; }

        public Carta(TipoCarta tipo, int valor = 0)
        {
            Tipo = tipo;
            Valor = valor;
        }

        public bool EsMercenario() => Tipo.Equals(TipoCarta.Mercenario);
        public bool EsCortesana() => Tipo.Equals(TipoCarta.Cortesana);
        public bool EsTambor() => Tipo.Equals(TipoCarta.Tambor);
        public bool EsPrimavera() => Tipo.Equals(TipoCarta.Primavera);
        public bool EsInvierno() => Tipo.Equals(TipoCarta.Invierno);
        public bool EsObispo() => Tipo.Equals(TipoCarta.Obispo);
        public bool EsRendicion() => Tipo.Equals(TipoCarta.Rendicion);
        public bool EsEspantapajaros() => Tipo.Equals(TipoCarta.Espantapajaros);

        public string Ruta() => Tipo.ToString() + (EsMercenario() ? Valor.ToString() : "");
        public string RutaSD() => Ruta() + "sd";

        public override string ToString() => Tipo.Descripcion() + (EsMercenario() ? " " + Valor.ToString() : "");

        public override bool Equals(Object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                throw new ArgumentOutOfRangeException(GetType().ToString() + " no puede ser nulo");
            Carta o = obj as Carta;
            return o.Tipo.Equals(Tipo) && o.Valor == Valor;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
