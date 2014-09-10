using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo
{
    class EnemigosManager
    {

        List<Enemigo> enemigos;


        public EnemigosManager()
        {
            enemigos = new List<Enemigo>();
        }



        //<summary>
        //Se va a llamar una sola vez para crear todos los enemigos y darles una posicion inicial
        //</summary>
        public void generarEnemigos(int cantidad)
        {
            enemigos.Add(new Enemigo(new Vector3(0, 0, 0)));

        }



        //<summary>
        //Llama al metodo render de cada enemigo que haya
        //</summary>
        public void update(float elapsedTime)
        {
            foreach (Enemigo enemigo in enemigos)
            {
                enemigo.render(elapsedTime);
            }
        }

        public void dispose()
        {
            foreach (Enemigo enemigo in enemigos)
            {
                enemigo.dispose();
            }
        }

    }
}
