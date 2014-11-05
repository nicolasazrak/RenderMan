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
        private TgcStaticSound explosion;
        private TgcStaticSound headshot;
        private TgcStaticSound vientoLigero;
        private TgcStaticSound vientoFuerte;

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
            explosion = new TgcStaticSound();
            headshot = new TgcStaticSound();
            vientoLigero = new TgcStaticSound();
            vientoFuerte = new TgcStaticSound();

            sinMunicion.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sonidos\\sinMunicion.wav");

            pasoIzq.loadSound (GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sonidos\\pasoIzq.wav");
            pasoDer.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sonidos\\pasoDer.wav");

            sonidoPasoEnemigo.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\pisada hierba dcha.wav");
            sonidoEnemMuerto.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\golpe seco.wav");

            sonidoDisparo.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\sonidoDisparo.wav");

            sonidoRecarga.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\armas\\Pump_Shotgun 2x-SoundBible.com-278688366.wav");

            sonidoBackground.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\background-alternativo.wav");
            sonidoBackground.play();

            vientoLigero.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\Pure Arctic Wind Corto.wav");
            vientoLigero.play();

            vientoFuerte.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\Ventizca Fuerte.wav");

            sonidoEnemigoAlcanzaPersonaje.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\puñetazo.wav");
            sonidoMunicion.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\tic.wav");
            sonidoAviso.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\sirena, continuo.wav");

            sonidoFin.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\background-alternativo.wav");

            explosion.loadSound(GuiController.Instance.ExamplesMediaDir + "\\Sound\\explosión, grande.wav");
            headshot.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\" + EjemploAlumno.nombreGrupo + "\\sonidos\\headshot.wav");

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
                    pasoIzq.play();
                    esPasoIzquierdo = false;
                   
                }
                else
                {
                    pasoDer.play();
                    esPasoIzquierdo = true;
                }

                tiempoCordinacionCaminar = DateTime.Now;
            }
        }

        public void playVentizcaFuerte()
        {
            this.vientoLigero.stop();
            this.vientoFuerte.play();
        }

        public void playVentizcaLigera()
        {
            this.vientoFuerte.stop();
            this.vientoLigero.play();
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

        public void playSonidoExplosion()
        {
            explosion.play();
        }

        public void playHeadShot()
        {
            headshot.play();
        }

        public void stopSonidoFin()
        {
            sonidoFin.dispose();
        }
        public void dispose()
        {
            sonidoPasoEnemigo.dispose();
            sonidoEnemMuerto.dispose();
            sonidoDisparo.dispose();
            sonidoRecarga.dispose();
            sonidoEnemigoAlcanzaPersonaje.dispose();
            sonidoMunicion.dispose();
            sonidoAviso.dispose();
            sinMunicion.dispose();
            //sonidoFin.dispose();
            vientoLigero.dispose();
            vientoFuerte.dispose();
            sonidoBackground.dispose();

            pasoIzq.dispose();
            pasoDer.dispose();

        }

    }
}
