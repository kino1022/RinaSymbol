using System;

namespace RinaSymbol.LifeSycle {

    /// <summary>
    /// シンボルが死亡した際に発火されるイベントに約束するインターフェース
    /// </summary>
    public interface ISymbolDeadEventBus {
        
        /// <summary>
        /// 死亡したシンボル
        /// </summary>
        ASymbol Symbol { get; }
        
    }
    
    public readonly struct SymbolDeadEventBus : ISymbolDeadEventBus {
        
        private readonly ASymbol m_symbol;
        
        public ASymbol Symbol => m_symbol;
        
        public SymbolDeadEventBus(ASymbol symbol) {
            m_symbol = symbol ?? throw new ArgumentNullException();
        }
        
    }
}