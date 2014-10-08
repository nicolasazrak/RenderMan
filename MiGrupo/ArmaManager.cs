﻿using Microsoft.DirectX;
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

        private TgcSprite sprite;

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


        }

        public void spriteRender()
        {
            sprite.render();
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



        public void manejarDisparo()
        {
            if (ContadorBalas.Instance.puedoDisparar())
            {
                soundManager.playSonidoDisparo();
                pickingRay.updateRay();

                Vector3 collisionPoint;

                foreach (TgcMesh barril in EscenarioManager.Instance.getBarriles())
                {
                    if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, barril.BoundingBox, out collisionPoint))
                    {
                        EscenarioManager.Instance.explotaBarril(barril);
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
            sprite.dispose();
        }




    }
}
