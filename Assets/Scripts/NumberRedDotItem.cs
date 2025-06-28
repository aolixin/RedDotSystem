using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedDotSys
{
    public class NumberRedDotItem : MonoBehaviour, IRedDotItem
    {
        [SerializeField] public GameObject m_DotObj;

        [SerializeField] private TextMeshProUGUI m_DotCountText;

        public void SetDotState(bool isShow, int dotCount = -1)
        {
            if (isShow)
            {
                m_DotObj.gameObject.SetActive(true);
                m_DotCountText.gameObject.SetActive(true);
                if (m_DotCountText)
                    m_DotCountText.SetText(dotCount >= 0 ? dotCount.ToString() : "");
            }
            else
            {
                m_DotObj.gameObject.SetActive(false);
                m_DotCountText.gameObject.SetActive(false);
                m_DotCountText.SetText("");
            }
        }

        private void OnDestroy()
        {
            m_DotObj = null;
            m_DotCountText = null;
        }
    }
}