using Engine.Interfaces;

namespace Engine.Controllers
{
    internal class TileSpriteController : ITileSpriteController
    {
        private ILog _log;
        private IWorld _world;

        public TileSpriteController(ILog log, IWorld world)
        {
            _log = log;
            _world = world;
        }
    }
}
