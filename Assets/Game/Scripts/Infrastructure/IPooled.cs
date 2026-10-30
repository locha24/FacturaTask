namespace Game.Infrastructure
{
    // Implemented by pooled objects that reset themselves every time they are taken out of the pool.
    public interface IPooled
    {
        void OnSpawned();
    }
}
