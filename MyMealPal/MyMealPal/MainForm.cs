using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyMealPal
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.MouseClick += new MouseEventHandler(this.mealCreatorForm_MouseClick);
        }

        private void mealCreatorForm_MouseClick(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
