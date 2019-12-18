using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discr_graph
{
    public partial class Form1 : Form
    {
        Graph g = new Graph(5, true);
        List<Graph> savedG = new List<Graph>();
        List<string> namesSaved = new List<string>();
        public Form1()
        {
            InitializeComponent();
            GD2.offsetX = pictureBox1.Width / 2;
            GD2.offsetY = pictureBox1.Height / 2;

            crascal_panel.Location = matrix_panel.Location;
            saveLoad_panel.Location = matrix_panel.Location;
            core_panel.Location = matrix_panel.Location;
            deikstr_panel.Location = matrix_panel.Location;

        }

        private void Action1()
        {
            pictureBox1.Refresh();
        }

        private void FormInput(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!(sender is TextBox)) return;
            TextBox tb = (TextBox)sender;

            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                //  tb.Text += e.KeyChar;
                //tb.Select(tb.SelectionStart + 1, 0);
            }
        }
        private void _KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
            {
                FormInput(sender, e);
                // e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Action1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //showNORMatrix(Int32.Parse(textBox2.Text),);
        }
        private void Update(object sender, PaintEventArgs e)
        {
            // GD2.DrawCircle(e, 50, 0, 0, Color.Red);
            // int n = 8;
            //  if (textBox1.Text != "")
            // n = Int32.Parse(textBox1.Text);
               g = new Graph(10, true);
            List<int> ccc = new List<int>();
            ccc.Add(1);
            ccc.Add(3);
            ccc.Add(5);
            ccc.Add(7);
            if (g != null)
            {
                GD2.DrawGraph(e, g, ccc);
            }
        }

        private void showTask(int n)
        {
            switch (n)
            {
                case 1:
                    matrix_panel.Visible = true;
                    deikstr_panel.Visible = false;
                    crascal_panel.Visible = false;
                    saveLoad_panel.Visible = false;
                    core_panel.Visible = false;
                    break;
                case 2:
                    matrix_panel.Visible = false;
                    deikstr_panel.Visible = false;
                    crascal_panel.Visible = false;
                    saveLoad_panel.Visible = true;
                    core_panel.Visible = false;
                    break;
                case 3:
                    matrix_panel.Visible = false;
                    deikstr_panel.Visible = true;
                    crascal_panel.Visible = false;
                    saveLoad_panel.Visible = false;
                    core_panel.Visible = false;
                    break;
                case 4:
                    matrix_panel.Visible = false;
                    deikstr_panel.Visible = false;
                    crascal_panel.Visible = false;
                    saveLoad_panel.Visible = false;
                    core_panel.Visible = true;
                    break;
                case 5:
                    matrix_panel.Visible = false;
                    deikstr_panel.Visible = false;
                    crascal_panel.Visible = true;
                    saveLoad_panel.Visible = false;
                    core_panel.Visible = false;
                    break;
            }
        }
        private void ShowError(string msg)
        {
            //msg_label.Text = msg;
            //msg_label.Visible = true;
            MessageBox.Show(
             msg,
              "Ошибка",
            MessageBoxButtons.OK,
      MessageBoxIcon.Error);
        }
        private void ShowMsg(string msg)
        {
            //msg_label.Text = msg;
            //msg_label.Visible = true;
            MessageBox.Show(
             msg,
              "Ошибка",
            MessageBoxButtons.OK,
      MessageBoxIcon.Information);
        }
        private void showORMatrix(int n)
        {
            bool lever = false;
            TextBox t = null;
            Label l = null;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    lever = (i > n);
                    l = (Label)matrix_panel.Controls["label" + i];
                    l.Visible = !lever;
                    l = (Label)matrix_panel.Controls["label2" + i];
                    l.Visible = !lever;
                    if (i != j)
                    {
                        lever = (i <= n && j <= n);
                        t = (TextBox)panel1.Controls["textBox" + (i.ToString()) + (j.ToString())];

                        t.Visible = lever;
                    }
                }
            }
        }
        private void showNORMatrix(int n)
        {
            bool lever = false;
            TextBox t = null;
            Label l = null;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    lever = (i > n);
                    l = (Label)matrix_panel.Controls["label" + i];
                    l.Visible = !lever;
                    l = (Label)matrix_panel.Controls["label2" + i];
                    l.Visible = !lever;
                    if (i != j)
                    {
                        lever = (i < j) && j <= n;
                        t = (TextBox)panel1.Controls["textBox" + (i.ToString()) + (j.ToString())];

                        t.Visible = lever;
                    }
                }
            }
        }

        private int[,] GetMatrix(int n)
        {
            TextBox t = null;
            int[,] res = new int[n, n];
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    t = (TextBox)panel1.Controls["textBox" + (i.ToString()) + (j.ToString())];
                    if (t.Text == "") t.Text += "0";
                    res[i - 1, j - 1] = Int32.Parse(t.Text);
                }
            }
            return res;
        }
        private void ClearHoles()
        {
            TextBox t = null;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    t = (TextBox)panel1.Controls["textBox" + (i.ToString()) + (j.ToString())];
                    t.Text = "0";
                }
            }
        }
        private void GraphToMatrix(Graph g)
        {
            if (g.Count > 10) { ShowError("Больше 10 элементов");return; }
            TextBox t = null;
            for (int i = 1; i <= g.Count; i++)
            {
                for (int j = 1; j <= g.Count; j++)
                {
                    t = (TextBox)panel1.Controls["textBox" + (i.ToString()) + (j.ToString())];
                    t.Text = g.Matrix[i-1,j-1].ToString();
                }
            }
        }
        private void DeleteSaved()
        {
            if (listBox1.SelectedIndex == -1) { ShowError("Не выбран граф для удаления"); return; }

            savedG.RemoveAt(listBox1.SelectedIndex);
            namesSaved.RemoveAt(listBox1.SelectedIndex);
            UpdateListbox(listBox1, namesSaved);
        }
        private void AddSaved()
        {
            string name;
            if (textBox5.Text == "") name = "Неназванный граф " + (savedG.Count + 1);
            else name = textBox5.Text;

            savedG.Add(g);
            namesSaved.Add(name);
            UpdateListbox(listBox1, namesSaved);
        }
        private void LoadSaved()
        {
            if (listBox1.SelectedIndex == -1) { ShowError("Не выбран граф для загрузки"); return; }

            g = savedG[listBox1.SelectedIndex];
            Action1();
            GraphToMatrix(g);
        }

        private void SaveList()
        {
            string filename = "saved.bin";
            FileStream f0 = new FileStream(filename, FileMode.Truncate);
            f0.Close();
            FileStream f1 = new FileStream(filename, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            for (int i = 0; i < savedG.Count; i++)
            {
                bf.Serialize(f1, savedG[i]);
                bf.Serialize(f1, namesSaved[i]);
            }
            f1.Close();
        }
        private void LoadList()
        {
            savedG.Clear();
            namesSaved.Clear();
            string filename = "saved.bin";
            FileStream f1 = new FileStream(filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            while (f1.Length != f1.Position)
            {
               Graph st = (Graph)bf.Deserialize(f1);
                string str = (string)bf.Deserialize(f1);
                savedG.Add(st);
                namesSaved.Add(str);
            }
            f1.Close();
            UpdateListbox(listBox1, namesSaved);
        }


            private void MakeGraph()
        {
            g = new Graph(GetMatrix((int)count_numeric1.Value), orgraph_ch1.Checked);
            Action1();
        }
        private void UpdateListbox(ListBox list1, List<string> list2 )
        {
            list1.Items.Clear();
            foreach (var i in list2)
            {
                list1.Items.Add(i);
            }
        }
        private void showGInfo(Graph g)
        {
            textBox3.Text = g.Count.ToString();
            textBox4.Text = g.Lines.ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Update(sender, e);
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            _KeyPress(sender, e);
        }

        private void textBox1010_KeyPress(object sender, KeyPressEventArgs e)
        {
            _KeyPress(sender, e);
        }

        private void orgraph_ch1_CheckedChanged(object sender, EventArgs e)
        {
            if (orgraph_ch1.Checked) showORMatrix((int)count_numeric1.Value);
            else showNORMatrix((int)count_numeric1.Value);
        }

        private void count_numeric1_ValueChanged(object sender, EventArgs e)
        {
            if (orgraph_ch1.Checked) showORMatrix((int)count_numeric1.Value);
            else showNORMatrix((int)count_numeric1.Value);
        }

        private void mkGraph_button_Click(object sender, EventArgs e)
        {
            MakeGraph();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            showTask(1);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            showTask(2);
        }

        private void deikstr_rb_CheckedChanged(object sender, EventArgs e)
        {
            showTask(3);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            showTask(4);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            showTask(5);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearHoles();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DeleteSaved();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddSaved();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadSaved();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveList();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            LoadList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                showGInfo(savedG[listBox1.SelectedIndex]);
            }
        }
    }
}
