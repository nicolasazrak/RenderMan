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
    class Indicadores
    {
        TgcSprite spriteBala;
        TgcSprite spriteCargador;
        TgcSprite spriteEnemigo;
        TgcSprite spriteVida;

        //se usan para ubicar los textos al lado
        int spriteVidaPosX;
        int spriteEnemigoPosX;
        int spriteBalaPosX;
        int spriteCargadorPosX;
        
        Size screenSize = GuiController.Instance.Panel3d.Size;

        public Indicadores()
        {

            spriteBala = new TgcSprite();
            spriteBala.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sprites\\bala.png");
            Size textureSize = spriteBala.Texture.Size;
            spriteBalaPosX = textureSize.Width;
            spriteBala.Scaling = new Vector2(ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 50), textureSize.Width), ajustarTexturaAPantalla(divicionEntera(screenSize.Height, 12), textureSize.Height));
            spriteBala.Position = new Vector2(25, screenSize.Height - 45);

            spriteCargador = new TgcSprite();
            spriteCargador.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sprites\\cargador.png");
            Size textureSizeCargador = spriteCargador.Texture.Size;
            spriteCargadorPosX = textureSizeCargador.Width;
            spriteCargador.Scaling = new Vector2(ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 30), textureSize.Width), ajustarTexturaAPantalla(divicionEntera(screenSize.Height, 3), textureSize.Height));
            spriteCargador.Position = new Vector2(120, screenSize.Height - 45);

            spriteEnemigo = new TgcSprite();
            spriteEnemigo.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sprites\\enemigo2.png");
            Size texturaSizeEnemigo = spriteEnemigo.Texture.Size;
            spriteEnemigoPosX = texturaSizeEnemigo.Width;
            spriteEnemigo.Scaling = new Vector2(ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 35), textureSize.Width), ajustarTexturaAPantalla(divicionEntera(screenSize.Height, 4), textureSize.Height));
            spriteEnemigo.Position = new Vector2(screenSize.Width - 140, 20);

            spriteVida = new TgcSprite();
            spriteVida.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sprites\\cruzRoja.png");
            Size texturaSizeVida = spriteVida.Texture.Size;
            spriteVidaPosX = texturaSizeVida.Width;
            spriteVida.Scaling = new Vector2(ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 35), textureSize.Width), ajustarTexturaAPantalla(divicionEntera(screenSize.Height, 3), textureSize.Height));
            spriteVida.Position = new Vector2(5, 15);
        }

        public void init()
        {

        }

        public int divicionEntera(int dividendo, int divisor)
        {
            int resultado = dividendo / divisor;
            return resultado;
        }

        public float ajustarTexturaAPantalla(int pantallaParametro, int texturaParametro)
        {
            //para poder hacer la divicion bien
            float pantalla = pantallaParametro;
            float textura = texturaParametro;
            return (pantalla / textura);
        }

        public void spriteRender()
        {
            spriteBala.render();
            spriteCargador.render();
            spriteEnemigo.render();
            spriteVida.render();
        }

        public float getPosicionXSpriteVida()
        {
            return ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 35), spriteVidaPosX) * spriteVidaPosX;
        }

        public float getPosicionXSpriteEnemigo()
        {
            return ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 35), spriteEnemigoPosX) * spriteEnemigoPosX;
        }

        public float getPosicionXSpriteCargador()
        {
            return ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 35), spriteCargadorPosX) * spriteCargadorPosX;
        }

        public float getPosicionXSpriteBala()
        {
            return ajustarTexturaAPantalla(divicionEntera(screenSize.Width, 35), spriteBalaPosX) * spriteBalaPosX;
        }

        public void dispose()
        {
            spriteBala.dispose();
            spriteCargador.dispose();
            spriteEnemigo.dispose();
            spriteVida.dispose();
        }

    }
}
