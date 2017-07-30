using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FuncionesSQLServer;
using System.Windows.Forms;

namespace Busqueda
{
    /// <summary>
    /// Generates a map by using Google Maps API
    /// </summary>
    /// 
    public class Generator
    {
        public static IniFile ini = new IniFile(Application.ExecutablePath + "../../INI/config.ini");

        public static string cadena_ROBOTEL = ini.IniReadValue("CADENA_ROBOTEL", "VALOR");

        public DataSet myds_usuarios = new DataSet();
        public DataSet myds_datos = new DataSet();
        public DataSet myds_virtual = new DataSet();
        public DataRow mydataRow;

        public Generator()
        {

        }

       public Properties GetMapMembers(string strTopic, string strValor)
        {
            Properties GMembers = new Properties();

            string sNombre = strValor.ToUpper();
            string sTableName = strTopic.ToUpper();
            string myquery = "SELECT * from " + sTableName + " WHERE Nombre ='" + sNombre + "'";     // aqui debo seleccionar la tabla según strTopic
                                                    // y de esa tabla sacar la información de strValor
            FuncionesSQL.Consulta_SELECT(myquery, myds_datos, cadena_ROBOTEL);

            // Primero comprobamos que hemos obtenido al menos un resultado
            // 
            if (myds_datos.Tables[0].Rows.Count != 0)

            {

                mydataRow = myds_datos.Tables[0].Rows[0];

                string[] cadena = new string[7];
                string datos;
                string img="";
                string com = "\\\"";

                GMembers.Latitude = mydataRow[1].ToString();  //"41.652112";
                GMembers.Longitude = mydataRow[2].ToString();  //"-4.728574";
                GMembers.MapWidth = 550;
                GMembers.MapHeight = 555;
                GMembers.ZoomLevel = 16;

                // Obtengo el nombre del establecimienyo y lo guardo en datos en formato html para luego 
                // Crear la infoWindow de GMaps
                datos = "<div><h3>" + sNombre + "</h3></div>";

                // Obtengo la imagen del establacimiento, si tiene.
                if (mydataRow[5].ToString() == "")
                {
                    // imagen por defecto
                    img = com + getImg(sTableName) + com;

                }
                else
                {
                    // Imagen en l base de datos
                    img = com + mydataRow[5].ToString() + com;
                }
                datos += "<img src=" + img + "height=" + com + "80" + com + "align=" + com + "left" + com + "/>";


                // Cadena obtiene Dir,CP,Ciudad,Tel1,Tel2,Correo,Pagina
                for (int i = 0; i < 7; i++)
                    cadena[i] = mydataRow[i + 7].ToString();

                datos += "<div style=" + com + "font-size:14px" + com + "><a>" + cadena[0] + "<br/>" + cadena[1] + " " + cadena[2] + "<br/>" + cadena[3] + cadena[4] + "<a></div>";

                GMembers.BubbleData = datos;


                
            }
            else 
            { 
                //No hemos obtenido ningun resultado
                //que hacemos??

            }
            return GMembers;
        }

       private string getImg(string strTopic)
        {
           string sTableName = strTopic.ToUpper();
           string sImg="";

            switch (sTableName)
            {
                case "RESTAURANTES":
                    {
                        sImg = "../../Resources/icono_restaurante.jpg";
                        break;
                    }
                case "MUSEOS":
                    {
                        sImg = "../../Resources/icono_museo.png";
                        break;
                    }
                 case "BIC":
                    {
                        sImg = "../../Resources/icono_bic.jpg";
                        break;
                    }
            }
           return sImg;
        }


        

    }
}
