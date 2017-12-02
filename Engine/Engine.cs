using Engine.Controllers;
using Engine.Interfaces;
using Engine.Models;
using Engine.Renderer;
using Engine.Utilities;
using SimpleInjector;

namespace Engine
{
    public class Engine
    {
        public readonly Container Container = new Container();

        public Engine()
        {
            // Utilities
            Container.Register<ILog, Log>();

            // Models
            Container.Register<IWorld, World>(Lifestyle.Singleton);
            Container.Register<ITime, Time>(Lifestyle.Singleton);

            // Controllers
            Container.Register<ITileSpriteController, TileSpriteController>(Lifestyle.Singleton);
            Container.Register<IWorldController, WorldController>(Lifestyle.Singleton);
            Container.Register<ICameraController, CameraController>(Lifestyle.Singleton);

            // Renderers
            Container.Register<IWindow, SDLWindow>(Lifestyle.Singleton);

            // Check
            Container.Verify();
        }

        public void Run()
        {
            Container.GetInstance<IWindow>().Start();

            while (true)
            {
                Container.GetInstance<ITime>().Update();
                Container.GetInstance<IWindow>().Update();

                Container.GetInstance<IWorldController>().Update();

                Container.GetInstance<IWindow>().Render();
            }
        }
    }
}
