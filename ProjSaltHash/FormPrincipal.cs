using System;
using System.Windows.Forms;

namespace ProjSaltHash
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            var passwordHash = new PasswordHash();
            var retorno = passwordHash.GetHash(txtPassword.Text);

            txtSalt.Text = retorno.Salt;
            txtPasswordHash.Text = retorno.Hash; 
        }

        private void btnTestarLogin_Click(object sender, EventArgs e)
        {
            var pass = txtPassword.Text;
            var hash = txtPasswordHash.Text;

            var passwordHash = new PasswordHash();
            var retorno = passwordHash.CompareHashs(hash, pass);
             
            if (retorno)
                MessageBox.Show("Logado com sucesso");
            else
                MessageBox.Show("Usuário ou Password incorreto");

        }
    }
}
