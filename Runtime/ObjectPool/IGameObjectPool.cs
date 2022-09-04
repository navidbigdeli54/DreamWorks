namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public interface IGameObjectPool
    {
        CPoolableGameObject Get(string name);

        void Retrive(CPoolableGameObject poolableObject);
    }
}