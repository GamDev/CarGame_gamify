using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamify.SnakeGame.StateMachine
{
    public abstract class StateManagerBase
    {
        protected List<IState> statesList = new List<IState>();
        public Transform _uiParentTransform;
        public abstract void ChangeState(Enum statesEnum);
        public virtual void LoadUIPanels()
        {
            foreach (Transform trans in _uiParentTransform)
            {
                if (trans.GetComponent<IState>() != null)
                {
                    statesList.Add(trans.GetComponent<IState>());
                    trans.gameObject.SetActive(false);
                }
            }
        }
        public void Init(Transform uiParentTrans)
        {
            _uiParentTransform = uiParentTrans;
            LoadUIPanels();
        }
        public virtual IState GetCurrentState()
        {
            return null;
        }
    }
}
