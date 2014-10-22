using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo.Efectos
{
    class Huella : TgcBox
    {
        //si es 0 es huella Derecha si es 1 o otro es huella izq
        public Huella(int tipoDeHuella)
        {
            Vector3 posicion = GuiController.Instance.CurrentCamera.getPosition();
            
            this.AlphaBlendEnable = true;
            this.Size = new Vector3(20, 0, 20);
            this.Position = new Vector3(posicion.X, 1, posicion.Z);

            if (tipoDeHuella == 0)
            {
                this.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\texturas\\pisadaDer2.png"));
            }
            else
            {
                this.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\texturas\\pisadaIzq2.png"));
            }

            this.updateValues();
        }

        public void cambiaDeLugar(Vector3 vecPosicion, Vector3 vecRotacion)
        {
            //roto la huella
            this.rotateY((float)Math.Atan2(vecRotacion.X, vecRotacion.Z) - this.Rotation.Y);
            
            //muevo la huella
            this.Position = new Vector3(vecPosicion.X, 1 ,vecPosicion.Z);

            this.updateValues();
        }

        public void renderHuella()
        {
            this.render();
        }

        public void disposeHuella()
        {
            this.dispose();
        }
    }
}
