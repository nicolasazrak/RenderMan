using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo
{
    class ArmaManager
    {
        private EnemigosManager enemigosManager;
        private SoundManager soundManager;


        public ArmaManager(EnemigosManager enemigosManager, SoundManager soundManager)
        {
            this.enemigosManager = enemigosManager;
            this.soundManager = soundManager;
        }


        public void update()
        {
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.R))
            {
                soundManager.playSonidoRecarga();
            }

            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT) == true)
            {
                soundManager.playSonidoDisparo();
            }
        }

        public void dispose()
        {

        }




    }
}
