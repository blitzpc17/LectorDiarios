using CapaDatos.Diario;
using DocumentFormat.OpenXml.Spreadsheet;
using Entidades;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LectorDiarios
{
    public partial class LectorDiario : Form
    {

        #region Variables
        private OpenFileDialog ofd;
        private FolderBrowserDialog fbd;
        private CONTROLVOLUMETRICO cv;
        private List<clsControlVolumetrico> LstControlVolumetrico;
        private string pathArchivos = "";
        private string pathDestino = "";
        #endregion

        public LectorDiario()
        {
            InitializeComponent();
            Inicializar();
        }


        #region Metodos Generales
        private void Inicializar()
        {
            ReiniciarForm();
            InicializarVariables();
        }

        private void InicializarVariables()
        {
            cv = null;
            LstControlVolumetrico = new List<clsControlVolumetrico>();
            pathDestino = "";
            pathArchivos = "";
        }

        private void ReiniciarForm()
        {
            dgvRegistros.DataSource = null;
            tsTotalRegistros.Text = @"0";
        }

        private void VisualizarInventario()
        {
            Inventarios frmInventario = new Inventarios();
            frmInventario.ShowDialog();
        }

        private void LeerXmlDescomprimidos(String pathLocalizacion)
        {/*
            DirectoryInfo dinfo = new DirectoryInfo(pathLocalizacion);
            foreach(DirectoryInfo dir in dinfo.GetDirectories())
            {
                foreach(DirectoryInfo subdir in dir.GetDirectories()){
                    if (subdir.Name != @"Diario") continue;
                    FileInfo[] lstFiles =  subdir.GetFiles();
                    LstControlVolumetrico = new List<clsControlVolumetrico>();
                    foreach (FileInfo file in lstFiles)
                    {
                        
                    }
                }
            }*/
            RecorrerArchivosDirectorioDestino(pathLocalizacion);

        }

        private void RecorrerArchivosDirectorioDestino(string pathDestino)
        {
            string [] files = Directory.GetFiles(pathDestino);
            foreach (string file in files)
            {
                LlenarControlVolumetrico(file);
            }
        }

        private void LlenarControlVolumetrico(string path)
        {
            LeerXml(/*file.FullName*/path);
            if (cv.PRODUCTO != null && cv.PRODUCTO.Count > 0)
            {
                foreach (var prod in cv.PRODUCTO)
                {
                    foreach (var dispensario in prod.Dispensarios)
                    {
                        foreach (var manguera in dispensario.Mangueras)
                        {
                            if (manguera == null) continue;
                            foreach (var entrega in manguera.Entregas.ENTREGA)
                            {
                                if (entrega.COMPLEMENTO == null) continue;
                                NACIONAL nac = entrega.COMPLEMENTO.Complemento_Expendio.Nacional;
                                clsControlVolumetrico renglon = new clsControlVolumetrico
                                {
                                    RfcClienteOProveedor = nac.RfcClienteOProveedor,
                                    NombreClienteOProveedor = nac.NombreClienteOProveedor,
                                    CFDI = nac.Cfdis.Cfdi,
                                    FechaYHoraTransaccion = nac.Cfdis.FechaYHoraTransaccion,
                                    ValorNumerico = nac.Cfdis.VolumenDocumentado.ValorNumerico
                                };
                                LstControlVolumetrico.Add(renglon);
                            }
                        }
                    }
                }
                LstControlVolumetrico = LstControlVolumetrico.OrderBy(x => x.NombreClienteOProveedor).ThenBy(x => x.CFDI).ThenBy(x => x.ValorNumerico).ThenBy(x => x.FechaYHoraTransaccion).ToList();
                dgvRegistros.DataSource = LstControlVolumetrico;
                tsTotalRegistros.Text = dgvRegistros.RowCount.ToString("N0");
            }
            else
            {
                MessageBox.Show(
                    "No se detectaron productos en los archivos. Verifique el archivo " + path,
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
            }
        }

        private void LeerXml(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
            nsm.AddNamespace("Covol", "https://repositorio.cloudb.sat.gob.mx/Covol/xml/Diarios");

            XmlNode nodeVersion = doc.SelectSingleNode("//Covol:Version", nsm);
            XmlNode nodeRfcContribuyente = doc.SelectSingleNode("//Covol:RfcContribuyente", nsm);
            XmlNode nodeRfcRepresentanteLegal = doc.SelectSingleNode("//Covol:RfcRepresentanteLegal", nsm);
            XmlNode nodeRfcProveedor = doc.SelectSingleNode("//Covol:RfcProveedor", nsm);
            //XmlNode nodeCaracter = doc.SelectSingleNode("//Covol:Caracter", nsm);
            XmlNode nodeClaveInstalacion = doc.SelectSingleNode("//Covol:ClaveInstalacion", nsm);
            XmlNode nodeDescripcionInstalacion = doc.SelectSingleNode("//Covol:DescripcionInstalacion", nsm);
            XmlNode nodeNumeroPozos = doc.SelectSingleNode("//Covol:NumeroPozos", nsm);
            XmlNode nodeNumeroTanques = doc.SelectSingleNode("//Covol:NumeroTanques", nsm);
            XmlNode nodeNumeroDuctosEntradaSalida = doc.SelectSingleNode("//Covol:NumeroDuctosEntradaSalida", nsm);
            XmlNode nodeNumeroDuctosTransporteDistribucion = doc.SelectSingleNode("//Covol:NumeroDuctosTransporteDistribucion", nsm);
            XmlNode nodeNumeroDispensarios = doc.SelectSingleNode("//Covol:NumeroDispensarios", nsm);
            XmlNode nodeFechaYHoraCorte = doc.SelectSingleNode("//Covol:FechaYHoraCorte", nsm);
            XmlNodeList nodePRODUCTO = doc.SelectNodes("//Covol:PRODUCTO", nsm);

            cv = new CONTROLVOLUMETRICO
            {
                Version = nodeVersion.InnerText,
                RfcContribuyente = nodeRfcContribuyente.InnerText,
                RfcProveedor = nodeRfcProveedor.InnerText,
                RfcRepresentanteLegal = nodeRfcRepresentanteLegal.InnerText,
                ClaveInstalacion = nodeClaveInstalacion.InnerText,
                DescripcionInstalacion = nodeDescripcionInstalacion.InnerText,
                NumeroPozos = int.Parse(nodeNumeroPozos.InnerText),
                NumeroTanques = int.Parse(nodeNumeroTanques.InnerText),
                NumeroDuctosEntradaSalida = int.Parse(nodeNumeroDuctosEntradaSalida.InnerText),
                NumeroDuctosTransporteDistribucion = int.Parse(nodeNumeroDuctosTransporteDistribucion.InnerText),
                NumeroDispensarios = int.Parse(nodeNumeroDispensarios.InnerText),
                FechaYHoraCorte = DateTime.Parse(nodeFechaYHoraCorte.InnerText)

            };

            //creando listado de productos
            cv.PRODUCTO = new List<PRODUCTO>();

            foreach (XmlNode pro in nodePRODUCTO)
            {
                PRODUCTO prod = new PRODUCTO
                {
                    ClaveProducto = pro.ChildNodes[0].InnerText,
                    ClaveSubProducto = pro.ChildNodes[1].InnerText,
                    MarcaComercial = pro.ChildNodes[3].InnerText,
                };
                var nodeGasolina = pro.ChildNodes[2];
                prod.Gasolina = new GASOLINA
                {
                    ComposOctanajeGasolina = nodeGasolina.ChildNodes[0].InnerText,
                    GasolinaConCombustibleNoFosil = nodeGasolina.ChildNodes[1] != null ? nodeGasolina.ChildNodes[1].InnerText : ""

                };
                var nodeTanque = pro.ChildNodes[4];
                prod.Tanque = new TANQUE
                {
                    ClaveIdentificacionTanque = nodeTanque.ChildNodes[0].InnerText,
                    LocalizacionYODescripcionTanque = nodeTanque.ChildNodes[1].InnerText,
                    VigenciaCalibracionTanque = Convert.ToDateTime(nodeTanque.ChildNodes[2].InnerText),
                    EstadoTanque = nodeTanque.ChildNodes[7].InnerText
                };
                var nodeCapacidadTotalTanque = nodeTanque.ChildNodes[3];
                prod.Tanque.CapacidadTotalTanque = new CAPACIDADTOTALTANQUE
                {
                    ValorNumerico = decimal.Parse(nodeCapacidadTotalTanque.ChildNodes[0].InnerText),
                    UM = nodeCapacidadTotalTanque.ChildNodes[1].InnerText
                };

                var nodeCapacidadOperativaTanque = nodeTanque.ChildNodes[4];
                prod.Tanque.CapacidadOperativaTanque = new CAPACIDADOPERATIVATANQUE()
                {
                    ValorNumerico = decimal.Parse(nodeCapacidadOperativaTanque.ChildNodes[0].InnerText),
                    UM = nodeCapacidadTotalTanque.ChildNodes[1].InnerText
                };

                var nodeCapacidadUtilTanque = nodeTanque.ChildNodes[5];
                prod.Tanque.CapacidadUtilTanque = new CAPACIDADUTILTANQUE
                {
                    ValorNumerico = decimal.Parse(nodeCapacidadUtilTanque.ChildNodes[0].InnerText),
                    UM = nodeCapacidadUtilTanque.ChildNodes[1].InnerText
                };

                var nodeVolumenMinimoOperacion = nodeTanque.ChildNodes[6];
                prod.Tanque.VolumenMinimoOperacion = new VOLUMENMINIMOOPERACION
                {
                    ValorNumerico = decimal.Parse(nodeVolumenMinimoOperacion.ChildNodes[0].InnerText),
                    UM = nodeVolumenMinimoOperacion.ChildNodes[1].InnerText
                };

                var nodeMedicionTanque = nodeTanque.ChildNodes[8];
                prod.Tanque.MedicionTanque = new MEDICIONTANQUE
                {
                    SistemaMedicionTanque = nodeMedicionTanque.ChildNodes[0].InnerText,
                    LocalizODescripSistMedicionTanque = nodeMedicionTanque.ChildNodes[1]==null?"": nodeMedicionTanque.ChildNodes[1].InnerText,
                    VigenciaCalibracionSistMedicionTanque = nodeMedicionTanque.ChildNodes[2]!=null?DateTime.Parse(nodeMedicionTanque.ChildNodes[2].InnerText):null,
                    IncertidumbreMedicionSistMedicionTanque = nodeMedicionTanque.ChildNodes[3]!=null?decimal.Parse(nodeMedicionTanque.ChildNodes[3].InnerText):null
                };
                var nodeExistencias = nodeTanque.ChildNodes[9];
                prod.Tanque.Existencias = new EXISTENCIAS
                {
                    HoraRecepcionAcumulado = nodeExistencias.ChildNodes[2]!=null?nodeExistencias.ChildNodes[2].InnerText:"",
                    HoraEntregaAcumulado = nodeExistencias.ChildNodes[4]!=null? nodeExistencias.ChildNodes[4].InnerText:"",
                    FechaYHoraEstaMedicion = nodeExistencias.ChildNodes[6]!=null?DateTime.Parse(nodeExistencias.ChildNodes[6].InnerText):null,
                    FechaYHoraMedicionAnterior = nodeExistencias.ChildNodes[7]!=null?DateTime.Parse(nodeExistencias.ChildNodes[7].InnerText):null
                };

                var nodeVolumenExistenciasAnterior = nodeExistencias.ChildNodes[0];
                prod.Tanque.Existencias.VolumenExistenciasAnterior = new VOLUMENEXISTENCIASANTERIOR
                {
                    ValorNumerico = decimal.Parse(nodeVolumenExistenciasAnterior.ChildNodes[0].InnerText),
                };

                var nodeVolumenAcumOpsRecepcion = nodeExistencias.ChildNodes[1];
                prod.Tanque.Existencias.VolumenAcumOpsRecepcion = new VOLUMENACUMOPSRECEPCION
                {
                    ValorNumerico = decimal.Parse(nodeVolumenAcumOpsRecepcion.ChildNodes[0].InnerText),
                    UM = nodeVolumenAcumOpsRecepcion.ChildNodes[1].InnerText
                };

                var nodeVolumenAcumOpsEntrega = nodeExistencias.ChildNodes[3];
                prod.Tanque.Existencias.VolumenAcumOpsEntrega = new VOLUMENACUMOPSENTREGA
                {
                    ValorNumerico = decimal.Parse(nodeVolumenAcumOpsEntrega.ChildNodes[0].InnerText),
                    UM = nodeVolumenAcumOpsEntrega.ChildNodes[1].InnerText
                };

                var nodeVolumenExistencias = nodeExistencias.ChildNodes[5];
                prod.Tanque.Existencias.VolumenExistencias = new VOLUMENEXISTENCIAS
                {
                    ValorNumerico = decimal.Parse(nodeVolumenExistencias.ChildNodes[0].InnerText),
                };

                var nodeRecepciones = nodeTanque.ChildNodes[10];
                prod.Tanque.Recepciones = new RECEPCIONES
                {
                    TotalRecepciones = int.Parse(nodeRecepciones.ChildNodes[0].InnerText),
                    TotalDocumentos = int.Parse(nodeRecepciones.ChildNodes[2].InnerText)
                };

                var nodeSumaVolumenRecepcion = nodeRecepciones.ChildNodes[1];
                prod.Tanque.Recepciones.SumaVolumenRecepcion = new SUMAVOLUMENRECEPCION
                {
                    ValorNumerico = decimal.Parse(nodeSumaVolumenRecepcion.ChildNodes[0].InnerText),
                    UM = nodeSumaVolumenRecepcion.ChildNodes[1].InnerText
                };

                var nodeEntregas = nodeTanque.ChildNodes[11];
                prod.Tanque.Entregas = new ENTREGAS
                {
                    TotalEntregas = int.Parse(nodeEntregas.ChildNodes[0].InnerText),
                    TotalDocumentos = int.Parse(nodeEntregas.ChildNodes[2].InnerText),
                };

                var nodeSumaVolumenEntregado = nodeEntregas.ChildNodes[1];
                prod.Tanque.Entregas.SUMAVOLUMENENTREGADO = new SUMAVOLUMENENTREGADO
                {
                    ValorNumerico = decimal.Parse(nodeSumaVolumenEntregado.ChildNodes[0].InnerText),
                    UM = nodeSumaVolumenEntregado.ChildNodes[1].InnerText
                };

                //obtener dispensario
                prod.Dispensarios = new List<DISPENSARIO>();
                //agregar dispensario a la lista de dispensarios
                if (pro.ChildNodes[5] != null)
                {
                    prod.Dispensarios.Add(ObtenerDispensario(pro.ChildNodes[5]));
                }
                if (pro.ChildNodes[6] != null)
                {
                    prod.Dispensarios.Add(ObtenerDispensario(pro.ChildNodes[6]));
                }
                //agregando producto a control volumetrico
                cv.PRODUCTO.Add(prod);
            }


        }
        private DISPENSARIO ObtenerDispensario(XmlNode nodeDispensario)
        {
            if (nodeDispensario.ChildNodes[0] != null)
            {
                DISPENSARIO dis = new DISPENSARIO
                {
                    ClaveDispensario = nodeDispensario.ChildNodes[0].InnerText
                };
                var nodeMediacionDispensarioUno = nodeDispensario.ChildNodes[1];
                dis.MedicionDispensario = new MEDICIONDISPENSARIO
                {
                    SistemaMedicionDispensario = nodeMediacionDispensarioUno.ChildNodes[0].InnerText,
                    LocalizODescripSistMedicionDispensario = (nodeMediacionDispensarioUno.ChildNodes[1] != null ? nodeMediacionDispensarioUno.ChildNodes[1].InnerText : ""),
                    VigenciaCalibracionSistMedicionDispensario = (nodeMediacionDispensarioUno.ChildNodes[2] != null ? DateTime.Parse(nodeMediacionDispensarioUno.ChildNodes[2].InnerText) : DateTime.MinValue),
                    IncertidumbreMedicionSistMedicionDispensario = (nodeMediacionDispensarioUno.ChildNodes[3] != null ? decimal.Parse(nodeMediacionDispensarioUno.ChildNodes[3].InnerText) : 0)
                };
                dis.Mangueras = new List<MANGUERA>();



                //ontener manguera
                if (nodeDispensario.ChildNodes[2] != null)
                {
                    dis.Mangueras.Add(ObtenerManguera(nodeDispensario.ChildNodes[2]));
                }
                if (nodeDispensario.ChildNodes[3] != null)
                {
                    dis.Mangueras.Add(ObtenerManguera(nodeDispensario.ChildNodes[3]));
                }
                return dis;
            }

            return null;
            
        }
        public MANGUERA ObtenerManguera(XmlNode nodeEntregasDispensario)
        {
            if (nodeEntregasDispensario.Name != "Covol:MANGUERA") return null;
            MANGUERA man = new MANGUERA();
            if (nodeEntregasDispensario.ChildNodes[1] != null)
            {
                var nodeEntregaDispensarioUno = nodeEntregasDispensario.ChildNodes[1];
                man.Entregas = new ENTREGAS
                {
                    TotalEntregas = int.Parse(nodeEntregaDispensarioUno.ChildNodes[0].InnerText),
                    TotalDocumentos = int.Parse(nodeEntregaDispensarioUno.ChildNodes[2].InnerText)
                };
                var nodeSumaValorNumerocoUno = nodeEntregaDispensarioUno.ChildNodes[1];
                man.Entregas.SUMAVOLUMENENTREGADO = new SUMAVOLUMENENTREGADO
                {
                    ValorNumerico = decimal.Parse(nodeSumaValorNumerocoUno.ChildNodes[0].InnerText),
                    UM = nodeSumaValorNumerocoUno.ChildNodes[1].InnerText
                };

                man.Entregas.ENTREGA = new List<ENTREGA>();
                for (int i = 3; i < nodeEntregaDispensarioUno.ChildNodes.Count; i++)
                {
                    var nodeEntrega = nodeEntregaDispensarioUno.ChildNodes[i];
                    ENTREGA ent = new ENTREGA
                    {
                        NumeroDeRegistro = int.Parse(nodeEntrega.ChildNodes[0].InnerText),
                        TipoRegistro = nodeEntrega.ChildNodes[1].InnerText,
                        FechaYHoraEntrega = DateTime.Parse(nodeEntrega.ChildNodes[4].InnerText)
                    };
                    var nodeVolumenEntregadoTotalizadorAcum = nodeEntrega.ChildNodes[2];
                    var nodeVolumenEntregadoTotalizadorInsta = nodeEntrega.ChildNodes[3];
                    var nodeComplento = nodeEntrega.ChildNodes[5];
                    VOLUMENENTREGADOTOTALIZADORACUM volEntAcum = new VOLUMENENTREGADOTOTALIZADORACUM
                    {
                        ValorNumerico = decimal.Parse(nodeVolumenEntregadoTotalizadorAcum.ChildNodes[0].InnerText),
                        UM = nodeVolumenEntregadoTotalizadorAcum.ChildNodes[1].InnerText
                    };
                    VOLUMENENTREGADOTOTALIZADORINSTA volEntInsta = new VOLUMENENTREGADOTOTALIZADORINSTA
                    {
                        ValorNumerico = decimal.Parse(nodeVolumenEntregadoTotalizadorInsta.ChildNodes[0].InnerText),
                        UM = nodeVolumenEntregadoTotalizadorInsta.ChildNodes[1].InnerText
                    };
                    ent.VolumenEntregadoTotalizadorAcum = volEntAcum;
                    ent.VolumenEntregadoTotalizadorInsta = volEntInsta;

                    COMPLEMENTO complemento = new COMPLEMENTO();
                    complemento.Complemento_Expendio = new COMPLEMENTOEXPENDIO();
                    if(nodeComplento != null)
                    {
                        var nodeComplemento_Expendio = nodeComplento.ChildNodes[0];
                        var nodeNacional = nodeComplemento_Expendio.ChildNodes[0];
                        complemento.Complemento_Expendio.Nacional = new NACIONAL
                        {
                            RfcClienteOProveedor = nodeNacional.ChildNodes[0].InnerText,
                            NombreClienteOProveedor = nodeNacional.ChildNodes[1].InnerText,
                            PermisoProveedor = nodeNacional.ChildNodes[2].InnerText
                        };
                        var nodeCFDIs = nodeNacional.ChildNodes[3];
                        complemento.Complemento_Expendio.Nacional.Cfdis = new CFDI
                        {
                            Cfdi = nodeCFDIs.ChildNodes[0].InnerText,
                            TipoCFDI = nodeCFDIs.ChildNodes[1].InnerText,
                            PrecioCompra = decimal.Parse(nodeCFDIs.ChildNodes[2].InnerText),
                            PrecioDeVentaAlPublico = decimal.Parse(nodeCFDIs.ChildNodes[3].InnerText),
                            PrecioVenta = decimal.Parse(nodeCFDIs.ChildNodes[4].InnerText),
                            FechaYHoraTransaccion = DateTime.Parse(nodeCFDIs.ChildNodes[5].InnerText)
                        };
                        var nodeVolumenDocumentado = nodeCFDIs.ChildNodes[6];
                        complemento.Complemento_Expendio.Nacional.Cfdis.VolumenDocumentado = new VOLUMENDOCUMENTADO
                        {
                            ValorNumerico = decimal.Parse(nodeVolumenDocumentado.ChildNodes[0].InnerText),
                            UM = nodeVolumenDocumentado.ChildNodes[1].InnerText
                        };

                        ent.COMPLEMENTO = complemento;
                    }                   

                    man.Entregas.ENTREGA.Add(ent);

                }

                return man;
            }
            return null;            
        }

        private bool CargarZipRar()
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.Filter = "ZIP files|*.zip;*.rar";
                ofd.ShowDialog(this);                
                if (string.IsNullOrEmpty(ofd.FileName))
                {
                    MessageBox.Show(
                        "No se selecciono ningún archivo de tipo ZIP o RAR. Intente volver a seleccionar su archivo.",
                        "Advertencia",                        
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return false;
                }
                pathArchivos = ofd.FileName;
                fbd = new FolderBrowserDialog();
                fbd.ShowDialog(this);
                pathDestino = fbd.SelectedPath;
                if (string.IsNullOrEmpty(pathDestino))
                {
                    MessageBox.Show(
                        "No se selecciono la ruta destino de descomprensión. Vuelva a cargar su archivo y seleccione la ruta de descomprensión.",
                        "Advertencia",                        
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return false;
                }
                ZipFile.ExtractToDirectory(pathArchivos, pathDestino);              

                MessageBox.Show(
                    "Descompresión completa",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Se ha generado el siguiente error durante el proceso de descomprensión de sus archivos. Reporte: " + ex.Message,
                    "Error inesperado",                   
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                return false;
            }            

        }

        private void Exportar()
        {
            if(LstControlVolumetrico==null || LstControlVolumetrico.Count <= 0)
            {
                MessageBox.Show("No hay registros para exportar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ;
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "ReporteDiario";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string rutaSalida = dlg.FileName + ".xlsx";
                SLDocument sl = new SLDocument();
                sl.SetCellValue("A1", "CONTENIDO DE ARCHIVO CONTROL VOLUMETRICO");
                SLStyle styleEncabezados = sl.CreateStyle();
                SLStyle styleCantidades = sl.CreateStyle();
                SLStyle styleColorCV = sl.CreateStyle();

                //colores
                SLThemeSettings themeFacturacion = new SLThemeSettings();
                themeFacturacion.ThemeName = "colorFacturacion";
                themeFacturacion.Accent1Color = System.Drawing.Color.Aquamarine;
                themeFacturacion.Accent2Color = System.Drawing.Color.Yellow;
                //stylo encabezados
                styleEncabezados.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                styleEncabezados.Font.Bold = true;
                //stylo cantidades
                styleCantidades.Alignment.Horizontal = HorizontalAlignmentValues.Right;
                //stylo color cv
                styleColorCV.Fill.SetPattern(PatternValues.Solid, SLThemeColorIndexValues.Accent1Color, SLThemeColorIndexValues.Accent2Color);

                sl.MergeWorksheetCells("A1", "E1");

                sl.SetCellValue("A2", "RFC");
                sl.SetCellValue("B2", "Nombre Cliente");
                sl.SetCellValue("C2", "CFDI Cliente");
                sl.SetCellValue("D2", "Fecha Y Hora de Generación");
                sl.SetCellValue("E2", "Litros de Venta");

                int noRows = 2;
                noRows += LstControlVolumetrico.Count();
                GenerarRowsExcel(LstControlVolumetrico, sl);

                sl.SetColumnStyle(5, styleCantidades);
                sl.SetCellStyle(3, 1, noRows, 5, styleColorCV);
                sl.SetRowStyle(2, styleEncabezados);
                sl.SaveAs(rutaSalida);
                MessageBox.Show("Se han exportado los registros.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void GenerarRowsExcel(List<clsControlVolumetrico> LstResultadosExcel, SLDocument objExcel)
        {
            int index = 3;
            foreach (var row in LstResultadosExcel)
            {
                objExcel.SetCellValue("A" + index, row.RfcClienteOProveedor);
                objExcel.SetCellValue("B" + index, row.NombreClienteOProveedor);
                objExcel.SetCellValue("C" + index, row.CFDI);
                objExcel.SetCellValue("D" + index, row.FechaYHoraTransaccion.ToString());
                objExcel.SetCellValue("E" + index, row.ValorNumerico);

                index++;
            }

        }



        #endregion






        #region Metodos de controles
        private void btnInventarios_Click(object sender, EventArgs e)
        {
            if (LstControlVolumetrico == null|| LstControlVolumetrico.Count<=0)
            {
                MessageBox.Show(
                    "Advertencia", 
                    "No se ha cargado la información.", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }
            VisualizarInventario();
        }
        private void btnZip_Click(object sender, EventArgs e)
        {
            bool completado = false;

            mdCargaArchivo md =  new mdCargaArchivo();
            md.ShowDialog();
            Inicializar();
            if (md.esDirectorio==1)
            {
                folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.ShowDialog();
                pathDestino = folderBrowserDialog1.SelectedPath;

                if (string.IsNullOrEmpty(pathDestino))
                {
                    MessageBox.Show("No se selecciono la ruta de origen de los archivos diarios.",
                                "Advertencia",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                    completado = false;
                    return;
                }
                completado = true;
            }
            else if(md.esDirectorio == 2)
            {               
                completado = CargarZipRar();
            }

            if (completado)
            {
                LeerXmlDescomprimidos(pathDestino);
            }
        }

        #endregion

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Exportar();
        }
    }
}
