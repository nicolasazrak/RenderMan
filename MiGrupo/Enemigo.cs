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
        public EnemigoEstado estado{get; set;}
        public EscenarioManager escenarioManager;

        public Enemigo(Vector3 posicionInicial, EscenarioManager escenarioManager, TgcSkeletalMesh mesh)
        {

            this.escenarioManager = escenarioManager;

            this.mesh = mesh;

            Random rnd = new Random();
            mesh.Position = posicionInicial;
            
            mesh.Scale = new Vector3(1f, 1f, 1f);
            mesh.AutoTransformEnable = true;

            this.setEstado(new EnemigoQuieto(this));
        }


        public void render(float elapsedTime, Vida vidaPersona)
        {
            //Actualizar animacion
            estado.update(elapsedTime, vidaPersona);
            mesh.render();
            //mesh.BoundingBox.render();
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

        public void explotoBarril()
        {
            estado.explotoBarril();
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


    }
}
