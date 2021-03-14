using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A very basic class depicting a company employee.
/// Id: A unique UUID randomly generated
/// Name: The employees name. Assumes both first and last name for sake of simplicity.
///  A production database would likely have seperate values.
/// JobTitle: The employees title.  A production environment could have a set list of 
///  values to choose from.
/// </summary>
namespace Employee_Directory {
    class Employee {
        private Guid id;
        private string name;
        private string jobTitle;
        
        public Employee(string Name, string JobTitle) {
            id = Guid.NewGuid(); 
            name = Name;
            jobTitle = JobTitle;
        }        

        public Employee(Guid Id, string Name, string JobTitle) {
            id = Id;
            name = Name;
            jobTitle = JobTitle;
        }

        public Employee(string Id, string Name, string JobTitle) {
            try {
                id = Guid.Parse(Id);
                name = Name;
                jobTitle = JobTitle;
            }
            catch (ArgumentNullException e) {
                Console.WriteLine("Error: Employee Id is an empty string");
            }
            catch (FormatException e) {
                Console.WriteLine(String.Format("Error: Id {1} is not a valid UUID", Id));
            }
        }

        public Employee Copy() {
            var employeeCopy = new Employee(this.Name, this.JobTitle);
            employeeCopy.id = this.id;

            return employeeCopy;

        }

        public string Name { get => name; set => name = value; }
        public string JobTitle { get => jobTitle; set => jobTitle = value; }
        public Guid Id { get => id;  }
        public string StringId { get => id.ToString(); } // Mostly used for storing the to the DB
    }
}
