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


        internal void Child_Fetch(PersonDto personDto)
        {
            LoadProperty(PropertyName, personDto.Name);
            LoadProperty(PropertyDescription, personDto.Description);
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
                var personOne = DataPortal.FetchChild<Person>(new PersonDto() { Name = "Test 1", Description = "Test 1 Description" });
                var personTwo = DataPortal.FetchChild<Person>(new PersonDto() { Name = "Test 2", Description = "Test 2 Description" });

                Add(personOne);
                Add(personTwo);
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
    }
}
