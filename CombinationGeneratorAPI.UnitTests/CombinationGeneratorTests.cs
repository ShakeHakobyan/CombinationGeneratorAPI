using CombinationGeneratorAPI.Services;

namespace CombinationGeneratorAPI.Tests
{
    public class CombinationGeneratorTests
    {
        [Fact]
        public void Generate_BasicExample_ReturnsCorrectCombinations()
        {
            var letterCounts = new List<int> { 1, 2, 1 };
            int length = 2;

            var result = CombinationGenerator.Generate(letterCounts, length);

            var expected = new List<List<string>>
            {
                new() { "A1", "B1" },
                new() { "A1", "B2" },
                new() { "A1", "C1" },
                new() { "B1", "C1" },
                new() { "B2", "C1" }
            };

            Assert.True(AreCombinationsEqual(expected, result));
        }

        [Fact]
        public void Generate_LengthOne_ReturnsAllSingleItems()
        {
            var letterCounts = new List<int> { 2, 3 };
            int length = 1;

            var result = CombinationGenerator.Generate(letterCounts, length);

            var expected = new List<List<string>>
            {
                new() { "A1" },
                new() { "A2" },
                new() { "B1" },
                new() { "B2" },
                new() { "B3" }
            };

            Assert.True(AreCombinationsEqual(expected, result));
        }

        [Fact]
        public void Generate_NotEnoughDistinctLetters_ReturnsEmpty()
        {
            var letterCounts = new List<int> { 1, 0, 3 };
            int length = 3;

            var result = CombinationGenerator.Generate(letterCounts, length);

            Assert.Empty(result);
        }

        [Fact]
        public void Generate_WithZeroLetterCounts_ReturnsCorrectCombinations()
        {
            var letterCounts = new List<int> { 1, 0, 0, 2, 1 };
            int length = 3;

            var result = CombinationGenerator.Generate(letterCounts, length);

            var expected = new List<List<string>>
            {
                new() { "A1", "D1", "E1" },
                new() { "A1", "D2", "E1" }
            };
            Assert.True(AreCombinationsEqual(expected, result));
        }

        [Fact]
        public void Generate_WithZeroAtFirstPlace_ReturnsCorrectCombinations()
        {
            var letterCounts = new List<int> { 0, 0, 3, 2, 1 };
            int length = 2;

            var result = CombinationGenerator.Generate(letterCounts, length);

            var expected = new List<List<string>>
            {
                new() { "C1", "D1" },
                new() { "C1", "D2" },
                new() { "C1", "E1" },
                new() { "C2", "D1" },
                new() { "C2", "D2" },
                new() { "C2", "E1" },
                new() { "C3", "D1" },
                new() { "C3", "D2" },
                new() { "C3", "E1" },
                new() { "D1", "E1" },
                new() { "D2", "E1" }
            };
            Assert.True(AreCombinationsEqual(expected, result));
        }

        [Fact]
        public void Generate_WithEmptyList_ShouldReturnEmptyList()
        {
            var letterCounts = new List<int>();
            int length = 2;

            var result = CombinationGenerator.Generate(letterCounts, length);

            Assert.Empty(result);
        }

        [Fact]
        public void Generate_ComboLengthGreaterThanItems_ShouldReturnEmptyList()
        {
            var letterCounts = new List<int> { 1, 1 };
            int length = 5;

            var result = CombinationGenerator.Generate(letterCounts, length);

            Assert.Empty(result);
        }

        [Fact]
        public void Generate_WithZeroLength_ShouldReturnEmptyList()
        {
            var letterCounts = new List<int> { 2, 0, 6 };
            int length = 0;

            var result = CombinationGenerator.Generate(letterCounts, length);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 1 }, 1, 4)]
        [InlineData(new int[] { 2, 2 }, 2, 4)]
        [InlineData(new int[] { 3 }, 2, 0)]
        [InlineData(new int[] { 2, 2, 2, 2, 2 }, 3, 80)]
        [InlineData(
            new int[]
            {
                2,2,2,2,2,2,2,
                2,2,2,2,2,2,2,
                2,2,2,2,2,2,2,
                2,2,2,2,2
            },
            1,
            52
        )]
        [InlineData(
            new int[]
            {
                2,2,2,2,2,2,2,
                2,2,2,2,2,2,2,
                2,2,2,2,2,2,2,
                2,2,2,2,2
            },
            3,
            20800
        )]
        public void Generate_MultipleScenarios_ShouldReturnExpectedCount(int[] input, int comboLength, int expectedCount)
        {
            var letterCounts = input.ToList();

            var result = CombinationGenerator.Generate(letterCounts, comboLength);

            Assert.Equal(expectedCount, result.Count);
        }
        private static bool AreCombinationsEqual(List<List<string>> expected, List<List<string>> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            var expectedStrings = new List<string>();
            for (int i = 0; i < expected.Count; i++)
            {
                var inner = new List<string>(expected[i]);
                inner.Sort();
                expectedStrings.Add(string.Join(",", inner));
            }

            var actualStrings = new List<string>();
            for (int i = 0; i < actual.Count; i++)
            {
                var inner = new List<string>(actual[i]);
                inner.Sort();
                actualStrings.Add(string.Join(",", inner));
            }

            expectedStrings.Sort();
            actualStrings.Sort();

            for (int i = 0; i < expectedStrings.Count; i++)
            {
                if (expectedStrings[i] != actualStrings[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}