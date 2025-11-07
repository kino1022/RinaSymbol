using System;
using MessagePipe;
using ObservableCollections;
using R3;
using RinaSymbol.LifeSycle;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RinaSymbol.Container {

    /// <summary>
    /// 存在するシンボルを管理するクラスに対して約束するインターフェース
    /// </summary>
    public interface IEntitiesContainer : IEntitiesProvider {
        
        /// <summary>
        /// シンボルを登録する
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        bool Register (ASymbol symbol);
        
        /// <summary>
        /// シンボルを登録解除する
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        bool Unregister (ASymbol symbol);
        
        /// <summary>
        /// 登録されているシンボルを全て登録解除する
        /// </summary>
        void Clear ();
        
    }

    /// <summary>
    /// 他のクラスに対して登録されているシンボルを提供するインタフェース
    /// </summary>
    public interface IEntitiesProvider {
        
        /// <summary>
        /// 登録されているシンボル
        /// </summary>
        IReadOnlyObservableList<ASymbol> Symbols { get; }
        
    }
    
    [Serializable]
    public class EntitiesContainer : IEntitiesContainer,  IStartable, IDisposable{
        
        [OdinSerialize]
        [LabelText("管理しているシンボル")]
        [ReadOnly]
        private ObservableList<ASymbol> m_symbols = new ObservableList<ASymbol>();
        
        private ISubscriber<ISymbolSpawnEventBus> m_spawnSubscriber;
        
        private ISubscriber<ISymbolDeadEventBus> m_deadSubscriber;
        
        private IDisposable m_spawnSubscription;
        
        private IDisposable m_deadSubscription;
        
        private CompositeDisposable m_disposables = new CompositeDisposable();
        
        private IObjectResolver m_resolver;
        
        public IReadOnlyObservableList<ASymbol> Symbols => m_symbols;

        [Inject]
        public EntitiesContainer(IObjectResolver resolver) {
            m_resolver = resolver;
        }

        public void Start() {
            
            m_spawnSubscriber = m_resolver.Resolve<ISubscriber<ISymbolSpawnEventBus>>()
                                   ?? throw new NullReferenceException();
            
            m_deadSubscriber = m_resolver.Resolve<ISubscriber<ISymbolDeadEventBus>>()
                                  ?? throw new NullReferenceException();

            //購読処理
            m_spawnSubscription = m_spawnSubscriber.Subscribe(OnTakeSpawnEventBus);

            m_deadSubscription = m_deadSubscriber.Subscribe(OnTakeDeadEventBus);

            //disposablesに登録
            m_spawnSubscription.AddTo(m_disposables);
            
            m_deadSubscription.AddTo(m_disposables);
            
        }
        
        public void Dispose() {
            
            m_disposables?.Dispose();
            m_disposables?.Clear();
            
        }

        public bool Register(ASymbol symbol) {
            
            if (symbol is null) {
                Debug.LogError($"{nameof(symbol)} is null");
                return false;
            }
            
            m_symbols.Add(symbol);
            return true;
        }

        public bool Unregister(ASymbol symbol) {
                        
            if (symbol is null) {
                Debug.LogError($"{nameof(symbol)} is null");
                return false;
            }
            
            m_symbols.Remove(symbol);
            return true;
        }

        public void Clear() {

            if (m_symbols.Count is 0) {
                return;
            }
            
            for (int i = m_symbols.Count; i >= 0; i--) {
                var symbol = m_symbols[i];
                if (symbol is not null) {
                    Unregister(symbol);
                }
            }
        }

        protected virtual void OnTakeSpawnEventBus(ISymbolSpawnEventBus eventBus) {
            var symbol = eventBus.Symbol;

            if (symbol is null) {
                Debug.LogError($"{nameof(symbol)} is null");
                return;
            }

            Register(symbol);
        }

        protected virtual void OnTakeDeadEventBus(ISymbolDeadEventBus eventBus) {
            var symbol = eventBus.Symbol;

            if (symbol is null) {
                Debug.LogError($"{eventBus.Symbol} is null");
                return;
            }

            Unregister(symbol);
        }
        
    }
}