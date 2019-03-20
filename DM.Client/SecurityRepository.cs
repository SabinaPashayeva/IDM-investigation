using DM.Client.Model;
using DM.Shared;

namespace DM.Client
{
    public interface ISecurityRepository
    {
        EmployeeModel GetEmployee(int userKey);
    }

    public class SecurityRepository : ISecurityRepository
    {
        public EmployeeModel GetEmployee(int userKey)
        {
            var employee = IdmHttpClient.GetEmployeeDetails(userKey, ConfigSettings.CcApplicationId);
            if (employee.EmployeeId == 0 && employee.EmployeeGuid == null)
            {
                System.Console.WriteLine($"User with employee ID {userKey} does not exist in the user database");
                return null;
            }
            return employee;
        }
    }
}
