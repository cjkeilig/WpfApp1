using Csla;
using Csla.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class PersonList : Csla.BusinessListBase<PersonList, Person>, ITrack<PersonList, Person>
    {
        internal void Child_Fetch()
        {
            RaiseListChangedEvents = false;


            for (int i = 0; i < 100; i++)
            {
                var person = DataPortal.FetchChild<Person>(new PersonDto() { Name = "Test " + i.ToString(), Description = "Test " + i.ToString() + " Description", Ordinal=i });

                Add(person);
            }


            RaiseListChangedEvents = true;

        }



        public static PersonList GetSampleData()
        {
            return DataPortal.FetchChild<PersonList>();
        }

        public IUndoRedoHistory<PersonList, Person> GetTracker()
        {
            var undoRedoHistory = new ListUndoRedoHistory<PersonList, Person>(this);

            return undoRedoHistory;
        }
    }

    public class PersonDto
    {
        public string Name;
        public string Description;
        public Int32 Ordinal;
    }

    public class PropertyChangedMemento<T, U, V> : IListTOfVMemento<T,U>
        where U : BusinessBase<U>, IRestorePropertyInternal
    {

        private U _child;
        private PropertyInfo<V> _propertyInfo;
        private V _value;


        public PropertyChangedMemento(U person, PropertyInfo<V> propertyInfo, V value)
        {
            this._propertyInfo = propertyInfo;
            this._value = value;
            this._child = person;
        }

        public IListTOfVMemento<T, U> Restore(T target)
        {
            var returnMemento = new PropertyChangedMemento<T, U, V>(_child, _propertyInfo, _value);
            _child.RestorePropertyInternal(_propertyInfo, _value);
            return returnMemento;
        }

        public U GetChild()
        {
            return _child;
        }
    }

    public class MoveItemUpMemento<T, U> : IListTOfVMemento<T, U>
    where T : BusinessListBase<T, U>
    where U : BusinessBase<U>
    {

        private U _child;
        private int _index;

        public MoveItemUpMemento(U child, int index)
        {
            this._child = child;
            this._index = index;
        }

        public IListTOfVMemento<T, U> Restore(T target)
        {
            var returnMemento = new MoveItemDownMemento<T, U>(_child, _index + 1);

            target.Move(_index, _index + 1);


            return returnMemento;
        }

        public U GetChild()
        {
            return _child;
        }
    }

    public class MoveItemDownMemento<T, U> : IListTOfVMemento<T, U>
    where T : BusinessListBase<T,U>
    where U : BusinessBase<U>

    {

        private U _child;
        private int _index;

        public MoveItemDownMemento(U child, int index)
        {
            this._child = child;
            this._index = index;
        }

        public IListTOfVMemento<T, U> Restore(T target)
        {
            var returnMemento = new MoveItemUpMemento<T, U>(_child, _index - 1);

            target.Move(_index, _index - 1);

            return returnMemento;
        }

        public U GetChild()
        {
            return _child;
        }
    }

    public interface IRestorePropertyInternal
    {
        void RestorePropertyInternal(IPropertyInfo propertyInfo, object value);
    }
}
