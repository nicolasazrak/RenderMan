﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.SRC.Renderman.Efectos
{
    class Clima
    {
        //clase que controla el estado de la tormenta alternando los sonidos y la tormenta
        private Nieve tormenta;
        private TimeSpan inicioClimaActual; //se utiliza para alternar los climas
        private int climaActual; //lleva el registro de que clima hay que cambiar(1=ligero,2= fuerte)
        private SoundManager sonido;

        public Clima(Nieve nieve,SoundManager parametroSonido)
        {
            //modifiers que activa la alternacion de climas
            GuiController.Instance.Modifiers.addBoolean("Alternar Viento", "activar", true);

            sonido = parametroSonido;
            tormenta = nieve;
            inicioClimaActual = DateTime.Now.TimeOfDay;
            climaActual = 1;
        }

        public void alternarClima()
        {
            TimeSpan tiempoActual = DateTime.Now.TimeOfDay;
            if ((tiempoActual.Minutes - inicioClimaActual.Minutes) > 1 || (tiempoActual.Minutes < 3 && tiempoActual.Hours != inicioClimaActual.Hours))
            {
                if (climaActual == 2)
                {
                    tormenta.moverNube(800); // la definicion de porque lo puse esta en el metodo
                    //hacer el cambio de las variables
                    tormenta.cambiarParametrosClima(0, 0);
                    sonido.playVentizcaLigera();
                    climaActual = 1;
                }
                else
                {
                    if (climaActual == 1 && (Boolean)GuiController.Instance.Modifiers["Alternar Viento"])
                    {
                        tormenta.moverNube(-800);
                        //hacer el cambio de las variables
                        tormenta.cambiarParametrosClima(240, 560);
                        sonido.playVentizcaFuerte();
                        climaActual = 2;
                    }
                }
                
                inicioClimaActual = tiempoActual;
            }
        }
    }
}
