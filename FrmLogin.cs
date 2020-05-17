using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
namespace Shield_Wall
{
    public partial class FrmLogin : CCSkinMain
    {
        public FrmLogin()
        {
            InitializeComponent();
            txtUserName.Text = "Tod";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string userName = "Tod";
            string userCode = "678678";
            if (userCode== txtUserCode.Text.Trim() && userName==txtUserName.Text.Trim() )
            {
                
                frmMain f = new frmMain();
                f.Show();
                this.Hide();                
            }
            else
            {
                MessageBox.Show("用户或密码错误");
            }
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
