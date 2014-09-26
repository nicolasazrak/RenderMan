﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    class EnemigoQuieto : EnemigoEstado
    {

        public EnemigoQuieto(Enemigo enemigo) : base(enemigo)
        {
           
        }

        public override bool debeGirar()
        {
            return true;
        }



        public override void update(float elapsedTime, Vida vidaPersona)
        {

            girar();

            enemigo.mesh.playAnimation("StandBy", true);

            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();

            Vector3 dir_escape = enemigo.mesh.Position - pos;
            float dist = dir_escape.Length();
            dir_escape.Y = 0;

            if (Math.Abs(dist) < Juego.Instance.distanciaParaPerseguir)
            {
                enemigo.setEstado(new EnemigoPersiguiendo(enemigo));
            }
            else
            {
                enemigo.setPosAnterior(enemigo.mesh.Position);
            }
     
            enemigo.mesh.playAnimation("StandBy", true);
        }

        public override void teDispararon()
        {
            enemigo.setEstado(new EnemigoMuerto(enemigo));
        }

        public override void explotoBarril()
        {
            enemigo.setEstado(new EnemigoMuerto(enemigo));
        }
    }
}
