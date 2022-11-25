using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class EXISTENCIAS
    {
        public VOLUMENEXISTENCIASANTERIOR VolumenExistenciasAnterior { get; set; }
        public VOLUMENACUMOPSRECEPCION VolumenAcumOpsRecepcion { get; set; }
        public string HoraRecepcionAcumulado { get; set; }
        public VOLUMENACUMOPSENTREGA VolumenAcumOpsEntrega { get; set; }
        public string HoraEntregaAcumulado { get; set; }
        public VOLUMENEXISTENCIAS VolumenExistencias { get; set; }
        public DateTime? FechaYHoraEstaMedicion { get; set; }
        public DateTime? FechaYHoraMedicionAnterior { get; set; }


    }
}
