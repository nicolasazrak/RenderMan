using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.MiGrupo
{
    class Vida
    {

        TgcText2d textoVida;
        private SoundManager sonido;
        int vida;

        public void initialize()
        {
            sonido = new SoundManager();

            textoVida = new TgcText2d();
            textoVida.Color = Color.Red;
            textoVida.Align = TgcText2d.TextAlign.LEFT;
            textoVida.Position = new Point(5, 15);
            textoVida.Size = new Size(350, 100);
            textoVida.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            vida = 100;
            textoVida.Text = vida.ToString();

        }

        public void restaAtaqueEnemigo()
        {
            vida = vida - 10;
            textoVida.Text = vida.ToString();
            sonido.playSonidoJugadorAlcanzado();
        }

        public void render()
        {
            textoVida.render();
        }

        public void dispose()
        {
            textoVida.dispose();
        }

    }
}
