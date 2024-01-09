using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private SQLiteConnection SQLiteConn;
        private DataTable dTable;
        private SQLiteCommand SQLiteComm;

        List<List<int>> Blocks = new List<List<int>>();
        List<List<int>> Blocks2 = new List<List<int>>();
        List<List<List<int>>> MiniBlocks = new List<List<List<int>>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SQLiteConn = new SQLiteConnection();
            dTable = new DataTable();
            SQLiteComm = new SQLiteCommand();

            tabPage2.Enabled = false;
            tabPage3.Enabled = false;
            tabPage4.Enabled = false;
            tabPage5.Enabled = false;

            button2.Enabled = false;
            button3.Enabled = false;

            tabPage7.Enabled = false;

            tabPage9.Enabled = false;
        }
        private bool OpenDBFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Текстовые файлы (*.SQLite)|*.SQLite| Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SQLiteConn = new SQLiteConnection("Data Source =" + openFileDialog.FileName + ";Version = 3;");
                SQLiteConn.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = SQLiteConn;
                return true;
            }
            else return false;
        }

        private void ShowTable(string tableName)
        {
            string SQLQue;
            string SQLQuery = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name;";
            SQLiteCommand command = new SQLiteCommand(SQLQuery, SQLiteConn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString() == tableName)
                {
                    SQLQue = "SELECT * FROM [" + reader[0].ToString() + "] order by 1";

                    dTable.Clear();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(SQLQue, SQLiteConn);
                    adapter.Fill(dTable);

                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();


                    for (int col = 0; col < dTable.Columns.Count; col++)
                    {
                        string ColName = dTable.Columns[col].ColumnName;
                        dataGridView1.Columns.Add(ColName, ColName);
                        dataGridView1.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    for (int row = 0; row < dTable.Rows.Count; row++)
                    {
                        dataGridView1.Rows.Add(dTable.Rows[row].ItemArray);
                    }
                }
            }
            while (Convert.ToString(dataGridView1.Rows[0].Cells[dataGridView1.ColumnCount - 1].Value) == "")
            {
                dataGridView1.Columns.RemoveAt(dataGridView1.ColumnCount - 1);
            }
        }

        private void GetTablesNames()
        {
            string SQLQuery = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name;";
            SQLiteCommand command = new SQLiteCommand(SQLQuery, SQLiteConn);
            SQLiteDataReader reader = command.ExecuteReader();
            comboBox1.Items.Clear();
            while (reader.Read())
            {
                if (!reader[0].ToString().StartsWith("Бонус"))
                {
                    comboBox1.Items.Add(reader[0].ToString());
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void ShowPicture()
        {
            SQLiteComm = SQLiteConn.CreateCommand();
            SQLiteComm.CommandText = "SELECT picture FROM Бонус";
            var da = new SQLiteDataAdapter(SQLiteComm);
            var ds = new DataSet();
            da.Fill(ds, "Бонус");
            int count = ds.Tables["Бонус"].Rows.Count;

            if (count > 0)
            {
                var data = (Byte[])ds.Tables["Бонус"].Rows[0]["picture"];
                var stream = new MemoryStream(data);
                pictureBox1.Image = Image.FromStream(stream);
                pictureBox2.Image = Image.FromStream(stream);
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void ShowE()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            SQLiteComm = SQLiteConn.CreateCommand();
            SQLiteComm.CommandText = "SELECT E FROM Бонус";
            var daE = new SQLiteDataAdapter(SQLiteComm);
            var dsE = new DataSet();
            daE.Fill(dsE, "Бонус");
            int countE = dsE.Tables["Бонус"].Rows.Count;

            if (countE > 0)
            {
                var dataE = dsE.Tables["Бонус"].Rows[0]["E"];
                toolStripStatusLabel1.Text = "E = " + dataE.ToString();
                textBox1.Text = dataE.ToString();
            }
        }

        private void ShowA()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            SQLiteComm = SQLiteConn.CreateCommand();
            SQLiteComm.CommandText = "SELECT A FROM Бонус";
            var daA = new SQLiteDataAdapter(SQLiteComm);
            var dsA = new DataSet();
            daA.Fill(dsA, "Бонус");
            int countA = dsA.Tables["Бонус"].Rows.Count;

            if (countA > 0)
            {
                var dataA = dsA.Tables["Бонус"].Rows[0]["A"];
                toolStripStatusLabel2.Text = "A = " + dataA.ToString();
                textBox2.Text = dataA.ToString();
            }
        }

        private void Object_Count()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            SQLiteComm = SQLiteConn.CreateCommand();
            SQLiteComm.CommandText = "SELECT count FROM Бонус";
            var daCount = new SQLiteDataAdapter(SQLiteComm);
            var dsCount = new DataSet();
            daCount.Fill(dsCount, "Бонус");
            int countA = dsCount.Tables["Бонус"].Rows.Count;

            if (countA > 0)
            {
                var dataCount = dsCount.Tables["Бонус"].Rows[0]["count"];
                for (int i = 0; i < Convert.ToInt32(dataCount); i++)
                {
                    Blocks.Add(new List<int>());
                }
                Blocks.Add(new List<int>());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OpenDBFile() == true)
            {
                Blocks.Clear();

                GetTablesNames();
                ShowTable(comboBox1.Text);
                ShowPicture();
                ShowE();
                ShowA();
                Object_Count();

                for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
                {
                    Blocks[0].Add(i + 1);
                }

                tabPage2.Enabled = true;
                tabPage3.Enabled = true;
                tabPage5.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void Choose_Table(object sender, EventArgs e)
        {
            ShowTable(comboBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            int p = dataGridView1.RowCount - 1;
            double k = 0;
            string col = "\"Эпоха\"", value = ") VALUES (" + "\"" + p + "\"";

            for (int i = 0; i < dataGridView1.RowCount - 2; i++)
            {
                if (k < Math.Abs(Convert.ToDouble(dataGridView1.Rows[i + 1].Cells[1].Value) - Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value)))
                    k = Math.Abs(Convert.ToDouble(dataGridView1.Rows[i + 1].Cells[1].Value) - Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value));
            }

            Random rnd = new Random();
            SQLiteComm = SQLiteConn.CreateCommand();
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                col += ", " + "\"" + i + "\"";
                value += ", \"" + (Convert.ToDouble(dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[i].Value) + (rnd.Next(-Convert.ToInt32(Math.Round(k, 4) * 10000), Convert.ToInt32(Math.Round(k, 4) * 10000))) / 10000.0) + "\"";
            }
            SQLiteComm.CommandText = "INSERT INTO " + comboBox1.Text + " (" + col + value + ");";
            SQLiteComm.ExecuteNonQuery();
            ShowTable(comboBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int p = dataGridView1.RowCount - 2;
            SQLiteComm = SQLiteConn.CreateCommand();
            SQLiteComm.CommandText = "DELETE FROM " + comboBox1.Text + " WHERE Эпоха = (" + p + ");";
            SQLiteComm.ExecuteNonQuery();
            ShowTable(comboBox1.Text);
        }

        private void Schet(DataGridView dataGridViewData, DataGridView dataGridViewResult)
        {
            double s;


            //                                                     //


            string mplus = "M+";
            string m = "M";
            string mminus = "M-";
            string aplus = "A+";
            string a = "A";
            string aminus = "A-";
            string mplusprognoz = "M+ прогнозное";
            string mprognoz = "M прогнозное";
            string mminusprognoz = "M- прогнозное";
            string aplusprognoz = "A+ прогнозное";
            string aprognoz = "A прогнозное";
            string aminusprognoz = "A- прогнозное";
            string r = "R";
            string l = "L";
            string status = "Состояние";


            //                                                     //


            dataGridViewResult.Columns.Clear();
            dataGridViewResult.Rows.Clear();


            //                                                     //


            dataGridViewResult.Columns.Add(mplus, mplus);
            dataGridViewResult.Columns.Add(m, m);
            dataGridViewResult.Columns.Add(mminus, mminus);
            dataGridViewResult.Columns.Add(aplus, aplus);
            dataGridViewResult.Columns.Add(a, a);
            dataGridViewResult.Columns.Add(aminus, aminus);
            dataGridViewResult.Columns.Add(mplusprognoz, mplusprognoz);
            dataGridViewResult.Columns.Add(mprognoz, mprognoz);
            dataGridViewResult.Columns.Add(mminusprognoz, mminusprognoz);
            dataGridViewResult.Columns.Add(aplusprognoz, aplusprognoz);
            dataGridViewResult.Columns.Add(aprognoz, aprognoz);
            dataGridViewResult.Columns.Add(aminusprognoz, aminusprognoz);
            dataGridViewResult.Columns.Add(r, r);
            dataGridViewResult.Columns.Add(l, l);
            dataGridViewResult.Columns.Add(status, status);


            //                                                     //


            if (textBox1.Text.Length == 0)
            {
                textBox1.Text = "0.003";
            }
            if (textBox2.Text.Length == 0)
            {
                textBox2.Text = "0.9";
            }


            //                                                     //


            for (int i = 0; i < 15; i++)
                dataGridViewResult.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            for (int row = 0; row < dataGridViewData.RowCount - 1; row++)
            {
                dataGridViewResult.Rows.Add();
            }


            //                                                     //


            double[,] marks = new double[dataGridViewData.RowCount - 1, dataGridViewData.ColumnCount - 1];
            for (int i = 0; i < dataGridViewData.RowCount - 1; i++)
            {
                for (int j = 1; j < dataGridViewData.ColumnCount; j++)
                {
                    marks[i, j - 1] = Convert.ToDouble(dataGridViewData.Rows[i].Cells[j].Value);
                }
            }


            //                                                     //


            dataGridViewResult.RowHeadersWidth = 10 * "Эпоха".Length;
            dataGridViewResult.TopLeftHeaderCell.Value = string.Format("Эпоха", "0");
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                dataGridViewResult.Rows[i].HeaderCell.Value = string.Format((i).ToString(), "0");
            }


            //                                                     //


            double[] mplusMas = new double[marks.GetLength(0)];
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                s = 0;
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    s = s + Math.Pow(Convert.ToDouble(marks[i, j]) + Convert.ToDouble(textBox1.Text), 2);
                }
                mplusMas[i] = Math.Sqrt(s);
                dataGridViewResult.Rows[i].Cells[0].Value = Math.Round(mplusMas[i], 4);
            }


            //                                                     //


            double[] mMas = new double[marks.GetLength(0)];
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                s = 0;
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    s = s + Math.Pow(Convert.ToDouble(marks[i, j]), 2);
                }
                mMas[i] = Math.Sqrt(s);
                dataGridViewResult.Rows[i].Cells[1].Value = Math.Round(mMas[i], 4);
            }


            //                                                     //


            double[] mminusMas = new double[marks.GetLength(0)];
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                s = 0;
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    s = s + Math.Pow(Convert.ToDouble(marks[i, j]) - Convert.ToDouble(textBox1.Text), 2);
                }
                mminusMas[i] = Math.Sqrt(s);
                dataGridViewResult.Rows[i].Cells[2].Value = Math.Round(mminusMas[i], 4);
            }


            //                                                     //


            double[] aplusMas = new double[marks.GetLength(0)];
            aplusMas[0] = 0;
            dataGridViewResult.Rows[0].Cells[3].Value = "0";
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                s = 0;
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    s = s + (marks[0, j] + Convert.ToDouble(textBox1.Text)) * (marks[i, j] + Convert.ToDouble(textBox1.Text));
                }
                s = s / (mplusMas[0] * mplusMas[i]);
                if (Math.Acos(s) == double.NaN || mplusMas[0] == mplusMas[i])
                {
                    s = 1;
                }
                aplusMas[i] = Math.Acos(s);
                dataGridViewResult.Rows[i].Cells[3].Value = Math.Round(aplusMas[i] * ((3600 * 180) / Math.PI), 6).ToString();
            }


            //                                                     //


            double[] aMas = new double[marks.GetLength(0)];
            aMas[0] = 0;
            dataGridViewResult.Rows[0].Cells[4].Value = "0";
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                s = 0;
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    s = s + (marks[0, j] * marks[i, j]);
                }
                s = s / (mMas[0] * mMas[i]);
                if (Math.Acos(s) == double.NaN || mMas[0] == mMas[i])
                {
                    s = 1;
                }
                aMas[i] = Math.Acos(s);
                dataGridViewResult.Rows[i].Cells[4].Value = Math.Round(aMas[i] * ((3600 * 180) / Math.PI), 6).ToString();
            }


            //                                                     //


            double[] aminusMas = new double[marks.GetLength(0)];
            aminusMas[0] = 0;
            dataGridViewResult.Rows[0].Cells[5].Value = "0";
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                s = 0;
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    s = s + (marks[0, j] - Convert.ToDouble(textBox1.Text)) * (marks[i, j] - Convert.ToDouble(textBox1.Text));
                }
                s = s / (mminusMas[0] * mminusMas[i]);
                if (Math.Acos(s) == double.NaN || mminusMas[0] == mminusMas[i])
                {
                    s = 1;
                }
                aminusMas[i] = Math.Acos(s);
                dataGridViewResult.Rows[i].Cells[5].Value = Math.Round(aminusMas[i] * ((3600 * 180) / Math.PI), 6).ToString();
            }


            //                                                     //


            double[] mplusPrognoz = new double[marks.GetLength(0)];
            mplusPrognoz[0] = Convert.ToDouble(textBox2.Text) * mplusMas[0] + (1 - Convert.ToDouble(textBox2.Text)) * mplusMas.Average();
            dataGridViewResult.Rows[0].Cells[6].Value = Math.Round(mplusPrognoz[0], 4);
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                mplusPrognoz[i] = Convert.ToDouble(textBox2.Text) * mplusMas[i] + (1 - Convert.ToDouble(textBox2.Text)) * mplusPrognoz[i - 1];
                dataGridViewResult.Rows[i].Cells[6].Value = Math.Round(mplusPrognoz[i], 4);
            }
            double mplusPrognozZnach = Convert.ToDouble(textBox2.Text) * mplusPrognoz.Average() + (1 - Convert.ToDouble(textBox2.Text)) * mplusPrognoz[mplusPrognoz.Length - 1];
            dataGridViewResult.Rows[mplusPrognoz.Length].Cells[6].Value = Math.Round(mplusPrognozZnach, 4);


            //                                                     //


            double[] mPrognoz = new double[marks.GetLength(0)];
            mPrognoz[0] = Convert.ToDouble(textBox2.Text) * mMas[0] + (1 - Convert.ToDouble(textBox2.Text)) * mMas.Average();
            dataGridViewResult.Rows[0].Cells[7].Value = Math.Round(mPrognoz[0], 4);
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                mPrognoz[i] = Convert.ToDouble(textBox2.Text) * mMas[i] + (1 - Convert.ToDouble(textBox2.Text)) * mPrognoz[i - 1];
                dataGridViewResult.Rows[i].Cells[7].Value = Math.Round(mPrognoz[i], 4);
            }
            double mPrognozZnach = Convert.ToDouble(textBox2.Text) * mPrognoz.Average() + (1 - Convert.ToDouble(textBox2.Text)) * mPrognoz[mPrognoz.Length - 1];
            dataGridViewResult.Rows[mPrognoz.Length].Cells[7].Value = Math.Round(mPrognozZnach, 4);


            //                                                     //


            double[] mminusPrognoz = new double[marks.GetLength(0)];
            mminusPrognoz[0] = Convert.ToDouble(textBox2.Text) * mminusMas[0] + (1 - Convert.ToDouble(textBox2.Text)) * mminusMas.Average();
            dataGridViewResult.Rows[0].Cells[8].Value = Math.Round(mminusPrognoz[0], 4);
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                mminusPrognoz[i] = Convert.ToDouble(textBox2.Text) * mminusMas[i] + (1 - Convert.ToDouble(textBox2.Text)) * mminusPrognoz[i - 1];
                dataGridViewResult.Rows[i].Cells[8].Value = Math.Round(mminusPrognoz[i], 4);
            }
            double mminusPrognozZnach = Convert.ToDouble(textBox2.Text) * mminusPrognoz.Average() + (1 - Convert.ToDouble(textBox2.Text)) * mminusPrognoz[mminusPrognoz.Length - 1];
            dataGridViewResult.Rows[mminusPrognoz.Length].Cells[8].Value = Math.Round(mminusPrognozZnach, 4);


            //                                                     //


            double[] aplusPrognoz = new double[marks.GetLength(0)];
            aplusPrognoz[0] = Convert.ToDouble(textBox2.Text) * aplusMas[0] + (1 - Convert.ToDouble(textBox2.Text)) * aplusMas.Average();
            dataGridViewResult.Rows[0].Cells[9].Value = Math.Round(aplusPrognoz[0] * ((3600 * 180) / Math.PI), 6);
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                aplusPrognoz[i] = Convert.ToDouble(textBox2.Text) * aplusMas[i] + (1 - Convert.ToDouble(textBox2.Text)) * aplusPrognoz[i - 1];
                dataGridViewResult.Rows[i].Cells[9].Value = Math.Round(aplusPrognoz[i] * ((3600 * 180) / Math.PI), 6);
            }
            double aplusPrognozZnach = Convert.ToDouble(textBox2.Text) * aplusMas.Average() + (1 - Convert.ToDouble(textBox2.Text)) * aplusPrognoz[aplusPrognoz.Length - 1];
            dataGridViewResult.Rows[aplusPrognoz.Length].Cells[9].Value = Math.Round(aplusPrognozZnach * ((3600 * 180) / Math.PI), 6);


            //                                                     //


            double[] aPrognoz = new double[marks.GetLength(0)];
            aPrognoz[0] = Convert.ToDouble(textBox2.Text) * aMas[0] + (1 - Convert.ToDouble(textBox2.Text)) * aMas.Average();
            dataGridViewResult.Rows[0].Cells[10].Value = Math.Round(aPrognoz[0] * ((3600 * 180) / Math.PI), 6);
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                aPrognoz[i] = Convert.ToDouble(textBox2.Text) * aMas[i] + (1 - Convert.ToDouble(textBox2.Text)) * aPrognoz[i - 1];
                dataGridViewResult.Rows[i].Cells[10].Value = Math.Round(aPrognoz[i] * ((3600 * 180) / Math.PI), 6);
            }
            double aPrognozZnach = Convert.ToDouble(textBox2.Text) * aMas.Average() + (1 - Convert.ToDouble(textBox2.Text)) * aPrognoz[aPrognoz.Length - 1];
            dataGridViewResult.Rows[aPrognoz.Length].Cells[10].Value = Math.Round(aPrognozZnach * ((3600 * 180) / Math.PI), 6);


            //                                                     //


            double[] aminusPrognoz = new double[marks.GetLength(0)];
            aminusPrognoz[0] = Convert.ToDouble(textBox2.Text) * aminusMas[0] + (1 - Convert.ToDouble(textBox2.Text)) * aminusMas.Average();
            dataGridViewResult.Rows[0].Cells[11].Value = Math.Round(aminusPrognoz[0] * ((3600 * 180) / Math.PI), 6);
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                aminusPrognoz[i] = Convert.ToDouble(textBox2.Text) * aminusMas[i] + (1 - Convert.ToDouble(textBox2.Text)) * aminusPrognoz[i - 1];
                dataGridViewResult.Rows[i].Cells[11].Value = Math.Round(aminusPrognoz[i] * ((3600 * 180) / Math.PI), 6);
            }
            double aminusPrognozZnach = Convert.ToDouble(textBox2.Text) * aminusMas.Average() + (1 - Convert.ToDouble(textBox2.Text)) * aminusPrognoz[aminusPrognoz.Length - 1];
            dataGridViewResult.Rows[aminusPrognoz.Length].Cells[11].Value = Math.Round(aminusPrognozZnach * ((3600 * 180) / Math.PI), 6);


            //                                                     //


            double[] rMas = new double[marks.GetLength(0)];
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                rMas[i] = Math.Abs(mplusMas[i] - mminusMas[i]) / 2;
                dataGridViewResult.Rows[i].Cells[12].Value = Math.Round(rMas[i], 7);
            }
            double rPrognoz = Math.Abs(mplusPrognozZnach - mminusPrognozZnach) / 2;
            dataGridViewResult.Rows[rMas.Length].Cells[12].Value = Math.Round(rPrognoz, 7);


            //                                                     //


            double[] lMas = new double[marks.GetLength(0)];
            lMas[0] = 0;
            dataGridViewResult.Rows[0].Cells[13].Value = "0";
            for (int i = 1; i < marks.GetLength(0); i++)
            {
                lMas[i] = Math.Abs(mMas[i] - mMas[0]);
                dataGridViewResult.Rows[i].Cells[13].Value = Math.Round(lMas[i], 7).ToString();
            }
            double lPrognoz = Math.Abs(mPrognozZnach - mMas[0]);
            dataGridViewResult.Rows[lMas.Length].Cells[13].Value = Math.Round(lPrognoz, 7).ToString();


            //                                                     //


            string[] statusMas = new string[marks.GetLength(0)];
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                if (rMas[i] - lMas[i] == 0)
                {
                    statusMas[i] = "Предаварийное";
                    dataGridViewResult.Rows[i].Cells[14].Value = statusMas;
                    dataGridViewResult.Rows[i].Cells[14].Style.BackColor = Color.LightYellow;
                }
                else if (rMas[i] < lMas[i])
                {
                    statusMas[i] = "Аварийное";
                    dataGridViewResult.Rows[i].Cells[14].Value = statusMas[i];
                    dataGridViewResult.Rows[i].Cells[14].Style.BackColor = Color.Red;
                }
                else
                {
                    statusMas[i] = "Не аварийное";
                    dataGridViewResult.Rows[i].Cells[14].Value = statusMas[i];
                    dataGridViewResult.Rows[i].Cells[14].Style.BackColor = Color.LightGreen;
                }
            }
            string statusPrognoz;
            if (rPrognoz - lPrognoz == 0)
            {
                statusPrognoz = "Предаварийное";
                dataGridViewResult.Rows[statusMas.Length].Cells[14].Value = statusPrognoz;
                dataGridViewResult.Rows[statusMas.Length].Cells[14].Style.BackColor = Color.Yellow;
            }
            else if (rPrognoz < lPrognoz)
            {
                statusPrognoz = "Аварийное";
                dataGridViewResult.Rows[statusMas.Length].Cells[14].Value = statusPrognoz;
                dataGridViewResult.Rows[statusMas.Length].Cells[14].Style.BackColor = Color.Red;
            }
            else
            {
                statusPrognoz = "Не аварийное";
                dataGridViewResult.Rows[statusMas.Length].Cells[14].Value = statusPrognoz;
                dataGridViewResult.Rows[statusMas.Length].Cells[14].Style.BackColor = Color.LightGreen;
            }
        }

        private void Level1(object sender, LayoutEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                Schet(dataGridView1, dataGridView2);
            }
        }

        private void GrafikMA(Chart chart, CheckedListBox checkedListBox, DataGridView dataGridView)
        {
            chart.Series.Clear();

            double x, y, amax = double.MinValue, mmax = double.MinValue, mmin = double.MaxValue;

            //chart.ChartAreas[0].AxisY.Interval = 1;

            if (checkedListBox.GetItemChecked(0))
            {
                chart.Series.Add(new Series("График верхней границы"));

                chart.Series["График верхней границы"].ChartType = SeriesChartType.Spline;
                chart.Series["График верхней границы"].Enabled = true;
                chart.Series["График верхней границы"].BorderWidth = 2;
                chart.Series["График верхней границы"].MarkerSize = 5;
                chart.Series["График верхней границы"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount - 1; i++)
                {
                    x = Convert.ToDouble(dataGridView.Rows[i].Cells[0].Value);
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[3].Value);
                    chart.Series["График верхней границы"].Points.AddXY(x, y);
                    chart.Series["График верхней границы"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[0].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[0].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[0].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[0].Value);
                    }

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[3].Value) > amax)
                    {
                        amax = Convert.ToDouble(dataGridView.Rows[i].Cells[3].Value);
                    }
                }
            }

            if (checkedListBox.GetItemChecked(1))
            {
                chart.Series.Add(new Series("График фазовых координат"));

                chart.Series["График фазовых координат"].ChartType = SeriesChartType.Spline;
                chart.Series["График фазовых координат"].Enabled = true;
                chart.Series["График фазовых координат"].BorderWidth = 2;
                chart.Series["График фазовых координат"].MarkerSize = 5;
                chart.Series["График фазовых координат"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount - 1; i++)
                {
                    x = Convert.ToDouble(dataGridView.Rows[i].Cells[1].Value);
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[4].Value);
                    chart.Series["График фазовых координат"].Points.AddXY(x, y);
                    chart.Series["График фазовых координат"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[1].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[1].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[1].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[1].Value);
                    }

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[4].Value) > amax)
                    {
                        amax = Convert.ToDouble(dataGridView.Rows[i].Cells[4].Value);
                    }
                }
            }

            if (checkedListBox.GetItemChecked(2))
            {
                chart.Series.Add(new Series("График нижней границы"));

                chart.Series["График нижней границы"].ChartType = SeriesChartType.Spline;
                chart.Series["График нижней границы"].Enabled = true;
                chart.Series["График нижней границы"].BorderWidth = 2;
                chart.Series["График нижней границы"].MarkerSize = 5;
                chart.Series["График нижней границы"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount - 1; i++)
                {
                    x = Convert.ToDouble(dataGridView.Rows[i].Cells[2].Value);
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[5].Value);
                    chart.Series["График нижней границы"].Points.AddXY(x, y);
                    chart.Series["График нижней границы"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[2].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[2].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[2].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[2].Value);
                    }

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[5].Value) > amax)
                    {
                        amax = Convert.ToDouble(dataGridView.Rows[i].Cells[5].Value);
                    }
                }
            }

            if (checkedListBox.GetItemChecked(3))
            {
                chart.Series.Add(new Series("График прогнозов"));

                chart.Series["График прогнозов"].ChartType = SeriesChartType.Spline;
                chart.Series["График прогнозов"].Enabled = true;
                chart.Series["График прогнозов"].BorderWidth = 2;
                chart.Series["График прогнозов"].MarkerSize = 5;
                chart.Series["График прогнозов"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    x = Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value);
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[10].Value);
                    chart.Series["График прогнозов"].Points.AddXY(x, y);
                    chart.Series["График прогнозов"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value);
                    }

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[10].Value) > amax)
                    {
                        amax = Convert.ToDouble(dataGridView.Rows[i].Cells[10].Value);
                    }
                }
            }
            
            if (amax == 0)
            {
                amax = Convert.ToDouble(textBox1.Text);
            }

            chart.ChartAreas[0].AxisX.Interval = Math.Round((mmax - mmin) / 6, 4);
            chart.ChartAreas[0].AxisX.Minimum = mmin - chart.ChartAreas[0].AxisX.Interval;
            chart.ChartAreas[0].AxisX.Maximum = mmax + chart.ChartAreas[0].AxisX.Interval;
            chart.ChartAreas[0].AxisY.Interval = Math.Round(amax / 6, 4);
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisY.Maximum = amax + chart.ChartAreas[0].AxisY.Interval;
            chart.ChartAreas[0].AxisX.Title = "М";
            chart.ChartAreas[0].AxisY.Title = "α";
        }

        private void GrafikA(Chart chart, CheckedListBox checkedListBox, DataGridView dataGridView)
        {
            chart.Series.Clear();

            double y, amax = double.MinValue;

            if (checkedListBox.GetItemChecked(0))
            {
                chart.Series.Add(new Series("A"));

                chart.Series["A"].ChartType = SeriesChartType.Spline;
                chart.Series["A"].Enabled = true;
                chart.Series["A"].BorderWidth = 2;
                chart.Series["A"].MarkerSize = 5;
                chart.Series["A"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount - 1; i++)
                {
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[4].Value);
                    chart.Series["A"].Points.AddXY(i, y);
                    chart.Series["A"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[4].Value) > amax)
                    {
                        amax = Convert.ToDouble(dataGridView.Rows[i].Cells[4].Value);
                    }
                }
            }

            if (amax == 0)
            {
                amax = Convert.ToDouble(textBox1.Text);
            }

            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisX.Maximum = dataGridView1.RowCount - 2;
            chart.ChartAreas[0].AxisY.Interval = Math.Round(amax / 6, 4);
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisY.Maximum = amax + chart.ChartAreas[0].AxisY.Interval;
            chart.ChartAreas[0].AxisX.Title = "t";
            chart.ChartAreas[0].AxisY.Title = "A";
        }

        private void GrafikMT(Chart chart, CheckedListBox checkedListBox, DataGridView dataGridView)
        {
            chart.Series.Clear();

            double y, mmax = double.MinValue, mmin = double.MaxValue;

            if (checkedListBox.GetItemChecked(0))
            {
                chart.Series.Add(new Series("M(t)+"));

                chart.Series["M(t)+"].ChartType = SeriesChartType.Spline;
                chart.Series["M(t)+"].Enabled = true;
                chart.Series["M(t)+"].BorderWidth = 2;
                chart.Series["M(t)+"].MarkerSize = 5;
                chart.Series["M(t)+"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[6].Value);
                    chart.Series["M(t)+"].Points.AddXY(i, y);
                    chart.Series["M(t)+"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[6].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[6].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[6].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[6].Value);
                    }
                }
            }

            if (checkedListBox.GetItemChecked(1))
            {
                chart.Series.Add(new Series("M(t)"));

                chart.Series["M(t)"].ChartType = SeriesChartType.Spline;
                chart.Series["M(t)"].Enabled = true;
                chart.Series["M(t)"].BorderWidth = 2;
                chart.Series["M(t)"].MarkerSize = 5;
                chart.Series["M(t)"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value);
                    chart.Series["M(t)"].Points.AddXY(i, y);
                    chart.Series["M(t)"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[7].Value);
                    }
                }
            }

            if (checkedListBox.GetItemChecked(2))
            {
                chart.Series.Add(new Series("M(t)-"));

                chart.Series["M(t)-"].ChartType = SeriesChartType.Spline;
                chart.Series["M(t)-"].Enabled = true;
                chart.Series["M(t)-"].BorderWidth = 2;
                chart.Series["M(t)-"].MarkerSize = 5;
                chart.Series["M(t)-"].MarkerStyle = MarkerStyle.Circle;

                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    y = Convert.ToDouble(dataGridView.Rows[i].Cells[8].Value);
                    chart.Series["M(t)-"].Points.AddXY(i, y);
                    chart.Series["M(t)-"].Points[i].Label = i.ToString();

                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[8].Value) > mmax)
                    {
                        mmax = Convert.ToDouble(dataGridView.Rows[i].Cells[8].Value);
                    }
                    if (Convert.ToDouble(dataGridView.Rows[i].Cells[8].Value) < mmin)
                    {
                        mmin = Convert.ToDouble(dataGridView.Rows[i].Cells[8].Value);
                    }
                }
            }
            
            if (mmax == mmin)
            {
                mmax += Convert.ToDouble(textBox1.Text);
                mmin -= Convert.ToDouble(textBox1.Text);
            }

            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisX.Maximum = dataGridView1.RowCount - 1;
            chart.ChartAreas[0].AxisY.Interval = Math.Round((mmax - mmin) / 6, 4);
            chart.ChartAreas[0].AxisY.Minimum = mmin - chart.ChartAreas[0].AxisY.Interval;
            chart.ChartAreas[0].AxisY.Maximum = mmax + chart.ChartAreas[0].AxisY.Interval;
            chart.ChartAreas[0].AxisX.Title = "t";
            chart.ChartAreas[0].AxisY.Title = "М";
        }

        private void Selecting_GrafikMA(object sender, EventArgs e)
        {
            GrafikMA(chart1, checkedListBox1, dataGridView2);
        }

        private void Selecting_GrafikA(object sender, EventArgs e)
        {
            GrafikA(chart2, checkedListBox2, dataGridView2);
        }

        private void Selecting_GrafikMT(object sender, EventArgs e)
        {
            GrafikMT(chart6, checkedListBox6, dataGridView2);
        }

        private void Level1_GrafickOFF(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedListBox6.Items.Count; i++)
            {
                checkedListBox6.SetItemChecked(i, false);
            }

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart6.Series.Clear();
        }

        private void Selecting_tabControl1(object sender, TabControlCancelEventArgs e)
        {
            if (!e.TabPage.Enabled)
            {
                e.Cancel = true;
            }
            if (tabControl1.SelectedTab == tabPage4)
            {
                tabControl3.SelectedTab = tabPage8;
            }
        }

        private void textBox1_Input(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(".") & !textBox1.Text.Contains(".") & (textBox1.SelectionStart != 0)) | (e.KeyChar == 8))
                return;
            else
                e.Handled = true;
        }
        private void textBox2_Input(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(".") & !textBox2.Text.Contains(".") & (textBox2.SelectionStart != 0)) | (e.KeyChar == 8))
                return;
            else
                e.Handled = true;
        }

        private void FixBlock_Count(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == 8))
                return;
            else
                e.Handled = true;
        }

        private void Epsilon_Fill(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "E = " + textBox1.Text;
        }


        private void Alpha_Fill(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "A = " + textBox2.Text;
        }


        private void Level4(object sender, LayoutEventArgs e)
        {
            checkedListBox5.Items.Clear();

            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                checkedListBox5.Items.Add("Марка " + (i + 1));
            }
        }

        private void Selecting_GrafickLevel4(object sender, EventArgs e)
        {
            chart5.Series.Clear();

            double y, hmax = double.MinValue, hmin = double.MaxValue;
            string cName;

            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                if (checkedListBox5.GetItemChecked(i))
                {
                    cName = "Марка " + (i + 1);
                    chart5.Series.Add(new Series(cName));

                    chart5.Series[cName].ChartType = SeriesChartType.Spline;
                    chart5.Series[cName].Enabled = true;
                    chart5.Series[cName].BorderWidth = 2;
                    chart5.Series[cName].MarkerSize = 5;
                    chart5.Series[cName].MarkerStyle = MarkerStyle.Circle;

                    for (int j = 0; j < dataGridView1.RowCount - 1; j++)
                    {
                        y = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value);
                        chart5.Series[cName].Points.AddXY(j, y);
                        chart5.Series[cName].Points[j].Label = j.ToString();
                        
                        if (Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value) > hmax)
                        {
                            hmax = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value);
                        }
                        if (Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value) < hmin)
                        {
                            hmin = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value);
                        }
                    }
                }
            }

            if (hmin == hmax)
            {
                hmax += Convert.ToDouble(textBox1.Text);
                hmin -= Convert.ToDouble(textBox1.Text);
            }

            chart5.ChartAreas[0].AxisX.Interval = 1;
            chart5.ChartAreas[0].AxisX.Minimum = 0;
            chart5.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(dataGridView1.RowCount - 2);
            chart5.ChartAreas[0].AxisY.Interval = Math.Round((hmax - hmin) / 6, 4);
            chart5.ChartAreas[0].AxisY.Minimum = hmin - chart5.ChartAreas[0].AxisY.Interval;
            chart5.ChartAreas[0].AxisY.Maximum = hmax + chart5.ChartAreas[0].AxisY.Interval;
        }
        private void Level4_GrafickOFF(object sender, EventArgs e)
        {
            chart5.Series.Clear();
        }


        private void Level2(object sender, LayoutEventArgs e)
        {
            listBox1.Items.Clear();

            if (Blocks.Count > 0)
            {
                foreach (var number in Blocks[0])
                {
                    listBox1.Items.Add(number);
                }
            }

            textBox3.Text = Convert.ToString(Blocks.Count - 1);
            Choose_BlocksCount();
        }

        private void Selecting_tabControl2(object sender, TabControlCancelEventArgs e)
        {
            int c = Blocks[1].Count;
            bool same = false;

            for (int i = 2; i < Blocks.Count; i++)
            {
                if (Blocks[i].Count == c)
                {
                    same = true;
                }
                else
                {
                    same = false;
                    break;
                }
            }

            if (Blocks[comboBox2.SelectedIndex + 1].Count > 1 && same)
            {
                tabPage7.Enabled = true;
            }
            else
            {
                tabPage7.Enabled = false;
            }
            if (!e.TabPage.Enabled)
            {
                e.Cancel = true;
            }
        }

        private void ShowBlocks()
        {
            listBox1.Items.Clear();
            Blocks[0].Sort();
            foreach (var number in Blocks[0])
            {
                listBox1.Items.Add(number);
            }

            listBox2.Items.Clear();
            Blocks[comboBox2.SelectedIndex + 1].Sort();
            foreach (var number in Blocks[comboBox2.SelectedIndex + 1])
            {
                listBox2.Items.Add(number);
            }
        }

        private void Level3_Access()
        {
            int c = Blocks[1].Count;
            bool same = false;
            for (int i = 2; i < Blocks.Count; i++)
            {
                if (Blocks[i].Count == c)
                {
                    same = true;
                }
                else
                {
                    same = false;
                    break;
                }
            }

            if (same == true && c >= 4)
            {
                tabPage4.Enabled = true;
            }
            else
            {
                tabPage4.Enabled = false;
            }
        }

        private void ListBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (Blocks[comboBox2.SelectedIndex + 1].Count < (dataGridView1.ColumnCount - 1) / comboBox2.Items.Count)
                {
                    Blocks[comboBox2.SelectedIndex + 1].Add(Convert.ToInt32(listBox1.SelectedItem));
                    Blocks[0].Remove(Convert.ToInt32(listBox1.SelectedItem));
                }
                else
                {
                    MessageBox.Show("Достигнуто максимальное количество марок на данном блоке!");
                }
            }
            ShowBlocks();
            Level2_Marks();
            Level3_Access();
        }


        private void listBox2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                Blocks[0].Add(Convert.ToInt32(listBox2.SelectedItem));
                Blocks[comboBox2.SelectedIndex + 1].Remove(Convert.ToInt32(listBox2.SelectedItem));
            }
            ShowBlocks();
            Level2_Marks();
            Level3_Access();
        }


        private void Select_Block(object sender, EventArgs e)
        {
            if (comboBox2.Items.Count > 0)
            {
                listBox2.Items.Clear();

                foreach (var number in Blocks[comboBox2.SelectedIndex + 1])
                {
                    listBox2.Items.Add(number);
                }
            }
            Level2_Marks();
        }


        private void Choose_BlocksCount()
        {
            try
            {
                if (Convert.ToInt32(textBox3.Text) > 0)
                {
                    while (Blocks.Count - 1 < Convert.ToInt32(textBox3.Text))
                    {
                        Blocks.Add(new List<int>());
                    }
                    while (Blocks.Count - 1 > Convert.ToInt32(textBox3.Text))
                    {
                        if (Blocks[Blocks.Count - 1].Count > 0)
                        {
                            foreach (var number in Blocks[Blocks.Count - 1])
                            {
                                Blocks[0].Add(number);
                            }
                        }
                        comboBox2.Items.RemoveAt(Blocks.Count - 2);
                        Blocks.RemoveAt(Blocks.Count - 1);
                    }
                    comboBox2.Items.Clear();
                    for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
                    {
                        comboBox2.Items.Add(i + 1);
                    }
                    if (comboBox2.Items.Count > 0)
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    for (int i = 0; i < comboBox2.Items.Count; i++)
                    {
                        while (Blocks[i + 1].Count > (dataGridView1.ColumnCount - 1) / comboBox2.Items.Count)
                        {
                            Blocks[0].Add(Blocks[i + 1][Blocks[i + 1].Count - 1]);
                            Blocks[i + 1].Remove(Blocks[i + 1][Blocks[i + 1].Count - 1]);
                        }
                    }
                    if (Blocks.Count > 1)
                    {
                        ShowBlocks();
                    }
                    Level3_Access();
                }
            }
            catch
            {
                MessageBox.Show("Дядя, не надо!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Choose_BlocksCount();
        }

        private void Level2_Marks()
        {
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();

            int r = 0;
            int c;


            if (Blocks[comboBox2.SelectedIndex + 1].Count > 0)
            {
                for (int i = 0; i < Blocks[comboBox2.SelectedIndex + 1].Count + 1; i++)
                {
                    dataGridView3.Columns.Add((i + 1).ToString(), (i + 1).ToString());
                    dataGridView3.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    dataGridView3.Rows.Add();
                }

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    c = 0;
                    for (int j = 1; j < dataGridView1.ColumnCount; j++)
                    {
                        if (Blocks[comboBox2.SelectedIndex + 1].Contains(Convert.ToInt32(dataGridView1.Columns[j].HeaderText)))
                        {
                            dataGridView3.Rows[r].Cells[c + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                            c++;
                        }
                    }
                    r++;
                }

                for (int i = 1; i < dataGridView3.ColumnCount; i++)
                {
                    dataGridView3.Columns[i].HeaderText = Blocks[comboBox2.SelectedIndex + 1][i - 1].ToString();
                }

                dataGridView3.Columns[0].HeaderCell.Value = "Эпоха";
                for (int i = 0; i < dataGridView3.RowCount - 1; i++)
                {
                    dataGridView3.Rows[i].Cells[0].Value = Convert.ToString(i);
                }
            }
        }


        private void Level2_Calculation(object sender, LayoutEventArgs e)
        {
            if (tabControl2.SelectedTab == tabPage7)
            {
                Schet(dataGridView3, dataGridView4);

                comboBox3.Items.Clear();
                foreach (var item in comboBox2.Items)
                {
                    comboBox3.Items.Add(item);
                }
                comboBox3.SelectedIndex = 0;
            }
        }

        private void Select_Block2(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = comboBox3.SelectedIndex;

            if (tabControl2.SelectedTab == tabPage7)
            {
                Level2_Marks();
                Schet(dataGridView3, dataGridView4);
                GrafikMA(chart3, checkedListBox3, dataGridView4);
                GrafikA(chart4, checkedListBox4, dataGridView4);
                GrafikMT(chart7, checkedListBox7, dataGridView4);
            }
        }


        private void Selecting_GrafikMA_Level2(object sender, EventArgs e)
        {
            GrafikMA(chart3, checkedListBox3, dataGridView4);
        }

        private void Selecting_GrafikA_Level2(object sender, EventArgs e)
        {
            GrafikA(chart4, checkedListBox4, dataGridView4);
        }
        private void Selecting_GrafikMT_Level2(object sender, EventArgs e)
        {
            GrafikMT(chart7, checkedListBox7, dataGridView4);
        }

        private void Level2_GrafickOFF(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox3.Items.Count; i++)
            {
                checkedListBox3.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedListBox4.Items.Count; i++)
            {
                checkedListBox4.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedListBox7.Items.Count; i++)
            {
                checkedListBox7.SetItemChecked(i, false);
            }

            chart3.Series.Clear();
            chart4.Series.Clear();
            chart7.Series.Clear();
        }
        private void Level3_Data(object sender, EventArgs e)
        {
            bool change = true;
            
            if (Blocks.Count - 1 == Blocks2.Count && Blocks.Count - 1 == MiniBlocks.Count)
            {
                for (int i = 0; i < Blocks.Count - 1; i++)
                {
                    for (int j = 0; j < MiniBlocks[i].Count; j++)
                    {
                        foreach (var item in MiniBlocks[i][j])
                        {
                            if (Blocks[i + 1].Contains(item))
                            {
                                change = false;
                            }
                            else
                            {
                                change = true;
                                break;
                            }
                        }
                        if (change)
                        {
                            break;
                        }
                    }
                    if (change)
                    {
                        break;
                    }
                    foreach (var num in Blocks2[i])
                    {
                        if (Blocks[i + 1].Contains(num))
                        {
                            change = false;
                        }
                        else
                        {
                            change = true;
                            break;
                        }
                    }
                }
            }

            if (change)
            {
                while (Blocks2.Count > 0)
                {
                    Blocks2.RemoveAt(Blocks2.Count - 1);
                }
                while (MiniBlocks.Count > 0)
                {
                    MiniBlocks.RemoveAt(MiniBlocks.Count - 1);
                }

                while (MiniBlocks.Count < Convert.ToInt32(comboBox2.Items.Count))
                {
                    MiniBlocks.Add(new List<List<int>>());
                    Blocks2.Add(new List<int>());
                }
                while (MiniBlocks.Count > Convert.ToInt32(comboBox2.Items.Count))
                {
                    MiniBlocks.RemoveAt(MiniBlocks.Count - 1);
                    Blocks2.RemoveAt(Blocks2.Count - 1);
                }

                for (int i = 0; i < Blocks2.Count; i++)
                {
                    if (Blocks2[i].Count < Blocks[i + 1].Count)
                    {
                        for (int j = 0; j < Blocks[i + 1].Count; j++)
                        {
                            Blocks2[i].Add(Blocks[i + 1][j]);
                        }
                    }
                }
            }
        }

        private void HighTables()
        {
            int f = 0, first, second;
            for (int i = 1; i < Blocks[comboBox4.SelectedIndex + 1].Count; i++)
            {
                f += i;
            }

            double[,] dif = new double[dataGridView1.RowCount - 1, f];

            dataGridView5.Rows.Clear();
            dataGridView5.Columns.Clear();

            dataGridView6.Rows.Clear();
            dataGridView6.Columns.Clear();

            for (int i = 1; i < dataGridView3.ColumnCount - 1; i++)
            {
                for (int j = 1 + i; j < dataGridView3.ColumnCount; j++)
                {
                    dataGridView5.Columns.Add((dataGridView3.Columns[i].HeaderText + "-" + dataGridView3.Columns[j].HeaderText), (dataGridView3.Columns[i].HeaderText + "-" + dataGridView3.Columns[j].HeaderText));
                    dataGridView6.Columns.Add((dataGridView3.Columns[i].HeaderText + "-" + dataGridView3.Columns[j].HeaderText), (dataGridView3.Columns[i].HeaderText + "-" + dataGridView3.Columns[j].HeaderText));
                }
            }
            for (int i = 0; i < dataGridView5.ColumnCount; i++)
            {
                dataGridView5.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView6.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView5.Rows.Add();
                dataGridView6.Rows.Add();
            }

            for (int r = 0; r < dataGridView1.RowCount - 1; r++)
            {
                first = 1;
                second = first + 1;
                for (int i = 0; i < f; i++)
                {
                    dif[r, i] = Math.Abs(Convert.ToDouble(dataGridView3.Rows[r].Cells[first].Value) - Convert.ToDouble(dataGridView3.Rows[r].Cells[second].Value));
                    if (second < dataGridView3.ColumnCount - 1)
                    {
                        second++;
                    }
                    else
                    {
                        first++;
                        second = first + 1;
                    }
                }
            }

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                for (int j = 0; j < f; j++)
                {
                    dataGridView5.Rows[i].Cells[j].Value = Math.Round(dif[i, j], 4);
                }
            }

            dataGridView5.RowHeadersWidth = 10 * "Эпоха".Length;
            dataGridView5.TopLeftHeaderCell.Value = string.Format("Эпоха", "0");
            for (int i = 0; i < dataGridView5.RowCount - 1; i++)
            {
                dataGridView5.Rows[i].HeaderCell.Value = string.Format((i).ToString(), "0");
            }

            for (int i = 0; i < f; i++)
            {
                dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[i].Style.BackColor = Color.LightGreen;
            }

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                for (int j = 0; j < f; j++)
                {
                    if (dif[i, j] - dif[0, j] < Convert.ToDouble(textBox1.Text))
                    {
                        dataGridView6.Rows[i].Cells[j].Value = "+";
                    }
                    else if (dif[i, j] - dif[0, j] >= Convert.ToDouble(textBox1.Text))
                    {
                        dataGridView6.Rows[i].Cells[j].Value = "-";
                        dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[j].Style.BackColor = Color.Red;
                    }
                }
            }

            dataGridView6.RowHeadersWidth = 10 * "Эпоха".Length;
            dataGridView6.TopLeftHeaderCell.Value = string.Format("Эпоха", "0");
            for (int i = 0; i < dataGridView6.RowCount - 1; i++)
            {
                dataGridView6.Rows[i].HeaderCell.Value = string.Format((i).ToString(), "0");
            }
        }

        private void Level3(object sender, LayoutEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage4)
            {               
                comboBox4.Items.Clear();
                foreach (var item in comboBox2.Items)
                {
                    comboBox4.Items.Add(item);
                }

                comboBox5.Items.Clear();
                for (int i = 1; i < Blocks[1].Count / 2; i++)
                {
                    comboBox5.Items.Add(i + 1);
                }
                if (comboBox5.Items.Count < 1)
                {
                    comboBox5.Items.Add(1);
                }

                comboBox4.SelectedIndex = 0;
                comboBox5.SelectedIndex = 0;
            }
        }

        private void ChooseBlock_Level3(object sender, EventArgs e)
        {
            try
            {
                comboBox2.SelectedIndex = comboBox4.SelectedIndex;
                ShowMiniBlocks();
                Level3_Marks();
                HighTables();
            }
            catch
            {

            }
        }
        private void Choose_MiniBlocksCount(object sender, EventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(comboBox4.Items.Count); i++)
            {
                while (MiniBlocks[i].Count < Convert.ToInt32(comboBox5.SelectedItem))
                {
                    MiniBlocks[i].Add(new List<int>());
                }
                while (MiniBlocks[i].Count > Convert.ToInt32(comboBox5.SelectedItem))
                {
                    foreach (var num in MiniBlocks[i][MiniBlocks[i].Count - 1])
                    {
                        Blocks2[i].Add(num);
                    }
                    MiniBlocks[i].RemoveAt(MiniBlocks.Count - 1);
                }
            }

            comboBox6.Items.Clear();
            for (int i = 0; i < Convert.ToInt32(comboBox5.SelectedItem); i++)
            {
                comboBox6.Items.Add(i + 1);
            }

            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                while (Blocks[i + 1].Count > (dataGridView1.ColumnCount - 1) / comboBox2.Items.Count)
                {
                    Blocks[0].Add(Blocks[i + 1][Blocks[i + 1].Count - 1]);
                    Blocks[i + 1].Remove(Blocks[i + 1][Blocks[i + 1].Count - 1]);
                }
            }

            for (int i = 0; i < comboBox4.Items.Count; i++)
            {
                for (int j = 0; j < comboBox5.Items.Count; j++)
                {
                    try
                    {
                        while (MiniBlocks[i][j].Count > (dataGridView3.ColumnCount / comboBox6.Items.Count))
                        {
                            Blocks2[i].Add(MiniBlocks[i][j][MiniBlocks[i][j].Count - 1]);
                            MiniBlocks[i][j].Remove(MiniBlocks[i][j][MiniBlocks[i][j].Count - 1]);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            comboBox6.SelectedIndex = 0;

            ShowMiniBlocks();
            Level3_Marks();
        }

        private void Level3_Marks()
        {
            dataGridView7.Rows.Clear();
            dataGridView7.Columns.Clear();

            int r = 0;
            int c;


            if (MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Count > 0)
            {
                for (int i = 0; i < MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Count + 1; i++)
                {
                    dataGridView7.Columns.Add((i + 1).ToString(), (i + 1).ToString());
                    dataGridView7.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    dataGridView7.Rows.Add();
                }

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    c = 0;
                    for (int j = 1; j < dataGridView1.ColumnCount; j++)
                    {
                        if (MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Contains(Convert.ToInt32(dataGridView1.Columns[j].HeaderText)))
                        {
                            dataGridView7.Rows[r].Cells[c + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                            c++;
                        }
                    }
                    r++;
                }

                for (int i = 1; i < dataGridView7.ColumnCount; i++)
                {
                    dataGridView7.Columns[i].HeaderText = MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex][i - 1].ToString();
                }

                dataGridView7.Columns[0].HeaderCell.Value = "Эпоха";
                for (int i = 0; i < dataGridView7.RowCount - 1; i++)
                {
                    dataGridView7.Rows[i].Cells[0].Value = Convert.ToString(i);
                }
            }
        }

        private void ShowMiniBlocks()
        {
            listBox3.Items.Clear();
            Blocks2[comboBox4.SelectedIndex].Sort();
            foreach (var number in Blocks2[comboBox4.SelectedIndex])
            {
                listBox3.Items.Add(number);
            }

            listBox4.Items.Clear();
            MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Sort();
            foreach (var number in MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex])
            {
                listBox4.Items.Add(number);
            }
        }

        private void ListBox3_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                if (MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Count < (dataGridView3.ColumnCount / comboBox6.Items.Count))
                {
                    MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Add(Convert.ToInt32(listBox3.SelectedItem));
                    Blocks2[comboBox4.SelectedIndex].Remove(Convert.ToInt32(listBox3.SelectedItem));
                }
                else
                {
                    MessageBox.Show("Достигнуто максимальное количество марок на данном блоке!");
                }

                ShowMiniBlocks();
                Level3_Marks();
            }
        }

        private void ListBox4_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                Blocks2[comboBox4.SelectedIndex].Add(Convert.ToInt32(listBox4.SelectedItem));
                MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Remove(Convert.ToInt32(listBox4.SelectedItem));
            }

            ShowMiniBlocks();
            Level3_Marks();
        }

        private void Choose_MiniBlock(object sender, EventArgs e)
        {
            if (comboBox6.Items.Count > 0)
            {
                listBox4.Items.Clear();

                foreach (var number in MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex])
                {
                    listBox4.Items.Add(number);
                }
            }

            Level3_Marks();
        }

        private void Level3_Calculation(object sender, LayoutEventArgs e)
        {
            if (tabControl3.SelectedTab == tabPage9)
            {
                Schet(dataGridView7, dataGridView8);

                comboBox7.Items.Clear();
                foreach (var item in comboBox4.Items)
                {
                    comboBox7.Items.Add(item);
                }
                comboBox7.SelectedIndex = 0;

                comboBox8.Items.Clear();
                foreach (var item in comboBox6.Items)
                {
                    comboBox8.Items.Add(item);
                }
                comboBox8.SelectedIndex = 0;
            }
        }

        private void SelectingGrafikMA_Level3(object sender, EventArgs e)
        {
            GrafikMA(chart8, checkedListBox8, dataGridView8);
        }

        private void SelectingGrafikA_Level3(object sender, EventArgs e)
        {
            GrafikA(chart9, checkedListBox9, dataGridView8);
        }

        private void SelectingGrafikMT_Level3(object sender, EventArgs e)
        {
            GrafikMT(chart10, checkedListBox10, dataGridView8);
        }

        private void Level3_GrafikOFF(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox8.Items.Count; i++)
            {
                checkedListBox8.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedListBox9.Items.Count; i++)
            {
                checkedListBox9.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedListBox10.Items.Count; i++)
            {
                checkedListBox10.SetItemChecked(i, false);
            }

            chart8.Series.Clear();
            chart9.Series.Clear();
            chart10.Series.Clear();
        }

        private void Level3_SelectBlock2(object sender, EventArgs e)
        {
            comboBox4.SelectedIndex = comboBox7.SelectedIndex;

            if (tabControl3.SelectedTab == tabPage9)
            {
                Level3_Marks();
                Schet(dataGridView7, dataGridView8);
                GrafikMA(chart8, checkedListBox8, dataGridView8);
                GrafikA(chart9, checkedListBox9, dataGridView8);
                GrafikMT(chart10, checkedListBox10, dataGridView8);
            }
        }

        private void Level3_SelectMiniBlock2(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = comboBox8.SelectedIndex;

            if (tabControl3.SelectedTab == tabPage9)
            {
                Level3_Marks();
                Schet(dataGridView7, dataGridView8);
                GrafikMA(chart8, checkedListBox8, dataGridView8);
                GrafikA(chart9, checkedListBox9, dataGridView8);
                GrafikMT(chart10, checkedListBox10, dataGridView8);
            }
        }

        private void tabControl3_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                int c = MiniBlocks[0][0].Count;
                int same = 0;

                for (int i = 1; i < MiniBlocks.Count; i++)
                {
                    for (int j = 0; j < MiniBlocks[i].Count; j++)
                    {
                        if (MiniBlocks[i][j].Count == c)
                        {
                            same = 1;
                        }
                        else
                        {
                            same = 2;
                            break;
                        }
                    }
                    if (same == 2)
                    {
                        break;
                    }
                }

                if (MiniBlocks[comboBox4.SelectedIndex][comboBox6.SelectedIndex].Count > 1 && same == 1)
                {
                    tabPage9.Enabled = true;
                }
                else
                {
                    tabPage9.Enabled = false;
                }
                if (!e.TabPage.Enabled)
                {
                    e.Cancel = true;
                }
            }
            catch
            {

            }
        }

        private void About_Program(object sender, EventArgs e)
        {
            MessageBox.Show("Автор: Кандыба Захар, студент группы БИ-21.2\n\nПрограмма предназначена для геодезистов!", "О программе!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}