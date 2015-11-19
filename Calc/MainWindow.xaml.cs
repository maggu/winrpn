using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Calc
{
    /// <summary>
    /// A simple RPN calculator
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Custom stack implementation that returns 0 if empty
        /// </summary>
        private class InfiniteDoubleStack : Stack<Double>
        {
            new public Double Pop()
            {
                return Count == 0 ? 0 : base.Pop();
            }
        }

        private readonly InfiniteDoubleStack calcStack = new InfiniteDoubleStack();
        private String displayValue = "";

        public MainWindow()
        {
            // Assumes "," as decimal separator
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("sv-SE");
            InitializeComponent();
        }

        private static Double ToDouble(String value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        private void UpdateDisplay()
        {
            Display.Text = displayValue;
        }

        private void Show(Double value)
        {
            // Display interpreted result
            displayValue = Convert.ToString(value);
            UpdateDisplay();
            // Cleanup
            displayValue = "";
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            if (displayValue.Length > 0)
            {
                displayValue = displayValue.Remove(displayValue.Length - 1);
                UpdateDisplay();
            }
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            calcStack.Clear();
            Show(0);
        }

        private void Button_Enter(object sender, RoutedEventArgs e)
        {
            Double result = displayValue != "" ? ToDouble(displayValue) : calcStack.Peek();
            calcStack.Push(result);
            Show(result);
        }

        private void Button_Value(object sender, RoutedEventArgs e)
        {
            displayValue += (String)((Button)sender).Content;
            UpdateDisplay();
        }

        private void Button_Operator(object sender, RoutedEventArgs e)
        {
            Double result = 0;
            Double second = displayValue != "" ? ToDouble(displayValue) : calcStack.Pop();
            Double first = calcStack.Pop();
            switch ((String)((Button)sender).Content)
            {
                case "+":
                    result = first + second;
                    break;
                case "−":
                    result = first - second;
                    break;
                case "×":
                    result = first * second;
                    break;
                case "÷":
                    result = first / second;
                    break;
            }
            calcStack.Push(result);
            Show(result);
        }
    }
}
