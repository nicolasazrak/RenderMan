using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using TgcViewer.Utils.TgcGeometry;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Sound;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TgcViewer.Utils.Modifiers;


namespace AlumnoEjemplos.MiGrupo
{
    class EnemigosManager
    {

        List<TgcSkeletalMesh> enemigos;

        private SoundManager soundManager;

        //0 muerto, 1 quieto, 2 corriendo, ¿3 disparando? --> (se vera despues bien)
        int[] estadoEnemigo = new int[3];
        //va a tener las posiciones previas para usar en caso de colisiones


        public EnemigosManager()
        {
            enemigos = new List<TgcSkeletalMesh>();
        }

        public void init (EscenarioManager escenario)
        {

            soundManager = new SoundManager();
            TgcSkeletalLoader enemigo = new TgcSkeletalLoader();
            Random rnd = new Random();

            for (int t = 0; t < 3; ++t)
            {
                enemigos.Add(enemigo.loadMeshAndAnimationsFromFile(
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "CombineSoldier-TgcSkeletalMesh.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                    new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                }));

                //Configurar animacion inicial
                enemigos[t].playAnimation("StandBy", true);

                Vector3 pos = new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250);
                enemigos[t].Position = pos;
                enemigos[t].Scale = new Vector3(1f, 1f, 1f);

                estadoEnemigo[t] = 1;

            }

            girarEnemigos();

        }

        private void girarEnemigos()
        {

            int posicion = 0;

            foreach (TgcSkeletalMesh e in enemigos)
            {
                if (estadoEnemigo[posicion] != 0)
                {
                    Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
                    Vector3 dirMirar = e.Position - pos;
                    dirMirar.Y = 0;
                    e.rotateY((float)Math.Atan2(dirMirar.X, dirMirar.Z) - e.Rotation.Y);
                }

                posicion++;
            }

        }


        //<summary>
        //Se va a llamar una sola vez para crear todos los enemigos y darles una posicion inicial
        //</summary>

        public void actualizarEnemigo(float elapsedTime, EscenarioManager e)
        {

            girarEnemigos();

            int posVector = 0;
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();

            foreach (TgcSkeletalMesh enemigo in enemigos)
            {

                Vector3 dir_escape = enemigo.Position - pos;
                float dist = dir_escape.Length();
                dir_escape.Y = 0;

                //Solamente lo va a evaluar en caso que este vivo
                if (estadoEnemigo[posVector] != 0)
                {
                    //De estar cerca lo persigue hasta que se le escape
                    if (Math.Abs(dist) < 300)
                    {
                        estadoEnemigo[posVector] = 2;
                    }
                    else
                    {
                        estadoEnemigo[posVector] = 1;
                    }

                }

                //Que accion voy a tomar en base al estado del enemigo
                //estadoEnemigo[posVector] = 2;
                switch (estadoEnemigo[posVector])
                {

                    case 0:
                        enemigo.rotateX(3.1415f * 0.5f - enemigo.Rotation.X);
                        //Subo un poco al muerto asi no queda cortado por el piso al acostarse
                        Vector3 posicionMuerto = enemigo.Position;
                        posicionMuerto.Y = 8;
                        enemigo.Position = posicionMuerto;
                        enemigo.playAnimation("StandBy", true);
                        //soundManager.sonidoEnemigoMuerto();
                        break;

                    case 1:

                        enemigo.playAnimation("StandBy", true);
                        break;

                    case 2:

                        TgcBoundingBox algo = enemigo.BoundingBox;
                        Vector3 posAnterior = enemigo.Position;
                        
                        if (!(e.verificarColision(algo)))
                        {
                            //Aca se les dice que hagan el movimiento de correr
                            enemigo.move(dir_escape * (-0.5f * elapsedTime));
                            enemigo.playAnimation("Run", true, 20);
                            soundManager.sonidoCaminandoEnemigo();
                            break;
                        }

                        break;

                }
                
                enemigo.updateAnimation();

                posVector++;
            }

        }

        public void manejarDisparo (ArmaManager arma)
        {
            arma.manejarDisparo(enemigos, estadoEnemigo);
        }

        //<summary>
        //Llama al metodo render de cada enemigo que haya
        //</summary>
        public void update(float elapsedTime, EscenarioManager e)
        {
            actualizarEnemigo(elapsedTime, e);
            foreach (TgcSkeletalMesh enemigo in enemigos)
            {
                enemigo.render();
            }
        }

        public void dispose()
        {
            foreach (TgcSkeletalMesh enemigo in enemigos)
                enemigo.dispose();
        }


        internal void murio(Enemigo enemigoDisparado)
        {

        }


    }
}
