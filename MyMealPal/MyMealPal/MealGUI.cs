using Bytescout.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/*
* @author Ahmed ElShafei
* 
* Copyright 2020, all rights reserved.
*/

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
        /// method <c>generateNutritionFacts</c> creates a pdf that contains all the nutritional values of the meal.
        /// </summary>
        private void generateNutritionFacts()
        {
            resetNutritionalValues();
            if (meal == null) createMeal();
            string name = string.Empty;
            List<string> strDetails = new List<string>();
            for (int i = 0; i < meal.getListOfProducts().Count; i++)
            {
                if (i == 0) name = meal.getListOfProducts()[i].getRecipe();
                else name += ", " + meal.getListOfProducts()[i].getRecipe();
                if (meal.getListOfProducts()[i].getQuantity() == 1) strDetails.Add("*" + meal.getListOfProducts()[i].getQuantity().ToString() + " " + meal.getListOfProducts()[i].getUnit(meal.getListOfProducts()[i].getMainIngredient()) + " of " + meal.getListOfProducts()[i].getMainIngredient());
                else strDetails.Add("*" + meal.getListOfProducts()[i].getQuantity().ToString() + " " + meal.getListOfProducts()[i].getUnit(meal.getListOfProducts()[i].getMainIngredient()) + "s of " + meal.getListOfProducts()[i].getMainIngredient());
            }
            NutritionFacts_PDF n = new NutritionFacts_PDF(resources_directory, name, meal.getNutritionFacts(), "1 Meal", strDetails);
        }

        /// <summary>
        /// method <c>generateMealValues</c> generates and adds the important values of each product in the GUI.
        /// </summary>
        private void generateMealValues()
        {
            resetNutritionalValues();
            if (meal == null) createMeal();

            // char length = 9points .. length between two headings 37points

            int location = list_of_button.Last().Location.Y + 100;

            createLabel("_____________________________________________________________________", new Point(40, location), 12);

            location += 30;

            createLabel("Product", new Point(40, location), 12);
            createLabel("Protein", new Point(140, location), 12);
            createLabel("Fats", new Point(240, location), 12);
            createLabel("Carbohydrates", new Point(313, location), 12);
            createLabel("Calories", new Point(467, location), 12);
            createLabel("Cost", new Point(576, location), 12);
            createLabel("P/F/C", new Point(649, location), 12);

            location += 10;
            createLabel("_____________________________________________________________________", new Point(40, location), 12);


            for (int i = 0; i < list_of_productsGUI.Count; i++)
            {
                location = list_of_nutrition_lables.Last().Location.Y + 40;
                createLabel((i+1).ToString(), new Point(40, location), 12);
                createLabel(list_of_productsGUI[i].getProduct().getNutritionFacts().getProtein() + "g", new Point(140, location), 12);
                createLabel(list_of_productsGUI[i].getProduct().getNutritionFacts().getFats() + "g", new Point(240, location), 12);
                createLabel(list_of_productsGUI[i].getProduct().getNutritionFacts().getCarbohydrates() + "g", new Point(313, location), 12);
                createLabel(list_of_productsGUI[i].getProduct().getNutritionFacts().getCalories(), new Point(467, location), 12);
                createLabel("$" + list_of_productsGUI[i].getProduct().getCost().ToString(), new Point(576, location), 12);
                createLabel(list_of_productsGUI[i].getProduct().getNutritionFacts().getMacroNutrientRatio(), new Point(649, location), 12);
            }

            location = list_of_nutrition_lables.Last().Location.Y + 40;
            createLabel("_____________________________________________________________________", new Point(40, location), 12);

            location = list_of_nutrition_lables.Last().Location.Y + 40;
            createLabel("Total", new Point(40, location), 12);
            createLabel(meal.getNutritionFacts().getProtein() + "g", new Point(140, location), 12);
            createLabel(meal.getNutritionFacts().getFats() + "g", new Point(240, location), 12);
            createLabel(meal.getNutritionFacts().getCarbohydrates() + "g", new Point(313, location), 12);
            createLabel(meal.getNutritionFacts().getCalories(), new Point(467, location), 12);
            createLabel("$" + meal.getCost().ToString(), new Point(576, location), 12);
            createLabel(meal.getNutritionFacts().getMacroNutrientRatio(), new Point(649, location), 12);
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
            generateMealValues();
        }

        private void nutritionFacts_Click(object sender, EventArgs e)
        {
            generateNutritionFacts();
        }

    }
}