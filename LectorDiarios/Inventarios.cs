using CapaDatos.Diario;
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
    public partial class Inventarios : Form
    {
        public CONTROLVOLUMETRICO obj;
        public Inventarios()
        {
            InitializeComponent();
            
        }

        public void InicializarInventario()
        {
            if (obj == null)
            {
                MessageBox.Show("No hay registro cargado para generar el inventario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtVersion.Text = obj.Version;
            txtRfcRepre.Text = obj.RfcRepresentanteLegal;
            txtRfcProveed.Text = obj.RfcProveedor;
            txtRfcContrib.Text = obj.RfcContribuyente;
            txtNoPermiso.Text = obj.Caracter.NumPermiso;
            txtSucursal.Text = Enumeraciones.CatalogSucursales().Where(x => x.Value == obj.Caracter.NumPermiso).First().Key;
            txtCaracter.Text = obj.Caracter.TipoCaracter;
            txtModPermiso.Text = obj.Caracter.ModalidadPermiso;
            txtPeriodo.Text = obj.FechaYHoraCorte.ToString();

            limpiarPanelInventarios();

            //calcular sumatoria de litros

            obj.Encabezado.DataProducto = obj.Encabezado.DataProducto.GroupBy(x => x.ClaveProducto).Select(x => new Entidades.Diario.ENCABEZADOPRODUCTO
            {
                ClaveProducto = x.First().ClaveProducto,    
                ClaveSubProducto = x.First().ClaveSubProducto,
                MarcaComercial = x.First().MarcaComercial,
                InventarioFinalMes = x.Sum(z=>z.InventarioFinalMes),
                VecesRecepcionProducto = x.Sum(z=>z.VecesRecepcionProducto),
                LitrosAcumuladosMes = x.Sum(z=>z.LitrosAcumuladosMes)
            }).ToList();

            int posicionDgvInventariosY = 0;
            foreach (var pro in obj.PRODUCTO.OrderByDescending(x => x.ClaveProducto).ToList())
            {

                DataGridView dgvEncabezadoInventario = new DataGridView();
                dgvEncabezadoInventario.AllowUserToAddRows = false;
                dgvEncabezadoInventario.AllowUserToDeleteRows = false;
                dgvEncabezadoInventario.ReadOnly = true;

                dgvEncabezadoInventario.Columns.Add("Texto", "");
                dgvEncabezadoInventario.Columns.Add("Valor", "");
                dgvEncabezadoInventario.Name = "dgvEncabezado" + pro.ClaveProducto;
                dgvEncabezadoInventario.Rows.Add("Producto:", pro.ClaveSubProducto + " " + pro.MarcaComercial);
                dgvEncabezadoInventario.Rows.Add("INVENTARIO EN TANQUE AL FINALIZAR EL MES:", pro.Tanque.Existencias.VolumenAcumOpsEntrega.ValorNumerico);//pro.REPORTEDEVOLUMENMENSUAL.CONTROLDEEXISTENCIAS.VolumenExistenciasMes.ValorNumerico);
                dgvEncabezadoInventario.Rows.Add("NÚMERO DE VECES QUE ENTRO PRODUCTO AL TANQUE:", pro.Tanque.Recepciones.SumaVolumenRecepcion.ValorNumerico); //pro.REPORTEDEVOLUMENMENSUAL.RECEPCIONES.TotalRecepcionesMes);
                dgvEncabezadoInventario.Rows.Add("TOTAL DE LITROS QUE MUESTRA LA FACTURA:", obj.Encabezado.DataProducto.Where(x => x.ClaveProducto == pro.ClaveProducto).First().LitrosAcumuladosMes);   //pro.REPORTEDEVOLUMENMENSUAL.RECEPCIONES.SumaVolumenRecepcionMes.ValorNumerico);
                panelInventarios.Controls.Add(dgvEncabezadoInventario);
                dgvEncabezadoInventario.Location = new System.Drawing.Point(10, posicionDgvInventariosY + 30);
                dgvEncabezadoInventario.Width = 1107;
                posicionDgvInventariosY = dgvEncabezadoInventario.Location.Y + dgvEncabezadoInventario.Size.Height + 15;
                dgvEncabezadoInventario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)))));
                dgvEncabezadoInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataGridView dgvPartidas = new DataGridView();
                dgvPartidas.Columns.Add("numero", "No.");
                dgvPartidas.Columns.Add("nombreCliente", "Nombre Cliente Proveedor");
                dgvPartidas.Columns.Add("rfcCliente", "Rfc Cliente Proveedor");
                dgvPartidas.Columns.Add("cfdi", "CFDI");
                dgvPartidas.Columns.Add("fechaHora", "Fecha y Hora");
                dgvPartidas.Columns.Add("precioCompra", "Precio Compra");
                dgvPartidas.Columns.Add("precioVenta", "Precio Venta Púb.");
                dgvPartidas.Columns.Add("valorNumerico", "ValorNumerico");

                panelInventarios.Controls.Add(dgvPartidas);
                dgvPartidas.Location = new System.Drawing.Point(10, posicionDgvInventariosY + 30);
                dgvPartidas.Width = 1107;
                dgvPartidas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)))));
                dgvPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                //foreach partidas
                int numeral = 1;
                decimal sumaRecepciones = 0;
                /*
                if (pro.REPORTEDEVOLUMENMENSUAL.RECEPCIONES.Complemento == null)
                {
                    sumaRecepciones = 0;
                    MessageBox.Show("El producto " + pro.ClaveSubProducto + " " + pro.MarcaComercial + " no tiene COMPLEMENTOS EN RECEPCIONES.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    foreach (var part in pro.REPORTEDEVOLUMENMENSUAL.RECEPCIONES.Complemento.Complemento_Expendio.NACIONAL)
                    {
                        dgvPartidas.Rows.Add(numeral, part.NombreClienteOProveedor, part.RfcClienteOProveedor, part.CFDIs.CFDI, part.CFDIs.FechaYHoraTransaccion, part.CFDIs.PrecioCompra, part.CFDIs.PrecioDeVentaAlPublico, part.CFDIs.VolumenDocumentado.ValorNumerico);
                        sumaRecepciones += part.CFDIs.VolumenDocumentado.ValorNumerico;
                        numeral++;
                    }
                }


                decimal diferenciaEntregadoRecepcion = pro.REPORTEDEVOLUMENMENSUAL.RECEPCIONES.SumaVolumenRecepcionMes.ValorNumerico - sumaRecepciones;
                dgvPartidas.Rows.Add(null, null, null, null, null, null, "TOTAL:", sumaRecepciones);
                dgvPartidas.Rows.Add(null, null, null, "VENTA LTS. POR MES:", pro.REPORTEDEVOLUMENMENSUAL.ENTREGAS.SumaVolumenEntregado.ValorNumerico, null, null, null);
                dgvPartidas.Rows.Add(null, null, null, "DIF- FACT. VS PIPAS:", diferenciaEntregadoRecepcion, null, null, null);
                dgvPartidas.Rows.Add(null, null, null, "LA FACTURA TRAE", diferenciaEntregadoRecepcion >= 0 ? " MÁS" : " MENOS");

                */
                posicionDgvInventariosY = dgvPartidas.Location.Y + dgvPartidas.Size.Height + 15;

            }


        }

        private void limpiarPanelInventarios()
        {
            foreach (var gctrl in this.Controls)
            {

                if (gctrl is Panel)
                {
                    if (((Panel)gctrl).Name != "panelInventariosMain")
                    {
                        ((Panel)gctrl).Controls.Clear();
                    }

                }

                //if (cont is TabControl)
                //{
                //    foreach (var tabPage in ((TabControl)cont).TabPages)
                //    {
                //        foreach (var gctrl in ((TabPage)tabPage).Controls)
                //        {

                //            if (gctrl is Panel)
                //            {
                //                if (((Panel)gctrl).Name != "panelInventariosMain")
                //                {
                //                    ((Panel)gctrl).Controls.Clear();
                //                }

                //            }
                //        }
                //    }

                //}
            }
        }

        private void Inventarios_Load(object sender, EventArgs e)
        {
            InicializarInventario();
        }

       
    }
}
