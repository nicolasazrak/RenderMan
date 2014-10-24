using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    abstract class EnemigoEstado
    {

        public Enemigo enemigo;

        public EnemigoEstado(Enemigo enemigo)
        {
            this.enemigo = enemigo;
        }

        public abstract Boolean debeGirar();

        public abstract void update(float elapsedTime, Vida vidaPersona);

        public abstract void teDispararon();

        /* La posicion se pasa para que cuando explote como el enemigo salga volando (aparece un poco mas lejos)*/
        public abstract void explotoBarril(Vector3 posicion);


        public void girar()
        {
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dirMirar = enemigo.mesh.Position - pos;
            dirMirar.Y = 0;
            enemigo.mesh.rotateY((float)Math.Atan2(dirMirar.X, dirMirar.Z) - enemigo.mesh.Rotation.Y);
        }


        public abstract void headShot();

        public abstract Boolean debeVerificarDispoaro();


    }

}
