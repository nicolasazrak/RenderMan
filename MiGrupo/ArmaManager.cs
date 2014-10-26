using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    class ArmaManager
    {
        TimeSpan tiempoDisparo;
        TimeSpan tiempoRecarga;
        TimeSpan tiempoZoom;

        static Size screenSize = GuiController.Instance.Panel3d.Size;
        static string path = GuiController.Instance.AlumnoEjemplosMediaDir;

        private TgcFpsMiCamara camara;

        private SoundManager soundManager;
        private TgcPickingRay pickingRay; //Encargado de chequear si los disparos dieron en el enemigo

        private TgcSprite spriteArma;
        private TgcSprite spriteMira;

        //-----------variables para las animaciones de las armas---------
        private Boolean empezoAnimacionDisparo;
        private Boolean volviendoAnimacion;
        private float posicionSpriteAnimacion;

        private Boolean volviendoAnimacionRecarga;
        private Boolean empezoAnimacionRecarga;
        private float posicionSpriteAnimacionRecarga;
        private TimeSpan tiempoInicioAnimacion; //variable que se utiliza para frenar la imagen cuando esta recargando, la baja la frena y despues la sube
        //------------------------------------------------------

        private EnemigosManager enemigosManager;
        private EscenarioManager escenarioManager;

        private Boolean hayZoom = true;
        private TgcTexture miraZoom  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\zoom.png");
        private TgcTexture miraSimple  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\SpriteMiraSola.png");
        private TgcTexture sniper = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\SpriteArmaSola.png");


        public static ArmaManager Instance;
        public static ArmaManager getInstance(EnemigosManager enemigosManager, TgcFpsMiCamara camara, EscenarioManager escenarioManager)
        {
            if (Instance == null)
            {
                Instance = new ArmaManager(enemigosManager, camara, escenarioManager);
            }
            return Instance;
        }

        private ArmaManager(EnemigosManager enemigosManager, TgcFpsMiCamara camara, EscenarioManager escenarioManager)
        {

            //this.enemigosManager = enemigosManager;
            this.soundManager = SoundManager.getInstance();
            this.camara = camara;
            this.escenarioManager = escenarioManager;
            this.enemigosManager = enemigosManager;

            pickingRay = new TgcPickingRay();

            //Crear Sprite
            spriteArma = new TgcSprite();
            spriteMira = new TgcSprite();

            setInitialValues();
    
        }

        public void setInitialValues()
        {
            empezoAnimacionDisparo = false;
            volviendoAnimacion = false;
            volviendoAnimacionRecarga = false;

            hacerZoom(0);

            tiempoDisparo = DateTime.Now.TimeOfDay;
            tiempoRecarga = DateTime.Now.TimeOfDay;
            tiempoZoom = DateTime.Now.TimeOfDay;
        }


        public void update(float elapsedTime)
        {

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                if (Juego.Instance.esperaCorrecta(tiempoRecarga, -1, 4, 1) && ContadorBalas.Instance.puedoRecargar() && !(empezoAnimacionDisparo))
                {
                    tiempoRecarga = DateTime.Now.TimeOfDay;
                    soundManager.playSonidoRecarga();
                    ContadorBalas.Instance.recargar();
                    empezoAnimacionRecarga = true;
                    tiempoInicioAnimacion = DateTime.Now.TimeOfDay;
                }
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                if (Juego.Instance.esperaCorrecta(tiempoDisparo, -1, 1, 1) && !(empezoAnimacionRecarga))
                {
                    manejarDisparo();
                }
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_RIGHT) == true)
            {
                if (Juego.Instance.esperaCorrecta(tiempoZoom, 500, 1, 1))
                {
                    tiempoZoom = DateTime.Now.TimeOfDay;
                    hacerZoom(elapsedTime);
                }
            }

            if (!(hayZoom)) 
            { // solo te mueva el sprite cuando no tenes mira
                if (empezoAnimacionDisparo && !(empezoAnimacionRecarga))// para que no se pisen las animaciones
                {
                    this.playAnimacionDisparo(elapsedTime);
                }

                if (empezoAnimacionRecarga && !(empezoAnimacionDisparo)) //para que no se pisen las animaciones
                {
                    this.playAnimacionRecarga(elapsedTime);
                }
            }
        }

        public void playAnimacionRecarga(float elapsedTime)
        {
            float limite = 1.45f;
            int tiempoTranscurridoAnimacionSeg = DateTime.Now.Second - tiempoInicioAnimacion.Seconds;
            int tiempoTranscurridoAnimacionMin = DateTime.Now.Minute - tiempoInicioAnimacion.Minutes;
            float pendienteDeLaRecta = 150f;

            if (empezoAnimacionRecarga)
            {
                if (posicionSpriteAnimacionRecarga <= limite && !(volviendoAnimacionRecarga))
                {
                    spriteArma.Position = new Vector2(spriteArma.Position.X + (5 * elapsedTime), spriteArma.Position.Y + (5 * elapsedTime * pendienteDeLaRecta));

                    posicionSpriteAnimacionRecarga = spriteArma.Position.X + (5 * elapsedTime);
                }
                else
                {
                    if((tiempoTranscurridoAnimacionSeg > 2 && tiempoTranscurridoAnimacionMin == 0) || (tiempoTranscurridoAnimacionMin == -1)) //parte donde frena la secuencia para simular que esta cargando
                    {
                        volviendoAnimacionRecarga = true;
                        spriteArma.Position = new Vector2(spriteArma.Position.X - (5 * elapsedTime), spriteArma.Position.Y - (5 * elapsedTime * pendienteDeLaRecta));

                        posicionSpriteAnimacionRecarga = spriteArma.Position.X - (5 * elapsedTime);

                        if (spriteArma.Position.X < 0)
                        {
                             spriteArma.Position = new Vector2(0, 0);
                             empezoAnimacionRecarga = false;
                             volviendoAnimacionRecarga = false;
                        }
                    }
                }
            }
        }
        
        public void playAnimacionDisparo(float elapsedTime)
        {
            float limite = 150;
            float pendienteDeLaRecta = 0.45f;

            if (empezoAnimacionDisparo)
            {
                if (posicionSpriteAnimacion <= limite && !(volviendoAnimacion))
                {
                    spriteArma.Position = new Vector2(spriteArma.Position.X + (1000 * elapsedTime), spriteArma.Position.Y + (1000 * elapsedTime * pendienteDeLaRecta));

                    posicionSpriteAnimacion = spriteArma.Position.X + (1000 * elapsedTime);
                }
                else
                {
                    volviendoAnimacion = true;
                    spriteArma.Position = new Vector2(spriteArma.Position.X - (200 * elapsedTime), spriteArma.Position.Y - (200 * elapsedTime * pendienteDeLaRecta));

                    posicionSpriteAnimacion = spriteArma.Position.X - (200 * elapsedTime);

                    if (spriteArma.Position.X < 0)
                    {
                        spriteArma.Position = new Vector2(0, 0);
                        empezoAnimacionDisparo = false;
                        volviendoAnimacion = false;
                    }
                }
            }
        }

        public void spriteRender()
        {
            spriteArma.render();
            spriteMira.render();
        }

        public bool getHayZoom()
        {
            return hayZoom;
        }

        private float timeFromZoom = 100;
        private void hacerZoom(float elapsedTime)
        {
           
            hayZoom = !hayZoom;
            if (hayZoom)
            {
                spriteArma.Texture = miraZoom;
            }
            else
            {
                spriteArma.Texture = sniper;
                spriteMira.Texture = miraSimple;
            }

            this.camara.hacerZoom();

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = spriteArma.Texture.Size;

            spriteMira.Scaling = new Vector2(ajustarTexturaAPantalla(screenSize.Width, textureSize.Width), ajustarTexturaAPantalla(screenSize.Height, textureSize.Height));
            spriteArma.Scaling = new Vector2(ajustarTexturaAPantalla(screenSize.Width,textureSize.Width), ajustarTexturaAPantalla(screenSize.Height,textureSize.Height));

            spriteArma.Position = new Vector2(0, 0);
            spriteMira.Position = new Vector2(0, 0);
        }

        public float ajustarTexturaAPantalla(int pantallaParametro, int texturaParametro)
        {   
            //para poder hacer la divicion bien
            float pantalla = pantallaParametro;
            float textura = texturaParametro;
            return (pantalla / textura);
        }



        public void manejarDisparo()
        {
            if (ContadorBalas.Instance.puedoDisparar())
            {
                soundManager.playSonidoDisparo();
                //----animacion------
                empezoAnimacionDisparo = true; //NO sacar, boleano que dice que empezo la animacion, se desactiva cuando halla terminado la animacion y se pone ya que la animacion para completarse necesita muchas iteraciones del update!!
                tiempoDisparo = DateTime.Now.TimeOfDay;
                //----------------
                pickingRay.updateRay();

                Vector3 collisionPoint;

                foreach (Barril barril in EscenarioManager.Instance.getBarriles())
                {
                    if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, barril.BoundigBox, out collisionPoint))
                    {
                        barril.explota();
                        break;
                    }
                }

                //Testear Ray contra el AABB de todos los meshes
                float t;
                foreach (Enemigo enemigo in this.enemigosManager.getEnemigos())
                {
                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    if ((enemigo.debeVerificarDispoaro() && TgcCollisionUtils.intersectRaySphere(pickingRay.Ray, enemigo.getCabeza(), out t, out collisionPoint)))
                    {
                        enemigo.headShot();
                        break;
                    }
                }

                //Testear Ray contra el AABB de todos los meshes
                foreach (Enemigo enemigo in this.enemigosManager.getEnemigos())
                {
                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    if (enemigo.debeVerificarDispoaro() && TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, enemigo.getBoundingBox(), out collisionPoint))
                    {
                        enemigo.teDispararon();
                        break;
                    }
                }


                ContadorBalas.Instance.huboDisparo();

                // propio de la animacion -----------------------
                if (!hayZoom)
                {
                    empezoAnimacionDisparo = true;
                    posicionSpriteAnimacion = 0;
                }
                //---------------------------------------

            }
            else
            {
                //Encontrar sonido que represente la falta de balas en el arma(ENCONTRADO)
                soundManager.playSonidoSinMunicion();
            }

        }

        public void dispose()
        {
            spriteArma.dispose();
            spriteMira.dispose();
        }

    }
}
