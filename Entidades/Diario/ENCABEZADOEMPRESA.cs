using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Diario
{
    public class ENCABEZADOEMPRESA
    {
        public string Version { get; set; }
        public string RfcRepresentanteLegal { get; set; }
        public string RfcProveedor { get; set; }
        public string RfcContribuyente { get; set; }
        public string NumeroPermiso { get; set; }
        public string TipoCaracter { get; set; }
        public string ModalidadPermiso { get; set; }
        public string Periodo { get; set; }
    }
}
