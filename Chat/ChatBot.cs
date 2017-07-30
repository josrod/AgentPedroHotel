using System;
using System.Collections.Generic;
using System.Text;
using AIMLbot;
using System.IO;
using AvatarGestos;
using Busqueda;

/*
C# Programming the chatBot behavior 
by Catalina Roncancio
*/

// ChatBot.cs -- Class that implements the AIMLBot.Dll 
//

namespace Chat
{
    public class ChatBot
    {
        private Bot myBot;
        private User myUser;
        private Request lastRequest = null;
        private Result lastResult = null;
        private SearchTopic mySearchTopic;
        private Dlgbehavior dlgBehavior;

        public static StreamWriter swWriter;
        private string fileName;

        private bool SaludoInicial = true;

        public ChatBot()
        {
            myBot = new Bot();
            myBot.loadSettings();
            myUser = new User("HotelUser", myBot);

            //IniLog();
          
            dlgBehavior = new Dlgbehavior();
            mySearchTopic = new SearchTopic();

            
        }

        /// <summary>
        /// Inicializo el bot.
        /// Cargando los archivos .aiml en el "brain" del bot
        /// </summary>
        #region CONOCIMIENTO INICIAL DEL BOT
        public void ChatBotConfig()
        {

            myBot.isAcceptingUserInput = false;
            myBot.loadAIMLFromFiles();
            myBot.loadCustomTagHandlers("CustomAIMLTags.dll");

            Console.WriteLine("Archivos AIML y CustomTag insertados correctamente en la memoria del Bot!!\n");
            Console.WriteLine("!! AriscoBot 2009 !!\n");

            myBot.isAcceptingUserInput = true;
        }
        #endregion

        #region Iniciar en Log el dialogo
        void IniLog()
        {

            StringBuilder sbFile = new StringBuilder();
            sbFile.Append("LOG/");
            sbFile.Append("usuario_Prueba");
            sbFile.Append(".log");
            fileName = sbFile.ToString();

            swWriter = new StreamWriter(fileName, true);
            swWriter.WriteLine("~~~~~~~~~~~~~~~~ Conversation ~~~~~~~~~~~~~~~~");
            swWriter.WriteLine("    Fecha: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            swWriter.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            swWriter.WriteLine("");
        }
        #endregion

        #region RESPUESTA DEL BOT
        public Dlgbehavior processInputFromUser(string strASRResult, double fConfidence)
        {

            string rawInput = strASRResult;
            string strBotResult = "";

            // borro el la emoción de la entrada anterior
            this.myUser.Predicates.addSetting("emotions", "");

            if (this.myBot.isAcceptingUserInput && rawInput != null && fConfidence > 0.35)
            {
                //QUITAR COMENTARIOS PARA QUE HAGA EL SALUDO INICIAL
                if (SaludoInicial)
                {
                    if (rawInput.Contains("hola"))
                    {
                        rawInput = "holainicial";
                        SaludoInicial = false;
                    }
                }

                Request myRequest = new Request(rawInput, this.myUser, this.myBot);
                Result myResult = this.myBot.Chat(myRequest);
                this.lastRequest = myRequest;
                this.lastResult = myResult;

                strBotResult = myResult.Output.ToString();
                GestorDelDialogo(strBotResult);

            }
            else if (this.myBot.isAcceptingUserInput && rawInput != null && (0.2 < fConfidence && fConfidence<= 0.35))
            {
                Request myRequest = new Request(rawInput, this.myUser, this.myBot);
                Result myResult = this.myBot.Chat(myRequest);

                dlgBehavior.ChatResult = myResult.Output.ToString();
                //dlgBehavior.ChatResult = "Perdona!. ¿puedes repetir lo que has dicho?."+Environment.NewLine;
            }
            else
                 dlgBehavior.ChatResult = "";

            // QUITAR COMENTARIO SI SE QUIERE USAR EL LOG
            //swWriter.WriteLine(" You: " + rawInput);
            //swWriter.WriteLine(" Bot: " + dlgBehavior.ChatResult);

            return dlgBehavior;
        }
#endregion

        private void GestorDelDialogo(string sBotResult)
        {
            // Actualizo el comportamiento del dialogo con la emocion
            // Topic, Acción, Atributo, Valor
            //dlgBehavior.ChatResult = AsignarEmocion(sBotResult);
            
            StringBuilder BotResult = new StringBuilder();
            BotResult.AppendLine(AsignarEmocion(sBotResult));

            string sAcction = accion();
            string sAtributo = atributo();
            string sValor = valor();
            string strTipoTopic = myUser.Topic;

            if (sAcction == "buscar")
            {
                // Para leer los resultados de la busqueda
                BotResult = mySearchTopic.Topic(strTipoTopic, sAtributo, sValor);
                if (BotResult.Length !=  0)
                {
                    dlgBehavior.GUrl = mySearchTopic.myURL;
                }

                else
                {
                    //sOutPut = " Lo siento, en este momento no dispongo de esa información." + Environment.NewLine;
                    //sOutPut += "¿Que otra información necesita?";
                    BotResult.AppendLine("Lo siento, en este momento no dispongo de esa información.");
                    BotResult.AppendLine("¿Que otra información necesita?");
                }
            }
            dlgBehavior.ChatResult = BotResult.ToString();
            dlgBehavior.Topic = strTipoTopic;
            dlgBehavior.Action = sAcction;
            dlgBehavior.Atributo = sAtributo;
            dlgBehavior.Valor = sValor;
        }
        private string AsignarEmocion(string strBotResult)
        {
            /*** Asigno la emocion a mi mensaje ****/
            string sEmotion = emotions();
            string MsgIPCEmo="";

            switch (sEmotion)
            {
                case "triste":
                    {
                        MsgIPCEmo = Emotions.iSadBye + strBotResult + Emotions.fSadBye;
                        MsgIPCEmo += Emotions.iParpadeo + Emotions.fParpadeo;
                        //MsgIPCEmo = strBotResult;
                        break;
                    }
                case "alegre":
                    {
                        MsgIPCEmo = Emotions.iBocaContento + strBotResult + Emotions.fBocaContento;
                        MsgIPCEmo += Emotions.iParpadeo + Emotions.fParpadeo;
                        break;
                    }
                case "mirar":
                    {
                        MsgIPCEmo = Emotions.iMirar + strBotResult + Emotions.fMirar;
                        MsgIPCEmo += Emotions.iParpadeo + Emotions.fParpadeo;
                        break;
                    }
                case "parpadeo":
                    {
                        MsgIPCEmo = strBotResult + Emotions.iParpadeo + Emotions.fParpadeo;
                        break;
                    }
                default:
                    {
                        MsgIPCEmo = strBotResult;
                        MsgIPCEmo += Emotions.iParpadeo + Emotions.fParpadeo;
                        break; // sin emocion
                    }

            }
            return MsgIPCEmo;
        }

        public void ReiniciarValores()
        {
            SaludoInicial = true;
            
            //borro el comportamiento del dialogo anterior
            this.myUser.Predicates.addSetting("accion", "");
            this.myUser.Predicates.addSetting("emotions", "");
            this.myUser.Predicates.addSetting("atributo", "");
            this.myUser.Predicates.addSetting("valor", "");
            this.myUser.Predicates.addSetting("topic", "");
        }



        #region Representación del comportamiento del diálogo
        string accion()
        {
            string result = "";
            foreach (string setting in this.myUser.Predicates.SettingNames)
            {
                if (setting.Equals("ACCION"))
                    result = this.myUser.Predicates.grabSetting(setting);
            }
            return result;
        }

        string emotions()
        {
            string result = "";
            foreach (string setting in this.myUser.Predicates.SettingNames)
            {
                if (setting.Equals("EMOTIONS"))
                    result = this.myUser.Predicates.grabSetting(setting);
            }
            return result;
        }
        
        string atributo()
        {
            string result = "";
            foreach (string setting in this.myUser.Predicates.SettingNames)
            {
                if (setting.Equals("ATRIBUTO"))
                    result = this.myUser.Predicates.grabSetting(setting);
            }
            return result;
        }

        string valor()
        {
            string result = "";
            foreach (string setting in this.myUser.Predicates.SettingNames)
            {
                if (setting.Equals("VALOR"))
                    result = this.myUser.Predicates.grabSetting(setting);
            }
            return result;
        }

        //string POP()
        //{
        //    string result = "";
        //    foreach (string setting in this.myUser.Predicates.SettingNames)
        //    {
        //        if (setting.Equals("TOP"))
        //            result = this.myUser.Predicates.grabSetting(setting);
        //    }
        //    return result;
        //}
        
        #endregion
        
    }
}
