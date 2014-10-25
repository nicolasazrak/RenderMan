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
            //Subo un poco al muerto asi no queda cortado por el piso al acostarse
            Vector3 posicionMuerto = enemigo.mesh.Position;
            posicionMuerto.Y = 4;
            enemigo.mesh.Position = posicionMuerto;
            enemigo.mesh.playAnimation("StandBy", true);
 
        }

        public override bool debeGirar()
        {
            return false;
        }

        public override void update(float elapsedTime, Vida vidaPersona)
        {
            tiempoMuerto += elapsedTime;


            if (tiempoMuerto <= 3.1415 / 3)
            {
                float angulo = elapsedTime * 1.5f;
                enemigo.mesh.rotateX(angulo);
            }
            else
            {
                enemigo.setEstado(new EnemigoMuerto(enemigo));
            }
            /*if (tiempoMuerto > 3)
            {
                enemigo.setEstado(new EnemigoMuerto(enemigo));
            }*/

        }

        public override void teDispararon()
        {

        }
        public override void explotoBarril(Vector3 posicion)
        {

        }


        public override void headShot()
        {
            
        }

        public override bool debeVerificarDispoaro()
        {
            return false;
        }

    }
}
