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

namespace Shield_Wall
{   
    public partial class Display : Form
    {
        public string sql = "";
      
        public Display()
        {
            InitializeComponent();
            InitCboSelectItem();
            InitChartlet();
            CboSelectChart();
            cboSelectItem.SelectedIndex = 0;
        }

        private void Display_Load(object sender, EventArgs e)
        {
   

        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }
        private void InitCboSelectItem()
        {
            cboSelectItem.Items.Add("花费统计");
            cboSelectItem.Items.Add("收入统计");
            cboSelectItem.Items.Add("收入支出统计");
        }

        private void CboSelectChart()
        {
            switch (cboSelectItem.SelectedIndex)
            {
                case 0:
                    sql = string.Format("select Category, sum(ConsumeMoney) from DBO.Consume where Type='支出' and ConsumeDate like '{0}' group by Category", ShareData.selectYearMonth);
                    break;
                case 1:
                    sql = string.Format("select Category, sum(ConsumeMoney) from DBO.Consume where Type='收入' and ConsumeDate like '{0}' group by Category", ShareData.selectYearMonth);
                    break;
                case 2:
                    sql = string.Format("select type, sum(ConsumeMoney) from DBO.Consume where ConsumeDate like '{0}' group by type", ShareData.selectYearMonth);
                    break;
                default:
                    break;
            }
        }


        private void cboSelectChange(object sender, EventArgs e)
        {
            try
            {
                CboSelectChart();
                SqlConnection conn = new SqlConnection();//实例化一个连接
                conn.ConnectionString = ShareData.connection;
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(sql, conn);
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                chartlet1.BindChartData(ds);
                chartlet1.Refresh();
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void InitChartlet()
        {          
            chartlet1.ChartTitle.Text = "图表分析";
            chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Pie_3D_Breeze_FlatCrystal_NoGlow_NoBorder;
           // chartlet1.Background.Paper = Color.FromArgb(0, 0, 0, 0);
           
        }

        private void Display_FormClosed(object sender, FormClosedEventArgs e)
        {
            //类似于所有控件，需要在推出前嗲用Dispose()方法来释放资源
            chartlet1.Dispose();
        }
    }
}
