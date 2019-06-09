﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using TravelPlanner.Application;
using TravelPlanner.Domain;
using TravelPlanner.Infrastructure;

namespace TravelPlanner.UserInterface
{
    class AddForm : MetroForm
    {
        private readonly IApplication app;
        private readonly DateTimePicker startPicker;
        private readonly DateTimePicker endPicker;
        private readonly ComboBox eventTypeBox;
        private readonly ComboBox subEventTypeBox;
        private readonly ComboBox currencyBox;
        private readonly NumericUpDown amountPicker;
        private readonly MetroTextBox cityBoxStart;
        private readonly MetroTextBox cityBoxEnd;

        public AddForm(IApplication app, IEnumerable<string> cities, ITravelEvent travelEvent)
        {
            this.app = app;
            Size = new Size(800, 600);
            ShadowType = MetroFormShadowType.None;
            startPicker = Elements.GeTimePicker();
            endPicker = Elements.GeTimePicker();
            eventTypeBox = Elements.TypeBox(app.EventHandler.GetEventsNames());
            subEventTypeBox =
                Elements.TypeBox(Enum.GetNames(app.EventHandler.GetEventSubType(app.EventHandler.GetEventsNames()[0])));
            eventTypeBox.SelectedIndexChanged += (sender, args) =>
            {
                subEventTypeBox.DataSource = Enum.GetNames(this.app.EventHandler.GetEventSubType(eventTypeBox.Text));
            };
            currencyBox = Elements.TypeBox(Enum.GetNames(typeof(Currency)));
            amountPicker = new NumericUpDown {Dock = DockStyle.Fill, DecimalPlaces = 2};
            cityBoxStart = Elements.CityBox(cities);
            cityBoxEnd = Elements.CityBox(cities);
            if (travelEvent != null)
            {
                InitElementsText(travelEvent);
            }
            InitTable();
        }

        public AddForm(IApplication app) : this(app, new List<string>(), null) { }

        private void InitElementsText(ITravelEvent travelEvent)
        {
            startPicker.Value = travelEvent.DateTimeInterval.Start;
            endPicker.Value = travelEvent.DateTimeInterval.End;
            eventTypeBox.Text = travelEvent.Name;
            currencyBox.Text = travelEvent.Cost.Currency.ToString();
            amountPicker.Value = travelEvent.Cost.Amount;
        }

        private void InitTable()
        {
            var table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));

            AddControls(table);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);
        }

        private void AddControls(TableLayoutPanel table)
        {
            table.Controls.Add(eventTypeBox, 1, 0);
            table.Controls.Add(subEventTypeBox, 1, 1);
            table.Controls.Add(cityBoxStart, 1, 2);
            table.Controls.Add(cityBoxEnd, 1, 3);
            table.Controls.Add(startPicker, 1, 4);
            table.Controls.Add(endPicker, 1, 5);
            table.Controls.Add(amountPicker, 1, 6);
            table.Controls.Add(currencyBox, 1, 7);
            table.Controls.Add(Elements.GetLabel("Тип события"), 0, 0);
            table.Controls.Add(Elements.GetLabel("Подтип события"), 0, 1);
            table.Controls.Add(Elements.GetLabel("Место отправления"), 0, 2);
            table.Controls.Add(Elements.GetLabel("Место прибытия"), 0, 3);
            table.Controls.Add(Elements.GetLabel("Дата1"), 0, 4);
            table.Controls.Add(Elements.GetLabel("Дата2"), 0, 5);
            table.Controls.Add(Elements.GetLabel("Стоимость"), 0, 6);
            table.Controls.Add(Elements.GetLabel("Валюта"), 0, 7);
            table.Controls.Add(GetNetworkButton(), 0, 8);
            table.Controls.Add(GetSaveButton(), 0, 9);
            table.Controls.Add(Elements.BackButton(this, "Отмена"), 0, 10);
        }

        private Button GetSaveButton()
        {
            var saveButton = Elements.GetButton("Сохранить", (sender, args) =>
            {
                var travelEvent = app.EventHandler.GetEvent(eventTypeBox.Text, 
                    startPicker.Value, endPicker.Value,
                    amountPicker.Value, currencyBox.Text, subEventTypeBox.Text);
                app.UserSessionHandler.AddEvent(travelEvent);
                Close();
            });
            saveButton.Dock = DockStyle.Bottom;
            return saveButton;
        }

        private Button GetNetworkButton()
        {
            var networkButton = Elements.GetButton("Посмотреть варианты в сети", (sender, args) => { });
            networkButton.Dock = DockStyle.Fill;
            return networkButton;
        }
    }
}
