using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    class EnemigoGolpeando : EnemigoEstado
    {
        TimeSpan tiempoDaño;
        TimeSpan tiempoEsperaGolpe;

        public EnemigoGolpeando(Enemigo enemigo)
            : base(enemigo)
        {
            tiempoDaño = DateTime.Now.TimeOfDay;
        }

        public override bool debeGirar()
        {
            return true;
        }

        public override void update(float elapsedTime, Vida vidaPersona)
        {

            girar();

            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dir_escape = enemigo.mesh.Position - pos;
            float dist = dir_escape.Length();
            dir_escape.Y = 0;

            TgcBoundingBox algo = enemigo.mesh.BoundingBox;
            Vector3 posAnterior = enemigo.mesh.Position;

            enemigo.mesh.playAnimation(enemigo.enemigoAmigacion, true, 20);

            int milisegundosEspera = Juego.Instance.esperaDañoMilisegundos;
            if (Math.Abs(dist) < 100)
            {
                if (Juego.Instance.esperaCorrecta(tiempoDaño, -1, 1, milisegundosEspera))
                {
                    tiempoDaño = DateTime.Now.TimeOfDay;
                    vidaPersona.restaAtaqueEnemigo();
                }
            }
            else
            {
                enemigo.setEstado(new EnemigoPersiguiendo(enemigo));
            }
            
            enemigo.mesh.updateAnimation();


        }


        public override void teDispararon()
        {
            enemigo.setEstado(new EnemigoMuriendo(enemigo));
        }

        public override void explotoBarril(Vector3 posicion)
        {
            enemigo.setEstado(new EnemigoMuriendo(enemigo));
        }

        public override void headShot()
        {
            SoundManager.Instance.playHeadShot();
            teDispararon();
        }

        public override bool debeVerificarDispoaro()
        {
            return true;
        }

    }
}
