using RinaSymbol.Container;
using RinaSymbol.Group;
using VContainer;
using VContainer.Unity;

namespace RinaSymbol.Installer {
    /// <summary>
    /// ゲームマネージャーに対してRinaSymbolの依存関係を注入するインストーラー
    /// </summary>
    public class RinaSymbolManagerInstaller  : IInstaller{

        public void Install(IContainerBuilder builder) {
            
            builder
                .Register<IEntitiesContainer>(Lifetime.Transient)
                .AsImplementedInterfaces();

            builder
                .Register<GroupEntitiesContainer>(Lifetime.Transient)
                .AsImplementedInterfaces();
            
        }
    }
}