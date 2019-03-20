using System;
using DM.Client;
using DM.Client.Model;
using DM.Shared;

namespace DM.Service
{
    public interface ICidmService
    {
        bool IsAuthorizedForAppAssist(int userKey);
    }

    public class CidmService : ICidmService
    {
        private ISecurityRepository _securityRepository = new SecurityRepository();

        public bool IsAuthorizedForAppAssist(int userKey)
        {
            var authorizedRoleIds = ConfigSettings.AuthorizedRoleId.Split(',');
            var employeeRoleId = GetRoleIdOfUser(userKey);

            foreach (var roleId in authorizedRoleIds)
            {
                if (employeeRoleId == roleId)
                    return true;
            }

            Console.WriteLine($"User {userKey} in RoleId {employeeRoleId} is not authorized for BD HealthSight Diversion Management");
            return false;
        }

        public string GetRoleIdOfUser(int userKey)
        {
            var employee = _securityRepository.GetEmployee(userKey);

            return  employee?.RoleId.ToString();
        }
        
    }
}
