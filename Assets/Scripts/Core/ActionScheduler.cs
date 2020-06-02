using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler: MonoBehaviour
    {
        IAction currentAction;//现在的动作

        /// <summary>
        /// 开始的动作
        /// </summary>
        /// <param name="action">将要更改的动作？</param>
        public void StartAction(IAction action)
        {
            if(currentAction == action) return;

            if(currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;    //更改动作？
        }
        /// <summary>
        /// 取消当前的行动
        /// </summary>
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}