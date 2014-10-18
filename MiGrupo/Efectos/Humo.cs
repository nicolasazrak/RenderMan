using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo.Efectos
{
    class Humo
    {

        TgcBox box;
        float totalTime;
        private static TgcTexture texture;

        private static TgcTexture getTexture()
        {
            if (texture == null)
            {
                texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "RenderMan\\texturas\\humo.png");
            }
            return texture;
        }


        public Humo(Vector3 position)
        {

            box = new TgcBox();
            box.UVTiling = new Vector2(300, 300);
            box.setPositionSize(position, new Vector3(50, 0, 50));
            box.setTexture(getTexture());
            box.AlphaBlendEnable = true;
            box.updateValues();
            
        }

        public void render()
        {
            totalTime += GuiController.Instance.ElapsedTime;

            box.Rotation = new Vector3(0.3f * totalTime, totalTime * 0.5f, 0);
            box.Scale = new Vector3(40 * totalTime, 10 * totalTime, 30 * totalTime);
            box.updateValues();
            box.render();
        }


    }
}
