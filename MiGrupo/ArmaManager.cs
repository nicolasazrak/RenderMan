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
        TimeSpan tiempoEsperaDisparo; 

        static Size screenSize = GuiController.Instance.Panel3d.Size;
        static string path = GuiController.Instance.AlumnoEjemplosMediaDir;

        private TgcFpsMiCamara camara;

        private SoundManager soundManager;
        private TgcPickingRay pickingRay; //Encargado de chequear si los disparos dieron en el enemigo

        private TgcSprite sprite;
        private TgcMesh armaMesh;

        private EnemigosManager enemigosManager;
        private EscenarioManager escenarioManager;

        private Boolean hayZoom = true;
        private TgcTexture miraZoom  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\zoom.png");
        //private TgcTexture miraSimple  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\05.png");
        private TgcTexture sniper = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\SpriteArma5.png");

        public ArmaManager(EnemigosManager enemigosManager, SoundManager soundManager, TgcFpsMiCamara camara, EscenarioManager escenarioManager)
        {
            //this.enemigosManager = enemigosManager;
            this.soundManager = soundManager;
            this.camara = camara;
            this.escenarioManager = escenarioManager;
            this.enemigosManager = enemigosManager;

            pickingRay = new TgcPickingRay();

            //Crear Sprite
            sprite = new TgcSprite();
            hacerZoom(0);
           
            //Cargo el arma
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\modelos\\arma\\arma.xml");
            armaMesh = scene.Meshes[0];
            armaMesh.Scale = new Vector3(0.005f, 0.005f, 0.005f);

            tiempoDisparo = DateTime.Now.TimeOfDay;
    
        }


        public void update(float elapsedTime)
        {

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                soundManager.playSonidoRecarga();
                ContadorBalas.Instance.recagar();
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                tiempoEsperaDisparo = DateTime.Now.TimeOfDay;
                TimeSpan resultado = tiempoEsperaDisparo - tiempoDisparo;
                if (resultado.Seconds >= 1 || resultado.Minutes > 1)
                {
                    tiempoDisparo = DateTime.Now.TimeOfDay;
                    manejarDisparo();
                }

            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_RIGHT) == true)
            {
                hacerZoom(elapsedTime);
            }


            GuiController.Instance.Drawer2D.beginDrawSprite();
            sprite.render();
            GuiController.Instance.Drawer2D.endDrawSprite();

            if (!hayZoom)
            {
                actualizarPosicionArma();
                //armaMesh.render();
            }
            
        }


        private float timeFromZoom = 100;
        private void hacerZoom(float elapsedTime)
        {
           
            hayZoom = !hayZoom;
            if (hayZoom)
            {
                sprite.Texture = miraZoom;
            }
            else
            {
                sprite.Texture = sniper;
                //sprite.Texture = miraSimple;
            }

            this.camara.hacerZoom();

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = sprite.Texture.Size;

            sprite.Scaling = new Vector2(ajustarTexturaAPantalla(screenSize.Width,textureSize.Width), ajustarTexturaAPantalla(screenSize.Height,textureSize.Height));

            sprite.Position = new Vector2(0, 0);
        }

        public float ajustarTexturaAPantalla(int pantallaParametro, int texturaParametro)
        {   
            //para poder hacer la divicion bien
            float pantalla = pantallaParametro;
            float textura = texturaParametro;
            return (pantalla / textura);
        }

        public TgcBoundingBox BoundinBox()
        {
            return armaMesh.BoundingBox;
        }

        public Vector3 posicionArma()
        {
            Vector3 pos = new Vector3();
            pos = armaMesh.Position;
            return pos;
        }

        public void actualizarPosArma()
        {
            //Hay que poner segun cada arma la distancia a la camara.
            Vector3 camaraPos = camara.getPosition();
            Vector3 mirandoA = camara.getLookAt();
            //armaMesh.Rotation = mirandoA;
            armaMesh.Rotation = new Vector3(0, 3.14159f * 0.5f, 0);
            armaMesh.Position = new Vector3(camaraPos.X + 4f, camaraPos.Y - 2f, camaraPos.Z - 1.5f);
        }

        private void actualizarPosicionArma()
        {
            //Hay que poner segun cada arma la distancia a la camara.
            Vector3 camaraPos = camara.getPosition();
            Vector3 mirandoA = camara.getLookAt();
            //armaMesh.Rotation = mirandoA;
            armaMesh.Rotation = new Vector3(0, 3.14159f * 0.5f, 0);
            armaMesh.Position = new Vector3(camaraPos.X + 4f, camaraPos.Y - 2f, camaraPos.Z - 1.5f);
        }

        public void manejarDisparo()
        {
            if (ContadorBalas.Instance.puedoDisparar())
            {
                soundManager.playSonidoDisparo();
                pickingRay.updateRay();

                Vector3 collisionPoint;

                foreach (TgcMesh barril in this.escenarioManager.getBarriles())
                {
                    if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, barril.BoundingBox, out collisionPoint))
                    {
                        //barril.explota();
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
            armaMesh.dispose();
        }




    }
}
