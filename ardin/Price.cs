﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ardin
{
    public partial class Price : Form
    {
        string nu;
        private string change_comma(string s)
        {
            int pos = s.IndexOf(",");
            if (pos > 0)
            {
                s = s.Substring(0, pos) + "." + s.Substring(pos + 1, 2);
            }
            return s;
        }

        public Price()
        {
            InitializeComponent();
        }

        private void Price_Activated(object sender, EventArgs e)
        {
            SqlConnection connection1 = new SqlConnection(Data.Glob_connection_string); connection1.Open();

            string SQL = "Select n_u, naim, stoim from Uslugi";

            SqlDataAdapter adapter = new SqlDataAdapter(SQL, connection1);
            DataTable tb = new DataTable();
            adapter.Fill(tb);

            dataGridView1.Refresh();
            dataGridView1.DataSource = tb;
            dataGridView1.Columns[0].HeaderText = "№ услуги";
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].HeaderText = "Назначение";
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].HeaderText = "Деньги";
            dataGridView1.Columns[2].Width = 100;
            connection1.Close();
        }

        private void Price_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connection1 = new SqlConnection(Data.Glob_connection_string);
            connection1.Open();

            string SQL_izm = "UPDATE Uslugi set naim=N'" + textBox1.Text + "', stoim=N'" + change_comma(textBox2.Text) + "' WHERE n_u=" + nu;
            SqlCommand command1 = new SqlCommand(SQL_izm, connection1);
            SqlDataReader dr = command1.ExecuteReader();
            connection1.Close();
            MessageBox.Show("Данные изменены");
            this.Activate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            nu = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SQL_text = "SELECT max(n_u) as max FROM Uslugi";

            SqlConnection connection1 = new SqlConnection(Data.Glob_connection_string);
            connection1.Open();

            SqlCommand command1 = new SqlCommand(SQL_text, connection1);
            SqlDataReader dr = command1.ExecuteReader();
            string max = "";
            int max2 = 0;
            while (dr.Read())
            {
                max = string.Format("{0}", dr["max"]);
            }
            dr.Close();
            connection1.Close();
            if (max == "") { max2 = 1; }
            else { max2 = Convert.ToInt32(max) + 1; }
            max = Convert.ToString(max2);

            SQL_text = "INSERT INTO Uslugi(n_u, naim, stoim) VALUES (" + (max) +", N'" + textBox1.Text + "', " + change_comma(textBox2.Text) + ")";


            connection1 = new SqlConnection(Data.Glob_connection_string);
            connection1.Open();

            command1 = new SqlCommand(SQL_text, connection1);
            dr = command1.ExecuteReader();
            dr.Close();
            connection1.Close();
            MessageBox.Show("Данные сохранены");
            this.Activate();

        }
    }
}
