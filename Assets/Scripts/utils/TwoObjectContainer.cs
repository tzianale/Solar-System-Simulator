namespace utils
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    public class TwoObjectContainer <TFirst, TSecond>
    {
        public TFirst FirstObject;
        public TSecond SecondObject;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="firstObjectInitializer"></param>
        /// <param name="secondObjectInitializer"></param>
        public TwoObjectContainer(TFirst firstObjectInitializer, TSecond secondObjectInitializer)
        {
            FirstObject = firstObjectInitializer;
            SecondObject = secondObjectInitializer;
        }
    }
}