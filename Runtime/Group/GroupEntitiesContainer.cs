using System;
using System.Collections.Generic;
using MessagePipe;
using ObservableCollections;
using R3;
using RinaSymbol.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;
using VContainer.Unity;

namespace RinaSymbol.Group {

    public interface IGroupEntitiesContainer : IGroupEntitiesProvider {
        
    }

    /// <summary>
    /// グループとシンボルの対応を提供するクラスに対して約束するインターフェース
    /// </summary>
    public interface IGroupEntitiesProvider {
        
        /// <summary>
        /// グループごとに該当するシンボル
        /// </summary>
        IReadOnlyObservableDictionary<IGroupTag, List<ASymbol>> GroupSymbol { get; }
        
    }
    
    [Serializable]
    public class GroupEntitiesContainer : IGroupEntitiesContainer, IStartable, IDisposable {
        
        [OdinSerialize]
        [LabelText("シンボル")]
        [ReadOnly]
        private ObservableDictionary<IGroupTag, List<ASymbol>> m_groupSymbol = new();
        
        private IEntitiesProvider m_entitiesProvider;
        
        private CompositeDisposable m_disposables = new();

        private IObjectResolver m_resolver;
        
        public IReadOnlyObservableDictionary<IGroupTag, List<ASymbol>> GroupSymbol => m_groupSymbol;

        [Inject]
        public GroupEntitiesContainer(IObjectResolver resolver) {
            m_resolver = resolver;
        }

        public void Start() {
            
            m_entitiesProvider = m_resolver.Resolve<IEntitiesProvider>()
                                 ?? throw new ArgumentNullException();
            
            RegisterEntitiesChange();
            
        }

        public void Dispose() {
            m_disposables?.Dispose();
            m_disposables = null;
        }

        private void RegisterEntitiesChange() {
            m_entitiesProvider
                .Symbols
                .ObserveAdd()
                .Subscribe(addEvent => {
                    
                    var symbol = addEvent.Value;
                    var tag = symbol.gameObject.transform.root.GetComponentsInChildren<IGroupTag>();

                    if (tag.Length is 0) {
                        return;
                    }

                    foreach (var t in tag) {
                        var list = m_groupSymbol.GetValueOrDefault(t);
                        if (list is null) {
                            list = new List<ASymbol>();
                            m_groupSymbol[t] = list;
                        }
                        list.Add(symbol);
                    }
                    
                })
                .AddTo(m_disposables);
            
            m_entitiesProvider.Symbols.ObserveRemove()
                .Subscribe(removeEvent => {
                    var tag = removeEvent.Value.transform.root.GetComponentsInChildren<IGroupTag>();

                    if (tag.Length is 0) {
                        return;
                    }

                    foreach (var t in tag) {
                        var list = m_groupSymbol.GetValueOrDefault(t);
                        if (list is null) {
                            continue;
                        }
                        list.Remove(removeEvent.Value);
                    }
                })
                .AddTo(m_disposables);
        }
        
    }
}