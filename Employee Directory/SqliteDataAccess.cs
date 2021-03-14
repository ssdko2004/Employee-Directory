using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Abstracts interactions with the SQLLite database. 
/// </summary>
namespace Employee_Directory {
    class SqliteDataAccess {

        public static List<Employee> LoadEmployees() {
            using (IDbConnection Cnn = new SQLiteConnection(LoadConnectionString())) {
                var Output = Cnn.Query<Employee>("select * from Employees ", new DynamicParameters());
                return Output.ToList();
            }
        }

        public static void AddEmployee(Employee Employee) {
            using (IDbConnection Cnn = new SQLiteConnection(LoadConnectionString())) {
                Cnn.Execute("insert into Employees (Id, Name, JobTitle) values (@StringId, @Name, @JobTitle)", Employee);
            }
        }

        public static void UpdateEmployee(Employee Employee, string Field) {
            using (IDbConnection Cnn = new SQLiteConnection(LoadConnectionString())) {
                var SqlCommand = string.Format("update Employees set ({0}) = @{0} where Id = (@StringId)", Field);

                switch (Field) {
                    case "Name":
                    case "JobTitle":
                        Cnn.Execute(SqlCommand, Employee);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Invalid field {0} passed to SqliteDataaAccess.UpdateEmployee", Field));
                }
            }
        }

        public static void DeleteEmployee(Employee Employee) {
            using (IDbConnection Cnn = new SQLiteConnection(LoadConnectionString())) {
                Cnn.Execute("delete from Employees where (Id) = (@StringId)", Employee);
            }
        }

        private static string LoadConnectionString(string Id = "EmployeeDirectoryConnection") {
            return ConfigurationManager.ConnectionStrings[Id].ConnectionString;
        }
    }
}
