namespace Assets._Project._Scripts.Core.Types
{
    public struct SimpleTuple<T, U>
    {
        public T Value1;
        public U Value2;

        public SimpleTuple(T value1, U value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}