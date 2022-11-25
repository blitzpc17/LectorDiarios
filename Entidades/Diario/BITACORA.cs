using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class BITACORA
    {
        public int NumeroRegistro { get; set; }
        public DateTime FechaYHoraEvento { get; set; }
        public string UsuarioResponsable { get; set; }
        public int TipoEvento { get; set; }
        public string DescripcionEvento { get; set; }
    }
}
