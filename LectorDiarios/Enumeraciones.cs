using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectorDiarios
{
    public class Enumeraciones
    {
        public static List<KeyValuePair<String, String>> CatalogSucursales()
        {
            List<KeyValuePair<String, String>> LstSucursales = new List<KeyValuePair<string, string>>();

            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Suc. Doncellas)", "PL/2916/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS DE CORDOBA, S.A. DE C.V.", "PL/3022/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS DE CORDOBA, S.A. DE C.V.", "PL/3744/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Suc. Tehuacán)", "PL/3267/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Matríz)", "PL/3449/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("GASOLINERIA ELE, S.A. DE C.V.", "PL/3664/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("COMBUSTIBLES Y SERVICIOS ESMERALDA, S.A. DE C.V.", "PL/3219/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Suc. Chapulco)", "PL/3456/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS SOLE, S.A. DE C.V.", "PL/3461/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS SANTA MARIA DEL MONTE, S.A. DE C.V.", "PL/3656/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS AHUACATLAN, S.A. DE C.V.", "PL/2614/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("GASOLINERA ZAVALETA, S.A. DE C.V.", "PL/19419/EXP/ES/2016"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO ALFA BRAVO COCA, SA DE CV", "PL/21370/EXP/ES/2018"));
            LstSucursales.Add(new KeyValuePair<String, String>("LITRO EXACTO OCOTLAN, S. DE R.L. DE C.V.", "PL/23036/EXP/ES/2019"));
            return LstSucursales;
        }
    }
}
