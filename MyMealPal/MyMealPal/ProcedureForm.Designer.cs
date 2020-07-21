using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyMealPal
{
    partial class ProcedureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Procedure";
            this.Size = new System.Drawing.Size(1200, 600);
            this.AutoScroll = true;
        }

        /// <summary>
        /// method <c>displayProcedure</c> displays the steps for the recipe in the Procedure Form.
        /// </summary>
        /// <param name="stepProcedure"></param>
        /// <param name="recipe"></param>
        public void displayProcedure(List<string> stepProcedure, string recipe)
        {
            createLabel("Recipe for " + recipe, new Point(30, 20), 18F);

            createLabel("___________________________________________________________", new Point(30, 50), 12);

            int location = 100;
            for (int i = 0; i < stepProcedure.Count; i++)
            {
                if (stepProcedure[i].Contains("For the")) { if (i != 0) { location += 30; } createLabel(stepProcedure[i], new Point(30, location), 12F); location += 40; }
                else { createLabel(stepProcedure[i], new Point(60, location), 9F); location += 30; }
            }

            createLabel("___________________________________________________________", new Point(30, location), 12);

            ShowDialog();
        }

        /// <summary>
        /// method <c>CreateLabel</c> creates a Label on the Procedure Form.
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

            this.Controls.Add(label);
        }

        #endregion
    }
}