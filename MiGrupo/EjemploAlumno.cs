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
        Vector3 ultimaPosicion;

        #region datosTP
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
            return "Simple juego FPS. Controles: Clic - Disparar, Clic derecho - Zoom, C - Liberar el mouse, A,W,S,D - Moverse";
        }
        #endregion

        public override void init()
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;
            ultimaPosicion = new Vector3(-200, 40, 0);

            soundManager = new SoundManager();

            escenarioManager = new EscenarioManager();
            escenarioManager.generarArboles(80);
            escenarioManager.generarPasto(200);
            escenarioManager.generarBarriles(10);

            enemigosManager = new EnemigosManager(escenarioManager, soundManager);
            enemigosManager.generarEnemigos(10);

            armaManager = new ArmaManager(enemigosManager, soundManager, camara);

        }


        public override void render(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;
            
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.C))
            {
                camara.swapMouseLock();
            }

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.W) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.S)
                || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.D) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.A))
            {
                Vector3 posArma = armaManager.posicionArma();
                Vector3 ultimaPos = camara.getPosition();
                Vector3 ultimoLookAt = camara.getLookAt();
                TgcBoundingBox arma = armaManager.BoundinBox();

                Boolean choque = escenarioManager.verificarColision(arma);
                if (!choque)
                {
                    soundManager.sonidoCaminando();
                    ultimaPosicion = ultimaPos;
                }
                else
                {
                    camara.setCamera(ultimaPosicion, ultimoLookAt);
                    armaManager.actualizarPosArma();
                }
            }

            enemigosManager.update(elapsedTime, escenarioManager);          
            escenarioManager.update();
            armaManager.update();

        }

         public override void close()
        {
            enemigosManager.dispose();

            soundManager.dispose();
            enemigosManager.dispose();
            escenarioManager.dispose();
            armaManager.dispose();
        }


    }

}
