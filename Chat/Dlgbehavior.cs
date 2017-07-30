using System;
using System.Collections.Generic;
using System.Text;

namespace Chat
{

    public class Dlgbehavior
    {
        #region Dialog Behavior Members
        string _ChatResult;
        string _Action;
        string _Topic;
        string _Atributo;
        string _Valor;
        string _GUrl;

        /// <summary>
        /// Chat result
        /// </summary>
        public string ChatResult
        {
            get
            {
                return _ChatResult;
            }
            set
            {
                _ChatResult = value;
            }
        }

        /// <summary>
        /// Dialog goal like Verbs
        /// </summary>
        public string Action
        {
            get
            {
                return _Action;
            }
            set
            {
                _Action = value;
            }
        }

        /// <summary>
        /// Dialog topic 
        /// </summary>
        public string Topic
        {
            get
            {
                return _Topic;
            }
            set
            {
                _Topic = value;
            }
        }

        /// <summary>
        /// Dialog attribute. Type of information
        /// </summary>
        public string Atributo
        {
            get
            {
                return _Atributo;
            }
            set
            {
                _Atributo = value;
            }
        }

        /// <summary>
        /// Attribute value
        /// </summary>
        public string Valor
        {
            get
            {
                return _Valor;
            }
            set
            {
                _Valor = value;
            }
        }

        /// <summary>
        /// Si es busqueda guardo la Url de GMap generado
        /// </summary>
        public string GUrl
        {
            get
            {
                return _GUrl;
            }
            set
            {
                _GUrl = value;
            }
        }
        public void LipiarVariables()
        {
            _ChatResult="";
            _Action="";
            _Topic="";
            _Atributo="";
            _Valor="";
            _GUrl="";
        }
        #endregion
    }
}
