using System.Windows;

namespace InventaireCannabis
{
    public partial class InputDialog : Window
    {
        public InputDialog(string title, string message)
        {
            InitializeComponent();
            Title = title;
            lblMessage.Text = message;
        }

        public string ResponseText
        {
            get { return txtResponse.Text; }
            set { txtResponse.Text = value; }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
