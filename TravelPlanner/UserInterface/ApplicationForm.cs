﻿using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace TravelPlanner.UserInterface
{
    class ApplicationForm : MetroForm
    {
        public ApplicationForm()
        {
            Size = new Size(800, 600);
            AddAddButton();
        }

        private void AddAddButton()
        {
            var addButton = Elements.GetBottomButton("Добавить");
            addButton.Click += (sender, args) =>
            {
                var createPathForm = new PathForm();
                createPathForm.Show(this);
            };
            Controls.Add(addButton);
        }

    }
}