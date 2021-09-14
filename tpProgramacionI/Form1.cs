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
    public partial class Form1 : Form
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

        public Form1( )
        {
            InitializeComponent();           
            
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            form menu = new form();
            this.Hide();
            menu.ShowDialog();
            this.Close();
        }
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == success.username && txtPassword.Text == success.password && success.username != "" && success.password != "")
            {
                username = txtUser.Text;
                Reproductor reproductor = new Reproductor();
                this.Hide();
                reproductor.ShowDialog();

            } else if (txtUser.Text == "" && txtPassword.Text == "")
            {
                MessageBox.Show("Ingrese sus credenciales", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            else
            {
                MessageBox.Show("Las credenciales son incorrectas. Intente nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(button1, "Salir");
        }

        private void btnCreate_MouseHover(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(btnCreate, "Crear nuevo usuario");
        }
    }
}
