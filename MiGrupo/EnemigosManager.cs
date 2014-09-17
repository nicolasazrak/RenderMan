using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using TgcViewer.Utils.TgcGeometry;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Sound;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TgcViewer.Utils.Modifiers;


namespace AlumnoEjemplos.MiGrupo
{
    class EnemigosManager
    {

        List<Enemigo> enemigos;

        private SoundManager soundManager;

        //0 muerto, 1 quieto, 2 corriendo, ¿3 disparando? --> (se vera despues bien)
        int[] estadoEnemigo = new int[3];
        //va a tener las posiciones previas para usar en caso de colisiones


        public EnemigosManager()
        {
            enemigos = new List<Enemigo>();
        }

        public void init (EscenarioManager escenario)
        {

            soundManager = new SoundManager();
            TgcSkeletalLoader enemigo = new TgcSkeletalLoader();
            Random rnd = new Random();

            for (int t = 0; t < 3; ++t)
            {
                enemigos.Add(new Enemigo(new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250), escenario));
            }


        }


        //<summary>
        //Llama al metodo render de cada enemigo que haya
        //</summary>
        public void update(float elapsedTime, EscenarioManager e)
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


        internal void murio(Enemigo enemigoDisparado)
        {

        }



        public List<Enemigo> getEnemigos()
        {
            return enemigos;
        }

    }
}
