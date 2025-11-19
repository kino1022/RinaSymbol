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
            
            if (m_list is not null) {
                m_list.Install(builder);
            }
            
            var symbol = transform.root.gameObject.GetComponentInChildren<ASymbol>();

            if (symbol is not null) {
                builder
                    .RegisterComponent(symbol)
                    .As<ASymbol>();
            }

            var serializeInstaller = transform.root.gameObject.GetComponentsInChildren<IInstaller>();

            if (m_list is not null) {
                serializeInstaller.ToList().Remove(m_list);
            }

            if (serializeInstaller?.Length is not 0) {
                
                serializeInstaller?.ToList().ForEach(x => x.Install(builder));
            }
            
        }
    }
}