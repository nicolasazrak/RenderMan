using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.SRC.Renderman.EnemigoEstados
{
    class EnemigoMuerto : EnemigoEstado
    {
        float tiempoMuerto = 0;

        public EnemigoMuerto(Enemigo enemigo) : base(enemigo)
        {
            
            Juego.Instance.enemigoAscesinado();

            enemigo.sangre.Center = new Vector3(enemigo.mesh.Position.X, enemigo.mesh.Position.Y - 3.8f, enemigo.mesh.Position.Z);
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
            if (tiempoMuerto <= 3.1415)
            {
                enemigo.sangre = new TgcCylinder(enemigo.sangre.Position, 0, 20 * tiempoMuerto / 2, 0);
                enemigo.sangre.updateValues();
            }

            enemigo.sangre.render();

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
