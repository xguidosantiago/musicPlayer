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

namespace tpProgramacionI
{
    public partial class success : Form
    {
        public static string username = "";
        public static string password = "";
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public success(form.Datos info)
        {
            InitializeComponent();
            username = info.Usuario;
            password = info.Clave;
            txtPresentacion.Text = $"Bienvenido, {username}";
            txtPresentacion.Font = new Font("Century gothic", 18);
            txtPresentacion.TextAlign = ContentAlignment.MiddleCenter;

        }

        private void success_Load(object sender, EventArgs e)
        {

        }


       private void btnEnter_Click(object sender, EventArgs e)
        {
            
            Reproductor reproductor = new Reproductor();
            this.Hide();
            reproductor.ShowDialog();
            this.Close();
        }


        private void success_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
