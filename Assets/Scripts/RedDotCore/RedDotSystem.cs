using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedDotSys
{
    public static class E_RedDotDefine
    {
        public const string rdRoot = "Root";

        public const string MailBox = "Root/Mail";
        public const string MailBox_System = "Root/Mail/System";
        public const string MailBox_Team = "Root/Mail/Team";
    }


    public class RedDotSystem
    {
        public RedDotSystem()
        {
            InitRedDotTreeNode();
            // Debug.Log("--------------- init RedDotSystem  ---------------");
        }

        public delegate void OnRdCountChange(RedDotNode node);


        private RedDotNode mRootNode;


        private static List<string> lstRedDotTreeList = new List<string>
        {
            E_RedDotDefine.rdRoot,

            E_RedDotDefine.MailBox,
            E_RedDotDefine.MailBox_System,
            E_RedDotDefine.MailBox_Team,
        };


        private void InitRedDotTreeNode()
        {
            mRootNode = new RedDotNode { rdName = E_RedDotDefine.rdRoot };

            foreach (string path in lstRedDotTreeList)
            {
                string[] treeNodeAy = path.Split('/');
                int nodeCount = treeNodeAy.Length;
                RedDotNode curNode = mRootNode;

                if (treeNodeAy[0] != mRootNode.rdName)
                {
                    Debug.LogError("根节点必须为Root，检查 " + treeNodeAy[0]);
                    continue;
                }

                if (nodeCount > 1)
                {
                    for (int i = 1; i < nodeCount; i++)
                    {
                        if (!curNode.rdChildrenDic.ContainsKey(treeNodeAy[i]))
                        {
                            curNode.rdChildrenDic.Add(treeNodeAy[i], new RedDotNode());
                        }

                        curNode.rdChildrenDic[treeNodeAy[i]].rdName = treeNodeAy[i];
                        curNode.rdChildrenDic[treeNodeAy[i]].parent = curNode;

                        curNode = curNode.rdChildrenDic[treeNodeAy[i]];
                    }
                }
            }
        }


        public void SetRedDotNodeCallBack(string strNode, OnRdCountChange callBack)
        {
            var nodeList = strNode.Split('/');

            if (nodeList.Length == 1)
            {
                if (nodeList[0] != E_RedDotDefine.rdRoot)
                {
                    Debug.LogError("Get Wrong Root Node! current is " + nodeList[0]);
                    return;
                }
            }

            var node = mRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.rdChildrenDic.ContainsKey(nodeList[i]))
                {
                    Debug.LogError("Does Not Contain child Node: " + nodeList[i]);
                    return;
                }

                node = node.rdChildrenDic[nodeList[i]];

                if (i == nodeList.Length - 1)
                {
                    node.countChangeFunc = callBack;
                    return;
                }
            }
        }

        public void Add(string nodePath, OnRdCountChange callBack = null, int rdCount = 1)
        {
            // foreach (string path in lstRedDotTreeList)
            string path = nodePath;

            string[] treeNodeAy = path.Split('/');
            int nodeCount = treeNodeAy.Length;
            RedDotNode curNode = mRootNode;

            if (treeNodeAy[0] != mRootNode.rdName)
            {
                Debug.LogError("根节点必须为Root，检查 " + treeNodeAy[0]);
                return;
            }

            if (nodeCount > 1)
            {
                for (int i = 1; i < nodeCount; i++)
                {
                    if (!curNode.rdChildrenDic.ContainsKey(treeNodeAy[i]))
                    {
                        curNode.rdChildrenDic.Add(treeNodeAy[i], new RedDotNode());
                        curNode.rdChildrenDic[treeNodeAy[i]].rdName = treeNodeAy[i];
                        curNode.rdChildrenDic[treeNodeAy[i]].parent = curNode;
                        curNode = curNode.rdChildrenDic[treeNodeAy[i]];
                        curNode.countChangeFunc = callBack;
                        curNode.SetRedDotCount(rdCount);
                    }
                    else
                    {
                        curNode = curNode.rdChildrenDic[treeNodeAy[i]];
                    }
                }
            }
        }

        public void Set(string nodePath, int rdCount = 0)
        {
            string[] nodeList = nodePath.Split('/');

            if (nodeList.Length == 1)
            {
                if (nodeList[0] != E_RedDotDefine.rdRoot)
                {
                    Debug.Log("Get Wrong RootNod！ current is " + nodeList[0]);
                    return;
                }
            }

            //遍历子红点
            RedDotNode node = mRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                //父红点的 子红点字典表 内，必须包含
                if (node.rdChildrenDic.ContainsKey(nodeList[i]))
                {
                    node = node.rdChildrenDic[nodeList[i]];

                    //设置叶子红点的红点数
                    if (i == nodeList.Length - 1)
                    {
                        node.SetRedDotCount(Math.Max(0, rdCount));
                    }
                }
                else
                {
                    Debug.LogError(
                        $"{node.rdName} child node has no Key={nodeList[i]}, check RedDotSystem.InitRedDotTreeNode()");
                    return;
                }
            }
        }


        public int GetRedDotCount(string nodePath)
        {
            string[] nodeList = nodePath.Split('/');

            int count = 0;
            if (nodeList.Length >= 1)
            {
                //遍历子红点
                RedDotNode node = mRootNode;
                for (int i = 1; i < nodeList.Length; i++)
                {
                    //父红点的 子红点字典表 内，必须包含
                    if (node.rdChildrenDic.ContainsKey(nodeList[i]))
                    {
                        node = node.rdChildrenDic[nodeList[i]];

                        if (i == nodeList.Length - 1)
                        {
                            count = node.rdCount;
                            break;
                        }
                    }
                }
            }

            return count;
        }
    }
}