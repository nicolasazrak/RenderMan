using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public abstract void update(float elapsedTime);

        public abstract void teDispararon();

    }
}
