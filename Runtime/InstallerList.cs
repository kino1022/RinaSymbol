using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;
using VContainer.Unity;

namespace RinaSymbol {
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