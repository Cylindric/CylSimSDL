using System;

namespace Engine.Models
{
    public static class Time
    {
        private static DateTime _lastUpdate;

        public static float DeltaTime { get; private set; }

        static Time()
        {
            _lastUpdate = DateTime.Now;
        }

        public static void Update()
        {
            DeltaTime = (float)(DateTime.Now - _lastUpdate).TotalSeconds;
            _lastUpdate = DateTime.Now;
        }
    }
}
