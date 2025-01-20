using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Common
{
    public partial class KnownUrls
    {
        public const string BaseUrl = "api/v1";
        public const string User = BaseUrl + "/User";
        public const string Role = BaseUrl + "/Role";
        public const string Library = BaseUrl + "/Library";
        public const string Authentification = "Authentification";



        public const string UserRole = "creat_role";
        public const string Permission = "creat_permission";
        public const string RolePermission = "creat_rolepermission";
        public const string Library_location = "creat_library_location";
        public const string Librarycreation = "creat_library";
        public const string CreateUser = "create_user";
        public const string CreateDepartment = "create_department";
        public const string Login = "user_login";


        public const string AddUserDetail = BaseUrl + "/UserDetails/add";
        public const string UpdateUserDetail = BaseUrl + "/UserDetails/update/{id}";
        public const string GetUserDetail = BaseUrl + "/UserDetails/get/{id}";
    }
}
