namespace Engine.Interfaces
{
    public interface ITime
    {
        float DeltaTime { get; }

        void Update();
    }
}