using System.Collections.Generic;

namespace CultivaTechPoc
{
    internal class Producao
    {
        public string Fruta { get; set; }
        public List<string> InsumosNecessarios { get; set; }
        public string Cidade { get; set; }
        public string MesIdeal { get; set; }
        public int PrazoColheitaMeses { get; set; }
    }
}
