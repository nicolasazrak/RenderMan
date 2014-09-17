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
        private EscenarioManager escenarioManager;


        public EnemigosManager(EscenarioManager escenario, SoundManager soundManager)
        {
            enemigos = new List<Enemigo>();
            this.soundManager = soundManager;
            this.escenarioManager = escenario;
        }


        public void generarEnemigos(int cantidad)
        {
            
            Random rnd = new Random();

            for (int t = 0; t < cantidad; ++t)
            {
                enemigos.Add(new Enemigo(new Vector3(-rnd.Next(0, 1000) - 250, 0, -rnd.Next(0, 1000) - 250), this.escenarioManager));
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
