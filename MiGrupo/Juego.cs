using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo
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

        public void enemigoAscesinado()
        {
            ContadorEnemigos.Instance.enemigoAscesinado();
            enemigosAscesinados++;

            if (enemigosAscesinados == 10)
            {
                distanciaParaPerseguir = 3000;
                cantidadEnemigosActuales = 10;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
            }

            if (enemigosAscesinados == 20)
            {
                distanciaParaPerseguir = 5000;
                cantidadEnemigosActuales = 25;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
            }

            if (enemigosAscesinados == 45)
            {
                distanciaParaPerseguir = 10000;
                cantidadEnemigosActuales = 50;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
            }

        }



    }
}
