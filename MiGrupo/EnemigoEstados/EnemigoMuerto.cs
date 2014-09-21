using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    class EnemigoMuerto : EnemigoEstado
    {

        public EnemigoMuerto(Enemigo enemigo) : base(enemigo)
        {
            Juego.Instance.enemigoAscesinado();
        }

        public override bool debeGirar()
        {
            return false;
        }

        public override void update(float elapsedTime, Vida vidaPersona)
        {
            enemigo.mesh.rotateX(3.1415f * 0.5f - enemigo.mesh.Rotation.X);
            //Subo un poco al muerto asi no queda cortado por el piso al acostarse
            Vector3 posicionMuerto = enemigo.mesh.Position;
            posicionMuerto.Y = 8;
            enemigo.mesh.Position = posicionMuerto;
            enemigo.mesh.playAnimation("StandBy", true);
            //soundManager.sonidoEnemigoMuerto();
        }

        public override void teDispararon()
        {

        }

    }
}
