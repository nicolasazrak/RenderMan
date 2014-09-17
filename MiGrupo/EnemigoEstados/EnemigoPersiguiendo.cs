using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo.EnemigoEstados
{
    class EnemigoPersiguiendo : EnemigoEstado
    {

        public EnemigoPersiguiendo(Enemigo enemigo) : base(enemigo) { }

        public override bool debeGirar()
        {
            return true;
        }

        public override void update(float elapsedTime)
        {

            girar(); 

            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dir_escape = enemigo.mesh.Position - pos;
            float dist = dir_escape.Length();
            dir_escape.Y = 0;

            TgcBoundingBox algo = enemigo.mesh.BoundingBox;
            Vector3 posAnterior = enemigo.mesh.Position;

            //Falta verificar las colisiones
            
            if (!(enemigo.escenarioManager.verificarColision(algo)))
            {
                //Aca se les dice que hagan el movimiento de correr
                enemigo.mesh.move(dir_escape * (-0.5f * elapsedTime));
                enemigo.mesh.playAnimation("Run", true, 20);
                //soundManager.sonidoCaminandoEnemigo();
            }
            else
            {
                enemigo.mesh.Position = posAnterior;
            }

            enemigo.mesh.updateAnimation();


        }

        public override void teDispararon()
        {
            enemigo.setEstado(new EnemigoMuerto(enemigo));
        }

    }
}
