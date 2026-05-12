using Android.App;
using Android.Content;
using AndroidX.Core.Content;
using AndroidX.Work;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Net.Http;
using Tankalarm.Data.API.Models;
using Tankalarm.Data.API.Services;
using Tankalarm.Data.DB.DBServices;
using AndroidApp = Android.App.Application;
using AndroidContext = Android.Content.Context;

namespace Tankalarm.Platforms.Android
{
    public class FuelPriceCheckWorker : Worker
    {
        readonly PriceAlarmService _alarmSvc;
        readonly TankerkoenigService _tankerkoenigSvc;
        readonly DeviceLocation _deviceLocation;

        //temp for emulator tests
        const double lat = 48.797592, lon = 10.024963;

        public FuelPriceCheckWorker(Context context, WorkerParameters parameters)
        : base(context, parameters)
        {
            _alarmSvc = new PriceAlarmService();
            _tankerkoenigSvc = new TankerkoenigService();
            _deviceLocation = new DeviceLocation();
        }

        public override Result DoWork()
        {
            try
            {
                var alarms = _alarmSvc.GetPriceAlarmsAsync().Result;
                if (alarms.Count() == 0)
                    return Result.InvokeSuccess();

                //get current phone location
                Location? currentLocation = _deviceLocation.GetCurrentDeviceLocationAsync().Result;
                if (currentLocation == null)
                    return Result.InvokeFailure();

                //do price check
                foreach (var alarm in alarms)
                {
                    //get cheapest prices in radius of 5km and check if cheapest one matches the target price
                    var currentPrices = _tankerkoenigSvc.GetCheapestFuelPricesAsync(lon, lat, 5, alarm.FuelType).Result;
                    if (currentPrices.Count() > 0 && currentPrices.First().Price <= alarm.TargetPrice)
                        SendNotification(currentPrices.First(), alarm.FuelType);
                }

                return Result.InvokeSuccess();
            }
            catch
            {
                return Result.InvokeRetry();
            }
        }

        private void SendNotification(FuelStation cheapestStation, string fuelType)
        {
            var channelId = "priceAlerts";
            var manager = AndroidApp.Context
                              .GetSystemService(AndroidContext.NotificationService)
                              as NotificationManager;

            if (manager.GetNotificationChannel(channelId) == null)
            {
                var channel = new NotificationChannel(
                    channelId, "Preis Alarm", NotificationImportance.Default);
                manager.CreateNotificationChannel(channel);
            }

            //intent is used for opening app when clicking on notification
            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, 0, intent, 
                PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

            var builder = new Notification.Builder(AndroidApp.Context, channelId)
                    .SetContentTitle($"Preisalarm {fuelType.ToUpper()}")
                    .SetContentText($"{cheapestStation.Price}€ bei {cheapestStation.Name}")
                    .SetSmallIcon(Resource.Drawable.notification_icon_background)
                    .SetContentIntent(pendingIntent)
                    .SetAutoCancel(true);

            //wichtig: für jede Spritsorte eine feste Notification ID verwenden, sodass immer die alte überschrieben wird
            //=> so bekommt man nicht unendlich viele Notifications
            manager.Notify(GetNotificationIdForFuelType(fuelType), builder.Build());
        }

        private int GetNotificationIdForFuelType(string fuelType)
        {
            return fuelType switch
            {
                "diesel" => 1001,
                "e5" => 2001,
                "e10" => 30001,
                _ => 9999
            };
        }
    }
}