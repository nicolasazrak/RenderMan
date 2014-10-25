﻿using Microsoft.DirectX;
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
        private float velocidadCaida;

        //al principio que el tamaño sea variable
        public CopoNieve(Vector3 posicion, int tipoTamanio, int semilla)
        {
            this.setColor(Color.White);

            this.tamanio = tipoTamanio * 3;

            Random varVelocidad = new Random(semilla);
            int variacionVel = varVelocidad.Next(60);
            this.velocidadCaida = tipoTamanio * 150 + variacionVel;

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

                Random posicionX = new Random(DateTime.Now.Millisecond);
                int posX = posicionX.Next((int)nubeMinX, (int)nubeMaxX);
                Random posicionZ = new Random(semillaRandom);
                int posZ = posicionZ.Next((int)nubeMinZ, (int)nubeMaxZ);

                this.Position = new Vector3(camaraPos.X + posX, 1000, camaraPos.Z + posZ);
            }
            else
            {
                //la velocidad hay que verla (osea cual es el criterio que tomamos para las distintas velocidades(200?)
                //se le suma 8 a la posicion de x cuando cae por que simula que el copo lo mueve el viento
                this.Position = new Vector3(this.Position.X + (150 * elapseTime), this.Position.Y - (velocidadCaida * elapseTime), this.Position.Z);
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
