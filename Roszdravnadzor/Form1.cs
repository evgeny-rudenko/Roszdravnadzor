using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Roszdravnadzor
{
    public partial class Form1 : Form
    {
        private int cntTimer = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            //using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.farmaConnectionString))
            //{
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.farmaConnectionString);
                String gsql = GetSQLFromResource("Roszdravnadzor.SQL.Roszdrav.sql");
                connection.Open();

                    timer1.Enabled = true;
                    Task.Factory.StartNew(() => ExportTable(connection, gsql, "_RST.txt" ));
                //   ExportTable(connection, gsql, "_RST.txt");



            //}

            /*
             * 
             * 
        SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = Zdrav_CSV + comboBox1.Text + "_" + comboBox2.Text + ".csv";
            StreamWriter FS = new StreamWriter(sf.FileName, true, System.Text.Encoding.GetEncoding(1251));
            BindingSource bs = new BindingSource();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("d2a_roszdravnadzor_csv", get_constr());
            //    DataRow dr = dt.NewRow();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
            bs.DataSource = dt;
            bs.MoveFirst();

            FS.Write("DrugID;PackNx;MnfID;PckID;Segment;Year;Month;IRECID;Series;Quantity;Funds;VendorID;MnfPrice;PrcPrice;RtlPrice;Remark;SrcOrg" + Environment.NewLine);
            for (int i = 1; i <= bs.Count; i++)
            {
                DataRow dr = dt.Rows[bs.Position];
                // this.textBox1.Text = dr["name"].ToString();


             listBox1.Items.Add(dr["data"].ToString());

              FS.Write(dr["data"].ToString() + Environment.NewLine);
                bs.MoveNext();
            }
            
            FS.Close();       
    */
        }
        /// <summary>
        /// Загрузка из ресурсов текстовых файлов - чтобы скрипты были внутри программы
        /// </summary>
        /// <param name="ResName"></param>
        /// <returns></returns>
        public static string GetSQLFromResource(string ResName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = ResName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }


        }
        private  void ExportTable(SqlConnection connection, string tableName, string fName)
        {
            fName = DateTime.Now.Year.ToString()+ DateTime.Now.Month.ToString()+".csv";// Properties.Settings.Default.ID + "_" + Properties.Settings.Default.SubID + "_" + DateTime.Now.ToString("yyyyMMdd") + "T" + DateTime.Now.ToString("HHmm") + fName;

           

           // Console.WriteLine("Writing " + fName);
            using (var output = new StreamWriter(Path.Combine(Properties.Settings.Default.ExportPath, fName), false, Encoding.GetEncoding("Windows-1251"))) // добавить дату fname
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = tableName;
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        //output.WriteLine("DrugID;PackNx;MnfID;PckID;Segment;Year;Month;IRECID;Series;Quantity;Funds;VendorID;MnfPrice;PrcPrice;RtlPrice;Remark;SrcOrg");
                          output.WriteLine("DrugID;Segment;Year;Month;Series;TotDrugQn;MnfPrice;PrcPrice;RtlPrice;Funds;VendorID;Remark;SrcOrg");

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {

                                //    output.Write('|');
                                String v = reader[i].ToString();

                                output.Write(v);
                            }
                            //output.Write("|"); // в конце строки разделитель не ставим
                            output.WriteLine();
                        }
                    }
                }
            }
            
            timer1.Enabled = false;
           // button1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            cntTimer++;
            label3.Text = cntTimer.ToString();
        }
    }
}
