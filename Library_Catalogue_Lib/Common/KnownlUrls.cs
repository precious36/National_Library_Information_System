using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Catalogue_Lib.Common
{
    public class KnownlUrls
    {
        public const string BaseUrl = "api/v1";
        public const string LibraryCatalogue = BaseUrl + "/Library_Catalogue";
        public const string Publishers = BaseUrl + "/Publishers";

        //Library Catalogue endpoints
        public const string CreateResource = "creat_resource";

        //publisher endpoint
        public const string CreatePublisher = "creat_publisher";
        public const string UpdadePublisher = "update_publisher";
       



    }
}
