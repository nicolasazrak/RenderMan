﻿using AlumnoEjemplos.MiGrupo;
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

        public void enemigoAscesinado()
        {
            ContadorEnemigos.Instance.enemigoAscesinado();
            enemigosAscesinados++;

            if (enemigosAscesinados == 10)
            {
                distanciaParaPerseguir = 300;
                cantidadEnemigosActuales = 10;
                totalEnemigos += cantidadEnemigosActuales;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
            }

            if (enemigosAscesinados == 20)
            {
                distanciaParaPerseguir = 500;
                cantidadEnemigosActuales = 25;
                totalEnemigos += cantidadEnemigosActuales;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
            }

            if (enemigosAscesinados == 45)
            {
                distanciaParaPerseguir = 1000;
                cantidadEnemigosActuales = 50;
                totalEnemigos += cantidadEnemigosActuales;
                EnemigosManager.Instance.generarEnemigos(cantidadEnemigosActuales);
            }

            

        }
    }
}
