using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {

        
        
        TgcFpsMiCamara camara;
        EnemigosManager enemigosManager;
        SoundManager soundManager;
        EscenarioManager escenarioManager;
        ArmaManager armaManager;
        List<TgcSkeletalMesh> enemigos = new List<TgcSkeletalMesh>();

        //0 muerto, 1 quieto, 2 corriendo, ¿3 disparando? --> (se vera despues bien)
        int[] estadoEnemigo = new int[3];

        public static String nombreGrupo = "RenderMan";

        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return EjemploAlumno.nombreGrupo;
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "MiIdea - Descripcion de la idea";
        }


        public override void init()
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;

            //enemigosManager = new EnemigosManager();
            //enemigosManager.generarEnemigos(1);

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
                enemigos[t].playAnimation("StandBy", true, 20);
                enemigos[t].Position = new Vector3(-rnd.Next(0, 1500) - 250, 0, -rnd.Next(0, 1500) - 250);
                enemigos[t].Scale = new Vector3(2f, 2f, 2f);

                estadoEnemigo[t] = 1;

            }

            girarEnemigos();

            soundManager = new SoundManager();

            escenarioManager = new EscenarioManager();
            escenarioManager.generarArboles(1);

            armaManager = new ArmaManager(enemigosManager, soundManager, camara);

        }

        private void girarEnemigos()
        {
            foreach (TgcSkeletalMesh e in enemigos)
            {
                Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
                Vector3 dirMirar = e.Position - pos;
                dirMirar.Y = 0;
                e.rotateY((float)Math.Atan2(dirMirar.X, dirMirar.Z) - e.Rotation.Y);
            }
        }

        public override void render(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;
            
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.C))
            {
                camara.swapMouseLock();
            }

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.W) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.S))
            {
                soundManager.sonidoCaminando();
            }


            actualizarEnemigo(elapsedTime);       
            //renderiza tras todos los calculos
            foreach (TgcSkeletalMesh enemigo in enemigos)
                enemigo.render();
            //enemigosManager.update(elapsedTime);
            
            
            escenarioManager.update();
            armaManager.update();

        }

        public void actualizarEnemigo(float elapsedTime)
        {
            girarEnemigos();
            
            int posVector = 0;
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            
            foreach (TgcSkeletalMesh enemigo in enemigos)
            {
                Vector3 dir_escape = enemigo.Position - pos;
                float dist = dir_escape.Length();
                //De estar cerca lo persigue hasta que se le escape
                if (Math.Abs(dist) < 300)
                {
                    estadoEnemigo[posVector] = 2;
                }
                else
                {
                    estadoEnemigo[posVector] = 1;
                }
                //Que accion voy a tomar en base al estado del enemigo
                //estadoEnemigo[posVector] = 2;
                switch (estadoEnemigo[posVector])
                {
                    case 0:
                        break;
                    case 1:
                        enemigo.playAnimation("StandBy", true, 20);
                        break;
                    case 2:
                        
                        //Aca se les dice que hagan el movimiento de correr
                        enemigo.move(dir_escape * (-0.3f * elapsedTime));
                        enemigo.playAnimation("Run", true, 20);
                        break;
                }
                enemigo.updateAnimation();

                posVector++;
            }
        }

        public override void close()
        {
            foreach (TgcSkeletalMesh enemigo in enemigos)
                enemigo.dispose();

            soundManager.dispose();
            enemigosManager.dispose();
            escenarioManager.dispose();
            armaManager.dispose();
        }


    }

}
