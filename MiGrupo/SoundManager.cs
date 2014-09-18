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
        public TgcStaticSound sonidoPasoEnemigo;
        public TgcStaticSound sonidoEnemMuerto;
        public TgcStaticSound sonidoDisparo;
        public TgcStaticSound sonidoRecarga;
        public TgcStaticSound sonidoEnemigoAlcanzaPersonaje;
        public Boolean esPasoDerecho;

        public SoundManager()
        {
            sonidoCaminandoIzq = new TgcStaticSound();
            sonidoCaminandoDer = new TgcStaticSound();
            sonidoDisparo = new TgcStaticSound();
            sonidoRecarga = new TgcStaticSound();
            sonidoPasoEnemigo = new TgcStaticSound();
            sonidoEnemMuerto = new TgcStaticSound();
            sonidoEnemigoAlcanzaPersonaje = new TgcStaticSound();

            sonidoCaminandoIzq.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada maleza izda.wav");
            sonidoCaminandoDer.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada maleza dcha.wav");

            sonidoPasoEnemigo.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada hierba dcha.wav");
            sonidoEnemMuerto.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\golpe seco.wav");

            sonidoDisparo.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\50_sniper_shot-Liam-2028603980.wav");
            sonidoRecarga.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\Pump_Shotgun 2x-SoundBible.com-278688366.wav");

            sonidoEnemigoAlcanzaPersonaje.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\puñetazo.wav");

            esPasoDerecho = true;
        }


        public void sonidoCaminando()
        {
            if (esPasoDerecho)
            {
                sonidoCaminandoDer.play();
            }
            else
            {
                sonidoCaminandoIzq.play();
            }
        }

        public void sonidoCaminandoEnemigo()
        {
            sonidoPasoEnemigo.play();
        }

        public void sonidoEnemigoMuerto()
        {
            sonidoEnemMuerto.play();
        }

        public void playSonidoRecarga()
        {
            sonidoRecarga.play();
        }

        public void playSonidoDisparo()
        {
            sonidoDisparo.play();
        }

        public void playSonidoJugadorAlcanzado()
        {
            sonidoEnemigoAlcanzaPersonaje.play();
        }

        public void dispose()
        {
            sonidoCaminandoIzq.dispose();
            sonidoCaminandoDer.dispose();
            sonidoPasoEnemigo.dispose();
            sonidoEnemMuerto.dispose();
            sonidoDisparo.dispose();
            sonidoRecarga.dispose();
            sonidoEnemigoAlcanzaPersonaje.dispose();
        }



    }
}
