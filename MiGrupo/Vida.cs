using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;


namespace AlumnoEjemplos.MiGrupo
{
    public class Vida
    {
        TgcText2d textoVida;
        private SoundManager sonido;
        int vida;
        Indicadores indicadorVida;
        EjemploAlumno menu;

        public void initialize(EjemploAlumno unMenu)
        {
            menu = unMenu;
            sonido = new SoundManager();
            indicadorVida = new Indicadores();

            textoVida = new TgcText2d();
            textoVida.Color = Color.Red;
            textoVida.Align = TgcText2d.TextAlign.LEFT;
            int tamañoTexturaX = (int)indicadorVida.getPosicionXSpriteVida();
            textoVida.Position = new Point(tamañoTexturaX + 35, 25);
            textoVida.Size = new Size(350, 100);
            textoVida.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            vida = 100;
            textoVida.Text = "%" + vida.ToString();

        }

        public void restaAtaqueEnemigo()
        {
            vida = vida - 10;
            textoVida.Text = "%" + vida.ToString();
            sonido.playSonidoJugadorAlcanzado();
            if (vida == 0)
            {
                EjemploAlumno.Instance.murioPersonaje();
            }
        }

        public void subirVida() {
            if (vida + Juego.Instance.recuperoVida <= 100)
            {
                vida += Juego.Instance.recuperoVida;
            }
            else
            {
                vida = 100;
            }
            
            textoVida.Text = "%" + vida.ToString();
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
