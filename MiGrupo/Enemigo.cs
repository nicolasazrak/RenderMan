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
        public Vector3 posAnterior;

        public Vector3 velocidad;
        public TgcBoundingSphere enemigoEsfera;

        public EnemigoEstado estado{get; set;}
        public EscenarioManager escenarioManager;

        public TgcCylinder sangre;

        public String enemigoAmigacion;
        private static List<TgcSkeletalMesh> staticMesh;
        private TgcBoundingSphere cabezaBounding;


        private static TgcSkeletalMesh getMesh()
        {
            if (staticMesh == null)
            {
                staticMesh = new List<TgcSkeletalMesh>();

                TgcSkeletalMesh comineSoldier = new TgcSkeletalLoader().loadMeshAndAnimationsFromFile(
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "CombineSoldier-TgcSkeletalMesh.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                    new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "HighKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "FlyingKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Push-TgcSkeletalAnim.xml",
                });

                staticMesh.Add(comineSoldier);

                TgcSkeletalMesh BasicHuman = new TgcSkeletalLoader().loadMeshAndAnimationsFromFile(
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "BasicHuman-TgcSkeletalMesh.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                    new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "HighKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "FlyingKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Push-TgcSkeletalAnim.xml",
                });

                staticMesh.Add(BasicHuman);

                TgcSkeletalMesh Quake2Scout = new TgcSkeletalLoader().loadMeshAndAnimationsFromFile(
                   GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "Quake2Scout-TgcSkeletalMesh.xml",
                   GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                   new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "HighKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "FlyingKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Push-TgcSkeletalAnim.xml",
                });

                staticMesh.Add(Quake2Scout);
                

                TgcSkeletalMesh Cs_Arctic = new TgcSkeletalLoader().loadMeshAndAnimationsFromFile(
                   GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "Cs_Arctic-TgcSkeletalMesh.xml",
                   GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                   new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "HighKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "FlyingKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Push-TgcSkeletalAnim.xml",
                });

                staticMesh.Add(Cs_Arctic);

            }

            Juego.Instance.cantidadTiposEnemgiosCreados += 1;
            return staticMesh[Juego.Instance.cantidadTiposEnemgiosCreados % staticMesh.Count].createMeshInstance("enemigo");
            
        }

        public static string getAnimacion()
        {
            Juego.Instance.animacionesDeEnemigos += 1;
            String[] animaciones = new string[3] { "HighKick", "FlyingKick", "Push" };
            return animaciones[Juego.getInstance().animacionesDeEnemigos % 3];
        }

        public Enemigo(Vector3 posicionInicial, EscenarioManager escenarioManager)
        {

            this.escenarioManager = escenarioManager;

            this.enemigoAmigacion = Enemigo.getAnimacion();

            this.mesh = Enemigo.getMesh();

            sangre = new TgcCylinder(posicionInicial, 0, 20, 0);
            sangre.Color = Color.Red;
            
            sangre.updateValues();

            enemigoEsfera = new TgcBoundingSphere(new Vector3(posicionInicial.X, 30, posicionInicial.Z), 10);

            Random rnd = new Random();
            mesh.Position = posicionInicial;
            
            mesh.Scale = new Vector3(1f, 1f, 1f);
            mesh.AutoTransformEnable = true;

            this.cabezaBounding = new TgcBoundingSphere(new Vector3(posicionInicial.X, posicionInicial.Y + 20, posicionInicial.Z), 20);

            this.setEstado(new EnemigoQuieto(this));
        }


        public void render(float elapsedTime, Vida vidaPersona)
        {
            //Actualizar animacion
            cabezaBounding = new TgcBoundingSphere(new Vector3(this.mesh.Position.X, this.mesh.Position.Y + 45, this.mesh.Position.Z), 4);
            estado.update(elapsedTime, vidaPersona);
            mesh.render();
        }

        public void setPosAnterior(Vector3 pos)
        {
            posAnterior = pos;
        }

        public void setPosicion (Vector3 pos)
        {
            mesh.Position = pos;
        }

        public Vector3 getPosAnterior()
        {
            return posAnterior;
        }

        public void dispose()
        {
            //La malla también hace dispose del attachment
            if (mesh != null)
            {
                //mesh.dispose();
                //No se porque el dispose aca me rompe todo
            }   
        }

        public void explotoBarril(Vector3 posicion)
        {
            estado.explotoBarril(posicion);
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
            mesh.updateBoundingBox();
            return mesh.BoundingBox;
        }

        public TgcBoundingSphere getCabeza()
        {
            return cabezaBounding;
        }

        public void headShot()
        {
            estado.headShot();
        }

        public Boolean debeVerificarDispoaro()
        {
            return estado.debeVerificarDispoaro();
        }


    }
}
