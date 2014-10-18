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
        public TgcStaticSound sonidoMunicion;
        //public TgcStaticSound sonidoCaminandoIzq, sonidoCaminandoDer;
        public TgcStaticSound sonidoPasoEnemigo;
        public TgcStaticSound sonidoEnemMuerto;
        public TgcStaticSound sonidoDisparo;
        public TgcStaticSound sonidoRecarga;
        public TgcStaticSound sonidoEnemigoAlcanzaPersonaje;
        public TgcStaticSound sonidoAviso;
        public TgcStaticSound sonidoFin;
        private Boolean esPasoIzquierdo;
        private TgcStaticSound sinMunicion;

        private TgcStaticSound pasoIzq;
        private TgcStaticSound pasoDer;

        private DateTime tiempoCordinacionCaminar;
        private TgcStaticSound sonidoBackground;

        public static SoundManager Instance;

        public static SoundManager getInstance()
        {
            if (Instance == null)
            {
                Instance = new SoundManager();
            }
            return Instance;
        }

        private SoundManager()
        {


            pasoIzq = new TgcStaticSound();
            pasoDer = new TgcStaticSound();
            tiempoCordinacionCaminar = DateTime.Now;

            //sonidoCaminandoIzq = new TgcStaticSound();
            //sonidoCaminandoDer = new TgcStaticSound();
            sonidoDisparo = new TgcStaticSound();
            sonidoRecarga = new TgcStaticSound();
            sonidoPasoEnemigo = new TgcStaticSound();
            sonidoEnemMuerto = new TgcStaticSound();
            sonidoEnemigoAlcanzaPersonaje = new TgcStaticSound();
            sonidoBackground = new TgcStaticSound();
            sonidoMunicion = new TgcStaticSound();
            sonidoAviso = new TgcStaticSound();
            sonidoFin = new TgcStaticSound();
            sinMunicion = new TgcStaticSound();

            sinMunicion.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sonidos\\sinMunicion.wav");

            pasoIzq.loadSound (GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sonidos\\pasoIzq.wav");
            pasoDer.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sonidos\\pasoDer.wav");
                 
            //sonidoCaminandoIzq.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada hierba izda.wav");
            //sonidoCaminandoDer.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada hierba dcha.wav");

            sonidoPasoEnemigo.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada hierba dcha.wav");
            sonidoEnemMuerto.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\golpe seco.wav");

            sonidoDisparo.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\50_sniper_shot-Liam-2028603980.wav");
            sonidoRecarga.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\Pump_Shotgun 2x-SoundBible.com-278688366.wav");

            sonidoBackground.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\background-alternativo.wav");
            //sonidoBackground.play();

            sonidoEnemigoAlcanzaPersonaje.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\puñetazo.wav");
            sonidoMunicion.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\tic.wav");
            sonidoAviso.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\sirena, continuo.wav");

            sonidoFin.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\background-alternativo.wav");

            esPasoIzquierdo = true;
        }


        public void sonidoCaminando()
        {
            DateTime tiempoSonido = DateTime.Now;
            
            // se pone esto con el objetivo de que no se pisen los sonidos, sino que antes de ejecutarse espere a que el otro sonido(el del otro paso) termine
            if ((tiempoSonido.Millisecond - tiempoCordinacionCaminar.Millisecond) > 500 || (tiempoSonido.Second != tiempoCordinacionCaminar.Second))
            {
                if (esPasoIzquierdo)
                {
                    //sonidoCaminandoDer.play();
                    pasoIzq.play();
                    esPasoIzquierdo = false;
                   
                }
                else
                {
                    //sonidoCaminandoIzq.play();
                    pasoDer.play();
                    esPasoIzquierdo = true;
                }

                tiempoCordinacionCaminar = DateTime.Now;
            }
        }

        public void playSonidoSinMunicion()
        {
            sinMunicion.play();
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

        public void playSonidoMunicion()
        {
            sonidoMunicion.play();
        }

        public void playSonidoAviso()
        {
            sonidoAviso.play();
        }

        public void playSonidoFin()
        {
            //sonidoFin.play();
        }

        public void stopSonidoFin()
        {
            sonidoFin.dispose();
        }
        public void dispose()
        {
            //sonidoCaminandoIzq.dispose();
            //sonidoCaminandoDer.dispose();
            sonidoPasoEnemigo.dispose();
            sonidoEnemMuerto.dispose();
            sonidoDisparo.dispose();
            sonidoRecarga.dispose();
            sonidoEnemigoAlcanzaPersonaje.dispose();
            sonidoMunicion.dispose();
            sonidoAviso.dispose();
            sinMunicion.dispose();
            //sonidoFin.dispose();

            pasoIzq.dispose();
            pasoDer.dispose();

        }

    }
}
