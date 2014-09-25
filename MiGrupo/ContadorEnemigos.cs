using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.MiGrupo
{
    class ContadorEnemigos
    {

        public static ContadorEnemigos Instance;
        public int enemigosAscecinados = 0;
        TgcText2d texto;

        public ContadorEnemigos(int cantTotal)
        {
            ContadorEnemigos.Instance = this;

            texto = new TgcText2d();
            texto.Color = Color.Red;
            texto.Align = TgcText2d.TextAlign.LEFT;
            texto.Position = new Point(5, 35);
            texto.Size = new Size(350, 100);
            texto.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            texto.Text = "Enemigos matados: " + enemigosAscecinados.ToString() + " / " + Juego.Instance.totalEnemigos.ToString();
        }


        public void render()
        {
            texto.render();
        }


        public void enemigoAscesinado()
        {
            enemigosAscecinados++;
            texto.Text = "Enemigos matados: " + enemigosAscecinados.ToString() + " / " + Juego.Instance.totalEnemigos.ToString();
        }

        public void reiniciarContador()
        {
            texto.Text = "Enemigos matados: " + enemigosAscecinados.ToString() + " / " + Juego.Instance.totalEnemigos.ToString();
        }

    }
}
