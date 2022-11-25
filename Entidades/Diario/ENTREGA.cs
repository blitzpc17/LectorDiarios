using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Diario
{
    public class ENTREGA
    {
        public int NumeroDeRegistro { get; set; }
        public string TipoRegistro { get; set; }
        public VOLUMENENTREGADOTOTALIZADORACUM VolumenEntregadoTotalizadorAcum { get; set; }
        public VOLUMENENTREGADOTOTALIZADORINSTA VolumenEntregadoTotalizadorInsta { get; set; }
        public DateTime FechaYHoraEntrega { get; set; }
        public COMPLEMENTO COMPLEMENTO { get; set; }



    }
}
