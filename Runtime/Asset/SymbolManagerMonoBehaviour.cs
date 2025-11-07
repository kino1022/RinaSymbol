using RinaSymbol.Container;
using RinaSymbol.Group;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;

namespace RinaSymbol.Asset {
    
    /// <summary>
    /// コンポーネントでIEntitiesContainerを持つアセットを管理するためのMonoBehaviour
    /// </summary>
    public class SymbolManagerMonoBehaviour : SerializedMonoBehaviour {
        
        [Title("参照")]

        [OdinSerialize]
        [LabelText("シンボル管理コンテナ")]
        [ReadOnly]
        private IEntitiesContainer m_container;
        
        [OdinSerialize]
        [LabelText("グループ管理コンテナ")]
        [ReadOnly]
        private IGroupEntitiesContainer m_groupContainer;

        private IObjectResolver m_resolver;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            m_resolver = resolver;
        }

        public void Start() {
            
            m_container = m_resolver.Resolve<IEntitiesContainer>();
            
            m_groupContainer = m_resolver.Resolve<IGroupEntitiesContainer>();
            
        }
        
    }
}