using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.SRC.Renderman.Efectos
{
    class Explosion
    {


        TgcSphere esfera;
        float totalTime;
        private static TgcTexture texture;

        private static TgcTexture getTexture()
        {
            if (texture == null)
            {
                texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "RenderMan\\texturas\\fuego.png");
            }
            return texture;
        }


        public Explosion(Vector3 position)
        {

            esfera = new TgcSphere();
            esfera.Position = position;
            esfera.Radius = 0;
            esfera.AlphaBlendEnable = true;
            esfera.setTexture(getTexture());
            esfera.updateValues();
            
        }

        public void render()
        {

            totalTime += GuiController.Instance.ElapsedTime;

            if (esfera.Radius < Juego.Instance.radioExplosion)
            {
                esfera.UVOffset = new Vector2(1f * totalTime, 3f * totalTime);
                esfera.Radius = totalTime * Juego.Instance.radioExplosion / 0.5f;
                esfera.render();
            }
            

        }



    }
}
