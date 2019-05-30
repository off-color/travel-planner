﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain;
using TravelPlanner.Infrastructure;

namespace TravelPlanner.Application
{
    public class MainApplication : IApplication
    {
        private readonly ITravelEvent[] travelEvents;
        private List<User> users;
        private int currentUserId;
        private User currentUser;

        public MainApplication(ITravelEvent[] travelEvents)
        {
            this.travelEvents = travelEvents;
            users = new List<User>();
            AddUser();
        }

        public void AddUser() // срабатывает мб при нажатии какой-нибудь кнопочки
        {
            var user = new User(currentUserId);
            currentUserId++;
            users.Add(user);
            currentUser = user;
        }

        public void AddTravel()
        {
            currentUser.AddTravel();
        }

        public void AddEvent(ITravelEvent travelEvent)
        {
            currentUser.AddEvent(travelEvent);
        }

        public List<string> GetEventsNames()
        {
            return travelEvents.Select(e => e.Name).OrderBy(n => n).ToList();
        }

        public ITravelEvent GetEvent(string name, DateTime startDate, DateTime endDate, decimal amountOfMoney, string currency,
            string eventSubType)
        {
            var interval = new DateTimeInterval(startDate, endDate);
            var parsedCurrency = (Currency)Enum.Parse(typeof(Currency), currency);
            var money = new Money(parsedCurrency, amountOfMoney);
            var parsedEventSubType = Enum.Parse(EventTypes[name], eventSubType);
            return GetEvent(name, interval, money, parsedEventSubType);
        }

        public ITravelEvent GetEvent(string name, params object[] parameters)
        {
            var foundEvent = travelEvents.FirstOrDefault(e => e.Name == name);
            if (foundEvent == null)
                throw new ArgumentException($"Unknown event name: {name}");

            var types = parameters.Select(p => p.GetType()).ToArray();
            var constructor = foundEvent.GetType().GetConstructor(types);
            if (constructor == null)
                throw new ArgumentException(
                    $"{foundEvent.GetType()} doesn't have constructor with arguments " +
                    $"of types: {string.Join(", ", types.Select(t => t.Name))}");

            return (ITravelEvent)constructor.Invoke(parameters);
        }

        public List<Travel> GetTravels()
        {
            return currentUser.GetTravels();
        }

        public Dictionary<string, Type> EventTypes => new Dictionary<string, Type>
        {
            { "Жилье", typeof(HousingType) },
            { "Перемещение", typeof(TransferType) },
        };
    }
}