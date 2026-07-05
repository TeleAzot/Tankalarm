using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Data.API.Services
{
    public static class NotificationNavigationService
    {
        public static event Action<string>? RouteReceived;

        private static string? _pendingRoute;

        public static void SetRoute(string route)
        {
            _pendingRoute = route;
            RouteReceived?.Invoke(route);
        }

        public static string? ConsumePendingRoute()
        {
            var route = _pendingRoute;
            _pendingRoute = null;
            return route;
        }
    }
}
