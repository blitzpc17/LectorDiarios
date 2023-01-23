using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Diario
{
    public class ENCABEZADOPRODUCTO
    {
        public string ClaveProducto { get; set; }
        public string ClaveSubProducto { get; set; }
        public string MarcaComercial { get; set; }
        public decimal InventarioFinalMes { get; set; }
        public decimal VecesRecepcionProducto { get; set; }
        public decimal LitrosAcumuladosMes { get; set; }
        public List<DIARIOFILE> ArchivosDiario { get; set; }

    }
}
