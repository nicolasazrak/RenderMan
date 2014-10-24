using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo.Efectos
{
    class CopoNieve : TgcSphere
    {
        private float tamanio;

        //al principio que el tamaño sea variable
        public CopoNieve(Vector3 posicion, int tipoTamanio)
        {
            this.setColor(Color.White);

            this.tamanio = tipoTamanio * 10;
            
            this.setPositionRadius(new Vector3(posicion.X,1000,posicion.Z),tamanio);

            this.updateValues();
        }

        public void setPosicion(Vector3 posicionNueva)
        {
            this.setPosicion(posicionNueva);
        }

        public void cae(float elapseTime,float nubeMinX,float nubeMaxX,float nubeMinZ,float nubeMaxZ,int semillaRandom)
        {
            if (this.Position.Y < 0)
            {
                Vector3 camaraPos = GuiController.Instance.CurrentCamera.getPosition();

                Random posicionX = new Random(semillaRandom);
                int posX = posicionX.Next((int)nubeMinX, (int)nubeMaxX);
                Random posicionZ = new Random(semillaRandom);
                int posZ = posicionX.Next((int)nubeMinZ, (int)nubeMaxZ);

                this.Position = new Vector3(camaraPos.X + posX, 1000, camaraPos.Z + posZ);
            }
            else
            {
                this.Position = new Vector3(this.Position.X, this.Position.Y - (50 * elapseTime), this.Position.Z);
            }

            this.updateValues();
        }

        public void renderBola()
        {
            this.render();
        }

        public void disposeBola()
        {
            this.dispose();
        }
    }
}
