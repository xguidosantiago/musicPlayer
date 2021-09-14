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
using System.Text.RegularExpressions;

namespace tpProgramacionI
{
    public partial class form : Form
    {
        string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public form()
        {
            InitializeComponent();
        }
        public struct Datos
        {
            public string Nombre;
            public string Apellido;
            public string Usuario;
            public string Clave;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCreate_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
        }

        private void form_Load(object sender, EventArgs e)
        {

        }

        private void form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtApellido.Text == "" || txtNombre.Text == "" || txtUserName.Text == "" || txtPassword.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Uno o mas campos no se completaron correctamente", "Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Regex.IsMatch(txtEmail.Text, pattern))
            {
                Datos info;
                info.Nombre = txtNombre.Text;
                info.Apellido = txtApellido.Text;
                info.Usuario = txtUserName.Text;
                info.Clave = txtPassword.Text;

                success success = new success(info);
                this.Hide();
                success.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("El correo ingresado no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            tMensaje.SetToolTip(txtPassword, "el mail debe contener @");
        }
    }
}
