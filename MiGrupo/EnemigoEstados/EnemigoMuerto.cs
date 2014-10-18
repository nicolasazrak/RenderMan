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
            enemigo.sangre.render();
        }

        public override void teDispararon()
        {

        }
        public override void explotoBarril(Vector3 posicion)
        {

        }
    }
}
