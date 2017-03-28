using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrigerV2._0
{
    class RepositoryData:ProcessActions
    {
        public string RepositoryLink { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Authentication_token { get; set; }
        public string Project_Uri { get; set; }
    }
}
