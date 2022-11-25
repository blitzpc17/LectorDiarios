using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class DISPENSARIO
    {
        public string ClaveDispensario { get; set; }
        public MEDICIONDISPENSARIO MedicionDispensario { get; set; }
        public List<MANGUERA> Mangueras { get; set; }

    }
}
