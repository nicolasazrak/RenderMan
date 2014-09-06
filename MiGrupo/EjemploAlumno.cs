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

        TgcBox box;
        TgcBox piso;
        TgcStaticSound sonidoDisparo;
        TgcStaticSound sonidoRecarga;
        TgcFpsMiCamara camara;
        EnemigosManager enemigosManager;
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

            //Crear caja vacia
            box = new TgcBox();
            box.setPositionSize(new Vector3(0, 5, 0), new Vector3(10, 10, 10));
            box.updateValues();
            box.setTexture(TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\madera.jpg"));

            piso = new TgcBox();
            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(100, 0, 100));
            piso.updateValues();
            piso.setTexture(TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));


            sonidoDisparo = new TgcStaticSound();
            sonidoDisparo.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\sonidos\\armas\\50_sniper_shot-Liam-2028603980.wav");
            sonidoRecarga = new TgcStaticSound();
            sonidoRecarga.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "\\sonidos\\armas\\Pump_Shotgun 2x-SoundBible.com-278688366.wav");  


            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-30, 10, 0), new Vector3(0, 10, 0));

            enemigosManager = new EnemigosManager();
            enemigosManager.generarEnemigos(1);


        }


        public override void render(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            //Renderizar caja
            box.render();
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
                //sonidoRecarga.play();
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                //sonidoDisparo.play();
            }

            enemigosManager.render(elapsedTime);

        }


        public override void close()
        {
            box.dispose();
            piso.dispose();
            sonidoDisparo.dispose();
            sonidoRecarga.dispose();
        }


    }

}
