using Unity.Collections;

namespace Assets._Project._Scripts.Core.Extentions
{
    public static class NativeArrayExtentions
    {
        public static float BurstMin(this NativeArray<float> array)
        {
            float min = float.PositiveInfinity;

            foreach (float f in array)
                if (f < min)
                    min = f;

            return min;
        }
        public static float BurstMax(this NativeArray<float> array)
        {
            float max = float.NegativeInfinity;

            foreach (float f in array)
                if (f > max)
                    max = f;

            return max;
        }
    }
}