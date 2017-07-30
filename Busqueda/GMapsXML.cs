using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;
using System.Data;

namespace Busqueda
{
    public class GMapsXML
    {
        XmlDocument xmlDoc;
        private String [,]arreglos = new String[10,4];
        private string strNombre;
        private string strDir;
        private string strTel;
        private string strLatLong;
        public string GMapsURL;

        public GMapsXML()
        {
            xmlDoc = new XmlDocument();
        }
        public string[,] buscar(string UserInput)
        {
            string strUsrInput = UserInput;
            xmlDoc.Load("http://maps.google.com/maps?q=" + strUsrInput.Replace(" ", "%20") + "&output=kml&view=text");

            GMapsURL = "";
            GMapsURL = "http://maps.google.com/maps?q=" + UserInput;

            XmlNodeList xnlTemp = xmlDoc.LastChild.ChildNodes[0].ChildNodes;

            int i=0;
            int j=0;
            int nTam;

            foreach (XmlNode xnTemp in xnlTemp)
            {
                if (xnTemp.Name == "Placemark")
                {
                    strNombre = xnTemp.FirstChild.InnerText;
                    strNombre = strNombre.Replace("®", "");
                    arreglos[i,j] = strNombre;

                    //leo el xml para sacar la Dirección
                    strDir = xnTemp.ChildNodes.Item(2).InnerText;
                    int nidx=strDir.IndexOf("<", 0);
                    strDir = strDir.Substring(0, nidx);
                    arreglos[i, j+1] = strDir;

                    //leo el xml para sacar el telefono
                    strTel = xnTemp.ChildNodes.Item(1).InnerText;
                    nTam = strTel.Length;
                    int nidx2 = strTel.IndexOf(">")+1;
                    int nTel = nTam - nidx2;
                    strTel = strTel.Substring(nidx2, nTel);
                    arreglos[i, j+2] = strTel;

                    //leo el xml para sacar el LatLong
                    strLatLong = xnTemp.ChildNodes.Item(4).InnerText;
                    arreglos[i, j + 3] = strLatLong;
                                  
                    i++;
                }
                
            }

            for (int k = 0; i < 10; i++)
            {
                if (arreglos[k, 0] == null)
                {
                    arreglos[k, 0] = "Sin resultado";
                }
            }

            return arreglos;
        }
    }
}
