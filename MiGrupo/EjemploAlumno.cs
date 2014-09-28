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
        Vida vida;
        Vector3 ultimaPosicion;
        ContadorEnemigos contadorEnemigos;
        ContadorBalas contadorBalas;
        Juego juego;
        Indicadores indicadores;
        //Size tamañoPantalla = GuiController.Instance.Panel3d.Size;
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
            return "Simple juego FPS. Controles: Clic - Disparar, Clic derecho - Zoom, C - Liberar el mouse, A,W,S,D - Moverse, R - Recargar arma";
        }
        #endregion

        public override void init()
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            juego = new Juego();
            //GuiController.Instance.UserVars.addVar("Ancho", tamañoPantalla.Width);
            //GuiController.Instance.UserVars.addVar("Alto", tamañoPantalla.Height);
            vida = new Vida();
            vida.initialize();

            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;
            ultimaPosicion = new Vector3(-200, 40, 0);

            soundManager = new SoundManager();

            escenarioManager = new EscenarioManager();
            escenarioManager.generarPosiciones();
           escenarioManager.generarBosque(500, 200, 20);
           /* escenarioManager.generarArboles(80);
            escenarioManager.generarPasto(200);
            escenarioManager.generarBarriles(10);
            */
            enemigosManager = new EnemigosManager(escenarioManager, soundManager);
            enemigosManager.generarEnemigos( 10);

            contadorEnemigos = new ContadorEnemigos(10);
            
            contadorBalas = new ContadorBalas(juego.cantidadBalas);

            armaManager = new ArmaManager(enemigosManager, soundManager, camara, escenarioManager);

            indicadores = new Indicadores();       
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

            enemigosManager.update(elapsedTime, escenarioManager, vida);          
            escenarioManager.update();
            armaManager.update(elapsedTime);
            vida.render();
            contadorEnemigos.render();
            contadorBalas.render();

            //Dibujo todos los sprites de la pantalla pero los indicadores solo cuando no hay zoom ---------------------
            GuiController.Instance.Drawer2D.beginDrawSprite();
           
            armaManager.spriteRender();

            if (!(armaManager.getHayZoom()))
            {
                indicadores.spriteRender();
            }
            GuiController.Instance.Drawer2D.endDrawSprite();
            //------------------------------------------------------------
        }

         public override void close()
        {
            enemigosManager.dispose();
            soundManager.dispose();
            enemigosManager.dispose();
            escenarioManager.dispose();
            armaManager.dispose();
            vida.dispose();
            indicadores.dispose();
        }


    }

}
