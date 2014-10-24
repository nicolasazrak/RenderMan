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

            TgcBoundingCylinder cilindroMasCercano = enemigo.escenarioManager.colisionAdistancia(enemigo.enemigoEsfera);
            if (cilindroMasCercano != null)
            {

                Vector3 puntoMasCercano = TgcCollisionUtils.closestPointCylinder(enemigo.enemigoEsfera.Center, cilindroMasCercano);

                Vector3 segmento = puntoMasCercano - enemigo.enemigoEsfera.Center;
                Vector3 tangente = new Vector3(segmento.Z, 0, segmento.X);


                if (Vector3.Cross(segmento, dir_escape).Y <= 0)
                {
                    tangente.Z = tangente.Z * (-1);
                }
                else
                {
                    tangente.X = tangente.X * (-1);
                }


                tangente.Normalize();

                enemigo.mesh.move(tangente * 50 * elapsedTime);
                enemigo.enemigoEsfera.moveCenter(new Vector3(tangente.X * 50 * elapsedTime, 0, tangente.Z * 50 * elapsedTime));
            }
            else
            {
                enemigo.setPosAnterior(enemigo.mesh.Position);
                //Aca se les dice que hagan el movimiento de correr
                enemigo.mesh.move(dir_escape * (-0.4f * elapsedTime));
                enemigo.enemigoEsfera.moveCenter(dir_escape * (-0.4f * elapsedTime));
                enemigo.mesh.playAnimation("Run", true, 20);
                //soundManager.sonidoCaminandoEnemigo();

                //Verificar que no lo golpee tan rapido
                int milisegundosEspera = Juego.getInstance().esperaDañoMilisegundos;
                if (Math.Abs(dist) < 100 && Juego.getInstance().esperaCorrecta(tiempoDaño, -1, 1, milisegundosEspera))
                {
                    enemigo.setEstado(new EnemigoGolpeando(enemigo));
                    //Random rnd = new Random();
                    // Vector3 posNueva = elegirNuevaPosicion(dist, enemigo);
                    //enemigo.setPosicion(new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250));
                }
            }
            

            enemigo.mesh.updateAnimation();


        }

        /* Se le pasa una lista de cilindros colisionables, y devuelve el mas cercano */
        /* Usado para ver por donde esquivar al arbol y barril */
        public TgcBoundingCylinder detectarMasCercano(List<TgcBoundingCylinder> cilindros)
        {
            float distMinimo = 0;
            TgcBoundingCylinder cilindroMasCercano = new TgcBoundingCylinder(new Vector3(0, 0, 0), 0, 0);
            foreach (TgcBoundingCylinder cilindro in cilindros)
            {
                Vector3 puntoCilindro = TgcCollisionUtils.closestPointCylinder(enemigo.enemigoEsfera.Center, cilindro);
                Vector3 distV = puntoCilindro - enemigo.enemigoEsfera.Center;
                if (distMinimo > Math.Abs(distV.Length()))
                {
                    distMinimo = distV.Length();
                    cilindroMasCercano = cilindro;
                }
            }
            return cilindroMasCercano;

        }

        private Vector3 elegirNuevaPosicion (float distancia, Enemigo enemigo)
        {
            Vector3 posNueva = new Vector3();
            Random rnd = new Random();

            while (Math.Abs(distancia) < Juego.getInstance().distanciaParaPerseguir)
            {
                posNueva = new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250);
            }

            return posNueva;

        }

        public override void teDispararon()
        {
            enemigo.setEstado(new EnemigoMuriendo(enemigo));
        }

        public override void explotoBarril(Vector3 posicion)
        {
            enemigo.setEstado(new EnemigoMuriendo(enemigo));
        }


        public override void headShot()
        {
            SoundManager.Instance.playHeadShot();
            teDispararon();
        }

        public override bool debeVerificarDispoaro()
        {
            return true;
        }

    }
}
