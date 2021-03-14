using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Employee_Directory {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Note: The following packages have been added when developing this project -
    ///  -Dapper
    ///  -System.Data.SQLite
    ///  UI Note: Focused on function over form but attempted to make the UI accessable.
    /// </summary>
    public partial class MainWindow : Window {
        List<string> CommandsList = new List<string>();

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            EmployeeDirectory.LoadEmployees();
            EmployeeListView.ItemsSource = EmployeeDirectory.Employees;
            CommandsListView.ItemsSource = CommandsList;
        }       

        private void NewEmployeeButton_Click(object sender, RoutedEventArgs e) {
            // Check for valid input
            if (NewEmployeeNameTextBox.Text == "") {
                MessageBox.Show("Please enter a name for the new employee.", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (NewEmployeeJobTitleTextBox.Text == "") {
                MessageBox.Show("Please enter a job title for the new employee.", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Add the employee
            EmployeeDirectory.AddEmployee(NewEmployeeNameTextBox.Text, NewEmployeeJobTitleTextBox.Text);
            EmployeeListView.Items.Refresh();

            var Id = EmployeeDirectory.Employees[EmployeeDirectory.Employees.Count - 1].Id.ToString();
            // Report command
            CommandsList.Add(string.Format("-Inserted Employee: {0} with the Id: {1}", NewEmployeeNameTextBox.Text, Id));
            CommandsListView.Items.Refresh();

            // Clear out input
            NewEmployeeNameTextBox.Text = "";
            NewEmployeeJobTitleTextBox.Text = "";
        }

        // The below method updates the Employee even if it hasn't changed. A 
        //  production environment would likely only want it updated on a actual 
        //  change in data.

        private void EmployeeNameTextBox_LostFocus(object sender, RoutedEventArgs e) {
            var TextBox = sender as TextBox;
            var Row = EmployeeListView.SelectedIndex;

            if (Row >= 0) { 
                EmployeeDirectory.UpdateEmployee(Row, "Name", TextBox.Text);

                var Id = EmployeeDirectory.Employees[Row].StringId;
                // Report command
                CommandsList.Add(string.Format("- Employee with Id: {0} has had Name column updated to {1}", Id, TextBox.Text));
                CommandsListView.Items.Refresh();
                Keyboard.ClearFocus();
            }
        }

        private void EmployeeNameTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                // Trigger Lost Focus event. Forcing update                
                EmployeeNameTextBox_LostFocus(sender, e);
            }
        }

        private void EmployeeJobTitleTextBox_LostFocus(object sender, RoutedEventArgs e) {
            var TextBox = sender as TextBox;
            var Row = EmployeeListView.SelectedIndex;
            EmployeeDirectory.UpdateEmployee(Row, "JobTitle", TextBox.Text);

            if (Row >= 0) {
                var Id = EmployeeDirectory.Employees[Row].StringId;
                // Report command
                CommandsList.Add(string.Format("- Employee with Id: {0} has had JobTitle column updated to {1}", Id, TextBox.Text));
                CommandsListView.Items.Refresh();
            }
        }

        private void EmployeeJobTitleTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                // Trigger Lost Focus event. Forcing update                
                EmployeeJobTitleTextBox_LostFocus(sender, e);
            }
        }

        


        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            var Index = EmployeeListView.SelectedIndex;
            var Id = EmployeeDirectory.Employees[Index].StringId;
            // Check for valid input
            if (Index >= 0) {               
                EmployeeDirectory.DeleteEmployee(Index);
                EmployeeListView.Items.Refresh();
                EmployeeListView.UnselectAll();
                
                // Report command
                CommandsList.Add(string.Format("- Employee with Id: {0} has been removed", Id));
                CommandsListView.Items.Refresh();
            }
        }

       
    }

    /// <summary>
    /// Simplfies displaying a row for each Employee.  Indexes starting at 1
    /// </summary>
    public class IndexConverter : IValueConverter {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture) {
            const int StartingIndex = 1;
            ListViewItem Item = (ListViewItem)Value;
            ListView ListView = ItemsControl.ItemsControlFromItemContainer(Item) as ListView;
            int Index = ListView.ItemContainerGenerator.IndexFromContainer(Item) + StartingIndex;
            return Index.ToString();
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) {
            throw new NotImplementedException();
        }
    }
}