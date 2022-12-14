using Unity.Mathematics;

namespace Assets._Project._Scripts.World.Generators.Noise
{
    public interface INoiseParameters
    {
        public float Evaluate(float3 point, PerlinNoiseEvaluator noiseEvaluator);
    }
}