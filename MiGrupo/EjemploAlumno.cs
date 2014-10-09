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
        Octree octree;
        Boolean gameOver;
        public static EjemploAlumno Instance;
        GameOver finalJuego;


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
            vida.initialize(this);

            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;
            ultimaPosicion = new Vector3(-200, 40, 0);

            soundManager = new SoundManager();
            contadorBalas = new ContadorBalas(juego.cantidadBalas);

            escenarioManager = new EscenarioManager(vida);
            escenarioManager.generarPosiciones();
            escenarioManager.generarBosque(500, 200, 20);

            octree = new Octree();
            octree.create(escenarioManager.getOptimizables(), escenarioManager.limites);
            octree.createDebugOctreeMeshes();

            enemigosManager = new EnemigosManager(escenarioManager, soundManager);
            enemigosManager.generarEnemigos(juego.totalEnemigos);

            juego.manejoEnemigos(enemigosManager);

            contadorEnemigos = new ContadorEnemigos(10);

            armaManager = new ArmaManager(enemigosManager, soundManager, camara, escenarioManager);

            indicadores = new Indicadores();

            GuiController.Instance.Modifiers.addFloat("zoom", 2f, 5f, 3f);

            camara.setEscenarioManger(escenarioManager);

            gameOver = false;

            EjemploAlumno.Instance = this;

            finalJuego = new GameOver();
       }

        
        public override void render(float elapsedTime)
        {
            if (!gameOver)
            {

                Device d3dDevice = GuiController.Instance.D3dDevice;

                if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.C))
                {
                    camara.swapMouseLock();
                }


                if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.W) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.S)
                    || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.D) || GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.A))
                {

                    Vector3 ultimaPos = camara.getPosition();
                    Vector3 ultimoLookAt = camara.getLookAt();

                    TgcBoundingSphere arma = new TgcBoundingSphere(ultimaPos, 20f);

                    Boolean choque = escenarioManager.verificarColision(arma);
                    if (!choque)
                    {
                        soundManager.sonidoCaminando();
                        ultimaPosicion = ultimaPos;
                    }

                }

                enemigosManager.update(elapsedTime, escenarioManager, vida);
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
                    this.reiniciarJuego();
                }

            }
        }

        private void reiniciarJuego()
        {
            juego = new Juego();
            vida = new Vida();
            vida.initialize(this);
            ultimaPosicion = new Vector3(-200, 40, 0);
            contadorBalas = new ContadorBalas(juego.cantidadBalas);
            escenarioManager = new EscenarioManager(vida);
            escenarioManager.generarPosiciones();
            escenarioManager.generarBosque(500, 200, 20);
            
            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;
            octree = new Octree();
            octree.create(escenarioManager.getOptimizables(), escenarioManager.limites);
            octree.createDebugOctreeMeshes();

            enemigosManager = new EnemigosManager(escenarioManager, soundManager);
            enemigosManager.generarEnemigos(juego.totalEnemigos);

            juego.manejoEnemigos(enemigosManager);
            
            contadorEnemigos = new ContadorEnemigos(10);

            armaManager = new ArmaManager(enemigosManager, soundManager, camara, escenarioManager);

            indicadores = new Indicadores();

            camara.setEscenarioManger(escenarioManager);

            gameOver = false;

       }

        public void murioPersonaje()
        {
            gameOver = true;
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
        }


    }

}
