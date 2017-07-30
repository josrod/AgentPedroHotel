using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp
{
    public class HelpImg
    {
       protected bool bMsg = false;

        public string SelectImg(string strAtributo,string strValor)
        {
            string sValorName = strValor.ToUpper();
            string sAtributo = strAtributo.ToUpper();

            string sImg="";

            switch (sAtributo)
            {

                case "MAPA":
                {
                    if(sValorName=="HOTEL")
                        sImg=Constantes.ImpMapHotel;
                    break;
                }
                case "PLANO":
                {
                    if (sValorName == "HOTEL")
                        sImg = Constantes.ImgPlanoHotel;
                    else if (sValorName == "RESTAURANTE")
                        sImg = Constantes.ImgPlanoRest;
                    else if (sValorName == "SALAS")
                        sImg = Constantes.ImgPlanoSalas;
                    else if (sValorName == "NIÑOS")
                        sImg = Constantes.ImgPlanoJuegos;
                    else if (sValorName == "TERRAZA")
                        sImg = Constantes.ImgPlanoTerraza;
                    else if (sValorName == "ASCENSOR")
                        sImg = Constantes.ImgPlanoAscen;
                    else if (sValorName == "BAR")
                        sImg = Constantes.ImgPlanoBar;
                    else if (sValorName == "RECEP")
                        sImg = Constantes.ImgPlanoRecep;
                    else
                        sImg = Constantes.ImgPlanoHotel;

                    break;
                }

            }
            return sImg;
        }

        public string SelectMsg()
        {
            string sMsg = "";

            if (bMsg==false)
            {
                sMsg = Constantes.ImpMsgHotel1;
                bMsg = true;
            }
            else
            {
                sMsg = Constantes.ImpMsgHotel2;
                bMsg = false;
            }

            return sMsg;
        }
    }
}
