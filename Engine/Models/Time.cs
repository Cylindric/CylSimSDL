using Engine.Interfaces;
using System;

namespace Engine.Models
{
    public class Time : ITime
    {
        private DateTime _lastUpdate;

        public float DeltaTime { get; private set; }

        public void Update()
        {
            DeltaTime = (float)(DateTime.Now - _lastUpdate).TotalSeconds;
            _lastUpdate = DateTime.Now;
        }
    }
}
