using Csla;
using Csla.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    [Serializable]
    public class Person : Csla.BusinessBase<Person>, IRestorePropertyInternal
    {
        public static readonly PropertyInfo<String> PropertyName = RegisterProperty<String>(c => c.Name);
        public string Name
        {
            get
            {
                return GetProperty(PropertyName);
            }
            set
            {
                SetProperty(PropertyName, value);
            }
        }

        public static readonly PropertyInfo<String> PropertyDescription = RegisterProperty<String>(c => c.Description);
        public string Description
        {
            get
            {
                return GetProperty(PropertyDescription);
            }
            set
            {
                SetProperty(PropertyDescription, value);
            }
        }

        public static readonly PropertyInfo<Int32> PropertyOrdinal = RegisterProperty<Int32>(c => c.Ordinal);
        public Int32 Ordinal
        {
            get
            {
                return GetProperty<Int32>(PropertyOrdinal);
            }
            set
            {
                SetProperty<Int32>(PropertyOrdinal, value);
            }
        }

        public static readonly PropertyInfo<Boolean> PropertyDeleted = RegisterProperty<Boolean>(c => c.Deleted);
        public Boolean Deleted
        {
            get
            {
                return GetProperty<Boolean>(PropertyDeleted);
            }
            set
            {
                SetProperty<Boolean>(PropertyDeleted, value);
            }
        }


        internal void Child_Fetch(PersonDto personDto)
        {
            LoadProperty(PropertyName, personDto.Name);
            LoadProperty(PropertyDescription, personDto.Description);
            LoadProperty(PropertyOrdinal, personDto.Ordinal);

        }

        public void RestorePropertyInternal(IPropertyInfo propertyInfo, object value)
        {
            SetProperty(propertyInfo, value);
        }
    }

    [Serializable]
    public class PersonList : Csla.BusinessListBase<PersonList, Person>, ITrack<Person>
    {
        internal void Child_Fetch()
        {
            RaiseListChangedEvents = false;

            for (int i = 0; i < 100; i++)
            {
                var person = DataPortal.FetchChild<Person>(new PersonDto() { Name = "Test " + (99-i).ToString(), Description = "Test " + (99-i).ToString() + " Description", Ordinal=99 - i });

                Add(person);
            }

            RaiseListChangedEvents = true;

        }



        public static PersonList GetSampleData()
        {
            return DataPortal.FetchChild<PersonList>();
        }

        public UndoRedoHistory<Person> GetTracker()
        {
            var undoRedoHistory = new UndoRedoHistory<Person>();

            return undoRedoHistory;
        }
    }

    public class PersonDto
    {
        public string Name;
        public string Description;
        public Int32 Ordinal;
    }

    public class CslaPropertyChangedMemento<T, C> : IMemento<T>
        where T : BusinessBase<T>, IRestorePropertyInternal
    {

        private T _child;
        private PropertyInfo<C> _propertyInfo;
        private C _value;


        public CslaPropertyChangedMemento(T person, PropertyInfo<C> propertyInfo, C value)
        {
            this._propertyInfo = propertyInfo;
            this._value = value;
            this._child = person;
        }

        public IMemento<T> Restore(T target)
        {
            var returnMemento = new CslaPropertyChangedMemento<T, C>(_child, _propertyInfo, _value);
            _child.RestorePropertyInternal(_propertyInfo, _value);
            return returnMemento;
        }

        public T GetPointer()
        {
            return _child;
        }
    }

    public interface IRestorePropertyInternal
    {
        void RestorePropertyInternal(IPropertyInfo propertyInfo, object value);
    }
}
