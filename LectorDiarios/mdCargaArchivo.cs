using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LectorDiarios
{
    public partial class mdCargaArchivo : Form
    {
        public int esDirectorio = 0;
        public mdCargaArchivo()
        {
            InitializeComponent();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            SeleccionarAccion();
            this.Close();
        }

        private void SeleccionarAccion()
        {
            if (rbCarpeta.Checked)
            {
                esDirectorio = 1;
            }
            else if(rbZipRar.Checked)
            {
                esDirectorio=2;
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mdCargaArchivo_FormClosed(object sender, FormClosedEventArgs e)
        {
            CerrarOperacion();
        }

        private void CerrarOperacion()
        {
            if (esDirectorio==0)
            {
                MessageBox.Show("Se ha cancelado la operación", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
