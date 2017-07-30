using System;

namespace Busqueda
{
    /// <param name="latitude">Map center Latitude</param>
    /// <param name="longitude">Map center Longitude</param>
    /// <param name="bubbleData">Map Message</param>
    /// <param name="Width">Map Width</param>
    /// <param name="Height">Map Height</param>
    /// <param name="zoomLevel">Map Zoom Level, 11 Is the default value.</param>
   
    public class Properties
    {

        #region ISpatialMap Members
        string _latitude;
        string _longitude;
        string _BubbleData;
        int _MapHeight;
        int _MapWidth;
        int _ZoomLevel;

        /// <summary>
        /// Map center Latitude
        /// </summary>
        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }
        /// <summary>
        /// Map center Longitude
        /// </summary>
        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        /// <summary>
        /// Map Message
        /// </summary>
        public string BubbleData
        {
            get
            {
                return _BubbleData;
            }
            set
            {
                _BubbleData = value;
            }
        }
        /// <summary>
        /// Map Width
        /// </summary>
        public int MapWidth
        {
            get
            {
                return _MapWidth;
            }
            set
            {
                _MapWidth = value;
            }
        }

        /// <summary>
        /// Map Height
        /// </summary>
        public int MapHeight
        {
            get
            {
                return _MapHeight;
            }
            set
            {
                _MapHeight = value;
            }
        }

        /// <summary>
        /// Map Zoom Level, 11 Is the default value.
        /// </summary>
        public int ZoomLevel
        {
            get
            {
                return _ZoomLevel;
            }
            set
            {
                _ZoomLevel = value;
            }
        }

        #endregion
    }
}
