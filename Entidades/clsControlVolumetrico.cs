using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class clsControlVolumetrico
    {
        public string RfcClienteOProveedor { get; set; }
        public string NombreClienteOProveedor { set; get; }
        public string CFDI { get; set; }
        public DateTime FechaYHoraTransaccion { get; set; }
        public decimal ValorNumerico { get; set; }
    }
}
