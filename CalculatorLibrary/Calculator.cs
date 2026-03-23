// CalculatorLibrary.cs
using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }
        public void PreviousCalculations()
        {
            try
            {
                List<string> logLines = File.ReadAllLines("calculatorlog.json").ToList();
                Console.WriteLine("Previous calculations:");
                Console.WriteLine(logLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log file: {ex.Message}");
            }
        }
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;

                     case "sqrt":
                    if (num1 >= 0)
                    {
                        result = Math.Sqrt(num1);
                    }
                    else
                    {
                        writer.WriteValue("Square Root");
                        writer.WritePropertyName("Error");
                        writer.WriteValue("Cannot take square root of a negative number.");
                        writer.WriteEndObject();
                        return result;
                    }
                    break;

                case "pow":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Power");
                    break;

                case "Trig":
                    if (num2 == 0)
                    {
                        result = Math.Sin(num1);
                        writer.WriteValue("Sine");
                    }
                    else
                    {
                        if (num2 == 1)
                        {
                            result = Math.Cos(num1);
                            writer.WriteValue("Cosine");
                        }
                        else if (num2 == 2)
                        {
                            result = Math.Tan(num1);
                            writer.WriteValue("Tangent");
                        }
                        else
                        {
                            writer.WriteValue("Trigonometric Function");
                            writer.WritePropertyName("Error");
                            writer.WriteValue("Invalid trigonometric function code.");
                            writer.WriteEndObject();
                            return result;
                        }
                    }
                    break;
                case "log":
                    if (num1 > 0)
                    {
                        result = Math.Log(num1);
                    }
                    else
                    {
                        writer.WriteValue("Logarithm");
                        writer.WritePropertyName("Error");
                        writer.WriteValue("Cannot take logarithm of a non-positive number.");
                        writer.WriteEndObject();
                        return result;
                    }
                    break;
                default:
                    break;


            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}