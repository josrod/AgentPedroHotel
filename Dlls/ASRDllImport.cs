using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace BotASR
{
    //public delegate void MsgEventHandler();
    public class ASRDllImport
    {
        // PARAMETROS
        const string dllLocation = "LoquendoDLL.dll";
        const string dllAudioMMLocation = "LoqASRAudioMM.dll";
        string path = Environment.CurrentDirectory;
  
        // Variables publicas
        public string strLastResult=null;
        public string strStatus = "";
        public string strLibrary="";
        public string strFileSource="";
        public float fConfidence = 0;
        public int nEvento = 0;
        
        // punteros y variables de la clase
        private int iResultado = 999;
        private string strSummary = "";
        private IntPtr ptrInstance;
        private IntPtr ptrSession;
        private IntPtr ptrAudio;
        private IntPtr ptrElement = IntPtr.Zero;

        // Delegado a la función que se activa con los callback de LASR 
        protected delegate int eventos(IntPtr hInstance, string pUser, int nEvent, int nReason, string sZId, string pEventData, int nEventDataSize);
        protected eventos myEvent;
        // Delegado a los mensajes
        //public event MsgEventHandler OnMsg;


        #region Importo LoquendoDll, LoqAudioFileDll y LoqAudioMMDll

        [DllImport(dllLocation, EntryPoint = "lasrxNewSession")]
        protected static extern int lasrxNewSession(string sIniFile, string sIniInfo, out IntPtr hSession);

        [DllImport(dllLocation, EntryPoint = "lasrxDeleteSession")]
        protected static extern int lasrxDeleteSession(IntPtr hSession);

        [DllImport(dllLocation, EntryPoint = "lasrxSetAudioMode")]
        protected static extern int lasrxSetAudioMode(IntPtr hSession, int nAudioMode);

        [DllImport(dllLocation, EntryPoint = "lasrxNewInstance")]
        protected static extern int lasrxNewInstance(IntPtr hSession, string szCapabilities, bool bLock, out IntPtr hInstance);

        [DllImport(dllLocation, EntryPoint = "lasrxSetCallbackDataPointer")]
        protected static extern int lasrxSetCallbackDataPointer(IntPtr hInstance,IntPtr pUser);
        
        [DllImport(dllLocation,CharSet=CharSet.Ansi)]
        protected static extern int lasrxSetCallbackGetEvent(IntPtr hInstance, eventos pGetEvent);

        [DllImport(dllLocation, EntryPoint = "lasrxClearROs")]
        protected static extern int lasrxClearROs(IntPtr hInstance);

        [DllImport(dllLocation, EntryPoint = "lasrxGetROType")]
        protected static extern int lasrxGetROType(IntPtr hInstance, string szRoName, ref string pszValue);

        [DllImport(dllLocation, EntryPoint = "lasrxFree")]
        protected static extern int lasrxFree(ref string pPtr);

        [DllImport(dllLocation, EntryPoint = "lasrxAddRO")]
        protected static extern int lasrxAddRO(IntPtr hInstance, string szName, string eszRule);

        [DllImport(dllLocation, EntryPoint = "lasrxGetSamplingFrequency")]
        protected static extern int lasrxGetSamplingFrequency(IntPtr hInstance, ref uint pnSamplingFrequency);
        
        [DllImport(dllLocation, EntryPoint = "lasrxGetAudioMode")]
        protected static extern int lasrxGetAudioMode(IntPtr hSession, ref int pnAudioMode);

        [DllImport(dllLocation, EntryPoint = "lasrxRecog")]
        protected static extern int lasrxRecog(IntPtr hInstance, int nProcessingMode, string pszId);

        [DllImport(dllLocation, EntryPoint = "lasrxGetErrorMessage")]
        protected static extern int lasrxGetErrorMessage(IntPtr hInstance, ref string szErrMess);
        
        [DllImport(dllLocation, EntryPoint = "lasrxRRGetRejectionAdvice")]
        protected static extern int lasrxRRGetRejectionAdvice(IntPtr hInstance, ref int pnRejectionFlag);

        [DllImport(dllLocation, EntryPoint = "lasrxRRGetNumberOfHypothesis")]
        protected static extern int lasrxRRGetNumberOfHypothesis(IntPtr hInstance, ref int pnNumber);

        [DllImport(dllLocation, EntryPoint = "lasrxRRGetHypothesisRONameAndRule")]
        protected static extern int lasrxRRGetHypothesisRONameAndRule(IntPtr hInstance, int nHyposIndex, ref string pszName, ref string eszRule);
   
        [DllImport(dllLocation, EntryPoint = "lasrxRRGetHypothesisString")]
        protected static extern int lasrxRRGetHypothesisString(IntPtr hInstance, int nHyposIndex, ref string peszString);
        
        [DllImport(dllLocation, EntryPoint = "lasrxRRGetHypothesisConfidence")]
        protected static extern int lasrxRRGetHypothesisConfidence(IntPtr hInstance, int nHyposIndex, ref float pfConfidence);
    
        [DllImport(dllLocation, EntryPoint = "lasrxRRGetHypothesisNumberOfWords")]
        protected static extern int lasrxRRGetHypothesisNumberOfWords(IntPtr hInstance, int nHyposIndex, ref int pnNumber);

        [DllImport(dllLocation, EntryPoint = "lasrxRRGetWordHypothesisString")]
        protected static extern int lasrxRRGetWordHypothesisString(IntPtr hInstance, int nHyposIndex, int nWordIndex, ref string peszString);
    
        [DllImport(dllLocation, EntryPoint = "lasrxRRGetWordHypothesisLanguage")]
        protected static extern int lasrxRRGetWordHypothesisLanguage(IntPtr hInstance, int nHyposIndex, int nWordIndex,ref string pszLanguage);
    
        [DllImport(dllLocation, EntryPoint = "lasrxNLPComputeInterpretations")]
        protected static extern int lasrxNLPComputeInterpretations(IntPtr hInstance,int nHyposIndex, ref int pnNumber);
     
        [DllImport(dllLocation, EntryPoint = "lasrxNLPHandleToBuffer")]
        protected static extern int lasrxNLPHandleToBuffer(IntPtr hInstance, IntPtr nElement, int nFormat, ref string peszValue);
    
        [DllImport(dllLocation, EntryPoint = "lasrxNLPReleaseInterpretation")]
        protected static extern int lasrxNLPReleaseInterpretation(IntPtr hInstance);

        [DllImport(dllLocation, EntryPoint = "lasrxRRGetSpeechLimits")]
        protected static extern int lasrxRRGetSpeechLimits(IntPtr hInstance, int nFormat, ref int pnStart,ref int pnEnd);

        [DllImport(dllLocation, EntryPoint = "lasrxRRGetSignalToNoiseRatio")]
        protected static extern int lasrxRRGetSignalToNoiseRatio(IntPtr hInstance,ref float pfSnr);

        [DllImport(dllLocation, EntryPoint = "lasrxStop")]
        protected static extern int lasrxStop(IntPtr hInstance);   
        
        /****************** Importo la libreria para leer audio samples de un archivo ***************/      
        
        [DllImport("LoqASRAudioFile.dll", EntryPoint = "NewHandleAudioFile")]
        protected static extern int NewHandleAudioFile(int nFormat, out IntPtr hAudio);
        
        [DllImport("LoqASRAudioFile.dll", EntryPoint = "DeleteHandleAudioFile")]
        protected static extern int DeleteHandleAudioFile(IntPtr hAudio);

        [DllImport("LoqASRAudioFile.dll", EntryPoint = "SetDataAudioFile")]
        protected static extern int SetDataAudioFile(IntPtr hAudio, string filename);

        [DllImport("LoqASRAudioFile.dll", EntryPoint = "RegisterAudioFile")]
        protected static extern int RegisterAudioFile(IntPtr hAudio, IntPtr hInstance);

        ///****************** Importo la libreria cargar audio de Windows multimedia ***************/
        [DllImport(dllAudioMMLocation, EntryPoint = "NewHandleAudioMM")]
        protected static extern int NewHandleAudioMM(int nFormat, out IntPtr hAudio);

        [DllImport(dllAudioMMLocation, EntryPoint = "DeleteHandleAudioMM")]
        protected static extern int DeleteHandleAudioMM(IntPtr hAudio);

        [DllImport(dllAudioMMLocation, EntryPoint = "SetDataAudioMM")]
        protected static extern int SetDataAudioMM(IntPtr hAudio, string filename);

        [DllImport(dllAudioMMLocation, EntryPoint = "RegisterAudioMM")]
        protected static extern int RegisterAudioMM(IntPtr hAudio, IntPtr hInstance);
        #endregion

        #region Configuracion de LASR (Lenguaje, Sesion, Modo Audio, Instancia y ROs 
        public void ASRConfiguration()
        {
            iResultado = 999;
            myEvent = new eventos(GetEvent);
            
            string strIniInfo = Constants.INIINFO;
            strIniInfo = strIniInfo.Replace("#LANGUAGE#", Constants.LANGUAJE);

            if (ptrSession !=IntPtr.Zero)
            {
                lasrxDeleteSession(ptrSession);
            }
            

            if ((iResultado = lasrxNewSession("Default.session", strIniInfo, out ptrSession)) != Constants.LASRX_RETCODE_OK)
            {
                MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ptrSession = IntPtr.Zero;
            }
            else if ((iResultado = lasrxSetAudioMode(ptrSession, Constants.LASRX_SAMPLES_LIN16)) != Constants.LASRX_RETCODE_OK)
            {
                MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                lasrxDeleteSession(ptrSession);
                ptrSession = IntPtr.Zero;
            }
            else if ((iResultado = lasrxNewInstance(ptrSession, null, false, out ptrInstance)) != Constants.LASRX_RETCODE_OK)
            {
                ptrInstance = IntPtr.Zero;
                MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                lasrxDeleteSession(ptrSession);
                ptrSession = IntPtr.Zero;
            }
            else
            {
                strStatus= ("Lenguaje seleccionado " + "\"Español\""+"\n");
                lasrxSetCallbackDataPointer(ptrInstance, IntPtr.Zero);
                lasrxSetCallbackGetEvent(ptrInstance, myEvent);
                UpdateROs(Constants.LANGUAJE,Constants.RPFOLDER);
            }
        }

        void UpdateROs(string strLanguage, string strRPfolder)
        {
            string strRO = "";
            int nIdx = 0;
            path = @strRPfolder;
            DirectoryInfo DirInfo = new DirectoryInfo(path);
            DirectoryInfo[] DirFiles = DirInfo.GetDirectories();
            string[] strROIdList = new string[DirFiles.Length];
            //CARGO LOS OBJETOS DE RECONOCIMIENTO A UTILIZAR EN UNA LISTA
            foreach (DirectoryInfo Dirtemp in DirFiles)
            {
                strRO = (Dirtemp.Name);
                strROIdList[nIdx] = "rp_es-es"+"/"+strRO;
                nIdx++;
            }
            AddRO(strROIdList);
        }
        //ADD RO
        public void AddRO(string[] strROIdentifierList)
        {
            int nIdx;
            string strName = null;

            if ((iResultado = lasrxClearROs(ptrInstance)) != Constants.LASRX_RETCODE_OK)
            {
                MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            for (nIdx = 0; nIdx < strROIdentifierList.Length; nIdx++)
            {
                strName = strROIdentifierList[nIdx];
                string szRoRule = null;
                if (strName != null)
                { 
                    if ((iResultado=lasrxAddRO(ptrInstance, strName, szRoRule)) != Constants.LASRX_RETCODE_OK)
                    {
                        MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        strStatus+=("Seleccionado "+ strName+"\n");
                    }
                }
            }

        }
        
#endregion

        #region RECONOCIMIENTO
        //RECOG
        public void OnRecognize()
        {
            uint nSamplingFreq = 0;
            iResultado = 999;
            strStatus = "";
            strLastResult = "";
            nEvento = 0;
            if ((iResultado = lasrxGetSamplingFrequency(ptrInstance, ref nSamplingFreq)) != 0)
                MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                if (LoadAudioSource() != 0)
                {
                    MessageBox.Show("Error cargando la Libreria de audio", "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if ((iResultado = lasrxRecog(ptrInstance, Constants.LASRX_RUNMODE_BLOCKING, null)) != 0)
                {
                    Console.WriteLine(MapLasrxRetcode(iResultado) + " Result");
                    //MessageBox.Show(MapLasrxRetcode(iResultado), "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }
        #endregion

        #region AUDIO SOURCE
        public int LoadAudioSource()
        {
            int nRetCode = Constants.LASRX_RETCODE_OK;
            int nAudioMode=0;

            if ((nRetCode = lasrxGetAudioMode(ptrSession, ref nAudioMode)) != Constants.LASRX_RETCODE_OK)
            {
                return -1;
            }

            if (string.Compare(strLibrary, "AudioFile") == 0)
            {
                if (NewHandleAudioFile(nAudioMode, out ptrAudio) != 0)
                {
                    return -1;
                }
                if (File.Exists(strFileSource))
                {
                    if (SetDataAudioFile(ptrAudio, strFileSource) != 0)
                    {
                        return -1;
                    }
                }
                if (RegisterAudioFile(ptrAudio, ptrInstance) != 0)
                {
                    return -1;
                }
                return 0;
            }
            if (string.Compare(strLibrary, "AudioMM") == 0)
            {
                if (NewHandleAudioMM(nAudioMode, out ptrAudio) != 0)
                {
                    
                    return -1;
                }
                if (File.Exists(strFileSource))
                {
                    if (SetDataAudioMM(ptrAudio, strFileSource) != 0)
                    {
                        return -1;
                    }
                }
                if (RegisterAudioMM(ptrAudio, ptrInstance) != 0)
                {
                    return -1;
                }
                return 0;
            }
            else
                return -1;

        }
        public int UnloadAudioSource()
        {
            if (ptrAudio != IntPtr.Zero)
            {
                if (string.Compare(strLibrary, "AudioFile") == 0)
                    DeleteHandleAudioFile(ptrAudio);
                else
                    DeleteHandleAudioMM(ptrAudio);
                ptrAudio = IntPtr.Zero;
            }
            return 0;
        }
#endregion

        public void OnStop()
        {
            lasrxStop(ptrInstance);
        }

        //funcion que guadar en un strig el estado del reconocimiento
        // y activa la llamada de cada evento en el form. 
        public void Status(string estado)
        {
            strStatus = estado;
            Console.WriteLine(estado);
            //OnMsg();
        }

        #region CALLBACKS DEL RECONOCIMIENTO
        public int GetEvent(IntPtr hInstance, string pUser, int nEvent, int nReason, string sZId, string pEventData, int nEventDataSize)
        {
            switch (nEvent)
            {
                case Constants.LASRX_EVENT_START_VOICE_DETECTED:
                    Status("Event: LASRX_EVENT_START_VOICE_DETECTED\n");
                    break;
                case Constants.LASRX_EVENT_END_VOICE_DETECTED:
                    Status("Event: LASRX_EVENT_END_VOICE_DETECTED\n");
                    break;
                case Constants.LASRX_EVENT_PROMPT_STOP:
                    //Status("Event: LASRX_EVENT_PROMPT_STOP\n");
                    break;
                case Constants.LASRX_EVENT_END_RECOG:
                    Status("Event: LASRX_EVENT_END_RECOG: reason = " + MapLasrxRetcode(nReason) + "\n");
                    ReportResults(nReason, 1);
                    break;
                default:
                    Status("Event: UNKNOWN:" + nReason + "reason = " + MapLasrxRetcode(nReason) + "\n");
                    break;
            }
            return Constants.LASRX_RETCODE_OK;
        }
        
        public string MapLasrxRetcode(int nRetcode)
        {
            switch (nRetcode)
            { 
                case Constants.LASRX_RETCODE_IN_PROGRESS: return ("LASRX_RETCODE_IN_PROGRESS");
                case Constants.LASRX_RETCODE_STOPPED: return ("LASRX_RETCODE_STOPPED");
                case Constants.LASRX_RETCODE_NO_RESULTS: return ("LASRX_RETCODE_NO_RESULTS");
                case Constants.LASRX_RETCODE_OK: return ("LASRX_RETCODE_OK");
                case Constants.LASRX_RETCODE_ERROR: return ("LASRX_RETCODE_ERROR");
                case Constants.LASRX_RETCODE_INVALID_HANDLE: return ("LASRX_RETCODE_INVALID_HANDLE");
                case Constants.LASRX_RETCODE_INVALID_STATE: return ("LASRX_RETCODE_INVALID_STATE");
                case Constants.LASRX_RETCODE_AUDIO: return ("LASRX_RETCODE_AUDIO");
                case Constants.LASRX_RETCODE_AUDIO_TIMEOUT_SAMPLES: return ("LASRX_RETCODE_AUDIO_TIMEOUT_SAMPLES");
                case Constants.LASRX_RETCODE_AUDIO_TIMEOUT_STOP: return ("LASRX_RETCODE_AUDIO_TIMEOUT_STOP");
                case Constants.LASRX_RETCODE_BAD_PARAMETER: return ("LASRX_RETCODE_BAD_PARAMETER");
                case Constants.LASRX_RETCODE_DECODING: return ("LASRX_RETCODE_DECODING");
                case Constants.LASRX_RETCODE_FUNCTIONALITY_NOT_ENABLED: return ("LASRX_RETCODE_FUNCTIONALITY_NOT_ENABLED");
                case Constants.LASRX_RETCODE_WRONG_CONFIGURATION: return ("LASRX_RETCODE_WRONG_CONFIGURATION");
                case Constants.LASRX_RETCODE_NO_MORE_MEMORY: return ("LASRX_RETCODE_NO_MORE_MEMORY");
                case Constants.LASRX_RETCODE_NO_SEMANTIC: return ("LASRX_RETCODE_NO_SEMANTIC");
                case Constants.LASRX_RETCODE_RESOURCE_READ: return ("LASRX_RETCODE_RESOURCE_READ");
                case Constants.LASRX_RETCODE_RESOURCE_WRITE: return ("LASRX_RETCODE_RESOURCE_WRITE");
                case Constants.LASRX_RETCODE_RESOURCE_TIMEOUT: return ("LASRX_RETCODE_RESOURCE_TIMEOUT");
                case Constants.LASRX_RETCODE_RESOURCE_COMPATIBILITY: return ("LASRX_RETCODE_RESOURCE_COMPATIBILITY");
                case Constants.LASRX_RETCODE_LICENSE_MISSING: return ("LASRX_RETCODE_LICENSE_MISSING");
                case Constants.LASRX_RETCODE_LICENSE_INVALID: return ("LASRX_RETCODE_LICENSE_INVALID");
                case Constants.LASRX_RETCODE_LICENSE_BAD: return ("LASRX_RETCODE_LICENSE_BAD");
                case Constants.LASRX_RETCODE_LICENSE_EXPIRED: return ("LASRX_RETCODE_LICENSE_EXPIRED");
                case Constants.LASRX_RETCODE_LICENSE_TIER: return ("LASRX_RETCODE_LICENSE_TIER");
                case Constants.LASRX_RETCODE_LICENSE_OVERFLOW_INSTANCES: return ("LASRX_RETCODE_LICENSE_OVERFLOW_INSTANCES");
                case Constants.LASRX_RETCODE_LICENSE_LANGUAGE: return ("LASRX_RETCODE_LICENSE_LANGUAGE");
                case Constants.LASRX_RETCODE_LICENSE_OPTION: return ("LASRX_RETCODE_LICENSE_OPTION");
                case Constants.LASRX_RETCODE_LICENSE_OVERFLOW_OPTION: return ("LASRX_RETCODE_LICENSE_OVERFLOW_OPTION");
                case Constants.LASRX_RETCODE_TIMEOUT_SILENCE: return ("LASRX_RETCODE_TIMEOUT_SILENCE");
                case Constants.LASRX_RETCODE_TIMEOUT_SPEECH: return ("LASRX_RETCODE_TIMEOUT_SPEECH");
                case Constants.LASRX_RETCODE_OVERFLOW: return ("LASRX_RETCODE_OVERFLOW");
                case Constants.LASRX_RETCODE_OVERFLOW_INSTANCES: return ("LASRX_RETCODE_OVERFLOW_INSTANCES");
                case Constants.LASRX_RETCODE_OVERFLOW_LANGUAGES: return ("LASRX_RETCODE_OVERFLOW_LANGUAGES");
                case Constants.LASRX_RETCODE_OVERFLOW_UTTERANCES: return ("LASRX_RETCODE_OVERFLOW_UTTERANCES");
                case Constants.LASRX_RETCODE_EPD_MIN_LENGTH: return ("LASRX_RETCODE_EPD_MIN_LENGTH");
                case Constants.LASRX_RETCODE_EPD_MAX_LENGTH: return ("LASRX_RETCODE_EPD_MAX_LENGTH");
                case Constants.LASRX_RETCODE_EPD_MIN_SNR: return ("LASRX_RETCODE_EPD_MIN_SNR");
                case Constants.LASRX_RETCODE_EPD_BAD_TRIGGER: return ("LASRX_RETCODE_EPD_BAD_TRIGGER");
                case Constants.LASRX_RETCODE_EPD_ON_BEEP: return ("LASRX_RETCODE_EPD_ON_BEEP");
                case Constants.LASRX_RETCODE_EPD_CUT_OFF: return ("LASRX_RETCODE_EPD_CUT_OFF");
                case Constants.LASRX_RETCODE_EPD_REJECT: return ("LASRX_RETCODE_EPD_REJECT");
                case Constants.LASRX_RETCODE_SEMANTIC: return ("LASRX_RETCODE_SEMANTIC");
                default: return ("UNKNOWN");
            }

        }
        #endregion

        #region RECOGNITION RESULTS
        public void ReportResults(int nRet, int mode)
        {
            int nStart = 0, nEnd = 0;
            float fSNR=0;

            if (nRet != Constants.LASRX_RETCODE_OK)
            {
                switch (nRet)
                {
                    case Constants.LASRX_RETCODE_STOPPED:
                        nEvento = Constants.LASRX_RETCODE_STOPPED;
                        Status("El reconocimiento se ha deteneido\n");
                        break;
                    case Constants.LASRX_RETCODE_NO_RESULTS:
                        nEvento = Constants.LASRX_RETCODE_NO_RESULTS;
                        Status("Se ha detectado ruido\n");
                        break;
                    default:
                        {
                            string szErrMess = null;
                            lasrxGetErrorMessage(ptrInstance, ref szErrMess);
                            Status("Error Message" + szErrMess);
                            szErrMess = null;
                        }
                        nEvento = Constants.LASRX_RETCODE_NO_RESULTS;
                        Status("Error en el reconocimiento");
                        break;
                }
                return;
            }
            ReportSummary(nRet, mode);

            if (lasrxRRGetSpeechLimits(ptrInstance, Constants.LASRX_UNITS_FRAMES, ref nStart, ref nEnd) == Constants.LASRX_RETCODE_OK)
            {
                strSummary += ("Start-End frame: " + nStart.ToString() + "-" + nEnd.ToString() + "\n");
            }

            if (lasrxRRGetSignalToNoiseRatio(ptrInstance, ref fSNR) == Constants.LASRX_RETCODE_OK)
            {
                strSummary += ("SRN: " + fSNR.ToString() + "\n");
            }
            //nEvento = Constants.LASRX_EVENT_END_RECOG;
            Status("***\n");

        }
        public void ReportSummary(int nRet, int mode)
        {  
            int nInterpretations=0;
            int nRejection=0;
            int nNumber=0;
            fConfidence=0;
            string eszString=null, eszValue=null;
            
            strLastResult = "";
            strSummary = "";
            
            int nNumOfHypos = lasrxRRGetNumberOfHypothesis(ptrInstance, ref nNumber);
            lasrxRRGetHypothesisRONameAndRule(ptrInstance, nNumOfHypos, ref eszString, ref eszValue);
            lasrxRRGetRejectionAdvice(ptrInstance ,ref nRejection);

            if (lasrxRRGetHypothesisString(ptrInstance, 0, ref eszString) == Constants.LASRX_RETCODE_OK) 
            {
                lasrxRRGetHypothesisConfidence(ptrInstance ,0,ref fConfidence);
                strSummary += (eszString + "\n");
                strSummary += ("Confidence: ");
                strSummary += (fConfidence.ToString() + "\n");
                Console.WriteLine("Confidence: "+ fConfidence.ToString());
                eszString=null;
                // Build result string using specific language for each word
                int nWords = 0;
                lasrxRRGetHypothesisNumberOfWords(ptrInstance,0,ref nWords);
                for(int i = 0; i < nWords; i++)
                {
                    string eszWord=null;
                    string szLanguage=null;
                    lasrxRRGetWordHypothesisString(ptrInstance,0,i,ref eszWord);
                    lasrxRRGetWordHypothesisLanguage(ptrInstance,0,i,ref szLanguage);
                    if (string.Compare(eszWord,"$GARBAGE")!= 0) 
                    {
                        strLastResult +=eszWord;
                        strLastResult += " ";
                    }
                    eszWord=null;
                    szLanguage=null;
                }
            }

            if (lasrxNLPComputeInterpretations(ptrInstance, 0, ref nInterpretations) == Constants.LASRX_RETCODE_OK)
            {
                if (lasrxNLPHandleToBuffer(ptrInstance,ptrElement, Constants.LASRX_FORMAT_TEXT, ref eszValue) == Constants.LASRX_RETCODE_OK)
                {
                    if (eszValue != null) 
                    {
                        strSummary += ("Semantic Result:");
                        strSummary += (eszValue + "\n");
                        eszValue=null;
                    }
                }
                lasrxNLPReleaseInterpretation(ptrInstance);
            }
        }
        #endregion
    }
}
