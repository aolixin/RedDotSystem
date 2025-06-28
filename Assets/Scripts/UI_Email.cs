using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedDotSys
{
    public class UI_Email : MonoBehaviour
    {
        public NumberRedDotItem MailDot;

        public NumberRedDotItem MailSystemDot;

        public NormalRedDotItem MailTeamDot;

        public Dictionary<string, EmailRedDotItem> id_emailItemDic = new Dictionary<string, EmailRedDotItem>();

        public GameObject EmailPrefab;

        public GameObject MailSystemContent;

        public GameObject MailTeamContent;


        void Start()
        {
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox, MailCallBack);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_System, MailSystemCallBack);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_Team, MailTeamCallBack);

            //初始显示红点信息
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_System, 0);
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_Team, 0);
        }

        private void OnDestroy()
        {
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox, null);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_System, null);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_Team, null);
        }

        void MailCallBack(RedDotNode node)
        {
            MailDot.SetDotState(node.rdCount > 0, node.rdCount);
        }

        void MailSystemCallBack(RedDotNode node)
        {
            MailSystemDot.SetDotState(node.rdCount > 0, node.rdCount);
        }

        void MailTeamCallBack(RedDotNode node)
        {
            MailTeamDot.SetDotState(node.rdCount > 0, node.rdCount);
        }

        void SystemEmailCallBack(RedDotNode node)
        {
            if (node.rdCount > 0)
            {
                var go = Instantiate(EmailPrefab, MailSystemContent.transform);
                EmailRedDotItem emailItem = go.GetComponent<EmailRedDotItem>();
                emailItem.Init(node, OnReduceRdSystemBtnClick);
                emailItem.SetDotState(true, node.rdCount);

                id_emailItemDic.Add(node.rdName, emailItem);
            }
            else
            {
                if (id_emailItemDic.TryGetValue(node.rdName, out EmailRedDotItem emailItem))
                {
                    emailItem.SetDotState(false);
                }
            }
        }

        void TeamEmailCallBack(RedDotNode node)
        {
            if (node.rdCount > 0)
            {
                var go = Instantiate(EmailPrefab, MailTeamContent.transform);
                EmailRedDotItem emailItem = go.GetComponent<EmailRedDotItem>();
                emailItem.Init(node, OnReduceRdTeamBtnClick);
                emailItem.SetDotState(true, node.rdCount);

                id_emailItemDic.Add(node.rdName, emailItem);
            }
            else
            {
                if (id_emailItemDic.TryGetValue(node.rdName, out EmailRedDotItem emailItem))
                {
                    emailItem.SetDotState(false);
                }
            }
        }


        #region GM

        static int _emailId = 0;

        private int GenEmailID()
        {
            return _emailId++;
        }

        public void OnAddRdSystemBtnClick(Transform EmailParent)
        {
            var path = E_RedDotDefine.MailBox_System + "/" + GenEmailID();
            ManagerComponent.RedDotManager.Add(path, SystemEmailCallBack, 1);
        }

        public void OnAddRdTeamBtnClick(Transform EmailParent)
        {
            var path = E_RedDotDefine.MailBox_Team + "/" + GenEmailID();
            ManagerComponent.RedDotManager.Add(path, TeamEmailCallBack, 1);
        }

        public void OnReduceRdSystemBtnClick(RedDotNode node)
        {
            var path = E_RedDotDefine.MailBox_System + "/" + node.rdName;
            ManagerComponent.RedDotManager.Set(path, 0);
        }

        public void OnReduceRdTeamBtnClick(RedDotNode node)
        {
            var path = E_RedDotDefine.MailBox_Team + "/" + node.rdName;
            ManagerComponent.RedDotManager.Set(path, 0);
        }

        #endregion
    }
}