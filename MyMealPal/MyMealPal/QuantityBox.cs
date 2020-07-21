using System;
using System.Drawing;
using System.Windows.Forms;

/*
* @author Ahmed ElShafei
* 
* Copyright 2020, all rights reserved.
*/ 

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
        /// method <c>setPlaceHolder</c> generates the corresponding text in the textbox.
        /// </summary>
        /// <param name="db"> database for data. </param>
        /// <param name="categ"> name of a food category eg. chicken, meat, etc. </param>
        /// <param name="recipe"> name of a recipe </param>
        public void setPlaceHolder(Bytescout.Spreadsheet.Spreadsheet db, string categ, string recipe)
        {
            Bytescout.Spreadsheet.Worksheet value = db.Workbook.Worksheets.ByName("Values");
            Bytescout.Spreadsheet.Worksheet cat = db.Workbook.Worksheets.ByName(categ);

            int row1 = 1;
            while (cat.Cell(row1, 0).Value != null && !cat.Cell(row1, 0).Value.ToString().Equals(recipe))
            {
                row1++;
            }
            if (cat.Cell(row1, 0).Value != null)
            {
                string mainItem = cat.Cell(row1, 5).Value.ToString();
                int row = 4;
                while (value.Cell(row, 0).Value != null && !value.Cell(row, 0).Value.ToString().Equals(mainItem))
                {
                    row++;
                }

                if (value.Cell(row, 0).Value != null) { this.ForeColor = Color.Gray; place_holder = this.Text = value.Cell(row, 16).Value.ToString() + "s of " + mainItem; }
                else { place_holder = this.Text = "Not Found"; this.Enabled = false; }
            }
            else { place_holder = this.Text = "Not Found"; this.Enabled = false; }
        }

        ///// <summary>
        ///// method <c>reset</c> sets the textbox enable to true and removes any text.
        ///// </summary>
        //public void reset()
        //{
        //    this.Enabled = true;
        //    this.Text = "";
        //}

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
