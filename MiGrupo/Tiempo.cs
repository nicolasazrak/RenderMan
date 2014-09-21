using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.MiGrupo
{
    class Tiempo
    {

        public static Tiempo Instance;
        TgcText2d texto;
        private Timer _timer;

        public Tiempo()
        {
            Tiempo.Instance = this;

            _timer = new Timer();
            texto = new TgcText2d();
            texto.Color = Color.Red;
            texto.Align = TgcText2d.TextAlign.LEFT;
            texto.Position = new Point(5, 55);
            texto.Size = new Size(350, 100);
            texto.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            texto.Text = "Tiempo restante: ";
        }


        public void render(float elapsedTime)
        {
            texto.Text = "Tiempo restante: ";
            texto.render();
        }






    }
}
