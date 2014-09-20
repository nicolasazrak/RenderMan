using Microsoft.DirectX;
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

        
        private List<TgcMesh> arboles;
        private List<TgcMesh> pasto;
        public List<Barril> barriles;
        TgcMesh arbol;
        private TgcScene scene;
        TgcBox piso;
        TgcSceneLoader loader;
        string[] tipoArboles = new string[3] { "Pino\\Pino", "Palmera2\\Palmera2", "Palmera3\\Palmera3" };
        TgcSkyBox skyBox;
        private List<TgcBoundingBox> colisionables;

        public EscenarioManager()
        {

            arboles = new List<TgcMesh>();
            pasto = new List<TgcMesh>();
            barriles = new List<Barril>();
            loader = new TgcSceneLoader();

            piso = new TgcBox();
            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(5000, 0, 5000));
            piso.updateValues();
            piso.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));

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

        public void generarArboles(int cantidad)
        {
            Random rnd = new Random();
       
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\"+ tipoArboles[0] +"-TgcScene.xml");
            arbol = scene.Meshes[0];
            for (int i = 0; i < cantidad; i++)
            {
                TgcMesh instancia = arbol.createMeshInstance("arbol");
                instancia.Scale = new Vector3(3f, 3f, 3f);
                instancia.Position = new Vector3(rnd.Next(0, 2500), 0, rnd.Next(0, 2500));
                instancia.AlphaBlendEnable = true;
                arboles.Add(instancia);
            }
            //Genero en 1/4 del escenario los arboles y los copio en los demas cuartos1559326801 estela
            for (int j = 0; j < cantidad; j++)
            {
                TgcMesh instancia = arbol.createMeshInstance("arbol2");
                Vector3 vecPos = new Vector3(arboles[j].Position.X * (-1), 0, arboles[j].Position.Z);
                instancia.Position = vecPos;
                instancia.AlphaBlendEnable = true;
                instancia.Scale = new Vector3(2f, 2f, 2f);
                arboles.Add(instancia);

                TgcMesh instancia2 = arbol.createMeshInstance("arbol3");
                vecPos = new Vector3(arboles[j].Position.X * (-1), 0, arboles[j].Position.Z * (-1));
                instancia2.Position = vecPos;
                instancia2.AlphaBlendEnable = true;
                instancia2.Scale = new Vector3(3f, 3f, 3f);
                arboles.Add(instancia2);

                TgcMesh instancia3 = arbol.createMeshInstance("arbol4");
                vecPos = new Vector3(arboles[j].Position.X, 0, arboles[j].Position.Z * (-1));
                instancia3.Scale = new Vector3(1.5f, 1.5f, 1.5f);
                instancia3.AlphaBlendEnable = true;
                instancia3.Position = vecPos;
                arboles.Add(instancia3);
            }
//            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\" + tipoArboles[1] + "-TgcScene.xml");
            updateColisionables();
        }

        public void generarPasto(int cantidad)
        {

            Random rnd = new Random();
       
            TgcScene scenePasto = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pasto\\Pasto-TgcScene.xml");
            TgcMesh pastoMesh = scenePasto.Meshes[0];
            pasto.Add(pastoMesh);

            for (int i = 0; i < cantidad; i++)
            {
                TgcMesh instancia = pastoMesh.createMeshInstance("");
                instancia.Position = new Vector3(rnd.Next(-2000, 2000), 0, rnd.Next(-2000, 2000));
                instancia.Scale = new Vector3(1f, 0.5f, 1f);
                instancia.AlphaBlendEnable = true;
                pasto.Add(instancia);
            }

        }

        public void generarBarriles(int cantidad)
        {
            Random rnd = new Random();

            for (int i = 0; i < cantidad; i++)
            {
                Barril instancia = new Barril(new Vector3(rnd.Next(-1000, 1000), 0, rnd.Next(-1000, 1000)));
                barriles.Add(instancia);
            }
            updateColisionables();
        }

        public Boolean verificarColision (TgcBoundingBox personaje)
        {

            foreach (TgcBoundingBox colisionable in getColisionables())
            {
                colisionable.render();
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
        public void update()
        {

            foreach (TgcMesh arbol in arboles)
            {
                arbol.render();
            }

            foreach (TgcMesh pastito in pasto)
            {
                pastito.render();
            }

            foreach (Barril barril in barriles)
            {
                barril.render();
            }

            skyBox.render();
            piso.render();

        }


        //<summary>
        //Devuelve el bounding box de todos los arboles para que se puedan checkear las colisiones contra la camara o los enemigos
        //</summary>
        public List<TgcBoundingBox> getColisionables()
        {
            return colisionables;
        }
        public void updateColisionables()
        {
            colisionables = barriles.Select(barril =>barril.getBoundingBox()).ToList().Concat(arboles.Select(arbol => {
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

            piso.dispose();
        }

    }
}
