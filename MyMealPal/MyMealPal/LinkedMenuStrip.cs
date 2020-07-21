using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>LinkedMenuStrip</c> extends MenuStrip component with some extra functionalities.
    /// </summary>
    public class LinkedMenuStrip : MenuStrip
    {
        // Properties to be added, if necessary, for instances of this class are Location, Name, and TabIndex

        public static int LARGE = 1;

        private int size;
        private string role;
        private int index;

        private ToolStripMenuItem selected_menuItem;
        private LinkedMenuStrip previous;
        private LinkedMenuStrip next;
        private ProductGUI product_gui;

        /// <summary>
        /// constructor <c>LinkedMenuStrip</c> takes in as parameters, database, role/name, ProductGUI, and previous MenuStrip.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="prod_gui"></param>
        /// <param name="Prev"></param>
        public LinkedMenuStrip(string role, ProductGUI prod_gui, LinkedMenuStrip Prev)
        {
            this.role = role;
            this.product_gui = prod_gui;
            this.previous = Prev;

            this.size = LARGE;
            this.selected_menuItem = new System.Windows.Forms.ToolStripMenuItem();

            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.GripMargin = new System.Windows.Forms.Padding(0);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.selected_menuItem });
            this.Padding = new System.Windows.Forms.Padding(0);
            this.AutoSize = true;
            this.Text = "customMenuStrip";

            this.selected_menuItem.Image = global::MyMealPal.Properties.Resources.DropBlue;
            this.selected_menuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.selected_menuItem.Name = "selected_menuItem";
            this.selected_menuItem.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.selected_menuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.selected_menuItem.AutoSize = true;
            this.selected_menuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selected_menuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.selected_menuItem.DropDownOpened += new System.EventHandler(this.menuStrip_Opened);
            this.selected_menuItem.DropDownClosed += new System.EventHandler(this.menuStrip_Closed);

            this.selected_menuItem.Font = new System.Drawing.Font("Comic Sans MS", 11f);
            this.selected_menuItem.Text = "Choose " + role;

            if (this.previous == null)
            {
                this.index = 0;
                this.enable();
            }
            else
            {
                previous.next = this;
                next = null;
                this.index = this.previous.index + 1;
                this.clear();
                this.disable();
            }

        }

        /// <summary>
        /// methdod <c>selectedMenuItem</c> getter for the selected menu item.
        /// </summary>
        /// <returns>ToolStripMenuItem</returns>
        public ToolStripMenuItem selectedMenuItem()
        {
            return selected_menuItem;
        }

        /// <summary>
        /// method <c>isEmpty</c> checks whether the MenuStrip has been used.
        /// </summary>
        /// <returns>bool</returns>
        private bool isEmpty()
        {
            if ((this.selected_menuItem.Text.Contains("Choose"))) return true;
            else return false;
        }

        /// <summary>
        /// method <c>getText</c> getter for the text in the selected menu item. 
        /// </summary>
        /// <returns>string</returns>
        public string getText()
        {
            string line;
            if (isEmpty())
            {
                line = string.Empty;
            }
            else
            {
                line = this.selected_menuItem.Text;
            }
            return line;
        }

        /// <summary>
        /// method <c>addItem</c> adds a ToolStripMenuItem to the MenuStrip.
        /// </summary>
        /// <param name="item"></param>
        private void addItem(string item)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Name = "menuItem";
            menuItem.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            menuItem.AutoSize = true;
            menuItem.Text = item;
            menuItem.Click += new System.EventHandler(this.menuItem_Click);

            menuItem.Font = new System.Drawing.Font("Comic Sans MS", 11f);

            this.selected_menuItem.DropDownItems.Add(menuItem);
        }

        /// <summary>
        /// method <c>addItemList</c> adds a list of ToolStripMenuItem to the MenuStrip
        /// </summary>
        /// <param name="items"></param>
        private void addItemList(List<string> items)
        {
            foreach (string item in items)
            {
                this.addItem(item);
            }
        }

        /// <summary>
        /// method <c>clear</c> clears the MenuStrip.
        /// </summary>
        private void clear()
        {
            this.selected_menuItem.DropDownItems.Clear();
            this.selected_menuItem.Text = "Choose " + role;
        }

        /// <summary>
        /// method <c>enable</c> enables the MenuStrip.
        /// </summary>
        private void enable()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// method <c>disable</c> disables the MenuStrip.
        /// </summary>
        public void disable()
        {
            this.Enabled = false;
        }

        /// <summary>
        /// method <c>disableClearRest</c> clears, disable, and resets all the followed MenuStrip.
        /// </summary>
        private void disableClearRest()
        {
            if (this.next == null)
            {
                return;
            }
            else
            {
                this.next.clear();
                this.next.disable();
                this.next.disableClearRest();
            }
        }

        private void menuItem_Click(Object sender, EventArgs e)
        {
            ToolStripMenuItem temporaryMenuItem = sender as ToolStripMenuItem;
            selected_menuItem.Text = temporaryMenuItem.Text;

            // disable all Next MenuStrips
            this.disableClearRest();

            product_gui.getMainForm().ActiveControl = null;

            // Category Chosen
            if (this.index == 0)
            {
                if (!this.getText().Equals(""))
                {
                    this.next.enable();
                    this.product_gui.resetQuantityBox();
                }
                else
                {
                    this.product_gui.resetRecipe();
                }
            }
            //Recipe Chosen
            else
            {
                if (this.getText().Equals(""))
                {
                    this.product_gui.resetQuantityBox();
                }
            }
        }

        private void menuStrip_Opened(Object sender, EventArgs e)
        {
            this.selected_menuItem.Image = global::MyMealPal.Properties.Resources.LiftBlue;
        }

        private void menuStrip_Closed(Object sender, EventArgs e)
        {
            this.selected_menuItem.Image = global::MyMealPal.Properties.Resources.DropBlue;
        }

    }
}