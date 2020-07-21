using System;
using Bytescout.Spreadsheet;
using System.Collections.Generic;
using System.Linq;

/*
* @author Ahmed ElShafei
* 
* Copyright 2020, all rights reserved.
*/

namespace MyMealPal
{
    /// <summary>
    /// Class <c>Product</c> represents a food product such as butter chicken, chicken parmesan. Mainly a list of ingredients with certain proportions.
    /// </summary>
    public class Product
    {
        private List<Ingredient> list_of_ingredients;
        private string recipe;
        private double quantity;

        private NutritionFact nutrition_facts;
        private double cost;
        private string main_ingredient;

        public List<string> step_procedure;

        /// <summary>
        /// constructor <c>Product</c> takes ingredients, receipe name, quantity of the food, and steps on how to make the product.
        /// </summary>
        /// <param name="ingred"></param>
        /// <param name="recipe"></param>
        /// <param name="qnty"></param>
        /// <param name="procedure"></param>
        public Product(List<Ingredient> ingred, string recipe, double qnty, List<string> procedure)
        {
            list_of_ingredients = ingred;
            this.recipe = recipe;
            this.quantity = qnty;
            this.step_procedure = procedure;
            setValues();
            multiplyQuantity(qnty);
        }

        /// <summary>
        /// method <c>getUnit</c> get the base unit (grams, tbsp, tsp, ounce .. etc.) of the ingredient.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>string</returns> measurable unit of the ingredient.
        public string getUnit(string name)
        {
            for (int i = 0; i < list_of_ingredients.Count; i++)
            {
                if (list_of_ingredients[i].getName().Equals(name))
                {
                    return list_of_ingredients[i].getUnit();
                }
            }
            return null;
        }

        /// <summary>
        /// method <c>setMainItem</c> sets the main ingredient in the product. eg. chicken is the main item in butter chicken product.
        /// </summary>
        /// <param name="main_item"></param>
        public void setMainItem(string main_item)
        {
            this.main_ingredient = main_item;
        }

        /// <summary>
        /// method <c>getUnitOfMainIngredient</c>  gets the unit of the main ingredient in the product. eg. chicken is the main ingredient in chicken parmesan product.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mainItem"></param>
        /// <returns></returns>
        public string getUnitOfMainIngredient(Spreadsheet db, string mainItem)
        {
            Worksheet value_woksheet = db.Workbook.Worksheets.ByName("Values");
            int row = 4;
            while (value_woksheet.Cell(row, 0).Value != null && !value_woksheet.Cell(row, 0).Value.ToString().Equals(mainItem))
            {
                row++;
            }

            if (value_woksheet.Cell(row, 0).Value != null) { return value_woksheet.Cell(row, 16).Value.ToString(); }
            return null;
        }

        /// <summary>
        /// method <c>getNutritionFacts</c> gets the NutritionFact of the product.
        /// </summary>
        /// <returns></returns>
        public NutritionFact getNutritionFacts()
        {
            return this.nutrition_facts;
        }

        /// <summary>
        /// method <c>getMainIngredient</c> gets the main ingredient of the product.
        /// </summary>
        /// <returns></returns>
        public string getMainIngredient()
        {
            return main_ingredient;
        }

        /// <summary>
        /// method <c>getCost</c> gets the cost of the food product.
        /// </summary>
        /// <returns></returns>
        public double getCost()
        {
            return this.cost;
        }

        /// <summary>
        /// method <c>getRecipe</c> gets the recipe name of the food product.
        /// </summary>
        /// <returns></returns>
        public string getRecipe()
        {
            return this.recipe;
        }

        /// <summary>
        /// method <c>getQuantity</c> gets the quantity of the food product.
        /// </summary>
        /// <returns></returns>
        public double getQuantity()
        {
            return this.quantity;
        }

        /// <summary>
        /// method <c>setValues</c> sets the values of product from individual ingredients.
        /// </summary>
        private void setValues()
        {
            for (int i = 0; i < list_of_ingredients.Count; i++)
            {
                if (i == 0) nutrition_facts = list_of_ingredients[i].getNutritionFacts();
                else nutrition_facts += list_of_ingredients[i].getNutritionFacts();
                this.cost += list_of_ingredients[i].getCost();
            }
        }

        /// <summary>
        /// method <c>multiplyQuantity</c> multiplies the base quantity to the actual quantity the user inputs to get the exact values of the ingredient.
        /// </summary>
        /// <param name="qnty"></param>
        private void multiplyQuantity(double qnty)
        {
            nutrition_facts *= qnty;
            cost *= qnty;
            roundData();
        }

        /// <summary>
        /// method <c>roundData</c> round the nutritional values and round the cost to 2 decimal places.
        /// </summary>
        private void roundData()
        {
            nutrition_facts.round();
            cost = Math.Round(cost, 2);
        }

        /// <summary>
        /// method <c>mergeIngredients</c> adds to products together.
        /// </summary>
        /// <param name="a">product a</param>
        /// <param name="b">product b</param>
        /// <returns></returns>
        private static List<Ingredient> mergeIngredients(Product a, Product b)
        {
            for (int i = 0; i < a.list_of_ingredients.Count; i++)
            {
                a.list_of_ingredients[i].multiplyQuantity(a.quantity);
            }
            List<Ingredient> temp = a.list_of_ingredients;
            for (int i = 0; i < b.list_of_ingredients.Count; i++)
            {
                b.list_of_ingredients[i].multiplyQuantity(b.quantity);
                if (!containsIngredient(temp, b.list_of_ingredients[i])) temp.Add(b.list_of_ingredients[i]);
            }
            return temp;
        }

        /// <summary>
        /// method <c>containsIngredient</c> checks whether product contains the ingredient.
        /// </summary>
        /// <param name="a">list of incgredients</param>
        /// <param name="b">ingredient to be tested</param>
        /// <returns></returns>
        private static bool containsIngredient(List<Ingredient> a, Ingredient b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].isEqual(b)) { a[i] += b; return true; }
            }
            return false;
        }

        /// <summary>
        /// method <c>mergeStepProcedure</c> a product can contain another product as part of it. In this case steps for the first product are added to the steps of the second.
        /// </summary>
        /// <param name="a">steps for making food product 1</param>
        /// <param name="b">steps for making food product 2</param>
        /// <returns></returns>
        private static List<string> mergeStepProcedure(Product a, Product b)
        {
            List<string> temp;
            if (!a.step_procedure.First().Contains("For the"))
            {
                temp = new List<string>();
                temp.Add("For the " + a.recipe);
                for (int i = 0; i < a.step_procedure.Count; i++) temp.Add(a.step_procedure[i]);
            }
            else temp = a.step_procedure;

            temp.Add("For the " + b.recipe);
            for (int i = 0; i < b.step_procedure.Count; i++) temp.Add(b.step_procedure[i]);
            return temp;
        }

        /// <summary>
        /// constructot <c>Product</c> a private construtor that is only used in the overridden operators.
        /// </summary>
        /// <param name="ingred"></param>
        /// <param name="recipe"></param>
        /// <param name="procedure"></param>
        /// <param name="qnty"></param>
        private Product(List<Ingredient> ingred, string recipe, List<string> procedure, double qnty)
        {
            list_of_ingredients = ingred;
            this.recipe = recipe;
            this.step_procedure = procedure;
            this.quantity = qnty;
            setValues();
            roundData();
        }

        public static Product operator +(Product a, Product b)
        {
            return new Product(mergeIngredients(a, b), b.recipe, mergeStepProcedure(a, b), b.quantity);
        }
    }
}