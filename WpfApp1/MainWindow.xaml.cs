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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PersonList Persons { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            Persons = PersonList.GetSampleData();

            //this.ItemsControl1.ItemsSource = Persons;

            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ContentPresenter contentPresenter = this.ItemsControl1.ItemContainerGenerator.ContainerFromIndex(Int32.Parse(ScrollToIndex.Text)) as ContentPresenter;
            //contentPresenter.BringIntoView();
        }
    }
}
