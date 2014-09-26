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
        public abstract void explotoBarril();


        public void girar()
        {
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dirMirar = enemigo.mesh.Position - pos;
            dirMirar.Y = 0;
            enemigo.mesh.rotateY((float)Math.Atan2(dirMirar.X, dirMirar.Z) - enemigo.mesh.Rotation.Y);
        }

    }

}
