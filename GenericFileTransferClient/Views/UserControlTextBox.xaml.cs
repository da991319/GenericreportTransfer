using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenericFileTransferClient.Views
{
    /// <summary>
    /// Interaction logic for UserControlTextBox.xaml
    /// </summary>
    public partial class UserControlTextBox : UserControl
    {
        public static DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(String), typeof(UserControl));
        public static DependencyProperty TextBoxTextProperty = DependencyProperty.Register("TextBoxText", typeof(int), typeof(UserControl));


        public String LabelText
        {
            get { return (String)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public int TextBoxText
        {
            get { return (int)GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }

        public UserControlTextBox()
        {
            InitializeComponent();
            GridLayout.DataContext = this;
        }
    }
}
