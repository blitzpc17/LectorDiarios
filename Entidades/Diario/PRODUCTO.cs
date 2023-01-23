using Entidades.Diario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class PRODUCTO
    {
        public string ClaveProducto { get; set; }
        public string ClaveSubProducto { get; set; }
        public GASOLINA Gasolina { get; set; }
        public string MarcaComercial { get; set; }
        public List<TANQUE> Tanques { get; set; }
        public List<DISPENSARIO> Dispensarios { get; set; }

    }
}
