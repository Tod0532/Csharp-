using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace Shield_Wall
{    
    class DataCmd
    {
        public  int count=0;

        //数据库操作指令
        public void DataBaseCommand(string command)
        {
            try
            {
                string connString = ShareData.connection;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = command;
                        count = cmd.ExecuteNonQuery();
                        if (count != 0)
                        {
                            MessageBox.Show("数据操作成功！");

                        }
                        else
                        {
                            MessageBox.Show("数据操作失败！");
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
