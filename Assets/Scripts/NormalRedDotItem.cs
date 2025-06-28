using UnityEngine;
using UnityEngine.UI;

namespace RedDotSys
{
    public class NormalRedDotItem : MonoBehaviour, IRedDotItem
    {
        [SerializeField] public GameObject m_DotObj;
        
        public void SetDotState(bool isShow, int dotCount = -1)
        {
            if (isShow)
            {
                m_DotObj.gameObject.SetActive(true);
            }
            else
            {
                m_DotObj.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            m_DotObj = null;
        }
    }
}