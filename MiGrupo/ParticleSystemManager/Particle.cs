using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.ParticleSystemManager
{
    class Particle
    {
        Vector3 velocity;
        float weight;
        Color color;

        public Particle( Vector3 vel, float peso, Color col)
        {
            velocity = vel;
            weight = peso;
            color = col;

        }

        public void update(float elapsedTime){

        }
    }
}
