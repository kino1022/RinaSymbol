using System;

namespace RinaSymbol.LifeSycle {
    
    public interface ISymbolSpawnEventBus {
        
        ASymbol Symbol { get; }
        
    }
    
    public readonly struct SymbolSpawnEventBus : ISymbolSpawnEventBus{

        private readonly ASymbol m_symbol;
        
        public ASymbol Symbol => m_symbol;
        
        public SymbolSpawnEventBus(ASymbol symbol) {
            m_symbol = symbol ?? throw new ArgumentNullException();
        }

    }
}