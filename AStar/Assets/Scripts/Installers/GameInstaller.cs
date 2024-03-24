using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TileMap>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ITile>().To<Tile>().FromComponentInHierarchy().AsCached();
    }
}
