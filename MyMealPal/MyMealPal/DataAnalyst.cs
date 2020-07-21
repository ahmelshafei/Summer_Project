using Bytescout.Spreadsheet;
using System;
using System.Collections.Generic;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>DataAnalyst</c> class that analyze and retrieve data from the database.
    /// </summary>
    public class DataAnalyst
    {

        public Spreadsheet database;

        public string category;
        public string recipe;
        public double quantity;

        /// <summary>
        /// constructor <c>DataAnalyst</c>takes the databse, category, recipe, and quantity of food.
        /// </summary>
        /// <param name="db">database.</param>
        /// <param name="categ">category of food.</param>
        /// <param name="reci">recipe of food.</param>
        /// <param name="qnty">quantity of food.</param>
        public DataAnalyst(Spreadsheet db, string categ, string reci, double qnty)
        {
            this.database = db;
            this.category = categ;
            this.recipe = reci;
            this.quantity = qnty;
        }

        /// <summary>
        /// method <c>createProduct</c> analyze data from database as needed and produces a final food product.
        /// </summary>
        /// <returns>Product</returns>
        public Product createProduct()
        {
            Product prod = null;
            string main_item = null;

            Worksheet categ = database.Workbook.Worksheets.ByName(category);
            List<string> step_procedure = new List<string>();
            List<Ingredient> listOfIngreds = new List<Ingredient>();

            int row = getRowInWorksheet(categ, recipe);
            if (row != -1)
            {
                main_item = categ.Cell(row, 5).Value.ToString();
                string[] steps;
                if (categ.Cell(row, 1).Value.ToString().Contains("-")) steps = categ.Cell(row, 1).Value.ToString().Split('-');
                else { steps = new string[1]; steps[0] = categ.Cell(row, 1).Value.ToString(); }

                string[] ingredsList = new string[steps.Length];
                if (categ.Cell(row, 2).Value.ToString().Contains("-")) ingredsList = categ.Cell(row, 2).Value.ToString().Split('-');
                else { ingredsList[0] = categ.Cell(row, 2).Value.ToString(); }

                string[] quantitiesList = new string[steps.Length];
                if (categ.Cell(row, 3).Value.ToString().Contains("-")) quantitiesList = categ.Cell(row, 3).Value.ToString().Split('-');
                else { quantitiesList[0] = categ.Cell(row, 3).Value.ToString(); }

                for (int i = 0; i < steps.Length; i++)
                {
                    string[] ingreds;
                    if (ingredsList[i].Contains(",")) ingreds = ingredsList[i].Split(',');
                    else { ingreds = new string[1]; ingreds[0] = ingredsList[i]; }

                    string[] quantities = new string[ingreds.Length];
                    if (quantitiesList[i].Contains(",")) quantities = quantitiesList[i].Split(',');
                    else { quantities[0] = quantitiesList[i]; }

                    string str = "Step " + (i + 1) + ": ";
                    str += steps[i] + " ";
                    for (int j = 0; j < ingreds.Length; j++)
                    {
                        if (ingreds[j].Contains("*"))
                        {
                            ingreds[j] = ingreds[j].Replace("*", string.Empty);
                            Worksheet category_worksheet = findCategory(ingreds[j]);
                            if (category_worksheet != null)
                            {
                                if (prod != null) prod += addToProduct(category_worksheet, ingreds[j], quantity * Double.Parse(quantities[j]));
                                else prod = addToProduct(category_worksheet, ingreds[j], quantity * Double.Parse(quantities[j]));

                                if (j > 0) str += ", ";
                                if (quantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString()) == 1) str += (quantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString())) + " " + prod.getUnitOfMainIngredient(database, category_worksheet.Cell(row, 5).Value.ToString()) + " of " + ingreds[j];
                                else str += (quantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString())) + " " + prod.getUnitOfMainIngredient(database, category_worksheet.Cell(row, 5).Value.ToString()) + "s of " + ingreds[j];
                            }
                        }
                        else if (!ingreds[j].Contains("("))
                        {
                            if (j > 0) str += ", ";
                            Worksheet value = database.Workbook.Worksheets.ByName("Values");

                            if (quantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString()) == 1) str += (quantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString())) + " " + value.Cell(getRowInWorksheet(value, ingreds[j]), 16).Value.ToString() + " of " + ingreds[j];
                            else str += (quantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString())) + " " + value.Cell(getRowInWorksheet(value, ingreds[j]), 16).Value.ToString() + "s of " + ingreds[j];

                            Ingredient temp_ingredient = new Ingredient(ingreds[j], Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString()));
                            temp_ingredient.setValues(database);
                            if (containsIngredient(listOfIngreds, ingreds[j])) { addIngredient(listOfIngreds, temp_ingredient); }
                            else { listOfIngreds.Add(temp_ingredient); }
                        }

                        else
                        {
                            if (j > 0) str += ", ";
                            str += "Step " + ingreds[j].Replace("(", string.Empty).Replace(")", string.Empty);
                        }
                    }
                    step_procedure.Add(str);
                }
                if (prod != null) prod += new Product(listOfIngreds, recipe, quantity, step_procedure);
                else prod = new Product(listOfIngreds, recipe, quantity, step_procedure);

                prod.setMainItem(main_item);
            }

            return prod;
        }

        /// <summary>
        /// method <c>addToProduct</c> analyze data from database as per ingredient.
        /// </summary>
        /// <param name="categ"></param>
        /// <param name="recipe"></param>
        /// <param name="intermediateQuantity"></param>
        /// <returns>Product</returns>
        private Product addToProduct(Worksheet categ, string recipe, double intermediateQuantity)
        {
            List<string> step_procedure = new List<string>();
            List<Ingredient> listOfIngreds = new List<Ingredient>();

            int row = getRowInWorksheet(categ, recipe);
            if (row != -1)
            {
                string[] steps;
                if (categ.Cell(row, 1).Value.ToString().Contains("-")) steps = categ.Cell(row, 1).Value.ToString().Split('-');
                else { steps = new string[1]; steps[0] = categ.Cell(row, 1).Value.ToString(); }

                string[] ingredsList = new string[steps.Length];
                if (categ.Cell(row, 2).Value.ToString().Contains("-")) ingredsList = categ.Cell(row, 2).Value.ToString().Split('-');
                else { ingredsList[0] = categ.Cell(row, 2).Value.ToString(); }

                string[] quantitiesList = new string[steps.Length];
                if (categ.Cell(row, 3).Value.ToString().Contains("-")) quantitiesList = categ.Cell(row, 3).Value.ToString().Split('-');
                else { quantitiesList[0] = categ.Cell(row, 3).Value.ToString(); }

                for (int i = 0; i < steps.Length; i++)
                {
                    string[] ingreds;
                    if (ingredsList[i].Contains(",")) ingreds = ingredsList[i].Split(',');
                    else { ingreds = new string[1]; ingreds[0] = ingredsList[i]; }

                    string[] quantities = new string[ingreds.Length];
                    if (quantitiesList[i].Contains(",")) quantities = quantitiesList[i].Split(',');
                    else { quantities[0] = quantitiesList[i]; }

                    string str = "Step " + (i + 1) + ": ";
                    str += steps[i] + " ";
                    for (int j = 0; j < ingreds.Length; j++)
                    {
                        if (!ingreds[j].Contains("("))
                        {
                            if (j > 0) str += ", ";
                            Worksheet value = database.Workbook.Worksheets.ByName("Values");

                            if (intermediateQuantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString()) == 1) str += (intermediateQuantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString())) + " " + value.Cell(getRowInWorksheet(value, ingreds[j]), 16).Value.ToString() + " of " + ingreds[j];
                            else str += (intermediateQuantity * Double.Parse(quantities[j]) / Double.Parse(categ.Cell(row, 4).Value.ToString())) + " " + value.Cell(getRowInWorksheet(value, ingreds[j]), 16).Value.ToString() + "s of " + ingreds[j];

                            Ingredient temp_ingredient = new Ingredient(ingreds[j], (Double.Parse(quantities[j])) / (Double.Parse(categ.Cell(row, 4).Value.ToString())));
                            temp_ingredient.setValues(database);
                            if (containsIngredient(listOfIngreds, ingreds[j])) { addIngredient(listOfIngreds, temp_ingredient); }
                            else { listOfIngreds.Add(temp_ingredient); }
                        }
                        else
                        {
                            if (j > 0) str += ", ";
                            str += "Step " + ingreds[j].Replace("(", string.Empty).Replace(")", string.Empty);
                        }
                    }
                    step_procedure.Add(str);
                }
                return new Product(listOfIngreds, recipe, intermediateQuantity, step_procedure);
            }
            return null;
        }

        /// <summary>
        /// method <c>findCategory</c> searches the database for the required category.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns>Worksheet</returns>
        private Worksheet findCategory(string recipe)
        {
            for (int i = 0; i < database.Worksheets.Count; i++)
            {
                if (!database.Worksheets[i].Name.Equals("Values"))
                {
                    Worksheet temp = database.Worksheets[i];
                    int row = 1;
                    while (temp.Cell(row, 0).Value != null && !temp.Cell(row, 0).Value.ToString().Equals(recipe)) { row++; }
                    if (temp.Cell(row, 0).Value != null) return temp;
                }
            }
            return null;
        }

        /// <summary>
        /// method <c>getRowInWorksheet</c> searches for the correct row in the database that corresponds to the name provided.
        /// </summary>
        /// <param name="categ"></param>
        /// <param name="name"></param>
        /// <returns>int</returns>
        private int getRowInWorksheet(Worksheet categ, string name)
        {
            int row = 1;
            if (categ.Name.Equals("Values")) row = 4;
            while (categ.Cell(row, 0).Value != null && !categ.Cell(row, 0).Value.ToString().Equals(name)) { row++; }
            if (categ.Cell(row, 0).Value == null) return -1;
            else return row;
        }

        /// <summary>
        /// method <c>containsIngredient</c> checks whether the ingredient already exist in the current food product.
        /// </summary>
        /// <param name="listOfIngreds"></param>
        /// <param name="ingredName"></param>
        /// <returns>bool</returns>
        private bool containsIngredient(List<Ingredient> listOfIngreds, string ingredName)
        {
            for (int i = 0; i < listOfIngreds.Count; i++)
            {
                if (listOfIngreds[i].getName().Equals(ingredName)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// method <c>addIngredient</c> add ingredient to the current food product.
        /// </summary>
        /// <param name="listOfIngreds"></param>
        /// <param name="ingred"></param>
        private void addIngredient(List<Ingredient> listOfIngreds, Ingredient ingred)
        {
            for (int i = 0; i < listOfIngreds.Count; i++)
            {
                if (listOfIngreds[i].getName().Equals(ingred.getName())) { listOfIngreds[i] += ingred; }
            }
        }

    }
}
