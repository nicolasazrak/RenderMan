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
    public class GameOver
    {
        TgcText2d textoFin;
        TgcSprite spriteGameOver;
        Size screenSize = GuiController.Instance.Panel3d.Size;
        Indicadores indacadorGameOver;


        public GameOver()
        {
            indacadorGameOver = new Indicadores();
                       
            textoFin = new TgcText2d();
            textoFin.Color = Color.Black;
            textoFin.Align = TgcText2d.TextAlign.CENTER;
            textoFin.Position = new Point(0, 0);
            textoFin.Size = new Size(650, 300);
            textoFin.changeFont(new System.Drawing.Font("Arial", 16f, FontStyle.Bold));
            textoFin.Text = "GAME OVER!!";

            //cargo el sprite del gameover

            spriteGameOver = new TgcSprite();
            spriteGameOver.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\RenderMan\\sprites\\Game-Over.png");
            Size textureSize = spriteGameOver.Texture.Size;
            spriteGameOver.Scaling = new Vector2(indacadorGameOver.ajustarTexturaAPantalla(screenSize.Width, textureSize.Width),indacadorGameOver.ajustarTexturaAPantalla(screenSize.Height, textureSize.Height));
            spriteGameOver.Position = new Vector2(0, 0);
            //sonido.playSonidoFin();
        }

        
        public void render()
        {
            textoFin.render();

            GuiController.Instance.Drawer2D.beginDrawSprite();

            spriteGameOver.render();
            
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void dispose()
        {
            textoFin.dispose();
            indacadorGameOver.dispose();
        }

    }
}
