using Bytescout.Spreadsheet;
using System;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>Ingredient</c> class that represents an ingredient.
    /// </summary>
    public class Ingredient
    {
        private string name;
        private double base_quantity;

        private NutritionFact nutrition_facts;
        private double cost;
        private string unit;

        /// <summary>
        /// constructor <c>Ingredient</c>
        /// </summary>
        /// <param name="name">name of an ingredient</param>
        /// <param name="base_qnty">a unit relative quantity that all other ingredient values are based on, for eg. 1 gram of chicken has certain grams of protein and certain grams of carbs, 1 is the base quantity. </param>
        public Ingredient(string name, double base_qnty)
        {
            this.name = name;
            this.base_quantity = base_qnty;
        }

        /// <summary>
        /// method <c>setValues</c> gets the base values of the ingredient from the database.
        /// </summary>
        /// <param name="db"></param>
        public void setValues(Spreadsheet db)
        {
            Worksheet value_worksheet = db.Workbook.Worksheets.ByName("Values");

            int row = 4;
            while (value_worksheet.Cell(row, 0).Value != null && !value_worksheet.Cell(row, 0).Value.ToString().Equals(this.name)) { row++; }

            if (value_worksheet.Cell(row, 0).Value != null)
            {
                string[] temp = new string[13];
                for (int i = 0; i < 13; i++)
                {
                    if (value_worksheet.Cell(row, i + 1).Value != null && !value_worksheet.Cell(row, 0).Value.ToString().Equals("Butter") && !value_worksheet.Cell(row, 0).Value.ToString().Contains("Oil")) temp[i] = value_worksheet.Cell(row, i + 1).Value.ToString();
                    else temp[i] = null;
                }

                nutrition_facts = new NutritionFact(temp);
                nutrition_facts = nutrition_facts * base_quantity;

                unit = value_worksheet.Cell(row, 16).Value.ToString();
                cost = base_quantity * Double.Parse(value_worksheet.Cell(row, 17).Value.ToString());
            }
        }

        /// <summary>
        /// method <c>multiplyQuantity</c> multiplies the base quantity to the actual quantity the user inputs to get the exact values of the ingredient.
        /// </summary>
        /// <param name="qnty"></param>
        public void multiplyQuantity(double qnty)
        {
            nutrition_facts = nutrition_facts * qnty;
            cost *= qnty;
        }

        /// <summary>
        /// method <c>isEqual</c> checks if the ingredient the same as this ingredient.
        /// </summary>
        /// <param name="a"></param>
        /// <returns>bool</returns> true if same ingredient, otherwise false.
        public bool isEqual(Ingredient a)
        {
            return a.name.Equals(name);
        }

        /// <summary>
        /// method <c>getName</c> gets the name of the ingredient.
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return this.name;
        }

        /// <summary>
        /// method <c>getUnit</c> gets the unit of the ingredient.
        /// </summary>
        /// <returns></returns>
        public string getUnit()
        {
            return this.unit;
        }

        /// <summary>
        /// method <c>getCost</c> gets the cost of the ingredient.
        /// </summary>
        /// <returns></returns>
        public double getCost()
        {
            return this.cost;
        }

        /// <summary>
        /// method <c>getNutritionFacts</c> gets the NutritionFact of the ingredient.
        /// </summary>
        /// <returns></returns>
        public NutritionFact getNutritionFacts()
        {
            return this.nutrition_facts;
        }

        /// <summary>
        /// constructor <c>Ingredient</c> private constructor used for the + operator.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="baseQuantity"></param>
        /// <param name="nutr"></param>
        /// <param name="unit"></param>
        /// <param name="cost"></param>
        private Ingredient(string name, double baseQuantity, NutritionFact nutr, string unit, double cost)
        {
            this.name = name;
            base_quantity = baseQuantity;
            nutrition_facts = nutr;
            this.unit = unit;
            this.cost = cost;
        }

        public static Ingredient operator +(Ingredient a, Ingredient b)
        {
            if (!a.name.Equals(b.name))
            {
                throw new Exception("Conflict Products");
            }
            return new Ingredient(a.name, a.base_quantity, a.nutrition_facts + b.nutrition_facts, a.unit, a.cost + b.cost);
        }

    }
}