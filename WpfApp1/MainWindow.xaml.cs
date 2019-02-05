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
        private UndoRedoHistory<PersonList> _undoRedoHistory;

        public PersonList Persons { get; set; }
        public ListCollectionView CvsPersons { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            Persons = PersonList.GetSampleData();
            CvsPersons = (ListCollectionView)CollectionViewSource.GetDefaultView(Persons);
            CvsPersons.IsLiveSorting = true;
            CvsPersons.SortDescriptions.Add(new System.ComponentModel.SortDescription("Ordinal", System.ComponentModel.ListSortDirection.Ascending));
            CvsPersons.LiveSortingProperties.Add("Ordinal");
            _undoRedoHistory = new UndoRedoHistory<PersonList>(Persons);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Int32 ordinal = Int32.Parse(((Button)sender).Tag.ToString());

            var current = Persons.First(p => p.Ordinal == ordinal);
            var below = Persons.First(p => p.Ordinal == ordinal + 1);

            current.Ordinal = ordinal + 1;
            below.Ordinal = ordinal;

            DataGrid1.ScrollIntoView(current);
        }

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            if (_undoRedoHistory.CanUndo)
                _undoRedoHistory.Undo();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            //var selectedItem = DataGrid1.SelectedItem as Person;
            //PersonPropertyChangedMemento<String> personPropertyChangedMemento = new PersonPropertyChangedMemento<String>(Person.PropertyDescription, selectedItem.Description, selectedItem);
            //_undoRedoHistory.Do(personPropertyChangedMemento);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var selectedItem = ((TextBox)sender).Tag as Person;
            PersonPropertyChangedMemento<String> personPropertyChangedMemento = new PersonPropertyChangedMemento<String>(Person.PropertyDescription, selectedItem.Description, selectedItem);
            _undoRedoHistory.Do(personPropertyChangedMemento);

        }
    }
}
