using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.MiGrupo
{
    class ContadorEnemigos
    {
        Indicadores indicadorEnemigo;
        public static ContadorEnemigos Instance;
        public int enemigosAscecinados = 0;
        TgcText2d texto;

        public ContadorEnemigos()
        {

            ContadorEnemigos.Instance = this;

            Size screenSize = GuiController.Instance.Panel3d.Size;

            texto = new TgcText2d();
            texto.Color = Color.Red;
            texto.Align = TgcText2d.TextAlign.LEFT;
            //texto.Position = new Point(screenSize.Width - 75, 35);
            int tamañoTexturaX = (int)new Indicadores().getPosicionXSpriteEnemigo();
            texto.Position = new Point(screenSize.Width - 105 + tamañoTexturaX, 35);
            texto.Size = new Size(350, 100);
            texto.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));

            setInitialValues();

        }

        public void setInitialValues()
        {
            enemigosAscecinados = 0;
            texto.Text = enemigosAscecinados.ToString() + " / " + Juego.Instance.totalEnemigos.ToString();
        }

        public void render()
        {
            texto.render();
        }


        public void enemigoAscesinado()
        {
            enemigosAscecinados++;
            texto.Text = enemigosAscecinados.ToString() + " / " + Juego.Instance.totalEnemigos.ToString();
        }

        public void reiniciarContador()
        {
            texto.Text = enemigosAscecinados.ToString() + " / " + Juego.Instance.totalEnemigos.ToString();
        }

    }
}
