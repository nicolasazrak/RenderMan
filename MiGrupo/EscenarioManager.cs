using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class EscenarioManager
    {

        private List<TgcMesh> arboles;
        TgcMesh arbol;
        private TgcScene scene;
        TgcBox piso;

        string[] tipoArboles = new string[3] { "Pino\\Pino", "Palmera2\\Palmera2", "Palmera3\\Palmera3" };

        public EscenarioManager()
        {

            arboles = new List<TgcMesh>();

            //Cargar modelo de palmera original
            TgcSceneLoader loader = new TgcSceneLoader();
            //scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            //arbol = scene.Meshes[0];

            piso = new TgcBox();
            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(4000, 0, 4000));
            piso.updateValues();
            piso.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));
            
        }

        public void generarArboles(int cantidad)
        {
            Random rnd = new Random();
       
            TgcSceneLoader loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\"+ tipoArboles[0] +"-TgcScene.xml");
            arbol = scene.Meshes[0];
            for (int i = 0; i < cantidad; i++)
            {
                TgcMesh instancia = arbol.createMeshInstance("");
                instancia.Position = new Vector3(rnd.Next(0, 2000), 0, rnd.Next(0, 2000));
                arboles.Add(instancia);
            }
            //Genero en 1/4 del escenario los arboles y los copio en los demas cuartos1559326801 estela
            for (int j = 0; j < cantidad; j++)
            {
                TgcMesh instancia = arbol.createMeshInstance("");
                Vector3 vecPos = new Vector3(arboles[j].Position.X * (-1), 0, arboles[j].Position.Z);
                instancia.Position = vecPos;
                arboles.Add(instancia);

                TgcMesh instancia2 = arbol.createMeshInstance("");
                vecPos = new Vector3(arboles[j].Position.X * (-1), 0, arboles[j].Position.Z * (-1));
                instancia2.Position = vecPos;
                arboles.Add(instancia2);

                TgcMesh instancia3 = arbol.createMeshInstance("");
                vecPos = new Vector3(arboles[j].Position.X, 0, arboles[j].Position.Z * (-1));
                instancia3.Position = vecPos;
                arboles.Add(instancia3);
            }
//            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\" + tipoArboles[1] + "-TgcScene.xml");



        }

        public Boolean verificarColision (TgcBoundingBox personaje)
        {
            Boolean huboChoque = false;

            foreach (TgcMesh a in arboles)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(personaje, a.BoundingBox);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    huboChoque = true;
                    break;
                }
            }

            return huboChoque;
        }
        //<summary>
        //Llama al metodo render de cada arbol que haya
        //</summary>
        public void update()
        {
            foreach (TgcMesh arbol in arboles)
            {
                arbol.render();
            }
            piso.render();
        }


        //<summary>
        //Devuelve el bounding box de todos los arboles para que se puedan checkear las colisiones contra la camara o los enemigos
        //</summary>
        public void getColisionables()
        {

        }

        public void dispose()
        {
            foreach (TgcMesh arbol in arboles)
            {
                arbol.dispose();
            }
            piso.dispose();
        }

    }
}
