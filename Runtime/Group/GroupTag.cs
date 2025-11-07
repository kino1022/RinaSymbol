using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaSymbol.Group {

    public interface IGroupTag {
        
        /// <summary>
        /// グループのID
        /// </summary>
        int GroupId { get; }

        /// <summary>
        /// IDを変更させる
        /// </summary>
        /// <param name="next"></param>
        void ChangeID(int next);
    }
    
    public class GroupTag : SerializedMonoBehaviour, IGroupTag {

        [SerializeField]
        [LabelText("グループID")]
        [ProgressBar(0,10)]
        private int m_groupId = 1;
        
        public int GroupId => m_groupId;

        public void ChangeID(int next) {
            m_groupId = next;
        }
        
    }
}