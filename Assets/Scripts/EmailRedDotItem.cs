using System;
using UnityEngine;
using UnityEngine.UI;

namespace RedDotSys
{
    public class EmailRedDotItem : MonoBehaviour, IRedDotItem
    {
        [SerializeField] public GameObject m_DotObj;

        private RedDotNode rdNode;

        public Button btn;

        public void Init(RedDotNode node, RedDotSystem.OnRdCountChange callBack)
        {
            rdNode = node;
            btn.onClick.AddListener(() => { callBack?.Invoke(rdNode); });
        }

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
            rdNode = null;
            m_DotObj = null;
        }
    }
}