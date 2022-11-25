using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class CFDI
    {
        public string Cfdi { get; set; }
        public string TipoCFDI { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioDeVentaAlPublico { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaYHoraTransaccion { get; set; }
        public VOLUMENDOCUMENTADO VolumenDocumentado { get; set; }
    }
}
