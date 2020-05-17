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
    public partial class frmMain : CCSkinMain
    {       
        private void FrmMain_Load(object sender, EventArgs e)
        {
            
        }

        public frmMain()
        {
            InitializeComponent();
            FreshData();
            IniDataGrid();
            IniCombo();
            IniComboSelect();
        }

        //增加记录页面
        private void button1_Click(object sender, EventArgs e)
        {
            frmNewConsume f = new frmNewConsume();
            //f.ShowDialog();
            //this.Hide();
            f.Show();
        }       

        //刷新显示
        private void button2_Click(object sender, EventArgs e)
        {
            FreshData();
        }
        //确认修改
        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                string deleteId = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                string date = cboYear.Text + '-' + cboMonth.Text + '-' + cboDay.Text;
                string sql = string.Format("update Consume set ConsumeDate='{0}',Name='{1}',Type='{2}',Category='{3}',ConsumeMoney='{4}',Descrition='{5}'where Id={6}", date, cboName.Text, cboType.Text, cboCategory.Text, txtMoney.Text.Trim(), txtDescription.Text.Trim(),deleteId);
                DataCmd dc = new DataCmd();
                dc.DataBaseCommand(sql);
                FreshData();
                #region 清空CBO
                cboNumber.Text = "";
                cboName.Text = "";
                cboType.Text = "";
                cboCategory.Text = "";
                txtMoney.Text = "";
                txtDescription.Text = "";
                cboYear.Text = "";
                cboMonth.Text = "";
                cboDay.Text = "";
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //切换到修改状态
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            selectCells();            
        }
        //删除记录
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {
                return;
            }
            string deleteId = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
            if (MessageBox.Show("确认要删除吗？", "请确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("delete from Consume where Id={0}", deleteId);
                DataCmd d = new DataCmd();
                d.DataBaseCommand(sql);
            }
            FreshData();
        }
        //选中当前行内容，显示到CBO中
        public void selectCells()
        {
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {              
                return;
            }
            ShareData.selectId = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
            string sql= string.Format("select * from dbo.Consume where Id={0}", ShareData.selectId);
            string connString = ShareData.connection;
            int count = 0;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    count = cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cboNumber.Text = reader["Id"].ToString().Trim();
                        cboName.Text = reader["Name"].ToString().Trim();
                        cboType.Text = reader["Type"].ToString().Trim();
                        cboCategory.Text = reader["Category"].ToString().Trim();
                        txtMoney.Text = reader["ConsumeMoney"].ToString().Trim();
                        txtDescription.Text = reader["Descrition"].ToString().Trim();
                        cboYear.Text = reader["ConsumeDate"].ToString().Substring(0,4);
                        cboMonth.Text = reader["ConsumeDate"].ToString().Substring(5, 2);
                        cboDay.Text = reader["ConsumeDate"].ToString().Substring(8, 2);
                    }

                }

            }        
        }
        //保存EXCEL文件
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "保存为Exce2003格式(*.xls)|*.xls";//设置为数据文件或所有文件
            sfd.FilterIndex = 0;
            sfd.RestoreDirectory = true;
            sfd.CreatePrompt = true;
            sfd.Title = "另存为";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
              
                    Stream myStream;
                    myStream = sfd.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    string str = "";
                      try
                    {//写标题
                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        if (i>0)
                        {
                            str += "\t";
                        }
                        str += dataGridView1.Columns[i].HeaderText;
                    }
                    sw.WriteLine(str);

                    //写内容
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        string tempStr = "";
                        for (int k = 0; k < dataGridView1.Columns.Count; k++)
                        {
                            if (k>0)
                            {
                                tempStr += "\t";
                            }
                            tempStr += dataGridView1.Rows[j].Cells[k].Value.ToString();
                        }
                        sw.WriteLine(tempStr);                       
                    }
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception ex)
                {

                   // MessageBox.Show(ex.Message);
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }

        }
        //汇总刷新
        private void BtnAllCount_Click(object sender, EventArgs e)
        {
            decimal a = 0;
            decimal b = 0;
            FreshData();
            IniDataGrid();
            string sql = "select sum(ConsumeMoney) from DBO.Consume where Type='收入'";
            string sql1 = "select sum(ConsumeMoney) from DBO.Consume where Type='支出'";
            string connString = ShareData.connection; ;
            int count = 0;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    count = cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtIn.Text = reader[0].ToString();
                        a = decimal.Parse(txtIn.Text);
                    }
                }
            }
            using (SqlConnection connn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    connn.Open();
                    cmd.Connection = connn;
                    cmd.CommandText = sql1;
                    count = cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtOut.Text = reader[0].ToString();
                        b = decimal.Parse(txtOut.Text);
                    }
                }

            }
            txtSurplus.Text =(a-b).ToString().Trim();
        }

        #region 函数
        //初始化DataGrid控件
        public void IniDataGrid()
        {
            cboNumber.Enabled = false;

            dataGridView1.Columns[0].HeaderText = "流水号";
            dataGridView1.Columns[1].HeaderText = "日期";
            dataGridView1.Columns[2].HeaderText = "名字";
            dataGridView1.Columns[3].HeaderText = "类型";
            dataGridView1.Columns[4].HeaderText = "收支项目";
            dataGridView1.Columns[5].HeaderText = "金额";
            dataGridView1.Columns[6].HeaderText = "备注";

            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 70;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[4].Width = 80;
            dataGridView1.Columns[5].Width = 120;
            dataGridView1.Columns[6].Width = 180;

            //文字居中
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //设定包括Header和所有单元格的列宽和行高自动调整
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        //刷新显示
        public void FreshData()
        {
            SqlConnection conn = new SqlConnection();//实例化一个连接
            conn.ConnectionString = ShareData.connection;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("select * from dbo.Consume", conn);
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }
        //初始化combo 2PCS
        public void IniCombo()
        {
            for (int i = 1; i <= 31; i++)
            {
                cboDay.Items.Add(i);
            }
            for (int i = 1; i <= 12; i++)
            {
                cboMonth.Items.Add(i);
            }
            for (int i = 2019; i <= 2029; i++)
            {
                cboYear.Items.Add(i);
            }

            cboType.Items.Add("收入");
            cboType.Items.Add("支出");

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

            cboName.Items.Add("Tod");
            cboName.Items.Add("Li");
        }
        public void IniComboSelect()
        {

            for (int i = 1; i <= 12; i++)
            {
                cboSelectMonth.Items.Add(i);
            }
            for (int i = 2019; i <= 2029; i++)
            {
                cboSelectYear.Items.Add(i);
            }
            cboSelectMonth.SelectedIndex = 0;
            cboSelectYear.SelectedIndex = 1;

        }

        #endregion
        //将数据插入到月度表中


        //月度查询
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            string dateValue = "";
            if (cboSelectYear.Text=="" || cboSelectMonth.Text=="")
            {
                MessageBox.Show("输入正确的年份或月份");
                return;
            }
            try
            {
                if (Convert.ToInt32(cboSelectMonth.Text.Trim())<=9)
            {
                dateValue = cboSelectYear.Text + "-" +'0'+ cboSelectMonth.Text + "%";
            }
            else
            {
                dateValue = cboSelectYear.Text + "-" + cboSelectMonth.Text + "%";
            }
                ShareData.selectYearMonth = dateValue;
            decimal a = 0;
            decimal b = 0;            
            string sql = string.Format("select sum(ConsumeMoney) from DBO.Consume where Type='收入' and ConsumeDate like '{0}'",dateValue);
            string sql1 = string.Format("select sum(ConsumeMoney) from DBO.Consume where Type='支出'and ConsumeDate like '{0}'",dateValue);
            string connString = ShareData.connection; ;
            int count = 0;
            SqlDataReader reader;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    count = cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtSelectIn.Text = reader[0].ToString();
                        a = decimal.Parse(txtSelectIn.Text);
                    }
                }
            }
            using (SqlConnection connn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    connn.Open();
                    cmd.Connection = connn;
                    cmd.CommandText = sql1;
                    count = cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtSelectOut.Text = reader[0].ToString();
                        b = decimal.Parse(txtSelectOut.Text);
                    }
                }

            }
            txtSelectSurplus.Text = (a-b).ToString().Trim();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
        //月度汇总输出
        private void BtnMonthCount_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();//实例化一个连接
            conn.ConnectionString = ShareData.connection;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("select distinct * from dbo.MonthCount", conn);
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void BtnIntoMonthCount_Click(object sender, EventArgs e)
        {
            string sql = string.Format("insert into MonthCount (Year,Month,PayCount,IncomeCount,Count) values('{0}','{1}','{2}','{3}','{4}')", cboSelectYear.Text.Trim(), cboSelectMonth.Text.Trim(), txtSelectOut.Text.Trim(), txtSelectIn.Text.Trim(),  txtSelectSurplus.Text);
            DataCmd dc = new DataCmd();
            dc.DataBaseCommand(sql);
        }

        private void btnMonthDisplay_Click(object sender, EventArgs e)
        {
            Display f = new Display();
            f.Show();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }

}
