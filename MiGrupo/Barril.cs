using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Barril
    {

        TgcCylinder cilindro;


        /* Ojo con esta clase que probablemente tenga estados cuando tengamos que hacerle el humo */
        public Barril(Vector3 position)
        {
            cilindro = new TgcCylinder(position, 12, 25);
            cilindro.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));
            cilindro.updateValues();
        }

        public TgcBoundingCylinder bounding()
        {
            return cilindro.BoundingCylinder;
        }

        public void render()
        {
            cilindro.render();
        }


        public void dispose()
        {

        }


    }
}
