using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.MiGrupo
{
    class ContadorBalas
    {

        public static ContadorBalas Instance;
        public int balasRestantes = 0;
        Juego juego;
        TgcText2d texto;

        public ContadorBalas(int cantTotal)
        {
            juego = new Juego();

            ContadorBalas.Instance = this;

            texto = new TgcText2d();
            texto.Color = Color.Red;
            texto.Align = TgcText2d.TextAlign.LEFT;
            texto.Position = new Point(5, 55);
            texto.Size = new Size(350, 100);
            texto.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            balasRestantes = cantTotal;
            texto.Text = "Balas Restantes: " + balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
        }


        public void render()
        {
            texto.render();
        }


        public void huboDisparo()
        {
            balasRestantes--;
            texto.Text = "Balas Restantes: " + balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
        }

        public Boolean puedoDisparar()
        {
            return balasRestantes > 0;
        }

        public void recagar()
        {
            balasRestantes = juego.cantidadBalas;
            texto.Text = "Balas Restantes: " + balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
        }

    }
}
