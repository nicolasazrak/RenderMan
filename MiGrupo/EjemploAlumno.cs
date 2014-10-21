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
using TgcViewer.Utils._2D;
using Examples.Optimizacion.Octree;
using AlumnoEjemplos.MiGrupo.Efectos;


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
        ContadorEnemigos contadorEnemigos;
        ContadorBalas contadorBalas;
        Juego juego;
        Indicadores indicadores;
        Octree octree;
        GameOver finalJuego;
        Huellas huella;

        TimeSpan tiempoEsperaHuella;
        //Boolean esIzq;

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

            GuiController.Instance.Modifiers.addFloat("zoom", 2f, 5f, 3f);

            Device d3dDevice = GuiController.Instance.D3dDevice;

            juego = Juego.getInstance();
            //GuiController.Instance.UserVars.addVar("Ancho", tamañoPantalla.Width);
            //GuiController.Instance.UserVars.addVar("Alto", tamañoPantalla.Height);
            vida = Vida.getInstance();
            vida.initialize();

            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;
            
            soundManager = SoundManager.getInstance();
            contadorBalas = ContadorBalas.getInstance();
            escenarioManager = new EscenarioManager();

            escenarioManager.generarPosiciones();
            escenarioManager.generarBosque(500, 200, 20);

            octree = new Octree();
            octree.create(escenarioManager.getOptimizables(), escenarioManager.limites);
            octree.createDebugOctreeMeshes();


            enemigosManager = new EnemigosManager(escenarioManager);
            enemigosManager.generarEnemigos(Juego.Instance.totalEnemigos);

            Juego.Instance.manejoEnemigos(enemigosManager);

            contadorEnemigos = new ContadorEnemigos();

            armaManager = ArmaManager.getInstance(enemigosManager, camara, escenarioManager);

            indicadores = new Indicadores();

            camara.setEscenarioManger(escenarioManager);

            finalJuego = new GameOver();

            huella = new Huellas();
            huella.generarHuella();
            huella.generarHuella();
            tiempoEsperaHuella = DateTime.Now.TimeOfDay;
            //esIzq = true;
       }

        
        public override void render(float elapsedTime)
        {
            if (!Juego.Instance.gameOver)
            {

                Device d3dDevice = GuiController.Instance.D3dDevice;

                if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.C))
                {
                    camara.swapMouseLock();
                }


                if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.W) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.S)
                    || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.D) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.A))
                {

                    Vector3 ultimoLookAt = camara.getLookAt();


                    /* TODO, optimizar aca */
                    Boolean choque = escenarioManager.verificarColision(new TgcBoundingSphere(camara.getPosition(), 20f));
                    if (!choque)
                    {
                        soundManager.sonidoCaminando();
                    }

                    //generar huella----------------------------
                     TimeSpan tiempo = DateTime.Now.TimeOfDay;    

                     if((tiempo.Milliseconds - tiempoEsperaHuella.Milliseconds) > 300 ||(tiempo.Seconds != tiempoEsperaHuella.Seconds) || (tiempo.Minutes != tiempoEsperaHuella.Minutes))
                     {
                         /*if (esIzq)
                         {
                             huella.generarHuella(2);
                             esIzq = false;
                         }
                         else
                         {
                             huella.generarHuella(1);
                             esIzq = true;
                         }*/
                         huella.generarHuella();
                         tiempoEsperaHuella = DateTime.Now.TimeOfDay;
                     
                     }
                    //-----------------------------------------
                }

                huella.renderHuella();

                enemigosManager.update(elapsedTime, vida);
                escenarioManager.update(elapsedTime);
                armaManager.update(elapsedTime);

                vida.render();
                contadorEnemigos.render();
                contadorBalas.render();

                octree.render(GuiController.Instance.Frustum, false);

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
            else
            {
                finalJuego.render();
                soundManager.playSonidoFin();
                TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
                if (d3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.Y))
                {
                    soundManager.stopSonidoFin();
                    Juego.Instance.reiniciar();
                }

            }
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
            finalJuego.dispose();
            huella.dispose();
        }


    }

}
