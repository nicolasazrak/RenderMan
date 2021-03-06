﻿using Microsoft.DirectX;
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


namespace AlumnoEjemplos.SRC.Renderman
{
    class EnemigosManager
    {

        List<Enemigo> enemigos;

        private SoundManager soundManager;
        private EscenarioManager escenarioManager;
        
        public static EnemigosManager Instance;

        Boolean pasoNivel;

        public EnemigosManager(EscenarioManager escenario)
        {
            EnemigosManager.Instance = this;
            enemigos = new List<Enemigo>();
            this.soundManager = SoundManager.getInstance();
            this.escenarioManager = escenario;
            pasoNivel = false;
        }


        public void generarEnemigos(int cantidad)
        {
            enemigos.Clear();

            for (int t = 0; t < cantidad; ++t)
            {
                enemigos.Add(new Enemigo(this.escenarioManager.divisionesPiso[this.escenarioManager.ultimaPosicionUtilizada + t] , this.escenarioManager));
            }

        }

        public void pasarNivel()
        {
            pasoNivel = true;
        }

        //<summary>
        //Llama al metodo render de cada enemigo que haya
        //</summary>
        public void update(float elapsedTime, Vida vidaPersona)
        {
            foreach (Enemigo enemigo in enemigos)
            {
                enemigo.render(elapsedTime, vidaPersona);
                if (pasoNivel) break;
            }
            pasoNivel = false;
        }

        public void dispose()
        {
            foreach (Enemigo enemigo in enemigos)
            {
                enemigo.dispose();
            }   
        }

        public List<Enemigo> getEnemigos()
        {
            return enemigos;
        }

    }
}
