using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using CybersecurityChatbotGUI.Services;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace CybersecurityChatbotGUI.Converters
{
    public class SentimentColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Sentiment sentiment) return Brushes.Black;

            return sentiment switch
            {
                Sentiment.Positive => Brushes.Green,
                Sentiment.Negative => Brushes.Red,
                Sentiment.Worried => Brushes.Orange,
                Sentiment.Curious => Brushes.Blue,
                _ => Brushes.Black
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
