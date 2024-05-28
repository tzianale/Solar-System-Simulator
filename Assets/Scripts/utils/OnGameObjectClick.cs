using System;
using UnityEngine;
using System.Collections.Generic;


namespace utils
{
    /// <summary>
    /// OnGameObjectClick provides a framework for reacting to clicks on GameObjects.
    /// The Script also supports double / triple / quadruple / etc. click detection
    /// </summary>
    public class OnGameObjectClick : MonoBehaviour
    {
        private const int LeftMouseButton = 0;
        private const float MaxTimeBetweenMultipleClicks = 1f;
        
        private Camera _gameCamera;
        
        private int _lastClickCount;
        private float _lastClickTime;
        
        private List<Action<int>> _onClickActions;
        
        /// <summary>
        /// Called on Script instantiation, sets up the Camera and the last click time
        /// </summary>
        private void Awake()
        {
            _gameCamera = Camera.main;
            _lastClickTime = Time.time;
        }

        /// <summary>
        /// Sets the actions that will be executed on click
        /// </summary>
        /// 
        /// <param name="actionsInitializer">
        /// The Action list. Actions return void and take an integer (click count) as parameter
        /// </param>
        public void SetActions(List<Action<int>> actionsInitializer)
        {
            _onClickActions = actionsInitializer;
        }
        
        /// <summary>
        /// Update is called once per frame.
        /// If a click is detected, it tries to obtain the clicked object using Ray casting and, if the object
        /// corresponds to the one containing this instance of the script,
        /// the saved Actions will be scheduled for execution
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(LeftMouseButton))
            {
                var mousePosition = Input.mousePosition;
                var ray = _gameCamera.ScreenPointToRay(mousePosition);
                
                if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == gameObject)
                {
                    ExecuteClickActions(GetClickCount());
                }
            }
        }

        /// <summary>
        /// Executes all the actions saved in the _onClickActions list
        /// </summary>
        /// 
        /// <param name="clickCount">
        /// How many times the object has been clicked in a set timeframe
        /// </param>
        private void ExecuteClickActions(int clickCount)
        {
            foreach (var action in _onClickActions)
            {
                action.Invoke(clickCount);
            }
        }

        /// <summary>
        /// Helper method to obtain the number of times a planet has been clicked subsequently
        /// </summary>
        /// 
        /// <returns>
        /// The click count - lowest possible result is one, with no bounds for the highest count
        /// </returns>
        private int GetClickCount()
        {
            var currentTime = Time.time;
            var timeBetweenClicks = currentTime - _lastClickTime;

            if (timeBetweenClicks <= MaxTimeBetweenMultipleClicks)
            {
                _lastClickCount += 1;
            }
            else
            {
                _lastClickCount = 1;
            }

            _lastClickTime = currentTime;
            return _lastClickCount;
        }
    }
}