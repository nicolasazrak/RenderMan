using AlumnoEjemplos.MiGrupo.Efectos;
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
    public class Barril
    {
        
        public bool explotado = false;
        public TgcMesh mesh;
        public TgcBoundingCylinder cilindro;
        public TgcBoundingBox BoundigBox;

        private static TgcMesh originalMesh;
        
        private Humo humo;
        private Explosion explosion;

        public static TgcMesh getMesh()
        {
            if (originalMesh == null)
            {
                TgcScene sceneBarril = EscenarioManager.Instance.loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "RenderMan\\modelos\\BarrilPolvora\\BarrilPolvora-TgcScene.xml");
                TgcMesh barrilMesh = sceneBarril.Meshes[0];
                originalMesh = barrilMesh;
            }
            return originalMesh.createMeshInstance("barril");
        }

        public Barril(Vector3 position)
        {
            this.mesh = Barril.getMesh();
            this.mesh.Position = position;
            this.mesh.Scale = new Vector3(0.6f, 0.7f, 0.6f);
            this.mesh.AlphaBlendEnable = true;
            cilindro = new TgcBoundingCylinder(position, 10, 150);
            this.mesh.updateBoundingBox();
            BoundigBox = this.mesh.BoundingBox;

            
            explosion = new Explosion(position);
            humo = new Humo(this.mesh.Position);

        }


        public Vector3 getPosition()
        {
            return mesh.Position;
        }

        public void render()
        {
            if (!explotado){
                mesh.render();
            } 
            else
            {
                explosion.render();
                humo.render();
            }
               
        }


        public void dispose()
        {
            mesh.dispose();
        }

        public void explota()
        {

            explotado = true;

            int radio = Juego.Instance.radioExplosion;

            foreach (Enemigo enemigo in EnemigosManager.Instance.getEnemigos())
            {
                Vector3 dir = enemigo.mesh.Position - mesh.Position;
                float dist = dir.Length();
                if (Math.Abs(dist) < radio)
                {
                    enemigo.explotoBarril(mesh.Position);
                }
            }

            SoundManager.Instance.playSonidoExplosion();
            EscenarioManager.Instance.colisionables.Remove(cilindro);

        }


    }
}
