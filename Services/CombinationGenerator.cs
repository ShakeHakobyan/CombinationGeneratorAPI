namespace CombinationGeneratorAPI.Services
{
    public static class CombinationGenerator
    {
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

            for (int li = lastLetterIdx + 1; li < letterCounts.Count; li++)
            {
                int count = letterCounts[li];
                if (count == 0) continue;

                var items = GenerateLetterItems(li, count);

                foreach (var item in items)
                {
                    currentCombo.Add(item);

                    BacktrackCombinations(
                        letterCounts,
                        li,
                        comboLength,
                        currentCombo,
                        allCombos
                    );

                    currentCombo.RemoveAt(currentCombo.Count - 1);
                }
            }
        }

        public static List<List<string>> GenerateCombinations(
            List<int> letterCounts,
            int comboLength)
        {
            var allCombos = new List<List<string>>();
            var currentCombo = new List<string>();

            BacktrackCombinations(letterCounts, -1, comboLength, currentCombo, allCombos);

            return allCombos;
        }
    }
}
