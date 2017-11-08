namespace Calculation.NetStandard
{
    /// <summary>
    /// Where we make the CPU break into a sweat with my awesomeness!
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Does math!
        /// </summary>
        /// <param name="calculation">The string representing the calculation.</param>
        /// <returns>The calculated value.</returns>
        public static double Calculate(string calculation)
        {
            int startIndex = 0;
            double result = 0d;
            string operation = string.Empty;

            for (var i = 0; i < calculation.Length; i++)
            {
                if (calculation[i] == '+' || calculation[i] == '-' || calculation[i] == (char)215 || calculation[i] == 'x' || calculation[i] == '/')
                {
                    switch (operation)
                    {
                        case "+":
                            result += double.Parse(calculation.Substring(startIndex, i - startIndex));
                            break;
                        case "-":
                            result -= double.Parse(calculation.Substring(startIndex, i - startIndex));
                            break;
                        case "x":
                            result *= double.Parse(calculation.Substring(startIndex, i - startIndex));
                            break;
                        case "/":
                            result /= double.Parse(calculation.Substring(startIndex, i - startIndex));
                            break;
                        default:
                            result = double.Parse(calculation.Substring(startIndex, i - startIndex));
                            break;
                    }

                    operation = calculation[i] == (char)215 ? "x" : calculation[i].ToString();
                    startIndex = ++i;
                }
            }

            switch (operation)
            {
                case "+":
                    result += double.Parse(calculation.Substring(startIndex));
                    break;
                case "-":
                    result -= double.Parse(calculation.Substring(startIndex));
                    break;
                case "x":
                    result *= double.Parse(calculation.Substring(startIndex));
                    break;
                case "/":
                    result /= double.Parse(calculation.Substring(startIndex));
                    break;
                default:
                    result = double.Parse(calculation.Substring(startIndex));
                    break;
            }

            // Add the caluclation to the history.
            CalculationHistory.AddToHistory(calculation, result);
            return result;
        }
    }
}
