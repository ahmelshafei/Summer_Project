using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

/*
* @author Ahmed ElShafei
* 
* Copyright 2020, all rights reserved.
*/

namespace MyMealPal
{
    /// <summary>
    /// Class <c>NutritionFacts_PDF</c> this class creates a pdf file that contains a detailed breakdown of the nutrition facts of the meal.
    /// </summary>
    public class NutritionFacts_PDF
    {
        //string resources_directory;
        //string meal_name;
        //string serving_size;
        float current_position;

       // NutritionFact nutrition_facts;
        //List<string> details;

        public NutritionFacts_PDF(string resources_directory, string meal_name, NutritionFact nutr, string servSize, List<string> details)
        {
            generatePDF(resources_directory, meal_name, nutr, servSize, details);
        }

        /// <summary>
        /// method <c>generatePDF</c> creates, saves, and opens the pdf file that represents the nutrition facts of the meal.
        /// </summary>
        /// <param name="resources_directory"></param>
        /// <param name="meal_name"></param>
        /// <param name="nutrition_facts"></param>
        /// <param name="serving_size"></param>
        /// <param name="details"></param>
        private void generatePDF(string resources_directory, string meal_name, NutritionFact nutrition_facts, string serving_size, List<string> details)
        {
            Document document = new Document(PageSize.A7);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(resources_directory + "/Nutrition_Temp.pdf", FileMode.Create));
            document.Open();

            PdfContentByte content_byte = writer.DirectContent;

            current_position = document.Top + 18;

            // Create Border

            content_byte.Rectangle(document.Left - 18, document.Bottom - 18, document.Right, document.Top);
            content_byte.Stroke();

            writeLine(document, content_byte, "Nutrition Facts", null, (15 * (document.Right + 36) / 210), 18 + 7, true); current_position -= 5;
            writeLine(document, content_byte, meal_name, null, (7 * (document.Right + 36) / 210), 18 + 7, false); current_position -= 5;
            writeLine(document, content_byte, "Serving size: " + serving_size, "[" + nutrition_facts.getMacroNutrientRatio() + "]", (7 * (document.Right + 36) / 210), 18 + 7, false);

            for (int i = 0; i < nutrition_facts.getNutritionNames().Length; i++)
            {
                if (nutrition_facts.getNutritionValues()[i] != null)
                {
                    if (i == 0)
                    {
                        drawLine(document, content_byte, true, true);
                        writeLine(document, content_byte, nutrition_facts.getNutritionNames()[i], nutrition_facts.getNutritionValues()[i], (7 * (document.Right + 36) / 210), 18 + 7, true);
                    }
                    else
                    {
                        if (nutrition_facts.getNutritionNames()[i].Equals("Total Fat"))
                        {
                            drawLine(document, content_byte, false, true);
                            writeLine(document, content_byte, nutrition_facts.getNutritionNames()[i], nutrition_facts.getNutritionValues()[i] + "g", (7 * (document.Right + 36) / 210), 18 + 7, true);
                        }
                        else if (nutrition_facts.getNutritionNames()[i].Equals("Cholestrol") || nutrition_facts.getNutritionNames()[i].Equals("Sodium") || nutrition_facts.getNutritionNames()[i].Equals("Potassium") || nutrition_facts.getNutritionNames()[i].Equals("Total Carbohydrates") || nutrition_facts.getNutritionNames()[i].Equals("Protein"))
                        {
                            drawLine(document, content_byte, false, true);
                            if (nutrition_facts.getNutritionNames()[i].Equals("Total Carbohydrates") || nutrition_facts.getNutritionNames()[i].Equals("Protein")) writeLine(document, content_byte, nutrition_facts.getNutritionNames()[i], nutrition_facts.getNutritionValues()[i] + "g", (7 * (document.Right + 36) / 210), 18 + 7, true);
                            else writeLine(document, content_byte, nutrition_facts.getNutritionNames()[i], nutrition_facts.getNutritionValues()[i] + "mg", (7 * (document.Right + 36) / 210), 18 + 7, true);
                        }
                        else
                        {
                            drawLine(document, content_byte, false, false);
                            writeLine(document, content_byte, nutrition_facts.getNutritionNames()[i], nutrition_facts.getNutritionValues()[i] + "g", (7 * (document.Right + 36) / 210), 18 + 7 + 10, false);
                        }
                    }
                }
            }
            drawLine(document, content_byte, true, true);

            current_position -= 2;
            writeLine(document, content_byte, "Quantities in this meal: ", null, (9 * (document.Right + 36) / 210), 18 + 7, true);
            current_position -= 2;

            for (int i = 0; i < details.Count; i++)
            {
                current_position -= 2;
                writeLine(document, content_byte, details[i], null, (7 * (document.Right + 36) / 210), 18 + 7, false);
            }

            drawLine(document, content_byte, true, true);

            document.Close();

            System.Diagnostics.Process.Start(resources_directory + "/Nutrition_Temp.pdf");
        }

        /// <summary>
        /// method <c>drawLine</c> draws a horizontal line in a pdf document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="content_byte"></param>
        /// <param name="thick"></param>
        /// <param name="full"></param>
        private void drawLine(Document document, PdfContentByte content_byte, bool thick, bool full)
        {
            if (thick) { content_byte.SetLineWidth(2); current_position -= 6; }
            else { content_byte.SetLineWidth(0.25); current_position -= 4; }

            if (full) content_byte.MoveTo(18 + 7, current_position);
            else content_byte.MoveTo(18 + 7 + 10, current_position);

            content_byte.LineTo(document.Right + 18 - 7, current_position);
            content_byte.Stroke();
        }

        /// <summary>
        /// method <c>writeLine</c> writes a line of words in a pdf document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="content_byte"></param>
        /// <param name="txt"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="x"></param>
        /// <param name="bold"></param>
        private void writeLine(Document document, PdfContentByte content_byte, string txt, string value, float size, float x, bool bold)
        {
            current_position -= (size * 17.0F / 15.0F);
            content_byte.BeginText();
            if (bold) content_byte.SetFontAndSize(getFont("arial black", "ariblk.ttf").BaseFont, size);
            else content_byte.SetFontAndSize(getFont("arial", "arial.ttf").BaseFont, size);
            content_byte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txt, x, current_position, 0);
            if (value != null) content_byte.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, value, document.Right + 18 - 7, current_position, 0);
            content_byte.EndText();
        }

        /// <summary>
        /// method <c>getFont</c> gets a specific font from the font factory of itextsharp.
        /// </summary>
        /// <param name="fontName"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private Font getFont(string fontName, string filename)
        {
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = "C:/Windows/Fonts/" + filename;
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

    }
}
