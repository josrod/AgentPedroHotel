using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AIEBOT
{
    class IPCDllImport
    {

        protected delegate void manejadorAccion(IntPtr dato, IntPtr otrosDatos);
        protected manejadorAccion myEventAccion;

        // PARAMETROS
        const string dllLocation = "IPC_DLL.dll";

        public Boolean esperando_confirmacion = false;

        #region Importar IPC dll

        [DllImport(dllLocation, EntryPoint = "Iniciar_Ipc")]
        protected static extern void  Iniciar_Ipc(char[] nombreModulo, char[] dirIPCentral);

        [DllImport(dllLocation, EntryPoint = "EnviarTextoReconocido")]
        //protected static extern void EnviarTextoReconocido(char[] nombre, char[] texto);
        protected static extern void EnviarTextoReconocido(string nombre, string texto);


        [DllImport(dllLocation, EntryPoint = "DefinirMensaje")]
        protected static extern void DefinirMensaje(char[] tipoMensaje, char[] tipoDatos);

        //[DllImport(dllLocation, EntryPoint = "suma")]
        //protected static extern int Sumar(int a, int b);

        //[DllImport(dllLocation, EntryPoint = "Terminar_Ipc")]
        //protected static extern void Terminar_Ipc();

        //[DllImport(dllLocation, EntryPoint = "FuncionListen")]
        //protected static extern void FuncionListen();

        //[DllImport(dllLocation, EntryPoint = "EnviarAccionIPC")]
        //protected static extern void EnviarAccionIPC(char* NombreAccion,char* Argumentos);

        //[DllImport(dllLocation, EntryPoint = "PedirListaAcciones")]
        //protected static extern void PedirListaAcciones();

        //[DllImport(dllLocation, EntryPoint = "DLL_Suscribe")]
        //protected static extern void DLL_Suscribe(char[] tipoMensaje, manejadorAccion f, int data, int size);

        //[DllImport(dllLocation, EntryPoint = "DLL_IPC_subscribeConnect")]
        //protected static extern void DLL_IPC_subscribeConnect(ManejadorConexion f,void* data);

        #endregion

        public void Conectar(string nombreModulo, string dirIPCentral)
        {
            //Para la suscripcion de mensajes
            myEventAccion = new manejadorAccion(GetEventAccion);
            Iniciar_Ipc(nombreModulo.ToCharArray(), dirIPCentral.ToCharArray());
            Console.WriteLine("contectado");
        }
        public void Desconectar()
        {
            //Terminar_Ipc();
        }
        
        public void DefinirMensaje(string tipoMensaje, string tipoDatos)
        {
            DefinirMensaje(tipoMensaje.ToCharArray(), tipoDatos.ToCharArray());
        }

        public void EnviarTexto(string nombreASR, string textoASR) 
        {
            if (textoASR != "")
            {
                //EnviarTextoReconocido(nombreASR.ToCharArray(), textoASR.ToCharArray());
                EnviarTextoReconocido(nombreASR, textoASR);
                esperando_confirmacion = true;
                //Console.WriteLine("Texto enviado, esperando confirmacion = true");
            }
            
        }

        #region Eventos

        public void GetEventAccion(IntPtr dato, IntPtr otrosDatos)
        {
            esperando_confirmacion = false;
            //return 0;
        }
         #endregion
    }

}
