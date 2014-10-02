﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class EscenarioManager
    {
        public static EscenarioManager Instance;

        SoundManager sonido;
        TimeSpan tiempoInicial;

        private List<TgcMesh> arboles;
        private List<TgcMesh> pasto;
        private List<TgcMesh> barriles;
        TgcMesh arbol;
        TgcMesh municion;
        TgcMesh caja;

        private TgcScene scene;

        TgcBox piso;
        int pisoSize;
        int casillasPorEje;
        public Vector3[] divisionesPiso;
        public int ultimaPosicionUtilizada;
        Random _random;

        TgcSceneLoader loader;
        string[] tipoArboles = new string[3] { "Pino\\Pino", "Palmera2\\Palmera2", "Palmera3\\Palmera3" };
        TgcSkyBox skyBox;
        private List<TgcBoundingBox> colisionables;
        
        Vida vida;
        

        public EscenarioManager(Vida unaVida)
        {
            vida = unaVida;
            EscenarioManager.Instance = this;

            sonido = new SoundManager();
            arboles = new List<TgcMesh>();
            pasto = new List<TgcMesh>();
            barriles = new List<TgcMesh>();
            loader = new TgcSceneLoader();
            

            piso = new TgcBox();
            piso.UVTiling = new Vector2(100, 100);
            pisoSize = 4000;
            casillasPorEje = 50;
            divisionesPiso = new Vector3[2500];
            _random = new Random();

            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(pisoSize, 0, pisoSize));
            
            piso.updateValues();

            piso.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Vegetacion\\moss_rock60_512.jpg"));
            
            generarSkyBox();

            colisionables = new List<TgcBoundingBox>();


        }


        private void generarSkyBox()
        {
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox1\\";
            //Crear SkyBox 
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(6000, 6000, 6000);

            //Configurar color
            //skyBox.Color = Color.OrangeRed;

            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");

            //Hay veces es necesario invertir las texturas Front y Back si se pasa de un sistema RightHanded a uno LeftHanded
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");



            //Actualizar todos los valores para crear el SkyBox
            skyBox.updateValues();
        }

        public void generarBosque(int cantidadArboles, int cantidadPasto, int cantidadBarriles)
        {
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\" + tipoArboles[0] + "-TgcScene.xml");
            arbol = scene.Meshes[0];
            for (int i = 0; i < cantidadArboles; i++)
            {
                TgcMesh instancia = arbol.createMeshInstance("arbol");
                instancia.Scale = new Vector3(3f, 3f, 3f);
                instancia.Position = this.divisionesPiso[i];
                instancia.AlphaBlendEnable = true;
                arboles.Add(instancia);
            }

            TgcScene scenePasto = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pasto\\Pasto-TgcScene.xml");
            TgcMesh pastoMesh = scenePasto.Meshes[0];
            pasto.Add(pastoMesh);


            for (int i = 0; i < cantidadPasto; i++)
            {
                TgcMesh instancia = pastoMesh.createMeshInstance("");
                instancia.Position = this.divisionesPiso[cantidadArboles + i];
                instancia.Scale = new Vector3(1f, 0.5f, 1f);
                instancia.AlphaBlendEnable = true;
                pasto.Add(instancia);
            }

            TgcScene sceneBarril = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Objetos\\BarrilPolvora\\BarrilPolvora-TgcScene.xml");
            TgcMesh barrilMesh = sceneBarril.Meshes[0];
            //barriles.Add(barrilMesh);
            for (int i = 0; i < cantidadBarriles; i++)
            {
                TgcMesh instancia = barrilMesh.createMeshInstance("");
                instancia.Position = this.divisionesPiso[cantidadArboles + cantidadPasto + i];
                instancia.Scale = new Vector3(0.5f, 0.6f, 0.5f);
                instancia.AlphaBlendEnable = true;
                barriles.Add(instancia);
            }
            updateColisionables();

            ultimaPosicionUtilizada = cantidadArboles + cantidadBarriles + cantidadPasto;

            crearCajaVida(101,5,1);


            iniciarMunicion();
        }

        private void crearCajaVida(float x, float y, float z)
        {
            TgcScene cajaLoad = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "meshCruzRoja-TgcScene.xml");
            TgcMesh cajaMesh = cajaLoad.Meshes[0];
            caja = cajaMesh.createMeshInstance("");
            caja.Position = new Vector3(x,y,z);
            caja.Scale = new Vector3(1.5f,1.5f,1.5f);
            caja.AlphaBlendEnable = true;
            tiempoInicial = DateTime.Now.TimeOfDay;
            
        }

        private void cambiarCajaVida() {
            caja = null;
            var random = this._random;
            float x = random.Next(2000);
            float z = random.Next(2000);
            this.crearCajaVida(x,5,z);
        }
        
        private void iniciarMunicion()
        {
            TgcScene sceneMuniciones = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\CajaMuniciones\\CajaMuniciones-TgcScene.xml");
            TgcMesh municionMesh = sceneMuniciones.Meshes[0];
            municion = municionMesh.createMeshInstance("");
            municion.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            municion.Position = new Vector3(100, 0, 0);
            municion.AlphaBlendEnable = true;
            tiempoInicial = DateTime.Now.TimeOfDay;
        }

        public void generarPosiciones()
        {
            int i, j, q = 0;
            int salto = (pisoSize) / casillasPorEje;
            int x, z = 0;
            for (i = 0; i < casillasPorEje; i++)
            {
                for (j = 0; j < casillasPorEje; j++)
                {
                    x = -2000 + (salto * i);
                    z = -2000 + (salto * j);
                    divisionesPiso[q] = new Vector3(x + (salto / 2), 0, z + (salto / 2));
                    q++;
                }
            }

            this.desordenarPiso();

        }

        public void desordenarPiso()
        {
            var random = this._random;
            for (int i = divisionesPiso.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                Vector3 tmp = divisionesPiso[j];
                divisionesPiso[j] = divisionesPiso[i - 1];
                divisionesPiso[i - 1] = tmp;
            }
        }


        public void explotaBarril(TgcMesh barrilExplotado)
        {
            Vector3 posBarril = barrilExplotado.Position;
            int radio = Juego.Instance.radioExplosion;


            foreach (Enemigo enemigo in EnemigosManager.Instance.getEnemigos())
            {
                Vector3 dir = enemigo.mesh.Position - posBarril;
                float dist = dir.Length();
                if (Math.Abs(dist) < radio)
                {
                    enemigo.explotoBarril();
                }

            }

            barriles.Remove(barrilExplotado);
        }


        public Boolean verificarColision (TgcBoundingBox personaje)
        {

            foreach (TgcBoundingBox colisionable in getColisionables())
            {
                //colisionable.render();
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(personaje, colisionable);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    return true;
                }
            }

            return false;
        }

        //<summary>
        //Llama al metodo render de cada arbol y pasto que haya que haya
        //</summary>
        public void update(float elapsedTime)
        {

            foreach (TgcMesh arbol in arboles) arbol.render();
            foreach (TgcMesh pastito in pasto) pastito.render();
            foreach (TgcMesh barril in barriles) barril.render();

            skyBox.render();
            piso.render();
            municion.render();

            renderCaja(elapsedTime);
            recargoArma();
        }

        private void renderCaja(float elapsedTime)
        {
            caja.render();
            caja.rotateY(1.0f * elapsedTime);
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dir_escape = caja.Position - pos;
            float dist = dir_escape.Length();
            if (Math.Abs(dist) < 60) {
                vida.subirVida();
                cambiarCajaVida();
            }
            caja.render();
        }

        private void recargoArma()
        {
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dirDistancia = municion.Position - pos;
            float dist = dirDistancia.Length();

            if (Math.Abs(dist) < 80 && Juego.Instance.esperaCorrecta(tiempoInicial, -1, 5, 1))
            {
                tiempoInicial = DateTime.Now.TimeOfDay;
                ContadorBalas.Instance.obtenerMuniciones();
                sonido.playSonidoMunicion();
            }
        }
        //<summary>
        //Devuelve el bounding box de todos los arboles para que se puedan checkear las colisiones contra la camara o los enemigos
        //</summary>

        public List<TgcMesh> getBarriles()
        {
            return barriles;
        }
        public List<TgcBoundingBox> getColisionables()
        {
            return colisionables;
        }
        public void updateColisionables()
        {
            //colisionables = barriles.Select(barril =>barril.getBoundingBox()).ToList().Concat(arboles.Select(arbol => {
            //    TgcBoundingBox bounding = arbol.BoundingBox;
            //    bounding.scaleTranslate(arbol.Position, new Vector3(0.3f, 1f, 0.3f));
            //    return bounding; 
            //}).ToList()).ToList();
            colisionables = barriles.Select(barril => barril.BoundingBox).ToList().Concat(arboles.Select(arbol =>
            {
                TgcBoundingBox bounding = arbol.BoundingBox;
                bounding.scaleTranslate(arbol.Position, new Vector3(0.3f, 1f, 0.3f));
                return bounding;
            }).ToList()).ToList();

        }


        public void dispose()
        {
            foreach (TgcMesh arbol in arboles)
            {
                arbol.dispose();
            }

            foreach (TgcMesh pastito in pasto)
            {
                pastito.dispose();
            }

            foreach (TgcMesh barril in barriles)
            {
                barril.dispose();
            }

            caja.dispose();
            piso.dispose();
        }

    }
}
