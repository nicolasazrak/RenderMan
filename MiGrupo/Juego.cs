using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos
{
    class Juego
    {

        EnemigosManager eManager;


        private int enemigosAscesinados;
        public int distanciaParaPerseguir = 1500;
        public int cantidadEnemigosActuales = 10;
        public int totalEnemigos = 10;
        public int cantidadBalas = 10;
        public int esperaDañoMilisegundos = 500;
        public int cantidadDeCargadores = 3;
        public int radioExplosion = 270;
        public int recuperoVida = 5;
        public Boolean gameOver = false;


        public static Juego Instance;
        public static Juego getInstance()
        {
            if (Instance == null)
            {
                Instance = new Juego();
            }
            return Instance;
        }

        private Juego() { }

        public void enemigoAscesinado()
        {

            ContadorEnemigos.Instance.enemigoAscesinado();
            enemigosAscesinados++;
            Boolean pasodeNivel = false;

            switch (enemigosAscesinados) {

                case 0:
                    distanciaParaPerseguir = 1000;
                    cantidadEnemigosActuales = 10;
                    esperaDañoMilisegundos = 400;
                    recuperoVida = 10;
                    pasodeNivel = true;
                    break;
                case 10:
                    distanciaParaPerseguir = 2000;
                    cantidadEnemigosActuales = 15;
                    esperaDañoMilisegundos = 400;
                    recuperoVida = 10;
                    pasodeNivel = true;
                    break;
                case 25:
                    distanciaParaPerseguir = 2500;
                    cantidadEnemigosActuales = 25;
                    esperaDañoMilisegundos = 300;
                    recuperoVida = 20;
                    pasodeNivel = true;
                    break;

                case 50:
                case 100:
                case 150:
                case 200:
                    distanciaParaPerseguir = 3500;
                    cantidadEnemigosActuales = 50;
                    recuperoVida = 25;
                    pasodeNivel = true;
                    break;
            }


            if (pasodeNivel)
            {
                ContadorEnemigos.Instance.reiniciarContador();
                EnemigosManager.Instance.pasarNivel();
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                totalEnemigos += cantidadEnemigosActuales;
            }
            

        }

        public void agarroVida()
        {
            Vida.Instance.subirVida();
        }

        public Boolean esperaCorrecta (TimeSpan tiempoInicial, int mili, int seg, int min)
        {
            TimeSpan tiempoEspera = DateTime.Now.TimeOfDay;
            TimeSpan resultado = tiempoEspera - tiempoInicial;
            if (mili > 0)
            {
                return (resultado.Milliseconds > mili || resultado.Seconds >= seg || resultado.Minutes > min);
            }
            else
            {
                return (resultado.Seconds >= seg || resultado.Minutes > min);
            }
        }

        public void murioPersonaje()
        {
            gameOver = true;
        }

        public void reiniciar()
        {

            gameOver = false;

            Vida.Instance.setInitialValues();

            GuiController.Instance.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));

            EnemigosManager.Instance.generarEnemigos(Juego.Instance.totalEnemigos);

            ArmaManager.Instance.setInitialValues();

            ContadorBalas.Instance.setInitialValues();

            ContadorEnemigos.Instance.setInitialValues();

        }

        public void manejoEnemigos(EnemigosManager enemigo)
        {
            eManager = enemigo;
        }


        public int cantidadTiposEnemgiosCreados { get; set; }

        public int animacionesDeEnemigos { get; set; }



        /* Setea los valores del inicio para el juego */
        /* Antes estaban en EjemploAlumno.cs */
        public void init()
        {

        }


        public void headshot()
        {
            SoundManager.Instance.playHeadShot();
        }
    }
}
