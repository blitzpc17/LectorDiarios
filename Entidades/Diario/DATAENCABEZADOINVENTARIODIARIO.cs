using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Diario
{
    public class DATAENCABEZADOINVENTARIODIARIO
    {
        public ENCABEZADOEMPRESA DataEmpresa { get; set; }
        public List<ENCABEZADOPRODUCTO> DataProducto { get; set; }
        
    }
}
