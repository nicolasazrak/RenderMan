using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo.Efectos
{
    class Huellas
    {
        List<TgcBox> huellas;
        
        TgcBox huellaIzq;
        TgcBox huellaDer;
        TgcBox huella;
        Vector3 posicionAnterio;
        int tipoDeHuella;

        public Huellas()
        {
            huellas = new List<TgcBox>();
            posicionAnterio = GuiController.Instance.CurrentCamera.getPosition();
            tipoDeHuella = 1;
        }

        //si es 1 es huella Derecha si es 2 es huella izq
        public void generarHuella()
        {
            Vector3 posicion = GuiController.Instance.CurrentCamera.getPosition();
            
            huella = new TgcBox();
            huella.AlphaBlendEnable = true;
            huella.Size = new Vector3(20, 0, 20);
            huella.Position = new Vector3(posicion.X, 1, posicion.Z);

            if (tipoDeHuella == 1)
            {
                huella.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\texturas\\pisadaDer2.png"));
                tipoDeHuella = 2;
            }
            else
            {
                huella.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\texturas\\pisadaIzq2.png"));
                tipoDeHuella = 1;
            }

            Vector3 mirarHacia = posicion - posicionAnterio;
            mirarHacia.Y = 0;
            huella.rotateY((float)Math.Atan2(mirarHacia.X, mirarHacia.Z) - huella.Rotation.Y);

            huella.updateValues();

            if (huellas.Count > 30)
            {
                TgcBox huellaABorrar = huellas.ElementAt(1);
                huellas.RemoveAt(1);
                huellaABorrar.dispose();
            }

            huellas.Add(huella);

            posicionAnterio = posicion;
        }

        public void renderHuella() 
        {
            foreach (TgcBox huella in huellas)
            {
               huella.updateValues();
               huella.render();
            }
        }

        public void dispose()
        {
            foreach (TgcBox huella in huellas)
            {
                huella.dispose();
            }
        }

    }
}
