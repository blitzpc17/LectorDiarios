using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class ENTREGAS
    {
        public int TotalEntregas { get; set; }
        public SUMAVOLUMENENTREGADO SUMAVOLUMENENTREGADO { get; set; }
        public int TotalDocumentos { get; set; }
        public List<ENTREGA> ENTREGA { get; set; }
    }
}
