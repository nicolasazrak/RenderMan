using AlumnoEjemplos.MiGrupo.EnemigoEstados;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    class Enemigo
    {

        public TgcSkeletalMesh mesh;
        public Vector3 position;
        public EnemigoEstado estado{get; set;}
        public EscenarioManager escenarioManager;

        public Enemigo(Vector3 posicionInicial, EscenarioManager escenarioManager)
        {

            this.escenarioManager = escenarioManager;
            estado = new EnemigoQuieto(this);

            mesh = new TgcSkeletalLoader().loadMeshAndAnimationsFromFile(
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "CombineSoldier-TgcSkeletalMesh.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                    new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
            });

            Random rnd = new Random();
            mesh.Position = posicionInicial;
            mesh.Scale = new Vector3(1f, 1f, 1f);

        }


        public void render(float elapsedTime)
        {
            
            //Actualizar animacion
            estado.update(elapsedTime);
            mesh.render();
            Vector3 pos = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 dirMirar = mesh.Position - pos;
            dirMirar.Y = 0;
            mesh.rotateY((float)Math.Atan2(dirMirar.X, dirMirar.Z) - mesh.Rotation.Y);
        }


        public void dispose()
        {
            //La malla también hace dispose del attachment
            mesh.dispose();
            mesh = null;
        }



        public void teDispararon()
        {
            estado.teDispararon();
        }

        public void setEstado(EnemigoEstado estado)
        {
            this.estado = estado;
        }


        public TgcBoundingBox getBoundingBox()
        {
            return mesh.BoundingBox;
        }


    }
}
