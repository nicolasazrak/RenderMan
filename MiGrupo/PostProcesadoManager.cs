using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Interpolation;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class PostProcesadoManager
    {
        EjemploAlumno ejemploAlumno;
        Effect effect;
        VertexBuffer screenQuadVB;
        TgcTexture alarmTexture;
        InterpoladorVaiven intVaivenAlarm;

        TgcScreenQuad screenQuad;
        Texture texSceneRT;
        Texture blurTempRT;

        Surface pOldRT;
        Surface g_pDepthStencil;
        public PostProcesadoManager(EjemploAlumno ejemAlumno)
        {
            ejemploAlumno = ejemAlumno;
            GuiController.Instance.CustomRenderEnabled = true;

            screenQuad = new TgcScreenQuad();

            int backBufferWidth = GuiController.Instance.D3dDevice.PresentationParameters.BackBufferWidth;
            int backBufferHeight = GuiController.Instance.D3dDevice.PresentationParameters.BackBufferHeight;
            texSceneRT = new Texture(GuiController.Instance.D3dDevice, backBufferWidth, backBufferHeight, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);

            int cropWidth = (backBufferWidth - backBufferWidth % 8) / 4;
            int cropHeight = (backBufferHeight - backBufferHeight % 8) / 4;

            g_pDepthStencil = GuiController.Instance.D3dDevice.CreateDepthStencilSurface(GuiController.Instance.D3dDevice.PresentationParameters.BackBufferWidth,
                                                             GuiController.Instance.D3dDevice.PresentationParameters.BackBufferHeight,
                                                             DepthFormat.D24S8,
                                                             MultiSampleType.None,
                                                             0,
                                                             true);
            blurTempRT = new Texture(GuiController.Instance.D3dDevice, cropWidth, cropHeight, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);

            effect = TgcShaders.loadEffect(GuiController.Instance.AlumnoEjemplosDir + "MiGrupo\\Efectos\\PostProcesado\\PocaVidaRestante.fx");
            effect.Technique = "GaussianYalarma";
            //Alarma
            CustomVertex.PositionTextured[] screenQuadVertices = new CustomVertex.PositionTextured[]
		    {
    			new CustomVertex.PositionTextured( -1, 1, 1, 0,0), 
			    new CustomVertex.PositionTextured(1,  1, 1, 1,0),
			    new CustomVertex.PositionTextured(-1, -1, 1, 0,1),
			    new CustomVertex.PositionTextured(1,-1, 1, 1,1)
    		};
            //vertex buffer de los triangulos
            screenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                    4, GuiController.Instance.D3dDevice, Usage.Dynamic | Usage.WriteOnly,
                        CustomVertex.PositionTextured.Format, Pool.Default);
            screenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);

            //Cargar textura que se va a dibujar arriba de la escena del Render Target
            alarmTexture = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Textures\\efecto_alarma.png");

            //Interpolador para efecto de variar la intensidad de la textura de alarma
            intVaivenAlarm = new InterpoladorVaiven();
            intVaivenAlarm.Min = 0;
            intVaivenAlarm.Max = 1;
            intVaivenAlarm.Speed = 5;
            intVaivenAlarm.reset();

        }

        public void update(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            //Cargamos el Render Targer al cual se va a dibujar la escena 3D. Antes nos guardamos el surface original
            //En vez de dibujar a la pantalla, dibujamos a un buffer auxiliar, nuestro Render Target.
            pOldRT = d3dDevice.GetRenderTarget(0);
            Surface pSurf = texSceneRT.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, pSurf);

            Surface pOldDS = d3dDevice.DepthStencilSurface;
            d3dDevice.DepthStencilSurface = g_pDepthStencil;

            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);


            //Dibujamos la escena comun, pero en vez de a la pantalla al Render Target
            drawSceneToRenderTarget(d3dDevice, elapsedTime);

            //Liberar memoria de surface de Render Target
            pSurf.Dispose();

            //Si quisieramos ver que se dibujo, podemos guardar el resultado a una textura en un archivo para debugear su resultado (ojo, es lento)
            //TextureLoader.Save(GuiController.Instance.ExamplesMediaDir + "Shaders\\render_target.bmp", ImageFileFormat.Bmp, renderTarget2D);


            //Luego tomamos lo dibujado antes y lo combinamos con una textura con efecto de alarma
            drawPostProcess(d3dDevice, elapsedTime);

        }

        /// <summary>
        /// Dibujamos toda la escena pero en vez de a la pantalla, la dibujamos al Render Target que se cargo antes.
        /// Es como si dibujaramos a una textura auxiliar, que luego podemos utilizar.
        /// </summary>
        private void drawSceneToRenderTarget(Device d3dDevice, float elapsedTime)
        {
            //Arrancamos el renderizado. Esto lo tenemos que hacer nosotros a mano porque estamos en modo CustomRenderEnabled = true
            d3dDevice.BeginScene();

            //render
            ejemploAlumno.update(elapsedTime);

            //Terminamos manualmente el renderizado de esta escena. Esto manda todo a dibujar al GPU al Render Target que cargamos antes
            d3dDevice.EndScene();
        }

        /// <summary>
        /// Se toma todo lo dibujado antes, que se guardo en una textura, y se le aplica un shader para borronear la imagen
        /// </summary>
        private void drawPostProcess(Device d3dDevice, float elapsedTime)
        {
            //Arrancamos la escena
            d3dDevice.BeginScene();


            Surface blurTempS = blurTempRT.GetSurfaceLevel(0);

            //Gaussian blur horizontal
            Vector2[] texCoordOffsets;
            float[] colorWeights;
            TgcPostProcessingUtils.computeGaussianBlurSampleOffsets15(blurTempS.Description.Width, 1.2f, 1, true, out texCoordOffsets, out colorWeights);
            effect.SetValue("texSceneRT", texSceneRT);
            effect.SetValue("gauss_offsets", TgcParserUtils.vector2ArrayToFloat2Array(texCoordOffsets));
            effect.SetValue("gauss_weights", colorWeights);
            d3dDevice.SetRenderTarget(0, blurTempS);
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            screenQuad.render(effect);

            //Gaussian blur vertical
            TgcPostProcessingUtils.computeGaussianBlurSampleOffsets15(blurTempS.Description.Height, 1.2f, 1, false, out texCoordOffsets, out colorWeights);
            effect.SetValue("texSceneRT", blurTempRT);
            effect.SetValue("gauss_offsets", TgcParserUtils.vector2ArrayToFloat2Array(texCoordOffsets));
            effect.SetValue("gauss_weights", colorWeights);
            d3dDevice.SetRenderTarget(0, pOldRT);
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            screenQuad.render(effect);

            effect.Technique = "AlarmaTechnique";

            //Cargamos parametros en el shader de Post-Procesado
            effect.SetValue("texSceneRT", texSceneRT);
            effect.SetValue("textura_alarma", alarmTexture.D3dTexture);
            effect.SetValue("alarmaScaleFactor", intVaivenAlarm.update());

            //Limiamos la pantalla y ejecutamos el render del shader
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();

            blurTempS.Dispose();



            //Tambien hay que dibujar el indicador de los ejes cartesianos
            GuiController.Instance.AxisLines.render();


            //Terminamos el renderizado de la escena
            d3dDevice.EndScene();
        }
    }
}
