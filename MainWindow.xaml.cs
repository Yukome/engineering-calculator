using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1sem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Play(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Numb(object sender, RoutedEventArgs e)
        {
            string[] operators = new string[] { "+", "-", "÷", "×" };
            if (sender is Button button) {
                calc.Text += button.Content.ToString();
                calc2.Text += button.Content.ToString();
                if (operators.All(x => calc.Text.Contains(x))){

                }
                foreach (var op in operators)
                {
                    if (calc.Text.Contains(op))
                    {
                        switch (op)
                        {
                            case "+":
                                Button_Click_plus(sender, e);
                                break;
                            case "-":
                                Button_Click_minus(sender, e);
                                break;
                            case "÷":
                                Button_Click_division(sender, e);
                                break;
                            case "×":
                                Button_Click_multiplier(sender, e);
                                break;
                        }
                    }
                }
            }
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Button_Numb(sender, e);
        }

        private void Button_Click_plus(object sender, RoutedEventArgs e)
        {
            if (calc.Text.Contains("+") && !calc.Text.EndsWith("+"))
            {
                var nums = calc.Text.Split('+').Select(x => double.Parse(x)).ToList();
                double result = nums[0];
                for (int i = 1; i < nums.Count; i++)
                {
                    result += nums[i];
                }
                calc2.Text = result.ToString();
                calc.Text = result.ToString();
            }
            else if (!string.IsNullOrEmpty(calc.Text) && (double.TryParse(calc.Text, out double currentNum) || calc.Text.EndsWith("+")))
            {
                calc.Text += "+";
                calc2.Text = currentNum.ToString();
                
            }
        }

        private void Button_Click_minus(object sender, RoutedEventArgs e)
        {
            if (calc.Text.Contains("-") && !calc.Text.EndsWith("-"))
            {
                var nums = calc.Text.Split('-').Select(x => double.Parse(x)).ToList();
                double result = nums[0];
                for (int i = 1; i < nums.Count; i++)
                {
                    result -= nums[i];
                }
                calc2.Text = result.ToString();
                calc.Text = result.ToString();
            }
            else if (!string.IsNullOrEmpty(calc.Text) && (double.TryParse(calc.Text, out double currentNum) || calc.Text.EndsWith("-")))
            {
                calc.Text += "-";
                calc2.Text = currentNum.ToString();

            }
        }

        private void Button_Click_division(object sender, RoutedEventArgs e)
        {
            if (calc.Text.Contains("÷") && !calc.Text.EndsWith("÷"))
            {
                var nums = calc.Text.Split('÷').Select(x => double.Parse(x)).ToList();
                double result = nums[0];
                for (int i = 1; i < nums.Count; i++)
                {
                    result /= nums[i];
                }
                calc2.Text = result.ToString();
                calc.Text = result.ToString();
            }
            else if (!string.IsNullOrEmpty(calc.Text) && (double.TryParse(calc.Text, out double currentNum) || calc.Text.EndsWith("÷")))
            {
                calc.Text += "÷";
                calc2.Text = currentNum.ToString();

            }
        }

        private void Button_Click_multiplier(object sender, RoutedEventArgs e)
        {
            if (calc.Text.Contains("×") && !calc.Text.EndsWith("×"))
            {
                var nums = calc.Text.Split('×').Select(x => double.Parse(x)).ToList();
                double result = nums[0];
                for (int i = 1; i < nums.Count; i++)
                {
                    result *= nums[i];
                }
                calc2.Text = result.ToString();
                calc.Text = result.ToString();
            }
            else if (!string.IsNullOrEmpty(calc.Text) && (double.TryParse(calc.Text, out double currentNum) || calc.Text.EndsWith("×")))
            {
                calc.Text += "×";
                calc2.Text = currentNum.ToString();

            }
        }
    }
}
