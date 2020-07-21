using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>ProductGUI</c> this represents the product in the user interface where
    /// each productGUI is linked to the previous productGUI.
    /// </summary>
    public class ProductGUI
    {
        private List<Form> list_form;

        private Label product_label;
        private LinkedMenuStrip category;
        private LinkedMenuStrip recipe;
        private QuantityBox quantity_box;
        private Button recipe_button;

        private int id;
        private ProductGUI previous;
        private Product product;

        private Bytescout.Spreadsheet.Spreadsheet database;

        /// <summary>
        /// constructor <c>ProductGUI</c>
        /// </summary>
        /// <param name="f"> list of forms mainly, the main form and the procedure form. </param>
        /// <param name="db"> database. </param>
        /// <param name="prev"> reference to the previous productGUI. </param>
        public ProductGUI(List<Form> f, Bytescout.Spreadsheet.Spreadsheet db, ProductGUI prev)
        {
            this.list_form = f;
            this.database = db;
            this.previous = prev;

            if (prev == null) id = 1;
            else this.id = prev.getId() + 1;

            createLabel();
            createCategoryMenuStrip();
            createRecipeMenuStrip();
            createQuantityBox();
            createRecipeButton();
        }

        /// <summary>
        /// method <c>createLabel</c> creates a Label component, Product 1, Product 2 ... etc.
        /// </summary>
        private void createLabel()
        {
            product_label = new Label();
        
            if (previous == null) { product_label.Location = new Point(40, 200);}
            else { product_label.Location = new Point(40, previous.product_label.Location.Y + 30); }

            product_label.AutoSize = true;
            product_label.Size = new Size(100, 30);
            product_label.TabIndex = 0;
            product_label.Text = "Product " + this.id + ": ";
            product_label.Font = new Font("Comic Sans MS", 12f);
        }

        /// <summary>
        /// method <c>createCategoryMenuStrip</c> creates a MenuStrip component to display the categories of food.
        /// </summary>
        private void createCategoryMenuStrip()
        {
            category = new LinkedMenuStrip(database, "Category", this, null);

            if (previous == null) category.Location = new Point(140, 200);
            else category.Location = new Point(140, previous.category.Location.Y + 30);

        }

        /// <summary>
        /// method <c>createRecipeMenuStrip</c> creates a MenuStrip component to dispaly the recipes of food.
        /// </summary>
        private void createRecipeMenuStrip()
        {
            recipe = new LinkedMenuStrip(database, "Recipe", this, category);

            if (previous == null) recipe.Location = new Point(340, 200);
            else recipe.Location = new Point(340, previous.recipe.Location.Y + 30);

        }

        /// <summary>
        /// method <c>createQuantityBox</c> creates a TextBox component to take in quantity of food.
        /// </summary>
        private void createQuantityBox()
        {
            quantity_box = new QuantityBox(this);

            if (previous == null) quantity_box.Location = new Point(540, 200);
            else quantity_box.Location = new Point(540, previous.quantity_box.Location.Y + 30);

            quantity_box.Size = new Size(150, 25);
            quantity_box.Multiline = true;
            quantity_box.Text = "";
            quantity_box.BorderStyle = BorderStyle.Fixed3D;
            quantity_box.Font = new Font("Segoe UI Symbol", 10F);

        }

        /// <summary>
        /// method <c>createRecipeButton</c> creates a "Show Recipe" Button component when pressed displays the procedure steps of the recipe.
        /// </summary>
        private void createRecipeButton()
        {
            recipe_button = new Button();

            recipe_button.Name = "Button";
            if (previous == null) recipe_button.Location = new Point(740, 200);
            else recipe_button.Location = new Point(740, previous.recipe_button.Location.Y + 30);

            recipe_button.Size = new Size(120, 25);
            recipe_button.TabIndex = 0;
            recipe_button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            recipe_button.Text = "Show Recipe";
            recipe_button.Font = new Font("Arial Bold", 10F);
            recipe_button.UseVisualStyleBackColor = true;
            recipe_button.Enabled = false;
            recipe_button.Click += new System.EventHandler(this.displayProcedure_Click);
        }

        /// <summary>
        /// method <c>createProduct</c> creates a final food product.
        /// </summary>
        /// <returns></returns>
        private Product createProduct()
        {
            DataAnalyst d_a = new DataAnalyst(database, category.getText(), recipe.getText(), Double.Parse(quantity_box.getText()));
            return d_a.createProduct();
        }

        /// <summary>
        /// method <c>enableRecipeButton</c> enables/disables the "Show Recipe' button and then creates a final food product.
        /// </summary>
        /// <param name="enable">bool to enable/disable.</param>
        public void enableRecipeButton(bool enable)
        {
            recipe_button.Enabled = enable;
            if (enable) this.product = createProduct();
        }

        /// <summary>
        /// method <c>getMainForm</c> returns the main win form of the GUI. 
        /// </summary>
        /// <returns></returns>
        public Form getMainForm()
        {
            return list_form[0];
        }

        /// <summary>
        /// method <c>getQuantityBox</c> returns quantity TextBox component.
        /// </summary>
        /// <returns></returns>
        public QuantityBox getQuantityBox()
        {
            return quantity_box;
        }

        /// <summary>
        /// method <c>getProduct</c> returns food product.
        /// </summary>
        /// <returns></returns>
        public Product getProduct()
        {
            return product;
        }

        /// <summary>
        /// method <c>getProductLabel</c> returns the product_label component.
        /// </summary>
        /// <returns></returns>
        public Label getProductLabel()
        {
            return product_label;
        }

        /// <summary>
        /// method <c>reset_category</c> resets categorie, recipe, quantity components.
        /// </summary>
        public void resetCategory()
        {
            category.Text = "Choose Category";
            category.selectedMenuItem().Text = "Choose Category";

            resetRecipe();
        }

        /// <summary>
        /// method <c>getId</c> returns id.
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return id;
        }

        /// <summary>
        /// method <c>resetRecipe</c> resets recipe and quantity components.
        /// </summary>
        public void resetRecipe()
        {
            recipe.Text = "Choose Recipe";
            recipe.selectedMenuItem().Text = "Choose Recipe";
            recipe.disable();

            resetQuantityBox();
        }

        /// <summary>
        /// method <c>resetQuantityBox</c> resets quantity component.
        /// </summary>
        public void resetQuantityBox()
        {
            quantity_box.Text = "";
            quantity_box.Enabled = true;
        }

        /// <summary>
        /// method <c>addToControl</c> adds the productGUI to the GUI.
        /// </summary>
        public void addToControl()
        {
            ((MainForm)list_form[0]).Controls.Add(product_label);
            ((MainForm)list_form[0]).Controls.Add(category);
            ((MainForm)list_form[0]).Controls.Add(recipe);
            ((MainForm)list_form[0]).Controls.Add(quantity_box);
            ((MainForm)list_form[0]).Controls.Add(recipe_button);
        }

        /// <summary>
        /// method <c>removeFromControl</c> removes the productGUI to the GUI.
        /// </summary>
        public void removeFromControl()
        {
            ((MainForm)list_form[0]).Controls.Remove(product_label);
            ((MainForm)list_form[0]).Controls.Remove(category);
            ((MainForm)list_form[0]).Controls.Remove(recipe);
            ((MainForm)list_form[0]).Controls.Remove(quantity_box);
            ((MainForm)list_form[0]).Controls.Remove(recipe_button);
        }

        private void displayProcedure_Click(object sender, EventArgs e)
        {
            ((ProcedureForm) list_form[1]).displayProcedure(product.step_procedure, recipe.getText());
        }
    }
}