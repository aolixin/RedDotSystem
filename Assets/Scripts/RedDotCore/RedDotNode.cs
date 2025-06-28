using System.Collections.Generic;
using UnityEngine;

namespace RedDotSys
{
    public class RedDotNode
    {
        public string rdName { get; set; }
        
        public int rdCount { get; private set; }

        public RedDotNode parent;
        
        public RedDotSystem.OnRdCountChange countChangeFunc;

        public Dictionary<string, RedDotNode> rdChildrenDic = new Dictionary<string, RedDotNode>();


        private void CheckRedDotCount()
        {
            //该红点的计数 = 子红点的计数之和
            int num = 0;
            foreach (RedDotNode node in rdChildrenDic.Values)
                num += node.rdCount;

            //红点计数有变化
            if (num != rdCount)
            {
                rdCount = num;
                NotifyRedDotCountChange();
            }

            parent?.CheckRedDotCount();
        }


        private void NotifyRedDotCountChange()
        {
            countChangeFunc?.Invoke(this);
        }


        public void SetRedDotCount(int rdCount)
        {
            //只能对非根节点进行设定
            if (rdChildrenDic.Count > 0)
            {
                Debug.LogWarning("不可直接设定根节点的红点数");
                return;
            }

            //设定该红点的计数
            this.rdCount = rdCount;

            NotifyRedDotCountChange();

            parent?.CheckRedDotCount();
        }
    }
}