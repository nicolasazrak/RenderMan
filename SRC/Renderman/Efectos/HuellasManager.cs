using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.SRC.Renderman.Efectos
{
    class HuellasManager
    {

        private int indexDeHuellaAMover;
        private Vector3 posicionAnterio;
        private List<Huella> listaHuellas;
        private TimeSpan tiempoEsperaHuella;

        public HuellasManager(int cantidadHuellas)
        {
            listaHuellas = new List<Huella>();
            posicionAnterio = GuiController.Instance.CurrentCamera.getPosition();
            tiempoEsperaHuella = DateTime.Now.TimeOfDay;

            indexDeHuellaAMover = 0;

            //cargo las huellas en el vector
            for (int i = 0; i < cantidadHuellas; i++)
            {
                listaHuellas.Add(new Huella( i % 2));
            }

        }


        public void generarHuella(Vector3 posicion)
        {
            TimeSpan tiempo = DateTime.Now.TimeOfDay;  
            
            //compara el tiempo de ahora con el de la ultima huella para ver si genera o no
            if((tiempo.Milliseconds - tiempoEsperaHuella.Milliseconds) > 300 ||(tiempo.Seconds != tiempoEsperaHuella.Seconds) || (tiempo.Minutes != tiempoEsperaHuella.Minutes))
            {
            
                //calculo la rotacion de la nueva huella en base a la nueva posicion
                Vector3 posicionActual = posicion;

                Vector3 vectorRotacion = posicionActual - posicionAnterio;
                vectorRotacion.Y = 0;

                listaHuellas.ElementAt(indexDeHuellaAMover).cambiaDeLugar(posicionActual, vectorRotacion);

                //la posicion actual pasa a ser la vieja para la proxima huella
                posicionAnterio = posicionActual;

                //agarra la ultima huella para moverla a un nuevo lugar
                if (indexDeHuellaAMover < listaHuellas.Count - 1)
                {
                    indexDeHuellaAMover++;
                }
                else
                {
                    indexDeHuellaAMover = 0;
                }

                //setea el tiempo en que se hizo la huella
                tiempoEsperaHuella = DateTime.Now.TimeOfDay;
            }
        }

        public void render()
        {

            if (!(Boolean)GuiController.Instance.Modifiers.getValue("huellas"))
            {
                return;
            }

            foreach (Huella huella in listaHuellas)
            {
                huella.renderHuella();
            }
        }

        public void dispose()
        {
            foreach (Huella huella in listaHuellas)
            {
                huella.disposeHuella();
            }
        }

    }
}
