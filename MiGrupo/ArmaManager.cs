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



        static Size screenSize = GuiController.Instance.Panel3d.Size;
        static string path = GuiController.Instance.AlumnoEjemplosMediaDir;

        private TgcFpsMiCamara camara;

        private SoundManager soundManager;
        private TgcPickingRay pickingRay; //Encargado de chequear si los disparos dieron en el enemigo

        private TgcSprite sprite;
        private TgcMesh armaMesh;

        private EnemigosManager enemigosManager;

        private Boolean hayZoom = true;
        private TgcTexture miraZoom  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\zoom.png");
        private TgcTexture miraSimple  = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\05.png");
        private TgcTexture sniper = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\SpriteArma5.png");

        public ArmaManager(EnemigosManager enemigosManager, SoundManager soundManager, TgcFpsMiCamara camara)
        {
            //this.enemigosManager = enemigosManager;
            this.soundManager = soundManager;
            this.camara = camara;

            pickingRay = new TgcPickingRay();

            //Crear Sprite
            sprite = new TgcSprite();
            hacerZoom(0);
           
            //Cargo el arma
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\modelos\\arma\\arma.xml");
            armaMesh = scene.Meshes[0];
            armaMesh.Scale = new Vector3(0.005f, 0.005f, 0.005f);

            this.enemigosManager = enemigosManager;
        }


        public void update(float elapsedTime)
        {

            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                soundManager.playSonidoRecarga();
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                manejarDisparo();
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
                armaMesh.render();
            }
            
        }


        private float timeFromZoom = 100;
        private void hacerZoom(float elapsedTime)
        {



            hayZoom = !hayZoom;
            if (hayZoom)
            {
                sprite.Texture = miraZoom;
                sprite.Scaling = new Vector2(0.7f, 0.38f);
            }
            else
            {
                sprite.Texture = sniper;
                //sprite.Texture = miraSimple;
                sprite.Scaling = new Vector2(1f, 1f);
            }

            this.camara.hacerZoom();

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = sprite.Texture.Size;
            sprite.Position = new Vector2(FastMath.Max(screenSize.Width / 2 - textureSize.Width / 2, 0), FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));

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

            soundManager.playSonidoDisparo();
            pickingRay.updateRay();

            bool selected;
            Vector3 collisionPoint;

            //TODO manejar en enemigosManager

            //Testear Ray contra el AABB de todos los meshes
            foreach (Enemigo enemigo in this.enemigosManager.getEnemigos())
            {
               //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, enemigo.getBoundingBox(), out collisionPoint);
                if (selected)
                {
                    enemigo.teDispararon();
                    break;
                }

            }

        }

        public void dispose()
        {
            armaMesh.dispose();
        }




    }
}
