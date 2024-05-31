using System;
using System.Collections.Generic;

namespace Utils
{
    public class Wrapper<T>
    {
        private T _savedValue;
        private readonly List<Action<T, T>> _onSetValue;
        
        public Wrapper(T  elementInitializer)
        {
            _onSetValue = new List<Action<T, T>>();
            _savedValue = elementInitializer;
        }

        public void SetValue(T newValue)
        {
            foreach (var action in _onSetValue)
            {
                action.Invoke(_savedValue, newValue);
            }
            
            _savedValue = newValue;
        }

        public T GetValue()
        {
            return _savedValue;
        }

        public void AddOnSetValueAction(Action<T, T> action)
        {
            _onSetValue.Add(action);
        }
    }
}