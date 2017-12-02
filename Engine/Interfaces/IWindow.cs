using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public interface IWindow : IDisposable
    {
        void Start();
        void Update();
        void Render();
    }
}
