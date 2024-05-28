using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace utils
{
    public class OnClick : MonoBehaviour, IPointerClickHandler
    {
        private List<Action<int>> _onClickActions;
        
        
        public void SetActions(List<Action<int>> actionsInitializer)
        {
            _onClickActions = actionsInitializer;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Executing OnClick Actions");
            
            foreach (var action in _onClickActions)
            {
                action.Invoke(eventData.clickCount);
            }
        }
    }
}