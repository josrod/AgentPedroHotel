using System;
using System.Collections.Generic;
using System.Text;
using Animaonline.Geo.Maps;

namespace Busqueda
{
    class SearchTopic
    {
        private GMapsXML myXMLGMaps;

        private GoogleMapGenerator.GoogleMap myMap;
        private Generator MapGenerator;
        private Properties myProp;
        private string path;

        public string myURL;

        public SearchTopic()
        {
            myXMLGMaps = new GMapsXML();
            path = Environment.CurrentDirectory;

            MapGenerator = new Generator();
            myProp = new Properties();
        }

        public StringBuilder Topic(string sTopic, string sAtributo, string sValor)
        {
            StringBuilder result = new StringBuilder();
            switch (sAtributo)
            {
                case "tipo":
                    {
                        result = SearchTipo(sTopic, sValor);
                        myURL = myXMLGMaps.GMapsURL;
                        break;
                    }
                case "nombre":
                    {
                        result = SearchNombre(sTopic, sValor);
                        myURL = path + "\\map.html";
                        break;
                    }
            }
            return result;

        }

        /// <summary>
        /// Leo los resultado de la busqueda de restaurantes en GMaps.
        /// Para la busqueda general general p.e restaurante + tipo,
        /// Leo los 4 primeros resultados.
        /// </summary
        private StringBuilder SearchTipo(string strTopic, string strValor)
        {
            string sNombre = strValor;
            
            StringBuilder result = new StringBuilder();

            if (sNombre != "")
            {
                string[,] strMapResult = myXMLGMaps.buscar("Valladolid " + strTopic + " "+ sNombre);
                result.AppendLine("Según los criterios de busqueda, he encontrado:");

                for (int j = 0; j < 4; j++)
                {
                    result.AppendLine(strMapResult[j, 0] + ".");
                }
                
            }
            result.AppendLine("¿quiere la información de algún restaurante en específico?.");

            return result;
        }

        /// <summary>
        /// Leo los resultado de la busqueda de restaurantes en Generador.
        /// Porque la Busqueda es especifica p.e restaurante + nombre,
        /// Como la busqueda ha sido p.e Restaurante + nombre, el resultado con ese nombre
        /// más el numero de telefono y dirección y genero el mapa de GMaps correspondiente
        /// Con los datos de Long y Lat etc.
        /// </summary>
        private StringBuilder SearchNombre(string strTopic, string strValor)
        {
            myProp = MapGenerator.GetMapMembers(strTopic, strValor);
            
            StringBuilder result = new StringBuilder();

            if (myProp.BubbleData != null)
            {
                myMap = new GoogleMapGenerator.GoogleMap(myProp.Latitude, myProp.Longitude, myProp.BubbleData,
                    myProp.MapWidth, myProp.MapHeight, myProp.ZoomLevel);
                myMap.SaveMap(path + "/map.html");

                result.AppendLine("Según los criterios de busqueda, he encontrado:");
                result.AppendLine(MapGenerator.mydataRow[4].ToString() + ".");
                result.AppendLine("se encuentra en " + MapGenerator.mydataRow[7].ToString());
                result.AppendLine("Telefono " + MapGenerator.mydataRow[10].ToString()+".");
                result.AppendLine("¿Desea saber algo más de restaurantes?");

            }

            return result;
        }
    }
}
