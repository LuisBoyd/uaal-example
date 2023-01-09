using System;
using UnityEngine;

namespace RCR.Patterns
{
    public class BaseView<M,C> : MonoBehaviour where M: BaseModel, new()
    where C : BaseController<M>, new()
    {
        public M Model;
        protected C Controller;

        /// <summary>
        /// Set To false if there are any problems in which case it can be handled
        /// </summary>
        protected bool IsValid;

        protected virtual void Awake()
        {
            IsValid = true;
            Controller = new C();
            Model = new M();
            Controller.Setup(Model);
        }
    }
}