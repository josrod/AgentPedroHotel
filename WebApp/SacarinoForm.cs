using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Chat;
using Encuesta;

namespace WebApp
{
    public partial class SacarinoForm : Form
    {
        string baseURL = "http://www.google.com";
        Weather myWeather;
        Conditions conditions;
        HelpImg myHelpImg;
        List<Conditions> ListConditions;
        
        // preparo las frases del test
        Preguntas ListPreguntas; // Inicializo la lista
        int NumFrases; // Guardo el número de preguntas
        int nContFrases; // contador al numero de preguntas
        int NumSurvey;
        int nContSurvey; // Contador al número de preguntas survey
        Regex objNaturalPattern; // verificar si la respuesta del test esta entre [1-5]
        
        public Dlgbehavior myDlgBehavior;
        public string GmapsURL = "";

        delegate void SetTexCallback(string text, string text2); // Para poder ejecutor un subproceso

        public SacarinoForm()
        {
            InitializeComponent();

            // Inicio las clases que afectan el estado del SacarinoForm
            //Para visualizar el tiempo
            myWeather = new Weather();
            conditions = new Conditions();
            ListConditions = new List<Conditions>();

            // Para cambiar las imagenes de ayuda que queremos mostrar
            myHelpImg = new HelpImg();
            myDlgBehavior = new Dlgbehavior();

            //Para ejecutar la encuesta
            ListPreguntas = new Preguntas();
            NumFrases = ListPreguntas.frases.Count;
            nContFrases = 0;
            NumSurvey = ListPreguntas.survey.Count;
            nContSurvey = 0;
            objNaturalPattern = new Regex("[1-9]");

        }

        
        public void WebApp(string ChatAcction, string ChatTopic)
        {
            if (this.tabControlPestañas.InvokeRequired)
            {
                SetTexCallback d = new SetTexCallback(WebApp);
                this.Invoke(d, new object[] { ChatAcction, ChatTopic });
            }
            else
            {
                ProcesarDlg(ChatAcction, ChatTopic);
            }
        }

        /// <summary>
        /// Función para cambiar la visualización según la solicitud del usuario
        /// A partir de las propiedades de myDlgBehavior
        /// </summary>
        void ProcesarDlg(string ChatAcction, string ChatTopic)
        {
            string sAcction = ChatAcction;
            string strTipoTopic = ChatTopic;
            string sAtributo = myDlgBehavior.Atributo;
            string sValor = myDlgBehavior.Valor;
            //string sOutPut = myDlgBehavior.ChatResult;


            GmapsURL = myDlgBehavior.GUrl;

            if (sAcction == "mostrar")
            {
                this.toolStripLabelBarra.Text = "HOTEL";
                this.tabControlPestañas.SelectedTab = this.tabPageHotel;
                string img = myHelpImg.SelectImg(sAtributo, sValor);
                pictureBoxHotel.Image = Image.FromFile(img);
                img = myHelpImg.SelectMsg();
                pictureBoxMsg.Image = Image.FromFile(img);
                //pictureBoxHotel.Image = Image.FromFile(Constantes.ImgPlanoHotel);
                //pictureBoxMsg.Image = Image.FromFile(Constantes.ImpMsgPrincipal);
            }
            else if (sAcction == "buscar")
            {
                this.toolStripLabelBarra.Text = "RESTAURANTES";
                this.tabControlPestañas.SelectedTab = this.tabPageWebApp;
                if (sAtributo == "tipo")
                {
                    webBrowserMap.Size= new Size(1080, 705);
                    webBrowserMap.Location = new Point(-378, -120);
                    
                }
                else
                {
                    webBrowserMap.Size=new Size(550,555);
                    webBrowserMap.Location = new Point(0, 0);
                }
                webBrowserMap.Navigate(GmapsURL);
            }
            else if (sAcction == "tiempo")
            {
                this.toolStripLabelBarra.Text = "EL TIEMPO";
                this.tabControlPestañas.SelectedTab = this.tabPageWeather;
                getWeather("Valladolid");
            }
            else if (sAcction == "ayuda")
            {
                this.tabControlPestañas.SelectedTab = this.tabPageHelp;
            }
            else if (sAcction == "encuesta")
            {
                this.toolStripLabelBarra.Text = "ENCUESTA";
                this.tabControlPestañas.SelectedTab = this.tabPageTest;
                this.labelPregunta.Text= "ENCUESTA";

                if (nContFrases < NumFrases && objNaturalPattern.IsMatch(sValor))
                {
                    this.labelPregunta.Text = ListPreguntas.frases[nContFrases];
                    this.labelIndicador.Text = "Pregunta " + (nContFrases + 1) + " de " + NumFrases;
                    guardarBD(sAtributo, sValor);
                    nContFrases++;
                }
                else
                {
                    if (nContSurvey < NumSurvey && objNaturalPattern.IsMatch(sValor))
                    {
                        this.labelEncInfo.Text = "A continuación debes llenar los siguiente datos demográficos, sobre tu Sexo, Edad, y experiencia en sistemas como este y al final si lo deseas puedes hacer algunos comentarios.";
                        labelPregunta.Visible = false;
                        groupBoxDemoData.Visible = true;
                        this.labelPregunta.Text = ListPreguntas.survey[nContSurvey];
                        this.labelIndicador.Text = "Pregunta " + (nContSurvey + 1) + " de " + NumSurvey;
                        guardarBD(sAtributo, sValor);
                        nContSurvey++;
                    }
                    else
                    {
                        if (nContSurvey >= NumSurvey)
                        {
                            guardarBD(sAtributo, sValor);
                            EndEncuesta();
                        }
                    }
                }
            }
            else
            {
                this.toolStripLabelBarra.Text = "PRINCIPAL";

                this.tabControlPestañas.SelectedTab = this.tabPagePrincipal;
            }
 
        }

        /// <summary>
        /// Función para visualizar el tiempo del día como el pronostico
        /// A partir del archivo xml generado por google
        /// </summary>
        void getWeather(string City)
        {
            labelCity.Text = "El tiempo en " + City;
            conditions = myWeather.GetCurrentConditions(City);

            if (conditions != null)
            {
                labelCon.Text = "Condiciones: " + conditions.Condition;
                labelTEmp.Text = (conditions.TempC + "ºC");
                labelHum.Text = conditions.Humidity;
                labelWind.Text = conditions.Wind;

                #region FORECAST

                ListConditions = myWeather.GetForecast();

                // Today Forecast
                conditions = ListConditions[0];
                pictureBoxDay1.Load(baseURL + conditions.IconDay);
                labelDia1.Text = conditions.DayOfWeek;
                labelTemp1.Text = conditions.TempHigh + " ºC | " + conditions.TempLow + " ºC";

                // Tomorrow Forecast
                conditions = ListConditions[1];
                pictureBoxDay2.Load(baseURL + conditions.IconDay);
                labelDia2.Text = conditions.DayOfWeek;
                labelTEmp2.Text = conditions.TempHigh + " ºC | " + conditions.TempLow + " ºC";

                // Day 3 Forecast
                conditions = ListConditions[2];
                pictureBoxDay3.Load(baseURL + conditions.IconDay);
                labelDia3.Text = conditions.DayOfWeek;
                labelTemp3.Text = conditions.TempHigh + " ºC | " + conditions.TempLow + " ºC";

                // Day 4 Forecast
                conditions = ListConditions[3];
                pictureBoxDay4.Load(baseURL + conditions.IconDay);
                labelDia4.Text = conditions.DayOfWeek;
                labelTemp4.Text = conditions.TempHigh + " ºC | " + conditions.TempLow + " ºC";
                #endregion
            }
            else
            {
                labelTiempo.Text = "En estos momentos no puedo visualizar la información";
                //MessageBox.Show("There was an error processing the request.");
                //MessageBox.Show("Please, make sure you are using the correct location or try again later.");
            }
        }

        private void SacarinoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ESTA COMENTADO PARA EN ESTAS PRUBAS NO GENERAR UN LOG
            //Chat.ChatBot.swWriter.WriteLine("~~~~~~~~~~~~~~~~ Conversation ~~~~~~~~~~~~~~~~");
            //Chat.ChatBot.swWriter.WriteLine("    Cierre Fecha: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            //Chat.ChatBot.swWriter.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            //Chat.ChatBot.swWriter.WriteLine("");

            //Chat.ChatBot.swWriter.Close();
        }

        //Función para llamar a la base de datos y guardar los resultados de la encuesta
        private void guardarBD(string sAtributo, string sValor)
        {
            if (sAtributo != "")
            {
                listBoxResp.Items[0] = sAtributo;
                listBoxResp.Items[1] = sValor;
            }

        }

        private void EndEncuesta()
        {
            //Mensaje de salida y inicializo los componentes de la encuesta
            this.labelPregunta.Text = "Gracias por tu colaboración nos vemos en una proxima ocación.";
            this.labelEncInfo.Text = "A continuación se formulan algunas preguntas en relación a su experiencia de interacción. Por favor, diga la respuesta que prefiera:";
            this.labelIndicador.Text = "";
            labelPregunta.Visible = true;
            groupBoxDemoData.Visible = false;
            nContSurvey = 0;
            nContFrases = 0;

            // Start the BackgroundWorker.
            this.backgroundWorker1.RunWorkerAsync();
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Doy un tiempo de 3 seg para llevar a la ventana principal del dialogo
            // Wait 3 seconds.
            Thread.Sleep(3000);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Llamo a la aplicación para mostrar la pantalla inicial de interacíón
            WebApp("", "");

        }

    }
}