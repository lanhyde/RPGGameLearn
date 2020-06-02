using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]//序列化  使其可以在面板上面看到
        float healthPoints = 100;//Hp  初始化100
        bool isDead = false;//是否死亡

        /// <summary>
        /// 外部获取 角色是否死亡
        /// </summary>
        /// <returns>isDead</returns>
        public bool IsDead()
        {
            return isDead;
        }
        /// <summary>
        /// 受到的伤害
        /// </summary>
        /// <param name="damage">伤害量</param>
        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);//设定Hp
            if(healthPoints == 0)
            {
                //死亡
                Die();
            }
        }
        /// <summary>
        /// 魂归天际相关
        /// </summary>
        private void Die()
        {
            if(isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");//设置死亡动画的触发
            GetComponent<ActionScheduler>().CancelCurrentAction();//取消动作
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns>Hp?之后会有其他的?</returns>
        public object CaptureState()
        {
            return healthPoints;
        }
        /// <summary>
        /// 恢复状态
        /// </summary>
        /// <param name="state">？</param>
        public void RestoreState(object state)
        {
           healthPoints = (float)state;
           if(healthPoints == 0)
           {
               Die();
           }
        }

    }
}