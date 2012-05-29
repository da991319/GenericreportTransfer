using System;
using System.Windows;
using System.Windows.Controls;

namespace GenericFileTransferClient.Views
{
    /// <summary>
    /// Interaction logic for UserControlTextBox.xaml
    /// </summary>
    public partial class UserControlTextBox : UserControl
    {
        public static DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(String), typeof(UserControl));
        public static DependencyProperty TextBoxTextProperty = DependencyProperty.Register("TextBoxText", typeof(String), typeof(UserControl));


        public String LabelText
        {
            get { return (String)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public String TextBoxText
        {
            get { return (String)GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }

        public UserControlTextBox()
        {
            InitializeComponent();
            GridLayout.DataContext = this;
        }
    }
}
