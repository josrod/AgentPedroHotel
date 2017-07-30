using System;
using System.Collections.Generic;
using System.Text;

namespace Encuesta
{
 
    class Preguntas
    {
        public List<string> frases;
        public List<string> survey;
        
        public Preguntas()
        {
            frases = new List<string>();
            survey = new List<string>();

            frases.Add("Cual es tu experiencia previa con avatares virtuales como yo");
            frases.Add("Como te ha parecido mi vocabulario o las frases que utilizo para comunicarme");
            //frases.Add("Como valoras mi capacidad para entenderte");
            //frases.Add("Como Valoras mi capacidad para darte información");
            //frases.Add("Como valoras mi velocidad de respuesta");
            //frases.Add("Como valoras la sencillez de mi sistema");
            //frases.Add("Como valoras la funcionalidad que te ofresco");
            //frases.Add("Como me valorarías en general");
            //frases.Add("¿Usarías este sistema en un lugar convencional?");
            //frases.Add("¿He respondido a tus expectativas previas?");

            survey.Add("Cual es tu sexo: ¿Femenino o Masculino?");
            survey.Add("Cual es tu edad: Menos de 20, Entre 20 y 40, Entre 40 y 60, o Mas de 60");
            survey.Add("Valora tu experiencia en el manejo del ordenador");
            
        }
    }
}
