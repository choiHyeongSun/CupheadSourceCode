using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CustomComponent
{    
    public class ComponentBase : MonoBehaviour
    {
        [SerializeField]
        private FComponentData ComponentData;

        private AActor Owner;
        
        public bool IsEnable()
        {
            return enabled;
        }
        protected virtual void Awake()
        {
            Owner = gameObject.GetComponent<AActor>();
            if (Owner == null)
            {
                String error = gameObject.name + " : Owner Is not AActor Class";
                Debug.LogError(error);
            }
        }


        protected virtual void Start() {}
        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}
        protected virtual void Update() {}
        protected virtual void FixedUpdate() {}
        protected virtual void LateUpdate() {}



        public AActor GetOwner => Owner;
        public bool GetCanMove => ComponentData.gCanMove;
        public bool GetCanAim => ComponentData.gCanAim;
        public bool GetCanShoot => ComponentData.gCanShoot;
        public bool GetCanJump => ComponentData.gCanJump;
        public bool GetCanDash => ComponentData.gCanDash;
        public bool GetCanSubAction => ComponentData.gCanSubAction;


        public void SetEnable(bool IsEnable) => enabled = IsEnable;
        public bool GetEnable() => enabled;

    }
}