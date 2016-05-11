using System;
using System.Windows;
using AIMLbot.Utils;

namespace AIMLbot.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ChatBot _chatBot = new ChatBot();

        public MainWindow()
        {
            InitializeComponent();
            var loader = new AIMLLoader();
            const string path = @"Human.aiml";
            loader.LoadAIML(path);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = TextBox.Text;
            if (String.IsNullOrWhiteSpace(text))
            {
                return;
            }
            var output = _chatBot.Chat(text, "1");
            TextBlock.Text = output.Output;
        }
    }
}