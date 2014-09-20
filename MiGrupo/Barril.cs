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
        Vector3 position;
        TgcBoundingBox boundingBox;
        bool explotado = false;

        /* Ojo con esta clase que probablemente tenga estados cuando tengamos que hacerle el humo */
        public Barril(Vector3 position)
        {
            cilindro = new TgcCylinder(position, 12, 25);
            this.position = position;
            cilindro.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));
            cilindro.updateValues();
            boundingBox = new TgcBoundingBox(new Vector3(12 + position.X, 0, 12 + position.Z), new Vector3(-12 + position.X, 25, -12 + position.Z));
        }

        public TgcBoundingBox getBoundingBox()
        {
            return boundingBox;
        }

        public Vector3 centro()
        {
            return cilindro.Center;
        }

        public float radio()
        {
            return cilindro.TopRadius;
        }

        public void render()
        {
            if (!explotado)
                cilindro.render();
        }


        public void dispose()
        {

        }

        public void explota()
        {
            this.cilindro = null;
        }

    }
}
