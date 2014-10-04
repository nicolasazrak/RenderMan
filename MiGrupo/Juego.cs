using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos
{
    class Juego
    {

        public static Juego Instance;

        public Juego()
        {
            Juego.Instance = this;
        }

        private int enemigosAscesinados;
        public int distanciaParaPerseguir = 2000;
        public int cantidadEnemigosActuales = 10;
        public int totalEnemigos = 10;
        public int cantidadBalas = 10;
        public int esperaDañoMilisegundos = 500;
        public int cantidadDeCargadores = 3;
        public int radioExplosion = 100;
        public int recuperoVida = 5;

        public void enemigoAscesinado()
        {
            ContadorEnemigos.Instance.enemigoAscesinado();
            enemigosAscesinados++;

            switch (enemigosAscesinados) {
                case 10:
                    distanciaParaPerseguir = 3000;
                    cantidadEnemigosActuales = 15;
                    totalEnemigos += cantidadEnemigosActuales;
                    EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                    ContadorEnemigos.Instance.reiniciarContador();
                    esperaDañoMilisegundos = 400;
                    recuperoVida = 10;
                    break;
                case 25:
                    distanciaParaPerseguir = 5000;
                    cantidadEnemigosActuales = 25;
                    totalEnemigos += cantidadEnemigosActuales;
                    EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                    ContadorEnemigos.Instance.reiniciarContador();
                    esperaDañoMilisegundos = 300;
                    recuperoVida = 20;
                    break;
                case 50:
                    distanciaParaPerseguir = 9000;
                    cantidadEnemigosActuales = 50;
                    totalEnemigos += cantidadEnemigosActuales;
                    EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                    ContadorEnemigos.Instance.reiniciarContador();
                    recuperoVida = 25;
                    break;
            }

            

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

    }
}
