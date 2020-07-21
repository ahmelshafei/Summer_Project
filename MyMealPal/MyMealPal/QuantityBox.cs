using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyMealPal
{
    /// <summary>
    ///  Class <c>QuantityBox</c> extends TextBox.
    /// </summary>
    public class QuantityBox : TextBox
    {

        private string      place_holder;
        private string      quantity;
        private ProductGUI  product_line;

        public QuantityBox(ProductGUI pl)
        {
            this.product_line = pl;
            this.Enter += new System.EventHandler(this.textBox_Enter);
            this.Leave += new System.EventHandler(this.textBox_Leave);
            this.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.LostFocus += new System.EventHandler(this.textBox_LostFocus);
        }

        /// <summary>
        /// method <c>testEntry</c> tests whether the entry is valid.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool testEntry(string entry)
        {
            try { Double.Parse(entry); return true; }
            catch { return false; }
        }

        /// <summary>
        /// method <c>getText</c> returns the text.
        /// </summary>
        /// <returns></returns>
        public string getText()
        {
            return quantity;
        }

        private void textBox_Enter(object sender, EventArgs e)
        {

            if (this.Text.Equals(place_holder))
            {
                this.ForeColor = Color.Black;
                this.Text = "";
            }
        }

        private void textBox_LostFocus(object sender, EventArgs e)
        {

        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            if (this.Text.Equals(""))
            {
                this.ForeColor = Color.Gray;
                this.Text = place_holder;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            QuantityBox tempQuantity = sender as QuantityBox;
            if (testEntry(tempQuantity.Text))
            {
                quantity = tempQuantity.Text;
                product_line.enableRecipeButton(true);
            }
            else
            {
                quantity = null;
                product_line.enableRecipeButton(false);
            }
        }


    }
}
