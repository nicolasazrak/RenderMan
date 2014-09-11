using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Sound;

namespace AlumnoEjemplos.MiGrupo
{
    class SoundManager
    {

        public TgcStaticSound sonidoCaminandoIzq, sonidoCaminandoDer;
        public TgcStaticSound sonidoDisparo;
        public TgcStaticSound sonidoRecarga;

        public SoundManager()
        {
            sonidoCaminandoIzq = new TgcStaticSound();
            sonidoCaminandoDer = new TgcStaticSound();
            sonidoCaminandoIzq.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada crujido izda.wav");

            sonidoDisparo = new TgcStaticSound();
            sonidoDisparo.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\50_sniper_shot-Liam-2028603980.wav");
            sonidoRecarga = new TgcStaticSound();
            sonidoRecarga.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\Pump_Shotgun 2x-SoundBible.com-278688366.wav");  
        }


        public void sonidoCaminando()
        {
            //TODO ir variando de izquierda a derecha
            //sonidoCaminandoIzq.play();
        }

        public void playSonidoRecarga()
        {
            sonidoRecarga.play();
        }

        public void playSonidoDisparo()
        {
            sonidoDisparo.play();
        }


        public void dispose()
        {
            sonidoCaminandoIzq.dispose();
            sonidoDisparo.dispose();
            sonidoRecarga.dispose();
        }



    }
}
