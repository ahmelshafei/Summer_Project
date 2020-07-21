using System;

namespace MyMealPal
{
    /// <summary>
    /// Class <c>NutritionFact</c> class that represents the nutrition facts of a certain ingredient.
    /// </summary>
    public class NutritionFact
    {
        private string[] nutrient_name = new string[13] { "Calories", "Total Fat", "Saturated Fat", "Trans Fat", "Polysaturated Fat", "Monosaturated Fat", "Cholestrol", "Sodium", "Potassium", "Total Carbohydrates", "Dietary Fiber", "Sugar", "Protein" };
        private string[] nutrient_value = new string[13];

        /// <summary>
        /// constructor <c>NutritionFact</c> 
        /// </summary>
        /// <param name="nv">nutrition values</param>
        public NutritionFact(string[] nv)
        {
            nutrient_value = nv;
            // 6, 7, 8
            for (int i = 6; i < 9; i++)
                if (nutrient_value[i] != null) nutrient_value[i] = (Double.Parse(nutrient_value[i]) * 1000).ToString();
        }

        /// <summary>
        /// method <c>round</c> rounds all the nutritional-numerical values to 1 decimal.
        /// </summary>
        public void round()
        {
            for (int i = 0; i < nutrient_value.Length; i++)
            {
                if (nutrient_value[i] != null)
                {
                    nutrient_value[i] = (Math.Round(Double.Parse(nutrient_value[i]), 1).ToString());
                }
            }
        }

        /// <summary>
        /// method <c>macroNutrientRatio</c> produces the macrnutrients ratios of (Protein : Fat : Carbohydrates).
        /// </summary>
        /// <returns></returns>
        public string getMacroNutrientRatio()
        {
            string str = string.Empty;
            double total = Double.Parse(nutrient_value[12]) * 4.0 + Double.Parse(nutrient_value[1]) * 9.0 + Double.Parse(nutrient_value[9]) * 4.0;

            str += Math.Round(((Double.Parse(nutrient_value[12]) * 4.0) / total) * 100).ToString() + "/";
            str += Math.Round(((Double.Parse(nutrient_value[1]) * 9.0) / total) * 100).ToString() + "/";
            str += Math.Round(((Double.Parse(nutrient_value[9]) * 4.0) / total) * 100).ToString();

            return str;
        }

        /// <summary>
        /// method <c>getProtein</c> gets number of grams of protein in an ingredient.
        /// </summary>
        /// <returns></returns>
        public string getProtein()
        {
            return nutrient_value[12];
        }

        /// <summary>
        /// method <c>getFats</c> gets number of grams of fats in an ingredient.
        /// </summary>
        /// <returns></returns>
        public string getFats()
        {
            return nutrient_value[1];
        }

        /// <summary>
        /// method <c>getCarbohydrates</c> gets number of grams of carbohydrates in an ingredient.
        /// </summary>
        /// <returns></returns>
        public string getCarbohydrates()
        {
            return nutrient_value[9];
        }

        /// <summary>
        /// method <c>getCalories</c> gets the total calories in an ingredient.
        /// </summary>
        /// <returns></returns>
        public string getCalories()
        {
            return nutrient_value[0];
        }

        /// <summary>
        /// method <c>getNutritionNames</c> gets the list of nutrient names.
        /// </summary>
        /// <returns>string[]</returns>
        public string[] getNutritionNames()
        {
            return nutrient_name;
        }

        /// <summary>
        /// method <c>getNutritionValues</c> gets the list of nutrient values.
        /// </summary>
        /// <returns>string[]</returns>
        public string[] getNutritionValues()
        {
            return nutrient_value;
        }

        /// <summary>
        /// constructor <c>NutritionFact</c> dummy constructor used only in the overridden operators.
        /// </summary>
        /// <param name="nv">nutrition values</param>
        /// <param name="dummy"></param>
        private NutritionFact(string[] nv, string dummy)
        {
            nutrient_value = nv;
        }

        public static NutritionFact operator +(NutritionFact a, NutritionFact b)
        {
            string[] temp = new string[13];

            for (int i = 0; i < a.nutrient_value.Length; i++)
            {
                if (a.nutrient_value[i] == null)
                {
                    if (b.nutrient_value[i] == null) temp[i] = null;
                    else temp[i] = b.nutrient_value[i];
                }
                else
                {
                    if (b.nutrient_value[i] == null) temp[i] = a.nutrient_value[i];
                    else temp[i] = (Double.Parse(a.nutrient_value[i]) + Double.Parse(b.nutrient_value[i])).ToString();
                }
            }
            return new NutritionFact(temp, null);
        }

        public static NutritionFact operator *(NutritionFact a, double factor)
        {
            string[] temp = new string[13];

            for (int i = 0; i < a.nutrient_value.Length; i++)
            {
                if (a.nutrient_value[i] != null)
                {
                    temp[i] = (Double.Parse(a.nutrient_value[i]) * factor).ToString();
                }
            }
            return new NutritionFact(temp, null);
        }


    }
}
