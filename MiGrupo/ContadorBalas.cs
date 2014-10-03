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

        public ContadorBalas(int cantTotal)
        {
            Size screenSize = GuiController.Instance.Panel3d.Size;
            juego = new Juego();

            indicadorBala = new Indicadores();
            indicadorCargador = new Indicadores();

            ContadorBalas.Instance = this;

            texto = new TgcText2d();
            texto.Color = Color.Red;
            texto.Align = TgcText2d.TextAlign.LEFT;
            int tamañoTexturaBala = (int)indicadorBala.getPosicionXSpriteBala();
            texto.Position = new Point(tamañoTexturaBala + 25, screenSize.Height - 32);
            texto.Size = new Size(350, 500);
            texto.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            balasRestantes = cantTotal;
            texto.Text =  balasRestantes.ToString() + " / " + Juego.Instance.cantidadBalas.ToString();

            textoCargador = new TgcText2d();
            textoCargador.Color = Color.Red;
            textoCargador.Align = TgcText2d.TextAlign.LEFT;
            int tamañoTexturaCargador = (int)indicadorBala.getPosicionXSpriteCargador();
            textoCargador.Position = new Point(tamañoTexturaCargador + 190, screenSize.Height - 32);
            textoCargador.Size = new Size(350, 100);
            textoCargador.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            textoCargador.Text = Juego.Instance.cantidadDeCargadores.ToString();

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
            juego.cantidadDeCargadores++;
            cargadoresRestantes = juego.cantidadDeCargadores;
            textoCargador.Text = cargadoresRestantes.ToString();
        }

        public void dispose()
        {
        }
    }
}
