using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    class EnemigoMuriendo : EnemigoEstado
    {

        float tiempoMuerto = 0f;

        public EnemigoMuriendo(Enemigo enemigo) : base(enemigo)
        {
            //Juego.Instance.enemigoAscesinado();
            enemigo.mesh.rotateX(3.1415f * 0.5f - enemigo.mesh.Rotation.X);
            //Subo un poco al muerto asi no queda cortado por el piso al acostarse
            Vector3 posicionMuerto = enemigo.mesh.Position;
            posicionMuerto.Y = 8;
            enemigo.mesh.Position = posicionMuerto;
            enemigo.mesh.playAnimation("StandBy", true);
            enemigo.sangre.Center = new Vector3(enemigo.mesh.Position.X, enemigo.mesh.Position.Y - 7f, enemigo.mesh.Position.Z);
            enemigo.sangre.Rotation = enemigo.mesh.Rotation;
            enemigo.sangre.rotateX(-enemigo.mesh.Rotation.X);
            enemigo.sangre.moveOrientedY(30f);
        }

        public override bool debeGirar()
        {
            return false;
        }

        public override void update(float elapsedTime, Vida vidaPersona)
        {
            tiempoMuerto += elapsedTime;

            enemigo.sangre = new TgcCylinder(enemigo.sangre.Position, 0, 20 * tiempoMuerto / 3, 0);
            enemigo.sangre.updateValues();
            enemigo.sangre.render();

            if (tiempoMuerto > 3)
            {
                enemigo.setEstado(new EnemigoMuerto(enemigo));
                
            }

        }

        public override void teDispararon()
        {

        }
        public override void explotoBarril()
        {

        }


    }
}
