using Bytescout.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>MealGUI</c> represents all the buttons and layout of the GUI
    /// </summary>
    public class MealGUI
    {
        string resources_directory;
        List<Form> list_form;
        List<ProductGUI> list_of_productsGUI;
        Spreadsheet database;
        List<Button> list_of_button;
        List<Label> list_of_nutrition_lables;

        Meal meal;

        public MealGUI(List<Form> f)
        {

            list_of_productsGUI = new List<ProductGUI>();
            list_form = f;

            resources_directory = Environment.CurrentDirectory.Replace(@"bin\Debug", "Resources");

            string database_directory = resources_directory + @"\Database.xlsx";
            database = new Spreadsheet();
            database.LoadFromFile(database_directory);

            list_of_button = new List<Button>();
            list_of_nutrition_lables = new List<Label>();

            createLabel("Create Your Meal", new Point(360, 30), 20);

            createButton(resources_directory + @"\AddBlue.png", new Point(40, 100), new Size(60, 60));
            list_of_button.Last().Click += new System.EventHandler(this.addProduct_Click);
            createButton(resources_directory + @"\CleanBlue.png", new Point(150, 100), new Size(60, 60));
            list_of_button.Last().Click += new System.EventHandler(this.resetButton_Click);
            createButton(resources_directory + @"\TrashBlue.png", new Point(260, 100), new Size(60, 60));
            list_of_button.Last().Click += new System.EventHandler(this.deleteAll_Click);
            
        }

        /// <summary>
        /// method <c>createLabel</c> creates a Label that holds a text.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="p"></param>
        /// <param name="txtSize"></param>
        private void createLabel(string name, Point p, float txtSize)
        {
            Label label = new Label();

            label = new Label();

            label.Location = p;

            label.AutoSize = true;
            label.TabIndex = 0;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = name;
            label.Font = new Font("Comic Sans MS", txtSize);

            list_form[0].Controls.Add(label);
            if (!name.Equals("Create Your Meal")) list_of_nutrition_lables.Add(label);

        }

        /// <summary>
        /// method <c>createButton</c> creates a button.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="p"></param>
        /// <param name="sz"></param>
        private void createButton(string name, Point p, Size sz)
        {
            Button button = new Button();

            button.Name = name;
            button.Location = p;
            button.Size = sz;
            button.TabIndex = 0;
            if (name.Contains(@"\"))
            {
                button.Image = (Image) (new Bitmap(Image.FromFile(name), sz));
                button.ImageAlign = ContentAlignment.MiddleCenter;
                button.TabStop = false;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                //button.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                button.Text = name;
                button.Font = new Font("Segoe UI Symbol", 10F);
            }
            
            button.UseVisualStyleBackColor = true;

            list_of_button.Add(button);
            list_form[0].Controls.Add(button);

        }

        /// <summary>
        /// methos <c>addProductGUI</c> adds all the components that represents the product in the GUI.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ctrl"></param>
        private void addProductGUI(Spreadsheet db, Control.ControlCollection ctrl)
        {
            if (list_of_productsGUI.Count == 0) list_of_productsGUI.Add(new ProductGUI(list_form, db, null));
            else list_of_productsGUI.Add(new ProductGUI(list_form, db, list_of_productsGUI.Last()));

            list_of_productsGUI.Last().addToControl();

            if (list_of_productsGUI.Count == 1)
            {

                createButton(resources_directory + @"\FileBlue.png", new Point(655, list_of_productsGUI.Last().getProductLabel().Location.Y + 50), new Size(60, 60));
                list_of_button.Last().Click += new System.EventHandler(this.nutritionFacts_Click);

                createButton(resources_directory + @"\ViewBlue.png", new Point(800, list_of_productsGUI.Last().getProductLabel().Location.Y + 50), new Size(60, 60));
                list_of_button.Last().Click += new System.EventHandler(this.macronutrient_Click);
            }
            // Generate Values Button And Nutrition Values
            else
            {
                list_of_button[list_of_button.Count - 2].Location = new Point(655, list_of_productsGUI.Last().getProductLabel().Location.Y + 50);
                list_of_button[list_of_button.Count - 1].Location = new Point(800, list_of_productsGUI.Last().getProductLabel().Location.Y + 50);
            }

        }

        /// <summary>
        /// method <c>createMeal</c> creates the meal from the list of products.
        /// </summary>
        private void createMeal()
        {
            List<Product> listOfProDucts = new List<Product>();
            for (int i = 0; i < list_of_productsGUI.Count; i++) listOfProDucts.Add(list_of_productsGUI[i].getProduct());
            meal = new Meal(listOfProDucts);
        }

        /// <summary>
        /// method <c>resetNutritionalValues</c> removes any nutritional values displayed in the GUI.
        /// </summary>
        private void resetNutritionalValues()
        {
            for (int i = 0; i < list_of_nutrition_lables.Count; i++)
            {
                list_form[0].Controls.Remove(list_of_nutrition_lables[i]);
            }

            list_of_nutrition_lables.Clear(); meal = null;
        }

        private void addProduct_Click(object sender, EventArgs e)
        {
            addProductGUI(database, list_form[0].Controls);
            resetNutritionalValues();
        }

        private void deleteAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < list_of_productsGUI.Count; i++)
            {
                list_of_productsGUI[i].removeFromControl();
            }
            list_of_productsGUI.Clear();
            //remove Generate Values Button
            list_form[0].Controls.Remove(list_of_button.Last());
            list_of_button.RemoveAt(list_of_button.Count - 1);
            list_form[0].Controls.Remove(list_of_button.Last());
            list_of_button.RemoveAt(list_of_button.Count - 1);

            resetNutritionalValues();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < list_of_productsGUI.Count; i++)
            {
                list_of_productsGUI[i].resetCategory();
            }

            resetNutritionalValues();
        }

        private void macronutrient_Click(object sender, EventArgs e)
        {

        }

        private void nutritionFacts_Click(object sender, EventArgs e)
        {

        }

    }
}