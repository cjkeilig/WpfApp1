using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    [Serializable]
    public class Person : Csla.BusinessBase<Person>
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

        public void RestorePropertyInternal<T>(PropertyInfo<T> propertyInfo, T value)
        {
            SetProperty(propertyInfo, value);
        }


        internal void Child_Fetch(PersonDto personDto)
        {
            LoadProperty(PropertyName, personDto.Name);
            LoadProperty(PropertyDescription, personDto.Description);
            LoadProperty(PropertyOrdinal, personDto.Ordinal);

        }

    }

    [Serializable]
    public class PersonList : Csla.BusinessListBase<PersonList, Person>
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

    }

    public class PersonDto
    {
        public string Name;
        public string Description;
        public Int32 Ordinal;
    }

    public class PersonPropertyChangedMemento<T> : IMemento<PersonList>
    {
        private PropertyInfo<T> _propertyInfo;
        private T _value;
        private Person _person;

        public PersonPropertyChangedMemento(PropertyInfo<T> propertyInfo, T value, Person person)
        {
            this._propertyInfo = propertyInfo;
            this._value = value;
            this._person = person;
        }

        public IMemento<PersonList> Restore(PersonList target)
        {
            var returnMemento = new PersonPropertyChangedMemento<T>(_propertyInfo, _value, _person);
            _person.RestorePropertyInternal(_propertyInfo, _value);
            return returnMemento;
        }
    }


}
