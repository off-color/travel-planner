﻿using System.Collections.Generic;

namespace TravelPlanner.Application.Network.Skyscanner
{
    public class SkyscannerApiParameters
    {
        public static Dictionary<string, string> FromFlightParameters
            (TransportParameters parameters)
        {
            return new Dictionary<string, string>
            {
                {"country", "RU" },
                {"currency", parameters.Currency },
                {"locale", "ru-RU" },
                {"originPlace", parameters.OriginPlace + "-sky" }, //IATA
                {"destinationPlace", parameters.DestinationPlace + "-sky" }, //IATA
                {"outboundDate", parameters.OutboundDate.ToString("yyyy-MM-dd") },
                {"inboundDate", ""},
                {"adults", parameters.AdultsCount.ToString() },
                {"children", parameters.ChildrenCount.ToString() },
                {"cabinClass", parameters.CabinClass },
                {"groupPricing", "true" }
            };
        }
    }
}