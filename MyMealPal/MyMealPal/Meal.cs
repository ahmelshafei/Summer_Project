using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>Meal</c> represents a list of food products.
    /// </summary>
    public class Meal
    {
        private List<Product> list_of_products;

        private NutritionFact nutrition_facts;
        private double cost;

        /// <summary>
        /// constructor <c>Meal</c> takes list of Products.
        /// </summary>
        /// <param name="pro"></param>
        public Meal(List<Product> pro)
        {
            list_of_products = pro;
            setValues();
        }

        /// <summary>
        /// method <c>setValues</c> sets the values of the whole meal.
        /// </summary>
        private void setValues()
        {
            for (int i = 0; i < list_of_products.Count; i++)
            {
                if (i == 0) nutrition_facts = list_of_products[i].getNutritionFacts();
                else nutrition_facts += list_of_products[i].getNutritionFacts();
                cost += list_of_products[i].getCost();
            }
        }

        /// <summary>
        /// method <c>getListOfProducts</c> returns list of products.
        /// </summary>
        public List<Product> getListOfProducts()
        {
            return list_of_products;
        }

        /// <summary>
        /// method <c>getCost</c> returns the cost of the whole meal.
        /// </summary>
        public double getCost()
        {
            return cost;
        }

        /// <summary>
        /// method <c>getNutritionFacts</c> gets the nutritional values of the whole meal.
        /// </summary>
        public NutritionFact getNutritionFacts()
        {
            return nutrition_facts;
        }

    }
}
