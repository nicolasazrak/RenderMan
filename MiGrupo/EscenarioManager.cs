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
        private TgcScene scene;
        TgcBox piso;


        public EscenarioManager()
        {

            arboles = new List<TgcMesh>();

            //Cargar modelo de palmera original
            TgcSceneLoader loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");

            piso = new TgcBox();
            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(200, 0, 200));
            piso.updateValues();
            piso.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));

        }

        //<summary>
        //Se va a llamar una sola vez para crear todos los arboles y darles una posicion inicial
        //</summary>
        public void generarArboles(int cantidad)
        {
            arboles.Add(scene.Meshes[0]);
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
