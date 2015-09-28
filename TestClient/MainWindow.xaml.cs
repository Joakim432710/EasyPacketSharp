using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ViewModel _viewModel;
        public MainWindow()
        {
            _viewModel = new ViewModel();
            _viewModel.OnMessage += OnMessage;
            DataContext = _viewModel;
            InitializeComponent();
        }


        private void TextKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) ||
                        Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        textField.Text += Environment.NewLine;
                        textField.CaretIndex = textField.Text.Length - 1;
                        return;
                    }

                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) return;

                    //Send packet
                    var text = textField.Text;
                    textField.Text = string.Empty;

                    var cmd = _viewModel.SendMessage as ICommand;
                    cmd.Execute(text);

                    break;
            }
        }

        private void IPKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    var cmd = _viewModel.Connect as ICommand;
                    cmd.Execute(ipField.Text);
                    break;
            }
        }

        private bool ScrolledToBottom = true;

        private void OnScrolled(object sender, ScrollChangedEventArgs e)
        {
            var scroller = sender as ScrollViewer;
            if (scroller == null) return;
            ScrolledToBottom = Math.Abs(scroller.VerticalOffset - scroller.ScrollableHeight) < 5;
        }

        private void OnMessage(object sender, EventArgs e)
        {
            if (!ScrolledToBottom) return;
            chatScroll.UpdateLayout();
            chatScroll.ScrollToBottom();
        }
    }
}
