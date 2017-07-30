using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace WebApp
{
    public class Weather
    {
        
        private XmlDocument xmlConditions = new XmlDocument ();
        /// <summary> 
        /// The function that returns the current conditions for the specified location. 
        /// </summary> 
        /// <param name="location">City or ZIP code</param> 
        /// <returns></returns> 
        public Conditions GetCurrentConditions(string location)
        {
            Conditions conditions = new Conditions();

            string url = string.Format("http://www.google.com/ig/api?weather={0}", location + "&hl=es");

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            // Abrir el stream de la respuesta recibida.
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

            // Leer el contenido.
            string res = reader.ReadToEnd();
            // Lo cargo en un xml
            xmlConditions.LoadXml(res);

            //This 'if' checks whether the XML response contains a problem_cause node. The presence of this node in the response means that the API call failed for some reason 
            if (xmlConditions.SelectSingleNode("xml_api_reply/weather/problem_cause") != null)
            {
                conditions = null;
            }
            else
            {
                conditions.City = xmlConditions.SelectSingleNode("/xml_api_reply/weather/forecast_information/city").Attributes["data"].InnerText;
                conditions.Condition = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/condition").Attributes["data"].InnerText;
                conditions.TempC = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/temp_c").Attributes["data"].InnerText;
                conditions.Humidity = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/humidity").Attributes["data"].InnerText;
                conditions.Wind = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/wind_condition").Attributes["data"].InnerText;
            }
            return conditions;
        }

        /// <summary> 
        /// The function that gets the forecast for the next four days. 
        /// </summary> 
        /// <returns></returns> 
        public List<Conditions> GetForecast()
        {
            List<Conditions> ListConditions = new List<Conditions>();

            foreach (XmlNode node in xmlConditions.SelectNodes("/xml_api_reply/weather/forecast_conditions"))
            {
                Conditions condition = new Conditions();
                condition.City = xmlConditions.SelectSingleNode("/xml_api_reply/weather/forecast_information/city").Attributes["data"].InnerText;
                condition.Condition = node.SelectSingleNode("condition").Attributes["data"].InnerText;
                condition.TempHigh = node.SelectSingleNode("high").Attributes["data"].InnerText;
                condition.TempLow = node.SelectSingleNode("low").Attributes["data"].InnerText;
                condition.DayOfWeek = node.SelectSingleNode("day_of_week").Attributes["data"].InnerText;
                condition.IconDay = node.SelectSingleNode("icon").Attributes["data"].InnerText;
                ListConditions.Add(condition);
            }

            return ListConditions;
           
        }
    }
}
