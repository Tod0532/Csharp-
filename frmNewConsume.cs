using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

using CCWin;

namespace Shield_Wall
{
    public partial class frmNewConsume : CCSkinMain//CCSkinMain
    {
        public frmNewConsume()
        {
            InitializeComponent();
            IniCombo();
        }


        //初始化Date
        public void IniCombo()
        {
            for (int i = 1; i <= 31; i++)
            {
                cboDay.Items.Add(i);
            }
            for (int i = 1; i <=12; i++)
            {
                cboMonth.Items.Add(i);
            }
            for (int i = 2019; i <= 2029; i++)
            {
                cboYear.Items.Add(i);
            }
            //oDay.Text=dayo
            cboDay.SelectedIndex = 10;
            cboMonth.SelectedIndex = 0;
            cboYear.SelectedIndex = 1;

            cboType.Items.Add("收入");
            cboType.Items.Add("支出");
            cboType.SelectedIndex = 0;

            cboCategory.Items.Add("工资");
            cboCategory.Items.Add("红包");
            cboCategory.Items.Add("投资收益");
            cboCategory.Items.Add("购物餐饮");
            cboCategory.Items.Add("孩子教育");
            cboCategory.Items.Add("水电");
            cboCategory.Items.Add("交通");
            cboCategory.Items.Add("医疗");
            cboCategory.Items.Add("房贷");
            cboCategory.Items.Add("养老助游");
            cboCategory.Items.Add("装修专项");
            cboCategory.Items.Add("其他");
            cboCategory.SelectedIndex = 0;

            cboName.Items.Add("Tod");
            cboName.Items.Add("Li");
            cboName.SelectedIndex = 0;
        }
        //确认增加记录
        private void btnEnter_Click(object sender, EventArgs e)
        {
            string date = cboYear.Text + '-' + cboMonth.Text + '-' + cboDay.Text;
            string sql = string.Format("insert into Consume values('{0}','{1}','{2}','{3}','{4}','{5}')", date, cboName.Text.Trim(), cboType.Text.Trim(), cboCategory.Text.Trim(), txtMoney.Text.Trim(), txtDescription.Text.Trim());
            DataCmd dc = new DataCmd();
            dc.DataBaseCommand(sql);

        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            //frmMain f = new frmMain();
            //f.Show();
            this.Close();
        }

        private void FrmNewConsume_FormClosed(object sender, FormClosedEventArgs e)
        {
            //frmMain f = new frmMain();
            //f.Show();
           
        }

        private void FrmNewConsume_Load(object sender, EventArgs e)
        {

        }

        private void typeChange(object sender, EventArgs e)
        {
            //if (cboType.Text == "支出")
            //{

            //    cboCategory.Items.Add("购物餐饮");
            //    cboCategory.Items.Add("孩子教育");
            //    cboCategory.Items.Add("水电");
            //    cboCategory.Items.Add("交通");
            //    cboCategory.Items.Add("医疗");
            //    cboCategory.Items.Add("房贷");
            //    cboCategory.Items.Add("养老助游");
            //    cboCategory.Items.Add("装修专项");
            //    cboCategory.Items.Add("其他");
            //}

        }

        private void FrmNewConsume_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
