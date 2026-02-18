using Android.App;
using Android.Content;
using AndroidX.Work;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Net.Http;
using AndroidApp = Android.App.Application;
using AndroidContext = Android.Content.Context;

namespace Tankalarm.Platforms.Android
{
    public class FuelPriceCheckWorker : Worker
    {
        public FuelPriceCheckWorker(Context context, WorkerParameters parameters)
        : base(context, parameters)
        {
        }

        public override Result DoWork()
        {
            try
            {
                SendNotification("Preis niedrig!");

                return Result.InvokeSuccess();
            }
            catch
            {
                return Result.InvokeRetry();
            }
        }

        private void SendNotification(string message)
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

            var builder = new Notification.Builder(AndroidApp.Context, channelId)
                .SetContentTitle("Preisalarm")
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.notification_icon_background);

            manager.Notify(1001, builder.Build());
        }
    }
}
