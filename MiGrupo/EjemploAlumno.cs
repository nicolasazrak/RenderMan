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
        HuellasManager huellaManager;
        Nieve nieve;
        Clima clima;

        PostProcesadoManager ppManager;

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
            GuiController.Instance.Modifiers.addBoolean("huellas", "Mostrar huellas", true);
            GuiController.Instance.Modifiers.addBoolean("nieve", "Mostrar nieve", true);

            Device d3dDevice = GuiController.Instance.D3dDevice;

            juego = Juego.getInstance();
            vida = Vida.getInstance();
            vida.initialize();



            ppManager = new PostProcesadoManager(this);

            camara = new TgcFpsMiCamara();
            camara.Enable = true;
            camara.setCamera(new Vector3(-200, 40, 0), new Vector3(0, 10, 0));
            camara.MovementSpeed = 150;
            
            soundManager = SoundManager.getInstance();
            contadorBalas = ContadorBalas.getInstance();
            escenarioManager = new EscenarioManager();

            escenarioManager.generarPosiciones();
            escenarioManager.generarBosque(500, 200, 40);

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

            huellaManager = new HuellasManager(25);

            nieve = new Nieve(3000, 3000, 200);
            clima = new Clima(nieve, soundManager);
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
                    Boolean choque = escenarioManager.verificarColision(new TgcBoundingSphere(camara.getPosition(), 20f));
                    if (!choque)
                    {
                        soundManager.sonidoCaminando();
                    }

                    //generar huella
                    huellaManager.generarHuella(GuiController.Instance.CurrentCamera.getPosition());

                }
                
                update(elapsedTime);

                
               
                //Dibujo todos los sprites de la pantalla pero los indicadores solo cuando no hay zoom 
                GuiController.Instance.Drawer2D.beginDrawSprite();

                armaManager.spriteRender();

                if (!(armaManager.getHayZoom()))
                {
                    indicadores.spriteRender();
                }
                GuiController.Instance.Drawer2D.endDrawSprite();

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
            huellaManager.dispose();
            nieve.disposeNieve();
        }

        public void update(float elapsedTime)
        {
            
            huellaManager.render();

            nieve.renderNieve(elapsedTime);
            clima.alternarClima();

            enemigosManager.update(elapsedTime, vida);
            escenarioManager.update(elapsedTime);
            armaManager.update(elapsedTime);

            vida.render();
            contadorEnemigos.render();
            contadorBalas.render();

            octree.render(GuiController.Instance.Frustum, false);
        }


    }

}
