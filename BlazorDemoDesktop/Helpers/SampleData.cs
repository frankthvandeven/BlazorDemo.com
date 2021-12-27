using System.Collections.Generic;

namespace BlazorDesktopDemo
{
    public static class SampleData
    {

        public static List<Person> CreatePersonsList()
        {
            var list = new List<Person>();

            list.Add(new Person { IsSelected = true, Name = "Debra Burks", City = "Orchard Park" });
            list.Add(new Person { IsSelected = true, Name = "Gary Espinoza", City = "Forney" });
            list.Add(new Person { IsSelected = false, Name = "Adelle Larsen", City = "East Northport" });
            list.Add(new Person { IsSelected = false, Name = "Theo Reese", City = "Long Beach" });
            list.Add(new Person { IsSelected = false, Name = "Jeanice Frost", City = "Ossining" });

            return list;
        }

    }

    public class Person
    {
        public bool IsSelected = false;
        public string Name;
        public string City;
    }

}
