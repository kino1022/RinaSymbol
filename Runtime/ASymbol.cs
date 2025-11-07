using System;
using System.ComponentModel;
using MessagePipe;
using RinaSymbol.LifeSycle;
using Sirenix.OdinInspector;
using VContainer;

namespace RinaSymbol {

    /// <summary>
    /// キャラクターやオブジェクトなどのシンボルに約束するインターフェース
    /// </summary>
    public interface ISymbol {
        
        /// <summary>
        /// シンボルを死亡させる際に発火するイベント(ISymbolDeadEventBusはここからでしか呼ばないこと)
        /// </summary>
        void OnDead();
        
    }
    
    public abstract class ASymbol : SerializedMonoBehaviour, ISymbol {
        
        protected IPublisher<ISymbolSpawnEventBus> m_spawnPublisher;
        
        protected IPublisher<ISymbolDeadEventBus> m_deadPublisher;

        protected IObjectResolver m_resolver;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            m_resolver = resolver;
        }

        private void Start() {
            OnPreStart();
            
            m_spawnPublisher = m_resolver.Resolve<IPublisher<ISymbolSpawnEventBus>>()
                ?? throw new InvalidOperationException("ISymbolSpawnEventBus Publisher is not registered.");
            
            m_deadPublisher = m_resolver.Resolve<IPublisher<ISymbolDeadEventBus>>()
                ?? throw new InvalidOperationException("ISymbolDeadEventBus Publisher is not registered.");
            
            OnPostStart();
            
            m_spawnPublisher?.Publish(new SymbolSpawnEventBus(this));
        }

        private void OnDestroy() {
            OnPreDestroy();
            
            OnPostDestroy();

            m_deadPublisher?.Publish(new SymbolDeadEventBus(this));
        }
        
        public virtual void OnDead() {
            Destroy(gameObject);
        }
        
        protected virtual void OnPreStart () { }
        
        protected virtual void OnPostStart () { }
        
        protected virtual void OnPreDestroy () { }
        
        protected virtual void OnPostDestroy () { }
        
    }
    
}