using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RinaSymbol {
    public class SymbolLifetimeScope : LifetimeScope {
        
        [Title("参照")]

        [SerializeField]
        [LabelText("インストーラーリスト")]
        private InstallerList m_list;
        
        protected override void Configure(IContainerBuilder builder) {
            
            base.Configure(builder);
            
            var symbol = transform.root.gameObject.GetComponentInChildren<ASymbol>();

            if (symbol is not null) {
                builder
                    .RegisterComponent(symbol)
                    .As<ASymbol>();
            }

            var serializeInstaller = transform.root.gameObject.GetComponentsInChildren<IInstaller>();

            if (serializeInstaller is not null || serializeInstaller.Length is not 0) {
                serializeInstaller.ToList().ForEach(x => x.Install(builder));
            }
            
        }
    }

    public class InstallerList : SerializedMonoBehaviour, IInstaller {
        
        [OdinSerialize]
        [LabelText("インストーラー")]
        private List<IInstaller> m_installers = new List<IInstaller>();

        public void Install(IContainerBuilder builder) {

            if (m_installers.Count is 0) {
                return;
            }
            
            m_installers.ForEach(x => x.Install(builder));
            
        }
    }
}