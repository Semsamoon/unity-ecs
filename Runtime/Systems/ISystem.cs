namespace ECS
{
    public interface ISystem
    {
        public void Initialize(IWorld world);
        public void Update();
        public void Destroy();
    }
}