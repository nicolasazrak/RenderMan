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
        //------------------------------------------------------

        private EnemigosManager enemigosManager;
        private EscenarioManager escenarioManager;

        private Boolean hayZoom = true;
        private TgcTexture miraZoom  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\zoom.png");
        private TgcTexture miraSimple  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\SpriteMiraSola.png");
        private TgcTexture sniper = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\SpriteArmaSola.png");

        public ArmaManager(EnemigosManager enemigosManager, SoundManager soundManager, TgcFpsMiCamara camara, EscenarioManager escenarioManager)
        {
            //this.enemigosManager = enemigosManager;
            this.soundManager = soundManager;
            this.camara = camara;
            this.escenarioManager = escenarioManager;
            this.enemigosManager = enemigosManager;

            pickingRay = new TgcPickingRay();

            //Crear Sprite
            spriteArma = new TgcSprite();
            spriteMira = new TgcSprite();

            empezoAnimacionDisparo = false;
            volviendoAnimacion = false;

            hacerZoom(0);
           
            tiempoDisparo = DateTime.Now.TimeOfDay;
            tiempoRecarga = DateTime.Now.TimeOfDay;
            tiempoZoom = DateTime.Now.TimeOfDay;
    
        }


        public void update(float elapsedTime)
        {

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                if (Juego.Instance.esperaCorrecta(tiempoRecarga, -1, 4, 1) && ContadorBalas.Instance.puedoRecargar())
                {
                    tiempoRecarga = DateTime.Now.TimeOfDay;
                    soundManager.playSonidoRecarga();
                    ContadorBalas.Instance.recargar();
                }
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                if (Juego.Instance.esperaCorrecta(tiempoDisparo, -1, 1, 1))
                {
                    tiempoDisparo = DateTime.Now.TimeOfDay;
                    manejarDisparo();
                    
                    // propio de la animacion -----------------------
                    empezoAnimacionDisparo = true;
                    posicionSpriteAnimacion = 0;
                    //---------------------------------------
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
                this.playAnimacionDisparo(elapsedTime);
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
                foreach (Enemigo enemigo in this.enemigosManager.getEnemigos())
                {
                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, enemigo.getBoundingBox(), out collisionPoint))
                    {
                        enemigo.teDispararon();
                        break;
                    }
                }


                ContadorBalas.Instance.huboDisparo();

            }
            else
            {
                //Encontrar sonido que represente la falta de balas en el arma
            }

        }

        public void dispose()
        {
            spriteArma.dispose();
            spriteMira.dispose();
        }




    }
}
