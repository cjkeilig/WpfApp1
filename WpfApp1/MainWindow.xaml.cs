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
        private string _textAtGotFocus;
        private UndoRedoHistory<Person> _undoRedoHistory;

        public PersonList Persons { get; set; }
        //public ListCollectionView CvsPersons { get; set; }
        

        public MainWindow()
        {

            InitializeComponent();
            Persons = PersonList.GetSampleData();
            //CvsPersons = (ListCollectionView)CollectionViewSource.GetDefaultView(Persons);






            _undoRedoHistory = Persons.GetTracker();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Int32 ordinal = Int32.Parse(((Button)sender).Tag.ToString());

            var current = Persons.First(p => p.Ordinal == ordinal);
            var below = Persons.First(p => p.Ordinal == ordinal + 1);

            _undoRedoHistory.BeginCompoundDo(0); // Passing in zero to get the first memento pointer back

            CslaPropertyChangedMemento<Person, int> MoveDownPropertyChangedMemento = new CslaPropertyChangedMemento<Person, int>(current, Person.PropertyOrdinal, current.Ordinal);
            CslaPropertyChangedMemento<Person, int> MoveUpPropertyChangedMemento = new CslaPropertyChangedMemento<Person, int>(below, Person.PropertyOrdinal, below.Ordinal);

            _undoRedoHistory.CheckPoint(MoveDownPropertyChangedMemento);
            _undoRedoHistory.CheckPoint(MoveUpPropertyChangedMemento);

            _undoRedoHistory.EndCompoundDo();

            current.Ordinal = ordinal + 1;
            below.Ordinal = ordinal;

            DataGrid1.ScrollIntoView(current);
        }

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            if (_undoRedoHistory.CanUndo)
            {
                Person person = _undoRedoHistory.Undo();

                DataGrid1.SelectedItem = person;
                DataGrid1.CurrentItem = person;
                //selectedRow.BringIntoView();
                DataGrid1.ScrollIntoView(person);
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string _textAtLostFocus = ((TextBox)sender).Text;
            if (_textAtGotFocus == _textAtLostFocus)
                _undoRedoHistory.DiscardTop();

            _textAtGotFocus = string.Empty;

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _textAtGotFocus = ((TextBox)sender).Text;
            var selectedItem = ((TextBox)sender).Tag as Person;

            if (selectedItem is null)
                return;

            CslaPropertyChangedMemento<Person, String> personPropertyChangedMemento = new CslaPropertyChangedMemento<Person, String>(selectedItem, Person.PropertyDescription, selectedItem.Description);
            _undoRedoHistory.CheckPoint(personPropertyChangedMemento);

        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            Person person = e.Item as Person;

            if (person.Deleted)
            {
                e.Accepted = false;
            } else
            {
                e.Accepted = true;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Person selectedItem = ((Button)sender).Tag as Person;

            if (selectedItem.Deleted)
                return;

            CslaPropertyChangedMemento<Person, Boolean> personPropertyChangedMemento = new CslaPropertyChangedMemento<Person, Boolean>(selectedItem, Person.PropertyDeleted, selectedItem.Deleted);
            _undoRedoHistory.CheckPoint(personPropertyChangedMemento);

            selectedItem.Deleted = true;


        }
    }
}
