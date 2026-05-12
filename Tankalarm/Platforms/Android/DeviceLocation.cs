using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Platforms.Android
{
    public class DeviceLocation
    {
        bool _isCheckingLocation = false;
        CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        public async Task<Location?> GetCurrentDeviceLocationAsync()
        {
            try
            {
                _isCheckingLocation = true;
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                return await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}