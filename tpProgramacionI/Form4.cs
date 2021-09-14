using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace tpProgramacionI
{
    public partial class Reproductor : Form
    {
        bool play = true;
        
        
        string[] routesMusic;
        string[] filesMusic;

        public static int volumeBuffer;
        public static int repeat = 0;
        public static int shuffle = 0;
        public static int shuffleNum;

        public Reproductor()
        {
            InitializeComponent();
            
                                                                                                           
            volume.Value = 50;                                                                              // establece barra de volumen en 50%
            mediaPlayer.settings.volume = 50;                                                               // establece valor inicial de volumen al 50%
            
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        //boton adjuntar musica
        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            btnCargar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            OpenFileDialog boxSearchFiles = new OpenFileDialog();                                           // abrir ventana de diálogo
            boxSearchFiles.Multiselect = true;                                                              // permitir multiselección
            boxSearchFiles.Filter = "Audio Files(*.wav; *.mp3; *.WAV; *.mp3)|*.wav; *.mp3; *.WAV; *.MP3";   // filtrar por tipo de datos mp3/Wav
            if (boxSearchFiles.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lstMusic.Items.Clear();                                                                     // limpiar lista al cargar nuevos archivos
                filesMusic = boxSearchFiles.SafeFileNames;                                                  // obtener nombre y extension del archivo
                routesMusic = boxSearchFiles.FileNames;                                                     // obtener full path del archivo
                foreach (var fileMusic in filesMusic)
                {
                    lstMusic.Items.Add(fileMusic);                                                          // cargar archivos a la lista
                }
                mediaPlayer.URL = routesMusic[0];                                                           // posiciona selector en el indice cero
                lstMusic.SelectedIndex = 0;                                                                 // comenzar del indice 0 de la lista
                tMensaje.SetToolTip(btn_Play, "Pausar");                                                    // cambiar tooltip a "pausar"
                btn_Play.Image = Properties.Resources.btnPause_gs;                                          // cambiar imagen de boton a "pausar"

            }
        }

        private void btnCargar_MouseHover_1(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(btnCargar, "Cargar Archivos");                                              // tooltip "cargar archivo"

        }

        private void lstMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            mediaPlayer.URL = routesMusic[lstMusic.SelectedIndex];                                          // mostrar canción elegida segun el índice
            
        }

        private void btn_Play_MouseHover(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(btn_Play, "Reproducir");                                                    // tooltip reproducir 
        }

        // reproducir/pausar canción

        private void btn_Play_Click(object sender, EventArgs e)
        {
            btn_Play.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            
            switch (play)                                                                                   // switch para reproducir o pausar musica
            {                                                                                               // al clickear sobre btnPlay
                case true:
                    mediaPlayer.Ctlcontrols.play();                                                         // iniciar reproducción
                    btn_Play.Image = Properties.Resources.btnPause_gs;                                      // mostrar icono pausa
                    play = false;                                                                           // setea variable play en falso
                    tMensaje.SetToolTip(btn_Play, "Pausar");                                                // tooltip pausar
                    break;

                case false:
                    mediaPlayer.Ctlcontrols.pause();                                                        // pausar reproducción
                    btn_Play.Image = Properties.Resources.btnPlay_gs;                                       // mostrar icono reproducir
                    play = true;                                                                            // setea variable play en verdadero
                    tMensaje.SetToolTip(btn_Play, "Reproducir");                                            // tooltip reproducir
                    break;
            }
        }

                                                                                                            
        private void timer1_Tick(object sender, EventArgs e)                                                
        {
            if (mediaPlayer.playState==WMPLib.WMPPlayState.wmppsPlaying)
            {                                                                                               
                p_bar.Maximum = (int)mediaPlayer.Ctlcontrols.currentItem.duration;                          // valor maximo de la cancion en la barra
                p_bar.Value = (int)mediaPlayer.Ctlcontrols.currentPosition;                                 // transcurso de la cancion en la barra
            }

            try
            {
                lbl_start.Text = mediaPlayer.Ctlcontrols.currentItem.durationString;                       
                lbl_start.Text = mediaPlayer.Ctlcontrols.currentPositionString;                             // mostrar tiempo de reproduccion actual
                lbl_end.Text = mediaPlayer.Ctlcontrols.currentItem.durationString;                          // y tiempo total de la cancion
                
            }            
            catch { }

            try {
                Random rnd = new Random();
                shuffleNum = rnd.Next(0, lstMusic.Items.Count);
                if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped && shuffle == 1)
                {
                    lstMusic.SelectedIndex = shuffleNum;                                                    // reproducir siguiente cancion al finalizar actual
                }else if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
                { 
                    lstMusic.SelectedIndex = lstMusic.SelectedIndex + 1;                                    // reproducir siguiente cancion al finalizar actual
                }
            }
            catch { }

            try
            {

                if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped && repeat == 1)
                {
                    lstMusic.SelectedIndex = 0;                                                             // volver a comenzar la lista si repeat vale 1
                }
            }
            catch { }

        }
                                                                                                                                                                                     
        private void volume_Scroll(object sender, EventArgs e)                                             
        {                                                                                                 
            mediaPlayer.settings.volume = volume.Value;                                                     // modificar el volumen al mover barra
            if (volume.Value == 0)                                                                          // condicional cambiar icono de volumen a mute
            {
                btnVolumen.Image = Properties.Resources.btnMute_gs;                                         // mostrar mute si el volumen es igual a cero
                //btnVolumen.BackgroundImageLayout = ImageLayout.Stretch;
            } else
            {
                btnVolumen.Image = Properties.Resources.btnVolumeUp_gs;                                     // mostrar volume up si el volumen no es cero
                //btnVolumen.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void volume_MouseMove(object sender, MouseEventArgs e)
        {
            if (volume.Value == 0)
            {
                tMensaje.SetToolTip(volume, "mute");                                                       // tooltip mostrar mute si volumen es cero
            }
            else
            {
                tMensaje.SetToolTip(volume, volume.Value.ToString());                                      // tooltip mostrar valor del volumen actual
            }

        }

        private void p_bar_MouseDown(object sender, MouseEventArgs e)                                       // avanzar/retroceder tiempo desde la barra
        {
            try { mediaPlayer.Ctlcontrols.currentPosition = mediaPlayer.currentMedia.duration * e.X / p_bar.Width; }
            catch { }
        }

        private void Reproductor_MouseDown(object sender, MouseEventArgs e)                                
        {
            if (e.Button == MouseButtons.Left)                                                              // funcion que permite mover ventana
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnVolumen_Click(object sender, EventArgs e)
        {
            btnVolumen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            if (volume.Value == 0)                                                                         // condicional para mute/un-mute
            {
                mediaPlayer.settings.volume = volumeBuffer;                                                // restablece volumen a nivel anterior
                volume.Value = volumeBuffer;                                                               // restablece barra de volumen a nivel anterior
                btnVolumen.Image = Properties.Resources.btnVolumeUp_gs;                                    // muestra icono volume up
                tMensaje.SetToolTip(btnVolumen, "mute");                                                   // muestra tooltip mute
            }
            else
            {
                volumeBuffer = volume.Value;                                                               // guarda valor de volumen en variable
                mediaPlayer.settings.volume = 0;                                                           // establece volumen en 0%
                volume.Value = 0;                                                                          // establece barra de volumen en 0%
                btnVolumen.Image = Properties.Resources.btnMute_gs;                                        // muestra icono mute
                tMensaje.SetToolTip(btnVolumen, "unmute");                                                 // muestra tooltip unmute
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            shuffleNum = rnd.Next(0, lstMusic.Items.Count);
            btnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            if (shuffle == 1)
            {
                lstMusic.SelectedIndex = shuffleNum;                                                       // random select
            }
            else if (lstMusic.SelectedIndex < lstMusic.Items.Count - 1)
            {
                lstMusic.SelectedIndex = lstMusic.SelectedIndex + 1;                                       // reproducir canción siguiente
            }
        }

        private void btnNext_MouseHover(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(btnNext, "Siguiente");                                                     // tooltip "siguiente"
        }


        private void btnPrev_Click(object sender, EventArgs e)
        {
            btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            Random rnd = new Random();
            shuffleNum = rnd.Next(0, lstMusic.Items.Count);

            if (shuffle == 1)                                                               
            {
                lstMusic.SelectedIndex = shuffleNum;                                                       // random select
            }
            else if (lstMusic.SelectedIndex > 0)                                                             
            {
                lstMusic.SelectedIndex = lstMusic.SelectedIndex - 1;                                       // reproducir canción anterior
            }
        }

        private void btnPrev_MouseHover(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(btnPrev, "Anterior");                                                      // tooltip "anterior"
        }

        // tooltip boton salir
        private void btnExit_MouseHover(object sender, EventArgs e)
        {  
            tMensaje.SetToolTip(btnExit, "Salir");                                                         // tooltip salir
        }

        // evento salir del reproductor 
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();                                                                            //  salir 
        }

        // evento minimizar
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;                                                  // minimizar
        }

        // tooltip minimizar
        private void btnMin_MouseHover(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(btnMin, "Minimizar");                                                      // tooltip minimizar
        }


        private void btnRepeat_Click(object sender, EventArgs e)
        {
            btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            if (repeat == 1)
            {
                repeat = 0;                                                                              // deshabilitar repetir
                btnRepeat.BackgroundImage = Properties.Resources.repeat;                                 // cambiar icono repetir blanco     
                tMensaje.SetToolTip(btnRepeat, "Repetir Lista");                                         // tooltip habilitar
            }
            else
            {
                repeat = 1;                                                                              // habilitar repetir
                btnRepeat.BackgroundImage = Properties.Resources.repeatOn;                               // cambiar icono repetir amarillo
                tMensaje.SetToolTip(btnRepeat, "No Repetir Lista");                                      // tooltip deshabilitar
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();        
            form5.ShowDialog();                                                                          // abre ventana de info 

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.code;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.code2;
        }

        private void Reproductor_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            if (shuffle == 1)
            {
                shuffle = 0;                                                                              // deshabilitar shuffle
                btnShuffle.BackgroundImage = Properties.Resources.random_grey;                           // cambiar icono shuffle blanco     
                tMensaje.SetToolTip(btnRepeat, "Activar Aleatorio");                                      // tooltip habilitar
            }
            else
            {
                shuffle = 1;                                                                              // habilitar shuffle
                btnShuffle.BackgroundImage = Properties.Resources.random_solid;                            // cambiar icono shuffle amarillo
                tMensaje.SetToolTip(btnRepeat, "Desactivar Aleatorio");                                   // tooltip deshabilitar
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
