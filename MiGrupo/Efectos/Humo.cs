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

        TgcBox box1;
        TgcBox box2;
        TgcBox box3;
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

            box1 = new TgcBox();
            box1.UVTiling = new Vector2(1, 1);
            box1.setPositionSize(position, new Vector3(200, 0, 200));
            box1.setTexture(getTexture());
            box1.AlphaBlendEnable = true;
            box1.updateValues();

            box2 = new TgcBox();
            box2.UVTiling = new Vector2(1, 1);
            box2.setPositionSize(position, new Vector3(200, 0, 200));
            box2.setTexture(getTexture());
            box2.AlphaBlendEnable = true;
            box2.updateValues();

            box3 = new TgcBox();
            box3.UVTiling = new Vector2(1, 1);
            box3.setPositionSize(position, new Vector3(200, 0, 200));
            box3.setTexture(getTexture());
            box3.AlphaBlendEnable = true;
            box3.updateValues();
            
        }

        public void render()
        {
            totalTime += GuiController.Instance.ElapsedTime;
            if (totalTime < 10)
            {
                box1.Rotation = new Vector3(0.3f * totalTime, 0, 0);
                box1.Size = new Vector3(box1.Size.X + totalTime, box1.Size.Y + totalTime, 0);
                box1.updateValues();

                box2.Rotation = new Vector3(0.1f * totalTime, 0.6f * totalTime, 0);
                box2.Size = new Vector3(box2.Size.X + 1.1f * totalTime, box2.Size.Y + 0.8f * totalTime, 0);
                box2.updateValues();

                box3.Rotation = new Vector3(0.6f * totalTime, 0, 0.1f * totalTime);
                box3.Size = new Vector3(box3.Size.X + 0.5f * totalTime, box3.Size.Y + 0.7f * totalTime, 0);
                box3.updateValues();


                box1.render();
                box2.render();
                box3.render();
            }

        }


    }
}
