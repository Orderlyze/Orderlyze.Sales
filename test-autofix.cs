using System;
using System.Collections.Generic;
using System.Linq;

namespace TestAutoFix
{
    public class TestClass
    {
        // Bad naming convention
        public string test_variable = "test";
        
        // Unused variable
        private int unusedVar = 42;
        
        // Poor error handling
        public void ProcessData(string data)
        {
            var result = data.Split(',');
            Console.WriteLine(result[0]); // Potential IndexOutOfRangeException
        }
        
        // Inefficient loop
        public List<int> GetEvenNumbers(List<int> numbers)
        {
            List<int> evens = new List<int>();
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] % 2 == 0)
                {
                    evens.Add(numbers[i]);
                }
            }
            return evens;
        }
        
        // Missing null check
        public int GetLength(string text)
        {
            return text.Length;
        }
    }
}