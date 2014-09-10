using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    class ArmaManager
    {

        private EnemigosManager enemigosManager;
        private SoundManager soundManager;
        private TgcPickingRay pickingRay; //Encargado de chequear si los disparos dieron en el enemigo

        public ArmaManager(EnemigosManager enemigosManager, SoundManager soundManager)
        {
            this.enemigosManager = enemigosManager;
            this.soundManager = soundManager;
            pickingRay = new TgcPickingRay();
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

        }




    }
}
