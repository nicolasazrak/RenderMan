using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    class EnemigoPersiguiendo : EnemigoEstado
    {
        TimeSpan tiempoDaño;
        TimeSpan tiempoEsperaGolpe;

        public EnemigoPersiguiendo(Enemigo enemigo) : base(enemigo) { 
            tiempoDaño = DateTime.Now.TimeOfDay;
        }

        public override bool debeGirar()
        {
            return true;
        }

        public override void update(float elapsedTime, Vida vidaPersona)
        {

            girar(); 

            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dir_escape = enemigo.mesh.Position - pos;
            float dist = dir_escape.Length();
            dir_escape.Y = 0;

            TgcBoundingBox algo = enemigo.mesh.BoundingBox;
            Vector3 posAnterior = enemigo.mesh.Position;

            //Falta verificar las colisiones
            if (Math.Abs(dist) > Juego.Instance.distanciaParaPerseguir)
            {
                enemigo.mesh.Position = enemigo.getPosAnterior();
                enemigo.setEstado(new EnemigoQuieto(enemigo));
            }
            else
            {
                if (!(enemigo.escenarioManager.verificarColision(algo)))
                {
                    enemigo.setPosAnterior(enemigo.mesh.Position);
                    //Aca se les dice que hagan el movimiento de correr
                    enemigo.mesh.move(dir_escape * (-0.5f * elapsedTime));
                    enemigo.mesh.playAnimation("Run", true, 20);
                    //soundManager.sonidoCaminandoEnemigo();

                    //Verificar que no lo golpee tan rapido
                    int milisegundosEspera =  Juego.Instance.esperaDañoMilisegundos;
                    if (Math.Abs(dist) < 60 && Juego.Instance.esperaCorrecta(tiempoDaño, -1, 1, milisegundosEspera))
                    {
                        tiempoDaño = DateTime.Now.TimeOfDay;
                        vidaPersona.restaAtaqueEnemigo();
                        //Random rnd = new Random();
                       // Vector3 posNueva = elegirNuevaPosicion(dist, enemigo);
                        //enemigo.setPosicion(new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250));
                    }
                }
                else
                {
                    enemigo.mesh.Position = enemigo.getPosAnterior();
                }
            }
            

            enemigo.mesh.updateAnimation();


        }

        private Vector3 elegirNuevaPosicion (float distancia, Enemigo enemigo)
        {
            Vector3 posNueva = new Vector3();
            Random rnd = new Random();

            while (Math.Abs(distancia) < Juego.Instance.distanciaParaPerseguir)
            {
                posNueva = new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250);
            }

            return posNueva;

        }

        public override void teDispararon()
        {
            enemigo.setEstado(new EnemigoMuerto(enemigo));
        }

    }
}
