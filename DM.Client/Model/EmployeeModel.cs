using System.Collections.Generic;
using System.Linq;

namespace DM.Client.Model
{
    public class EmployeeModel
    {
        private int roleId;

        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmployeeGuid { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public int RoleId
        {
            get
            {
                if (Resources != null && Resources.Count() > 0)
                {
                    var resource = Resources.FirstOrDefault().AccessRightId;
                    return resource;
                }
                else
                {
                    if (roleId != 0)
                    {
                        return roleId;
                    }
                }

                return 0;
            }
            set
            {
                roleId = value;
            }
        }
        public bool Status { get; set; }
        public int ApplicationId { get; set; }
        public List<AegisPrivilegeMatrixModel> PrivilegesToDelete { get; set; }
        public bool? InApplication { get; set; }
        public IEnumerable<AegisPrivilegeMatrixModel> Resources { get; set; }
        public int AuditEmployeeId { get; set; }

        public List<AegisPrivilegeModel> Privileges { get; set; }
    }
}
