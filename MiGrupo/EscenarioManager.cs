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

        private List<TgcMesh> arboles;
        private List<TgcMesh> pasto;
        TgcMesh arbol;
        private TgcScene scene;
        TgcBox piso;
        TgcSceneLoader loader;
        string[] tipoArboles = new string[3] { "Pino\\Pino", "Palmera2\\Palmera2", "Palmera3\\Palmera3" };
        TgcSkyBox skyBox;

        public EscenarioManager()
        {

            arboles = new List<TgcMesh>();
            pasto = new List<TgcMesh>();
            loader = new TgcSceneLoader();

            piso = new TgcBox();
            piso.setPositionSize(new Vector3(0, 0, 0), new Vector3(4000, 0, 4000));
            piso.updateValues();
            piso.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));

            generarSkyBox();

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
                instancia.Position = new Vector3(rnd.Next(0, 2000), 0, rnd.Next(0, 2000));
                arboles.Add(instancia);
            }
            //Genero en 1/4 del escenario los arboles y los copio en los demas cuartos1559326801 estela
            for (int j = 0; j < cantidad; j++)
            {
                TgcMesh instancia = arbol.createMeshInstance("arbol2");
                Vector3 vecPos = new Vector3(arboles[j].Position.X * (-1), 0, arboles[j].Position.Z);
                instancia.Position = vecPos;
                arboles.Add(instancia);

                TgcMesh instancia2 = arbol.createMeshInstance("arbol3");
                vecPos = new Vector3(arboles[j].Position.X * (-1), 0, arboles[j].Position.Z * (-1));
                instancia2.Position = vecPos;
                arboles.Add(instancia2);

                TgcMesh instancia3 = arbol.createMeshInstance("arbol4");
                vecPos = new Vector3(arboles[j].Position.X, 0, arboles[j].Position.Z * (-1));
                instancia3.Position = vecPos;
                arboles.Add(instancia3);
            }
//            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\" + tipoArboles[1] + "-TgcScene.xml");

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
                instancia.Position = new Vector3(rnd.Next(0, 2000), 0, rnd.Next(0, 2000));
                pasto.Add(instancia);
            }

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

            skyBox.render();
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

            foreach (TgcMesh pastito in pasto)
            {
                pastito.dispose();
            }

            piso.dispose();
        }

    }
}
