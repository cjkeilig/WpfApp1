using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Controls
{
    public class ExpandCollapseGroupBox : GroupBox
    {


        public bool? IsChecked
        {
            get { return (bool?)this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
          "IsChecked", typeof(bool?), typeof(ExpandCollapseGroupBox), new PropertyMetadata(true, OnCheckPropertyChanged));

        //static ExpandCollapseGroupBox()
        //{
        //    May need this to fix have to put the default style in the App.xaml, it is supposed to go in the Themes/Generic.xaml file
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandCollapseGroupBox), new FrameworkPropertyMetadata(typeof(ExpandCollapseGroupBox)));
        //}

        private static void OnCheckPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isChecked = e.NewValue as bool? ?? false;

            var visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;

            var childCount = LogicalTreeHelper.GetChildren(d);

            foreach (var child in childCount)
            {
                if (child is UIElement uiElement)
                {
                    uiElement.Visibility = visibility;
                }
            }
        }
    }
}
