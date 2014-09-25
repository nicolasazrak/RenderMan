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
        public int distanciaParaPerseguir = 300;
        public int cantidadEnemigosActuales = 10;
        public int totalEnemigos = 10;
        public int cantidadBalas = 10;
        public int esperaDañoMilisegundos = 500;
        public int cantidadDeCargadores = 3;


        public void enemigoAscesinado()
        {
            ContadorEnemigos.Instance.enemigoAscesinado();
            enemigosAscesinados++;

            if (enemigosAscesinados == 10)
            {
                distanciaParaPerseguir = 300;
                cantidadEnemigosActuales = 15;
                totalEnemigos += cantidadEnemigosActuales;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                ContadorEnemigos.Instance.reiniciarContador();
                esperaDañoMilisegundos = 400;
            }

            if (enemigosAscesinados == 25)
            {
                distanciaParaPerseguir = 500;
                cantidadEnemigosActuales = 25;
                totalEnemigos += cantidadEnemigosActuales;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                ContadorEnemigos.Instance.reiniciarContador();
                esperaDañoMilisegundos = 300;
            }

            if (enemigosAscesinados == 50)
            {
                distanciaParaPerseguir = 1000;
                cantidadEnemigosActuales = 50;
                totalEnemigos += cantidadEnemigosActuales;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
                ContadorEnemigos.Instance.reiniciarContador();
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
