using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class NACIONAL
    {
        public string RfcClienteOProveedor { get; set; }
        public string NombreClienteOProveedor { get;  set; }
        public string PermisoProveedor { get; set; }
        public CFDI Cfdis { get; set; }
    }
}
