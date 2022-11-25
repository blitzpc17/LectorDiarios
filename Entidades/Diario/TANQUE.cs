using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class TANQUE
    {
        public string ClaveIdentificacionTanque { get; set; }
        public string LocalizacionYODescripcionTanque { get; set; }
        public DateTime VigenciaCalibracionTanque { get; set; }
        public CAPACIDADTOTALTANQUE CapacidadTotalTanque { get; set; }
        public CAPACIDADOPERATIVATANQUE CapacidadOperativaTanque { get; set; }
        public CAPACIDADUTILTANQUE CapacidadUtilTanque { get; set; }
        public VOLUMENMINIMOOPERACION VolumenMinimoOperacion { get; set; }
        public string EstadoTanque { get; set; }
        public MEDICIONTANQUE MedicionTanque { get; set; }
        public EXISTENCIAS Existencias { get; set; }
        public RECEPCIONES Recepciones { get; set;}
        public ENTREGAS Entregas { get; set; }


    }
}
