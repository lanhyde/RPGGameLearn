using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public partial class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float maxSpeed = 6f;//最大速度
        private NavMeshAgent navMeshAgent;//导航网络
        private Health health;
        private void Start() {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        /// <summary>
        /// 更新动画师
        /// </summary>
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);//将方向从世界空间转换为局部空间

            #region  暂不明  
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed); //设置变换速度  
            #endregion
        }







        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
 
        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles =  data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}