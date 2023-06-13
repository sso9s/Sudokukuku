using System;
using System.Windows.Forms;

namespace Sudokukuku
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
            
        }
        Form2 f;

        private void button1_Click(object sender, EventArgs e)
        {
            if (f == null)
                f = new Form2();
            f.Show();
            this.Hide();
        }
    }
}
