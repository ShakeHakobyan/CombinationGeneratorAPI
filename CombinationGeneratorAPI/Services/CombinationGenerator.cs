namespace CombinationGeneratorAPI.Services
{
    public static class CombinationGenerator
    {
        private const long MAX_COMBINATIONS = 100_000;
        static List<string> GenerateLetterItems(int letterIdx, int count)
        {
            var items = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                items.Add(((char)('A' + letterIdx)).ToString() + i.ToString());
            }

            return items;
        }

        static void BacktrackCombinations(
            List<int> letterCounts,
            int lastLetterIdx,
            int comboLength,
            List<string> currentCombo,
            List<List<string>> allCombos)
        {
            if (currentCombo.Count == comboLength)
            {
                allCombos.Add(new List<string>(currentCombo));
                return;
            }

            for (int i = lastLetterIdx + 1; i < letterCounts.Count; i++)
            {
                int count = letterCounts[i];
                if (count == 0) 
                {
                    continue;
                }

                var items = GenerateLetterItems(i, count);

                foreach (var item in items)
                {
                    currentCombo.Add(item);

                    BacktrackCombinations(
                        letterCounts,
                        i,
                        comboLength,
                        currentCombo,
                        allCombos
                    );

                    currentCombo.RemoveAt(currentCombo.Count - 1);
                }
            }
        }
        private static long Binomial(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        private static long Factorial(int n)
        {
            long result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        private static bool PreGenerationChecks(List<int> letterCounts, int comboLength)
        {
            int distinctLetters = letterCounts.Count(n => n != 0);

            if (comboLength == 0 || distinctLetters < comboLength)
            {
                return false;
            }

            // Approximate upper bound on combinations.
            // Used as a safety check to avoid combinatorial explosion.
            long upperBound = Binomial(distinctLetters, comboLength);

            return upperBound <= MAX_COMBINATIONS;
        }

        public static List<List<string>> Generate(
            List<int> letterCounts,
            int comboLength)
        {
            if (!PreGenerationChecks(letterCounts, comboLength))
            {
                return [];
            }
            var allCombos = new List<List<string>>();
            var currentCombo = new List<string>();

            BacktrackCombinations(letterCounts, -1, comboLength, currentCombo, allCombos);

            return allCombos;
        }
    }
}