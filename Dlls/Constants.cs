using System;
using System.Collections.Generic;
using System.Text;

namespace BotASR
{
    class Constants
    {
        /****************************** Constants Definitions *******************************/
        public const string LANGUAJE = "es-es";
        //public const string INIINFO = "[languages]\n\"load\" = \"#LANGUAGE#\"\n[rp]\n\"path\" = \"../../conf/languages/#LANGUAGE#/builtin/binaries\"\n\"name\" = \"rp_#LANGUAGE#\"\n";
        //public const string RPFOLDER = "../../RP";

        public const string INIINFO = "[languages]\n\"load\" = \"es-es\"\n[rp]\n\"path\" = \"../../RP\"\n\"name\" = \"rp_es-es\"\n";
        public const string RPFOLDER = "../../RP/rp_es-es";
        public const int LASRX_SAMPLES_LIN16 = 4;
        public const int LASRX_FORMAT_TEXT = 1;
        public const int LASRX_FORMAT_XML = 0;
        public const int LASRX_RUNMODE_NON_BLOCKING = 0;
        public const int LASRX_RUNMODE_BLOCKING = 1;
        public const int LASRX_UNITS_FRAMES = 0;
        public const int LASRX_UNITS_SAMPLES = 1;
        public const int LASRX_UNITS_MILLISEC = 2;

        public const int LASRX_EVENT_START_VOICE_DETECTED = 0x00000010;
        public const int LASRX_EVENT_END_VOICE_DETECTED = 0x00000020;
        public const int LASRX_EVENT_PROMPT_STOP = 0x00000040;
        public const int LASRX_EVENT_END_RECOG = 0x00000800;
        
        /****************************** RETURNED ERRORS AND ANOMALIES ***********************/
        public const int LASRX_RETCODE_IN_PROGRESS = 3;
        public const int LASRX_RETCODE_STOPPED = 2;
        public const int LASRX_RETCODE_NO_RESULTS = 1;
        public const int LASRX_RETCODE_ERROR = -1;
        public const int LASRX_RETCODE_OK = 0;
        public const int LASRX_RETCODE_INVALID_HANDLE = -100;
        public const int LASRX_RETCODE_INVALID_STATE = -101;
        public const int LASRX_RETCODE_AUDIO = -102;
        public const int LASRX_RETCODE_AUDIO_TIMEOUT_SAMPLES = -103;
        public const int LASRX_RETCODE_AUDIO_TIMEOUT_STOP = -104;
        public const int LASRX_RETCODE_BAD_PARAMETER = -105;
        public const int LASRX_RETCODE_DECODING = -106;
        public const int LASRX_RETCODE_FUNCTIONALITY_NOT_ENABLED = -107;
        public const int LASRX_RETCODE_WRONG_CONFIGURATION = -108;
        public const int LASRX_RETCODE_NO_MORE_MEMORY = -109;
        public const int LASRX_RETCODE_NO_SEMANTIC = -110;
        public const int LASRX_RETCODE_RESOURCE_READ = -111;
        public const int LASRX_RETCODE_RESOURCE_WRITE = -112;
        public const int LASRX_RETCODE_RESOURCE_TIMEOUT = -113;
        public const int LASRX_RETCODE_RESOURCE_COMPATIBILITY = -114;
        public const int LASRX_RETCODE_LICENSE_MISSING = -115;
        public const int LASRX_RETCODE_LICENSE_INVALID = -116;
        public const int LASRX_RETCODE_LICENSE_BAD = -117;
        public const int LASRX_RETCODE_LICENSE_EXPIRED = -118;
        public const int LASRX_RETCODE_LICENSE_TIER = -119;
        public const int LASRX_RETCODE_LICENSE_OVERFLOW_INSTANCES = -120;
        public const int LASRX_RETCODE_LICENSE_LANGUAGE = -121;
        public const int LASRX_RETCODE_LICENSE_OPTION = -122;
        public const int LASRX_RETCODE_LICENSE_OVERFLOW_OPTION = -123;
        public const int LASRX_RETCODE_TIMEOUT_SILENCE = -124;
        public const int LASRX_RETCODE_TIMEOUT_SPEECH = -125;
        public const int LASRX_RETCODE_OVERFLOW = -126;
        public const int LASRX_RETCODE_OVERFLOW_INSTANCES = -127;
        public const int LASRX_RETCODE_OVERFLOW_LANGUAGES = -128;
        public const int LASRX_RETCODE_OVERFLOW_UTTERANCES = -129;
        public const int LASRX_RETCODE_EPD_MIN_LENGTH = -130;
        public const int LASRX_RETCODE_EPD_MAX_LENGTH = -131;
        public const int LASRX_RETCODE_EPD_MIN_SNR = -132;
        public const int LASRX_RETCODE_EPD_BAD_TRIGGER = -133;
        public const int LASRX_RETCODE_EPD_ON_BEEP = -134;
        public const int LASRX_RETCODE_EPD_CUT_OFF = -135;
        public const int LASRX_RETCODE_EPD_REJECT = -136;
        public const int LASRX_RETCODE_SEMANTIC = -137;
        public const int LASRX_RETCODE_NOT_AVAILABLE = -138;
        public const int LASRX_RETCODE_INTERNAL_ERROR = -139;
        public const int LASRX_RETCODE_INVALID_CONVERSION_REQUEST = -140;
        public const int LASRX_RETCODE_BAD_LANGUAGE = -141;
        public const int LASRX_RETCODE_UNKNOWN_PARAMETER = -142;
    }
}
