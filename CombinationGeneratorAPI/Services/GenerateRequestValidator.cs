using CombinationGeneratorAPI.Models;

namespace CombinationGeneratorAPI.Services
{
    public class GenerateRequestValidator
    {
        public static void Validate(GenerateRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Items == null || request.Items.Count == 0)
            {
                throw new ArgumentException("Items cannot be empty");
            }

            if (request.Length < 0)
            {
                throw new ArgumentException("Length can't be negative");
            }
        }
    }
}