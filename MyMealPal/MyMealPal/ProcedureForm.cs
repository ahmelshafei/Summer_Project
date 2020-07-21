using System;
using System.Windows.Forms;

namespace MyMealPal
{
    public partial class ProcedureForm : Form
    {
        public ProcedureForm()
        {
            InitializeComponent();
            this.Closed += new EventHandler(this.procedureForm_Closed);
        }

        private void procedureForm_Closed(object sender, System.EventArgs e)
        {
            this.Controls.Clear();
        }
    }
}
