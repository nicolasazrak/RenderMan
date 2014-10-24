using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

//IMPORTANTE FIJARSE POR QUE SE MUEVE MAL LA NUBE(el metodo mover nube ya esta implementado implicitamente)


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

                listaCopos.Add(new CopoNieve(new Vector3(camaraPos.X + posX, 1000, camaraPos.Z + posZ), i % 4));
            }
            
        }
        
        //mueve la nube
        /*public void movete()
        {
            Vector3 camara = GuiController.Instance.CurrentCamera.getPosition();

            nubeMaxX = camara.X + (anchoX / 2) - nubeMaxX;
            nubeMinX = camara.X - (anchoX / 2) - nubeMinX;
            nubeMaxZ = camara.Z + (anchoZ / 2) - nubeMaxZ;
            nubeMinZ = camara.Z - (anchoZ / 2) - nubeMinZ;
        }*/

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
