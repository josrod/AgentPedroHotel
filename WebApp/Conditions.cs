using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp
{
    public class Conditions
    {
        string city = "Sin Dato";
        string dayOfWeek =DateTime.Now.DayOfWeek.ToString();
        string condition = "Sin Dato";
        string tempC = "Sin Dato";
        string humidity = "Sin Dato";
        string wind = "Sin Dato";
        string tempHigh = "Sin Dato";
        string tempLow = "Sin Dato";
        string iconDay = "Sin Dato";

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public string TempC
        {
            get { return tempC; }
            set { tempC = value; }
        }

        public string Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }

        public string Wind
        {
            get { return wind; }
            set { wind = value; }
        }

        public string DayOfWeek
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; }
        }

        public string TempHigh
        {
            get { return tempHigh; }
            set { tempHigh = value; }
        }

        public string TempLow
        {
            get { return tempLow; }
            set { tempLow = value; }
        }

        public string IconDay
        {
            get { return iconDay; }
            set { iconDay = value; }
        }
    }
}
