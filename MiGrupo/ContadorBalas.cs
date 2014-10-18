using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;

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
        Indicadores indicadorBala;
        Indicadores indicadorCargador;

        public static ContadorBalas getInstance()
        {
            if (ContadorBalas.Instance == null)
            {
                ContadorBalas.Instance = new ContadorBalas();
            }
            return ContadorBalas.Instance;
        }

        private ContadorBalas()
        {

            Size screenSize = GuiController.Instance.Panel3d.Size;
            juego = Juego.Instance;

            indicadorBala = new Indicadores();
            indicadorCargador = new Indicadores();

            texto = new TgcText2d();
            texto.Color = Color.Red;
            texto.Align = TgcText2d.TextAlign.LEFT;

            int tamañoTexturaBala = (int)indicadorBala.getPosicionXSpriteBala();
            texto.Position = new Point(tamañoTexturaBala + 25, screenSize.Height - 32);
            texto.Size = new Size(350, 500);
            texto.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            
            textoCargador = new TgcText2d();
            textoCargador.Color = Color.Red;
            textoCargador.Align = TgcText2d.TextAlign.LEFT;

            int tamañoTexturaCargador = (int)indicadorBala.getPosicionXSpriteCargador();
            textoCargador.Position = new Point(tamañoTexturaCargador + 190, screenSize.Height - 32);
            textoCargador.Size = new Size(350, 100);
            textoCargador.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            

            setInitialValues();
        }

        public void setInitialValues()
        {

            balasRestantes = Juego.Instance.cantidadBalas;
            cargadoresRestantes = Juego.Instance.cantidadDeCargadores;

            textoCargador.Text = Juego.Instance.cantidadDeCargadores.ToString();
            texto.Text = balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
        }

        public void render()
        {
            texto.render();
            textoCargador.render();
        }


        public void huboDisparo()
        {
            balasRestantes--;
            texto.Text = balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
        }

        public Boolean puedoDisparar()
        {
            return balasRestantes > 0;
        }

        public Boolean puedoRecargar()
        {
            return cargadoresRestantes > 0;
        }

        public void recargar()
        {
            balasRestantes = juego.cantidadBalas;
            cargadoresRestantes--;
            texto.Text = balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();
            textoCargador.Text = cargadoresRestantes.ToString();
        }

        public void obtenerMuniciones()
        {
            //juego.cantidadDeCargadores++;
            cargadoresRestantes = juego.cantidadDeCargadores;
            textoCargador.Text = cargadoresRestantes.ToString();
        }


        public void dispose()
        {

        }



    }
}
