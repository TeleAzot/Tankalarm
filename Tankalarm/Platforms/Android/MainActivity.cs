using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Work;
using Tankalarm.Platforms.Android;

namespace Tankalarm
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SchedulePriceCheckWorker();
        }

        /// <summary>
        /// Check if a worker is running and creates / starts one if not.
        /// </summary>
        private void SchedulePriceCheckWorker()
        {
            var context = Android.App.Application.Context;

            //only start if user has network
            var constraints = new Constraints.Builder()
                .SetRequiredNetworkType(NetworkType.Connected)
                .Build();

            WorkManager.GetInstance(context)
                .Enqueue(
                    OneTimeWorkRequest.Builder
                        .From<FuelPriceCheckWorker>()
                        .SetConstraints(constraints)
                        .Build());

            var request =
                PeriodicWorkRequest.Builder
                    .From<FuelPriceCheckWorker>(TimeSpan.FromMinutes(15))
                    .SetConstraints(constraints)
                    .Build();

            WorkManager
                .GetInstance(context)
                .EnqueueUniquePeriodicWork(
                    "price_check_worker",                 
                    ExistingPeriodicWorkPolicy.Keep,
                    request);
        }
    }
}