namespace utils
{
    public class Wrapper<T>
    {
        private T _savedValue;
        
        public Wrapper(T  elementInitializer)
        {
            _savedValue = elementInitializer;
        }

        public void SetValue(T newValue)
        {
            _savedValue = newValue;
        }

        public T GetValue()
        {
            return _savedValue;
        }
    }
}