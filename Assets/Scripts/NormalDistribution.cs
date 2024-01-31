using UnityEngine;
namespace DefaultNamespace
{

    public class RandomNormalDistribution
    {
        // Variables for mean and variance
        private float mean;
        private float variance;
        private float leftBound;
        private float rightBound;
        
        public RandomNormalDistribution(float mean, float variance, float leftBound, float rightBound)
        {
            this.mean = mean;
            this.variance = variance;
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }
        
        public float GenerateRandomNumber()
        {
            float u1 = 1.0f - Random.value; // Uniform random number from (0,1]
            float u2 = 1.0f - Random.value;

            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            // Scale to the desired mean and variance
            float randNormal = mean + Mathf.Sqrt(variance) * randStdNormal;

            if (randNormal < leftBound)
            {
                randNormal = leftBound;
            }
            else if (randNormal > rightBound)
            {
                randNormal = rightBound;
            }
            
            return randNormal;
        }
    }
}