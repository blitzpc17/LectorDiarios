using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class MEDICIONDISPENSARIO
    {
        public string SistemaMedicionDispensario { get; set; }
        public string LocalizODescripSistMedicionDispensario { get; set; }
        public DateTime VigenciaCalibracionSistMedicionDispensario { get; set; }
        public decimal IncertidumbreMedicionSistMedicionDispensario { get; set; }
    }
}
