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
        public int cargadoresRestantes = 0;
        Juego juego;
        TgcText2d texto;
        TgcText2d textoCargador;

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

            textoCargador = new TgcText2d();
            textoCargador.Color = Color.Red;
            textoCargador.Align = TgcText2d.TextAlign.LEFT;
            textoCargador.Position = new Point(5, 75);
            textoCargador.Size = new Size(350, 100);
            textoCargador.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            textoCargador.Text = "Cargadores Restantes: " + Juego.Instance.cantidadDeCargadores;

            cargadoresRestantes = juego.cantidadDeCargadores;
        }


        public void render()
        {
            texto.render();
            textoCargador.render();
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

        public Boolean puedoRecargar()
        {
            return cargadoresRestantes > 0;
        }

        public void recagar()
        {
            balasRestantes = juego.cantidadBalas;
            cargadoresRestantes--;
            texto.Text = "Balas Restantes: " + balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
            textoCargador.Text = "Cargadores Restantes: " + cargadoresRestantes;
        }

    }
}
