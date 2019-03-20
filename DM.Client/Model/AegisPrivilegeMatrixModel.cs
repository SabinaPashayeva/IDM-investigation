
namespace DM.Client.Model
{
    public class AegisPrivilegeMatrixModel
    {
        public int ResourceTypeId { get; set; }
        public int AccessRightTypeId { get; set; }
        public int ResourceId { get; set; }
        public int AccessRightId { get; set; }
        public int ApplicationId { get; set; }
        public int PrincipalId { get; set; }
    }
}
