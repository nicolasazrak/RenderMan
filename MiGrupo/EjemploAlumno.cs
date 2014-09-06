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

        TgcBox piso;
        
        TgcFpsMiCamara camara;
        EnemigosManager enemigosManager;
        SoundManager soundManager;

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
            return "Grupo 99";
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

            piso = new TgcBox();
            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(200, 0, 200));
            piso.updateValues();
            piso.setTexture(TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));


            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;

            enemigosManager = new EnemigosManager();
            enemigosManager.generarEnemigos(1);

            soundManager = new SoundManager();

        }


        public override void render(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            piso.render();

            /*
             Para que esto no se nos vuelva un quilombo, aca solo capturemos las teclas y enviemosle el mensaje al objeto que corresponda
             */

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.C))
            {
                camara.swapMouseLock();
            }

    
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                soundManager.playSonidoRecarga();
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                soundManager.playSonidoDisparo();
            }

            enemigosManager.render(elapsedTime);

        }


        public override void close()
        {
            piso.dispose();
            soundManager.dispose();
            enemigosManager.dispose();
        }


    }

}
