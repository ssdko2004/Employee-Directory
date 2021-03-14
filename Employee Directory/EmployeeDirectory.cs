using System;
using System.Collections.Generic;

/// <summary>
/// Static class
/// Handles the duties of adding, removing and editing Employees
/// Use: LoadEmployees() should be called first otherwise Employees will
/// need to be added manually. A more fleshed out Class could be designed
/// to accept a particular database and or table to grab the data from.
/// </summary>
namespace Employee_Directory {
    class EmployeeDirectory {
        
        //Private attributes
        private static List<Employee> employees = new List<Employee>();

        //Accessor Methods
        internal static List<Employee> Employees { get => employees; }

        // General Methods
        public static void LoadEmployees() {
            employees = SqliteDataAccess.LoadEmployees();
        }

        public static void AddEmployee(string Name, string JobTitle) {

            var NewEmployee = new Employee(Name, JobTitle);           
            Employees.Add(NewEmployee);

            SqliteDataAccess.AddEmployee(NewEmployee);
        }

        public static void UpdateEmployee(int Index, string Field, string Value ) {
            // Handle invalid input
            if (Index < 0 || Index >= Employees.Count) {
                throw new ArgumentException(string.Format("Employee Index {0} is out of bounds", Index));
            }

            var oldEmployeeData = Employees[Index].Copy();

            switch (Field) {
                case "Name":
                    Employees[Index].Name = Value;
                    break;
                case "JobTitle":
                    Employees[Index].JobTitle = Value;
                    break;
            }

            SqliteDataAccess.UpdateEmployee(Employees[Index], Field);
        }

        public static void UpdateEmployee(Guid Id, string Field, string Value) {
            var Index = Employees.FindIndex(Emp => Emp.Id == Id);
            UpdateEmployee(Index, Field, Value);

        }

        public static void DeleteEmployee(int Index) {
            // Handle invalid input
            if (Index < 0 || Index >= Employees.Count) {
                throw new ArgumentException(string.Format("Employee Index {0} is out of bounds", Index));
            }
            SqliteDataAccess.DeleteEmployee(Employees[Index]);

            Employees.RemoveAt(Index);
        }

        public static void DeleteEmployee(Guid Id) {
            var Index = Employees.FindIndex(Emp => Emp.Id == Id);
            DeleteEmployee(Index);
        }
        
        
    }
}
