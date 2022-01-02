using FindFieldInAllDB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindFieldInAllDB
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            LogHelper.LogAction = this.AddLog;
        }

        string glTableFileName = "过滤的表名.txt";
        string glColFileName = "过滤的列名.txt";
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(glColFileName))
            {
                txtColName.Text = File.ReadAllText(glColFileName);
            }

            if (File.Exists(glTableFileName))
            {
                txtTableName.Text = File.ReadAllText(glTableFileName);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("请输入要查询的内容");
                return;
            }
            if (backgroundWorker1.IsBusy)
            {
                return;
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("请输入要查询的内容");
                return;
            }
            if (backgroundWorker2.IsBusy)
            {
                return;
            }
            backgroundWorker2.RunWorkerAsync();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public void AddSQLResult(string msg)
        {
            txtSQLResult.AppendText(msg + "\r\n");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            txtSQLResult.Text = "";
            List<SQLModel> AllSQLModel = DBManager.GetQuerySQL();
            List<string> guolvCol = txtColName.Text.ToUpper().Split('\r').ToList().Where(K => !string.IsNullOrWhiteSpace(K)).ToList();

            List<string> guolvTableName = txtTableName.Text.ToUpper().Split('\r').ToList().Where(K => !string.IsNullOrWhiteSpace(K)).ToList();

            List<string> AllSql = new List<string>();

            for (int i = 0; i < AllSQLModel.Count; i++)
            {
                SQLModel SQLModel = AllSQLModel[i];

                if (guolvTableName.Contains(SQLModel.TableName.ToUpper()))
                {
                    continue;
                }

                if (guolvCol.Contains(SQLModel.ColumnName.ToUpper()))
                {
                    continue;
                }
                AddSQLResult(SQLModel.GetSQL(txtInput.Text));
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }


        public void AddLog(string msg)
        {
            txtlog.AppendText(msg + "\r\n");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            File.Delete(glTableFileName);
            File.WriteAllText(glTableFileName,txtTableName.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.Delete(glColFileName);
            File.WriteAllText(glColFileName, txtColName.Text);
        }
    }
}
