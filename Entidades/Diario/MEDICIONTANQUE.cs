using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class MEDICIONTANQUE
    {
        public string SistemaMedicionTanque { get; set; }
        public string LocalizODescripSistMedicionTanque { get; set; }
        public DateTime? VigenciaCalibracionSistMedicionTanque { get; set; }
        public decimal? IncertidumbreMedicionSistMedicionTanque { get; set; }
    }
}
