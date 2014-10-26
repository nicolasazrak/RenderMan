using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo.Efectos
{
    class Nieve
    {

        private List<CopoNieve> listaCopos;
        private float nubeMaxX;
        private float nubeMaxZ;
        private float nubeMinX;
        private float nubeMinZ;
        private float anchoX;
        private float anchoZ;

        public Nieve(int largoX, int largoZ, int cantidadCopos)
        {
            //Modifiers para variar los parametros de la tormenta
            GuiController.Instance.Modifiers.addInt("Viento", 0, 800, 150);
            GuiController.Instance.Modifiers.addInt("Velocidad caida Nieve",0,400, 145);
            
            listaCopos = new List<CopoNieve>();

            Vector3 camaraPos = GuiController.Instance.CurrentCamera.getPosition();
            
            //seteo el tamaño de la nube
            Vector3 camara = GuiController.Instance.CurrentCamera.getPosition();
            anchoX = largoX;
            anchoZ = largoZ;

            nubeMaxX = camara.X + (anchoX / 2);
            nubeMinX = camara.X - (anchoX / 2);
            nubeMaxZ = camara.Z + (anchoZ / 2);
            nubeMinZ = camara.Z - (anchoZ / 2);

            //cargo los copos
            for (int i = 0; i < cantidadCopos; i++)
            {
                //genero una posicion random dentro de la nube
                Random posicionX = new Random(i);
                int posX = posicionX.Next((int) nubeMinX,(int) nubeMaxX);
                Random posicionZ = new Random(i);
                int posZ = posicionX.Next((int)nubeMinZ, (int)nubeMaxZ);

                listaCopos.Add(new CopoNieve(new Vector3(camaraPos.X + posX, 1000, camaraPos.Z + posZ), i % 4, i,this));
            }          
        }

        public int obtenerVelCaidaMod()
        {
            return (int)GuiController.Instance.Modifiers["Velocidad caida Nieve"];
        }

        public int obtenerVelViento()
        {
            return (int)GuiController.Instance.Modifiers["Viento"];
        }

        public void renderNieve(float elapseTime)
        {
            int i = 1;
            foreach (CopoNieve bola in listaCopos)
            {
                bola.cae(elapseTime,nubeMinX,nubeMaxX,nubeMinZ,nubeMaxZ,i);
                bola.renderBola();
                i++;
            }
        }

        public void disposeNieve()
        {
            foreach (CopoNieve bola in listaCopos)
            {
                bola.disposeBola();
            }
        }

    }
}
