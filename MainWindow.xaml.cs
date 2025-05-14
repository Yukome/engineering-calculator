using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1sem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StringBuilder bilder = new StringBuilder();
        private bool ResultDisplay = false;
        private bool Error = false;
        private bool NewInputExpected = false;
        private bool ConstantEntered = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearAll(object sender, RoutedEventArgs e)
        {
            bilder.Clear();
            calc.Text = "";
            calc2.Text = "0";
            ResultDisplay = false;
            Error = false;
            NewInputExpected = false;
            ConstantEntered = false;
        }

        private void Clear()
        {
            if (ResultDisplay || ConstantEntered)
            {
                ClearAll(null, null);
            }
            else
            {
                calc2.Text = "0";
            }
        }

        private void UpdateExpDisp()
        {
            calc.Text = bilder.ToString();

            calc.ScrollToEnd();

            if (calc.Text.Length > 20)
            {
                calc.FontSize = 16;
            }
            else
            {
                calc.FontSize = 20;
            }
        }

        private void UpdateResult(string value)
        {
            calc2.Text = value;
            calc2.ScrollToEnd();
        }

        private void AddExp(string value)
        {
            if (ResultDisplay || Error)
            {
                ClearAll(null, null);
            }
            bilder.Append(value);
            UpdateExpDisp();
        }

        private void Button_Numb(object sender, RoutedEventArgs e)
        {
            if (Error)
            {
                ClearAll(null, null);
                return;
            }

            var button = (Button)sender;
            string digit = button.Content.ToString();

            if (ConstantEntered || NewInputExpected || ResultDisplay)
            {
                calc2.Text = digit;
                bilder.Append(digit);
                NewInputExpected = false;
                ResultDisplay = false;
            }
            else if (calc2.Text == "0" || calc2.Text == "-")
            {
                calc2.Text = (calc2.Text == "-") ? "-" + digit : digit;
                bilder.Append(digit);
            }
            else
            {
                calc2.Text += digit;
                bilder.Append(digit);
            }

            calc.Text = bilder.ToString();
        }

        private void Button_Oper(object sender, RoutedEventArgs e)
        {
            if (Error)
            {
                return;
            }

            var button = (Button)sender;
            string oper = button.Content.ToString();

            if (bilder.Length == 0 && oper == "-")
            {
                bilder.Append("-");
                calc.Text = "-";
                calc2.Text = "-";
                return;
            }
            else if (bilder.Length == 0)
            {
                return;
            }

            if (bilder.ToString().EndsWith("."))
            {
                bilder.Remove(bilder.Length - 1, 1);
                calc.Text = bilder.ToString();
                calc2.Text = bilder.Length > 0 ? bilder.ToString() : "0";
            }

            if (bilder.Length > 0 && "+×÷^".Contains(bilder[bilder.Length - 1].ToString()))
            {
                bilder.Remove(bilder.Length - 1, 1);
                bilder.Append(oper);
                calc.Text = bilder.ToString();
                NewInputExpected = true;
                return;
            }

            if (bilder.Length > 1 && "+×÷^".Contains(bilder[bilder.Length - 2].ToString()) && bilder[bilder.Length - 1] == '-')
            {
                bilder.Remove(bilder.Length - 2, 2);
                bilder.Append(oper);
                calc.Text = bilder.ToString();
                NewInputExpected = true;
                return;
            }

            try
            {
                string currentExpression = bilder.ToString();
                if (currentExpression.Length > 0 && (char.IsDigit(currentExpression[currentExpression.Length - 1]) || currentExpression[currentExpression.Length - 1] == ')' || currentExpression[currentExpression.Length - 1] == '('))
                {
                    if (currentExpression[currentExpression.Length - 1] != '(')
                    {
                        double currentResult = EvaluateExpression(currentExpression);
                        calc2.Text = currentResult.ToString();
                    }
                    bilder.Append(oper);
                    calc.Text = bilder.ToString();
                    NewInputExpected = true;
                }
                else
                {
                    throw new ArgumentException("Некорректное выражение");
                }
            }
            catch (Exception ex)
            {
                calc2.Text = $"Ошибка: {ex.Message}";
                Error = true;
            }
        }

        private double EvaluateExpression(string expression)
        {
            try
            {
                expression = expression.Replace(".", ",").Replace(" ", "");

                if (expression.EndsWith("."))
                {
                    expression = expression.Substring(0, expression.Length - 1);
                }

                while (expression.Contains('(') && expression.Contains(')'))
                {
                    int openBracket = expression.LastIndexOf('(');
                    int closeBracket = expression.IndexOf(')', openBracket);

                    if (closeBracket == -1) break;

                    string insideBrackets = expression.Substring(openBracket + 1, closeBracket - openBracket - 1);
                    double bracketResult = EvaluateExpression(insideBrackets);
                    expression = expression.Substring(0, openBracket) + bracketResult.ToString() +
                               (closeBracket < expression.Length ? expression.Substring(closeBracket + 1) : "");
                }

                expression = expression.Replace("(", "").Replace(")", "");

                if (string.IsNullOrEmpty(expression)) return 0;

                if (expression.Contains('^'))
                {
                    int index = expression.LastIndexOf('^');
                    double left = EvaluateExpression(expression.Substring(0, index));
                    double right = EvaluateExpression(expression.Substring(index + 1));
                    return Math.Pow(left, right);
                }

                if (expression.Contains('×') || expression.Contains('÷'))
                {
                    int mulIndex = expression.LastIndexOf('×');
                    int divIndex = expression.LastIndexOf('÷');
                    int index = Math.Max(mulIndex, divIndex);
                    double left = EvaluateExpression(expression.Substring(0, index));
                    double right = EvaluateExpression(expression.Substring(index + 1));
                    return expression[index] == '×' ? left * right : left / right;
                }

                if (expression.Contains('+') || expression.Contains('-'))
                {
                    int index = expression.LastIndexOfAny(new[] { '+', '-' });
                    if (index <= 0) return double.Parse(expression);

                    double left = EvaluateExpression(expression.Substring(0, index));
                    double right = EvaluateExpression(expression.Substring(index + 1));
                    return expression[index] == '+' ? left + right : left - right;
                }

                if (double.TryParse(expression, out double result))
                    return result;

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private double Factorial(double n)
        {
            if (n < 0) throw new ArgumentException("Факториал отрицательного числа не определен");
            if (n == 0) return 1;
            if (n > 170) throw new ArgumentException("Слишком большое число для факториала");
            return n * Factorial(n - 1);
        }

        private void FunctionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string function = button.Content.ToString();

            try
            {
                if (function == "⌫")
                {
                    if (bilder.Length > 0)
                    {
                        bilder.Remove(bilder.Length - 1, 1);
                        calc.Text = bilder.ToString();
                        if (bilder.Length > 0)
                        {
                            try
                            {
                                double currentResult = EvaluateExpression(bilder.ToString());
                                calc2.Text = currentResult.ToString();
                            }
                            catch
                            {
                                calc2.Text = "0";
                            }
                        }
                        else
                        {
                            calc2.Text = "0";
                        }
                    }
                    return;
                }

                if (function == "(")
                {
                    if (ResultDisplay || Error)
                    {
                        ClearAll(null, null);
                    }

                    if (bilder.Length > 0 && (char.IsDigit(bilder[bilder.Length - 1]) || bilder[bilder.Length - 1] == ')'))
                    {
                        bilder.Append("×");
                    }

                    bilder.Append("(");
                    calc.Text = bilder.ToString();
                    calc2.Text = "0";
                    NewInputExpected = true;
                    return;
                }
                else if (function == ")")
                {
                    int openCount = bilder.ToString().Count(c => c == '(');
                    int closeCount = bilder.ToString().Count(c => c == ')');

                    if (openCount > closeCount &&
                        (bilder.Length == 0 || !"+-×÷^(".Contains(bilder[bilder.Length - 1].ToString())))
                    {
                        bilder.Append(")");
                        calc.Text = bilder.ToString();

                        try
                        {
                            double currentResult = EvaluateExpression(bilder.ToString());
                            calc2.Text = currentResult.ToString();
                        }
                        catch (Exception ex)
                        {
                            calc2.Text = $"Ошибка: {ex.Message}";
                            Error = true;
                        }
                    }
                    return;
                }

                double result = 0;
                double value = calc2.Text == "" ? 0 : double.Parse(calc2.Text);

                switch (function)
                {
                    case "sin":
                        result = Math.Sin(value * Math.PI / 180);
                        break;
                    case "π":
                        result = Math.PI;
                        ConstantEntered = true;
                        break;
                    case "e":
                        result = Math.E;
                        ConstantEntered = true;
                        break;
                    case "x²":
                        result = Math.Pow(value, 2);
                        break;
                    case "1/x":
                        if (value == 0) throw new DivideByZeroException("На ноль делить нельзя!");
                        result = 1 / value;
                        break;
                    case "|x|":
                        result = Math.Abs(value);
                        break;
                    case "cos":
                        result = Math.Cos(value * Math.PI / 180);
                        break;
                    case "tg":
                        result = Math.Tan(value * Math.PI / 180);
                        break;
                    case "√x":
                        result = Math.Sqrt(value);
                        break;
                    case "(":
                        AddExp("(");
                        return;
                    case ")":
                        AddExp(")");
                        return;
                    case "n!":
                        result = Factorial(Math.Round(value));
                        break;
                    case "xʸ":
                        if (bilder.Length > 0 && char.IsDigit(bilder[bilder.Length - 1]))
                        {
                            string currentValue = calc2.Text;
                            bilder.Remove(bilder.Length - calc2.Text.Length, calc2.Text.Length);
                            bilder.Append(currentValue);
                        }
                        bilder.Append("^");
                        calc.Text = bilder.ToString();
                        calc2.Text = "0";
                        NewInputExpected = true;
                        return;
                    case "10ˣ":
                        result = Math.Pow(10, value);
                        break;
                    case "log":
                        result = Math.Log10(value);
                        break;
                    case "ln":
                        result = Math.Log(value);
                        break;
                    case "+/-":
                        result = -value;
                        break;
                    case ".":
                        if (NewInputExpected || ResultDisplay)
                        {
                            calc2.Text = "0.";
                            bilder.Append("0.");
                            NewInputExpected = false;
                            ResultDisplay = false;
                        }
                        else if (!calc2.Text.Contains("."))
                        {
                            if (string.IsNullOrEmpty(calc2.Text) || calc2.Text == "-" ||
                                (bilder.Length > 0 && "+-×÷^".Contains(bilder[bilder.Length - 1].ToString())))
                            {
                                calc2.Text = "0.";
                                bilder.Append("0.");
                            }
                            else
                            {
                                calc2.Text += ".";
                                bilder.Append(".");
                            }
                            calc.Text = bilder.ToString();
                        }
                        return;
                    default:
                        return;
                }

                calc2.Text = result.ToString();

                if (bilder.Length > 0 && !"()".Contains(function))
                {
                    if (ConstantEntered)
                    {
                        NewInputExpected = true;
                    }
                    int lastNumStart = bilder.ToString().LastIndexOfAny("+-×÷^(".ToCharArray()) + 1;
                    if (lastNumStart < 0) lastNumStart = 0;

                    bilder.Remove(lastNumStart, bilder.Length - lastNumStart);
                    bilder.Append(result.ToString());
                    calc.Text = bilder.ToString();
                }
                else if (bilder.Length == 0)
                {
                    bilder.Append(result.ToString());
                    calc.Text = bilder.ToString();
                }

                ResultDisplay = true;
                NewInputExpected = true;
            }
            catch (Exception ex)
            {
                calc2.Text = $"Ошибка: {ex.Message}";
                Error = true;
            }
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Error || bilder.Length == 0) return;

            if (bilder.ToString().EndsWith("."))
            {
                bilder.Remove(bilder.Length - 1, 1);
                calc.Text = bilder.ToString();
                calc2.Text = bilder.Length > 0 ? bilder.ToString() : "0";
            }

            string expressionToEvaluate = bilder.ToString();
            if (expressionToEvaluate.Length > 0 && "+-×÷^".Contains(expressionToEvaluate[expressionToEvaluate.Length - 1].ToString()))
            {
                expressionToEvaluate = expressionToEvaluate.Substring(0, expressionToEvaluate.Length - 1);
            }

            int openCount = expressionToEvaluate.Count(c => c == '(');
            int closeCount = expressionToEvaluate.Count(c => c == ')');
            if (openCount != closeCount)
            {
                calc2.Text = "Ошибка: незакрытая скобка";
                Error = true;
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(expressionToEvaluate))
                {
                    double result = EvaluateExpression(expressionToEvaluate);
                    calc2.Text = result.ToString();
                    bilder.Clear();
                    bilder.Append(result.ToString());
                    calc.Text = result.ToString();
                    ResultDisplay = true;
                }
            }
            catch (Exception ex)
            {
                calc2.Text = $"Ошибка: {ex.Message}";
                Error = true;
            }
        }
    }
}
