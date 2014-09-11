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

            enemigosManager = new EnemigosManager();
            enemigosManager.generarEnemigos(1);

            soundManager = new SoundManager();

            escenarioManager = new EscenarioManager();
            escenarioManager.generarArboles(1);

            armaManager = new ArmaManager(enemigosManager, soundManager);

        }


        public override void render(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;
            
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.C))
            {
                camara.swapMouseLock();
            }

            enemigosManager.update(elapsedTime);
            escenarioManager.update();
            armaManager.update();

        }


        public override void close()
        {
            soundManager.dispose();
            enemigosManager.dispose();
            escenarioManager.dispose();
            armaManager.dispose();
        }


    }

}
