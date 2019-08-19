using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIClimaTempo
{
    public class ClimaTempo
    {
        public String Data { get; set; }
        public int UmidadeMax { get; set; }
        public int UmidadeMin { get; set; }
        public int ProbabilidadeChuva { get; set; }
        public int VelocidadeMinimaVento { get; set; }
        public int VelocidadeMaximaVento { get; set; }
        public int SensacaoTermicaMaxima { get; set; }
        public int SensacaoTermicaMinima { get; set; }
        public String Tempo { get; set; }
        public int TemperaturaMaxima { get; set; }
        public int TemperaturaMinima { get; set; }
        public String NascerSol { get; set; }
        public String PorSol { get; set; }

    }
}
