using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Shipping.UPS.track;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Kooboo.Globalization;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.Shipping.UPS
{
    [Dependency(typeof(IShipmentTracker), Key = "Kooboo.Commerce.Shipping.UPS.UPSShipmentTrackers")]
    public class UPSShipmentTracker : IShipmentTracker
    {
        private static readonly List<ShippingCarrier> _supportedCarriers = new List<ShippingCarrier>
        {
            new ShippingCarrier
            {
                Id = "UPS",
                Name = "UPS"
            }
        };

        private ISettingService _settingsService;

        public string Name
        {
            get
            {
                return "UPS";
            }
        }

        public string DisplayName
        {
            get
            {
                return "UPS";
            }
        }

        public UPSShipmentTracker(ISettingService settingsService)
        {
            _settingsService = settingsService;
        }

        public IEnumerable<ShippingCarrier> GetSupportedShippingCarriers()
        {
            return _supportedCarriers;
        }

        public IEnumerable<ShipmentStatusEvent> GetShipmentStatusEvents(string trackingNumber)
        {
            var settings = UPSSettings.LoadFrom(_settingsService);
            var track = CreateTrackService(settings);
            var request = new TrackRequest();
            var requestType = new RequestType();
            requestType.RequestOption = new string[] { "15" };
            request.Request = requestType;
            request.InquiryNumber = trackingNumber;
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            var trackResponse = track.ProcessTrack(request);
            var result = new List<ShipmentStatusEvent>();

            result.AddRange(trackResponse.Shipment.SelectMany(c => c.Package[0].Activity.Select(x => ToStatusEvent(x))).ToList());

            return result;
        }

        private ShipmentStatusEvent ToStatusEvent(ActivityType activity)
        {
            var ev = new ShipmentStatusEvent();
            switch (activity.Status.Type)
            {
                case "I":
                    if (activity.Status.Code == "DP")
                    {
                        ev.Description = "Departed".Localize();
                    }
                    else if (activity.Status.Code == "EP")
                    {
                        ev.Description = "Export scanned".Localize();
                    }
                    else if (activity.Status.Code == "OR")
                    {
                        ev.Description = "Origin scanned".Localize();
                    }
                    else
                    {
                        ev.Description = "Arrived".Localize();
                    }
                    break;
                case "X":
                    ev.Description = "Not delivered".Localize();
                    break;
                case "M":
                    ev.Description = "Booked".Localize();
                    break;
                case "D":
                    ev.Description = "Delivered".Localize();
                    break;
            }

            var dateString = string.Concat(activity.Date, " ", activity.Time);
            ev.DateUtc = DateTime.ParseExact(dateString, "yyyyMMdd HHmmss", CultureInfo.InvariantCulture);
            ev.CountryCode = activity.ActivityLocation.Address.CountryCode;
            ev.Location = activity.ActivityLocation.Address.City;

            return ev;
        }

        private TrackService CreateTrackService(UPSSettings settings)
        {
            var upss = new UPSSecurity();
            upss.ServiceAccessToken = new UPSSecurityServiceAccessToken
            {
                AccessLicenseNumber = settings.AccessKey
            };
            upss.UsernameToken = new UPSSecurityUsernameToken
            {
                Username = settings.Username,
                Password = settings.Password
            };

            return new TrackService
            {
                UPSSecurityValue = upss
            };
        }
    }
}