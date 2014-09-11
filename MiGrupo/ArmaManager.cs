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

namespace AlumnoEjemplos.MiGrupo
{
    class ArmaManager
    {

        static Size screenSize = GuiController.Instance.Panel3d.Size;
        static string path = GuiController.Instance.AlumnoEjemplosMediaDir;

        private TgcFpsMiCamara camara;

        private EnemigosManager enemigosManager;
        private SoundManager soundManager;
        private TgcPickingRay pickingRay; //Encargado de chequear si los disparos dieron en el enemigo

        private TgcSprite sprite;
        private TgcMesh armaMesh;

        public ArmaManager(EnemigosManager enemigosManager, SoundManager soundManager, TgcFpsMiCamara camara)
        {
            this.enemigosManager = enemigosManager;
            this.soundManager = soundManager;
            this.camara = camara;

            pickingRay = new TgcPickingRay();

            //Crear Sprite
            sprite = new TgcSprite();
            sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\sprites\\mira.png");

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = sprite.Texture.Size;
            sprite.Position = new Vector2(FastMath.Max(screenSize.Width / 2 - textureSize.Width / 2, 0), FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));

            //Cargo el arma
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + EjemploAlumno.nombreGrupo + "\\modelos\\arma\\arma.xml");
            armaMesh = scene.Meshes[0];
            armaMesh.Scale = new Vector3(0.005f, 0.005f, 0.005f);
        
        }


        public void update()
        {
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                soundManager.playSonidoRecarga();
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                manejarDisparo();
            }


            GuiController.Instance.Drawer2D.beginDrawSprite();
            sprite.render();
            GuiController.Instance.Drawer2D.endDrawSprite();

            actualizarPosicionArma();
            armaMesh.render();

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

        private void manejarDisparo()
        {
            soundManager.playSonidoDisparo();
            pickingRay.updateRay();

            bool selected;
            Vector3 collisionPoint;
            Enemigo enemigoDisparado = null;

            //Testear Ray contra el AABB de todos los meshes
            foreach (Enemigo enemigo in enemigosManager.enemigos)
            {

                //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, enemigo.boundingBox, out collisionPoint);
                if (selected)
                {
                    enemigoDisparado = enemigo;
                    break;
                }
            }

            if (enemigoDisparado != null)
            {
                enemigosManager.murio(enemigoDisparado);
            }

        }

        public void dispose()
        {
            armaMesh.dispose();
        }




    }
}
