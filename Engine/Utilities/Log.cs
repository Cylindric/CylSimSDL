using Engine.Interfaces;
using System;

namespace Engine.Utilities
{
    public class Log : ILog
    {
        public void Debug(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
