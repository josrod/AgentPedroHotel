using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AvatarGestos;
using BotASR;
using WebApp;
using Chat;
using Encuesta;
using System.Text.RegularExpressions;


namespace AIEBOT
{
    public class Principal
    {
        ASRDllImport AsrDll;
        private IPCDllImport myIPCDll;
        private ChatBot myChatBot;

        // Variables que manejan la entrada y salida del Chat
        private string rawInput;
        private string sOutPut;

        // preparo las frases del test
        Preguntas ListFrases; // Inicializo la lista
        int NumFrases; // Guardo el número de preguntas
        int nContFrases; // contador al numero de preguntas

        int NumSurvey;
        int nContSurvey; // Contador al número de preguntas survey
        Regex objNaturalPattern; // verificar si la respuesta del test esta entre [1-5]


        public Principal()
        {

            #region DefinicionesGlobalIPC
            string nombreModulo = "Sacarino_ASR";
            string dirIPCentral = "127.0.0.1";//"192.168.107.43";//"127.0.0.1"
            string tipoMensaje_ReconASR = "MSG_Asr_Recognition";
            string tipoDatos_ReconASR = "{string,string}";
            #endregion

            #region Inicialización de la Comunicacion IPC
            myIPCDll = new IPCDllImport();
            //Conexion al central
            myIPCDll.Conectar(nombreModulo, dirIPCentral);
            ////Definicion de mensajes
            myIPCDll.DefinirMensaje(tipoMensaje_ReconASR, tipoDatos_ReconASR);
            #endregion

            #region Inicialización del ASR;
            AsrDll = new ASRDllImport();
            //AsrDll.OnMsg += new MsgEventHandler(AsrDll_OnMsg);  // Eventos del LASR
            AsrDll.ASRConfiguration();
            Console.WriteLine(AsrDll.strStatus + "\n");
            AsrDll.strLibrary = "AudioMM";
            AsrDll.strFileSource = null;
            #endregion

            #region Inicialización del ChatBot;
            myChatBot = new ChatBot();
            myChatBot.ChatBotConfig();
            #endregion

            //MsgInicial(); // lo he quitado para hacer rapida la encuesta

            ListFrases = new Preguntas();
            NumFrases = ListFrases.frases.Count;
            nContFrases = 0;
            NumSurvey = ListFrases.survey.Count;
            nContSurvey = 0;

            objNaturalPattern = new Regex("[1-9]");

        }

        public void IniciarPrograma(SacarinoForm FormSacarino, string sInput)//Quitar sInput cuando sea x voz
        {
            Console.WriteLine("\n ESPERANDO ENTRADA DE VOZ");
            //Inicio reconocimiento
            //AsrDll.OnRecognize();
            //rawInput = AsrDll.strLastResult; 

            rawInput = sInput;
                
            // Obtengo el resultado del reconocimiento y lo imprimo                
            Console.WriteLine("You: " + rawInput + "\n");

            if (AsrDll.nEvento == Constants.LASRX_RETCODE_OK)
            {
                // Llamo al Chat para procesar la entrada del usuario
                FormSacarino.myDlgBehavior = myChatBot.processInputFromUser(rawInput, 0.44);// AsrDll.fConfidence); OR 0.44);
                sOutPut = FormSacarino.myDlgBehavior.ChatResult;

                // llamo a la acción a realizar según el promt del usuario
                if (sOutPut != "")
                {
                    if (FormSacarino.myDlgBehavior.Action == "encuesta")
                    {
                        Iniciar_Encuesta(FormSacarino.myDlgBehavior.Valor);
                    }
                    else
                    {
                        myIPCDll.EnviarTexto("ASR_OK", sOutPut);
                        Console.WriteLine("Bot: " + sOutPut + "\n");
                    }

                    //MostrarResultado en el SacarinoForm
                    FormSacarino.WebApp(FormSacarino.myDlgBehavior.Action, FormSacarino.myDlgBehavior.Topic);

                }
                else if (AsrDll.nEvento == Constants.LASRX_RETCODE_NO_RESULTS)
                {
                    Console.Write("You: -- \n");
                }
                Console.WriteLine("topic: " + FormSacarino.myDlgBehavior.Topic + "\n");
                Console.WriteLine("accion: " + FormSacarino.myDlgBehavior.Action + "\n");
                Console.WriteLine("valor: " + FormSacarino.myDlgBehavior.Valor + "\n");
            }
            else
            {
                //formSacarino.myDlgBehavior = myChatBot.processInputFromUser("salir", 0.44);// AsrDll.fConfidence); OR 0.44);
                //sOutPut = formSacarino.myDlgBehavior.ChatResult;
                myIPCDll.EnviarTexto("ASR_OK", sOutPut);
                Console.WriteLine("Bot: " + sOutPut + "\n");
            }

        }

        public void MsgInicial()
        {
            string MsgIPCEmo = "Bienvenido yo soy el botones Pedro.";
            myIPCDll.EnviarTexto("ASR_OK", MsgIPCEmo);
            MsgIPCEmo = "Como puedes ver en la pantalla de ayuda, puedes preguntarme acerca de mi, del tiempo, del hotel y tambien sobre restaurantes de la zona.";
            myIPCDll.EnviarTexto("ASR_OK", Emotions.iMirar + MsgIPCEmo + Emotions.fMirar);
            myIPCDll.EnviarTexto("ASR_OK", Emotions.iParpadeo + Emotions.fParpadeo);
            MsgIPCEmo = "Para empezar a interactuar conmigo solo debes decir !hola!";
            myIPCDll.EnviarTexto("ASR_OK", Emotions.iBocaContento + MsgIPCEmo + Emotions.fBocaContento);
            myIPCDll.EnviarTexto("ASR_OK", Emotions.iParpadeo + Emotions.fParpadeo);

        }

        private void Iniciar_Encuesta(string sValor)
        {
            if (nContFrases < NumFrases && objNaturalPattern.IsMatch(sValor))
            {
                sOutPut = ListFrases.frases[nContFrases];
                nContFrases++;
            }
            else
            {
                if (nContFrases >= NumFrases)
                {
                    if (nContSurvey == 0)
                    {
                        sOutPut = "A continuación debes llenar los siguiente datos demográficos, sobre tu Sexo, Edad, y experiencia en sistemas como este y al final si lo deseas puedes hacer algunos comentarios";
                        myIPCDll.EnviarTexto("ASR_OK", sOutPut);
                        Console.WriteLine("Bot: " + sOutPut + "\n");
                    }

                    if (nContSurvey < NumSurvey && objNaturalPattern.IsMatch(sValor))
                    {
                        sOutPut = ListFrases.survey[nContSurvey];
                        nContSurvey++;
                    }
                    else
                    {
                        if (nContSurvey >= NumSurvey)
                        {
                            sOutPut = "Gracias por tu colaboración nos vemos en una proxima ocación";
                            ReIniciarValores();
                        }
                    }

                 }
            }
            myIPCDll.EnviarTexto("ASR_OK", sOutPut);
            Console.WriteLine("Bot: " + sOutPut + "\n");
        }

        private void ReIniciarValores()
        {
            nContFrases = 0;
            nContSurvey = 0;
            myChatBot.ReiniciarValores();
        }

    }
}
