using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighAvaNoDb.Domain
{
    [JsonObject(MemberSerialization.OptOut)]
    public partial class Server : INode
    {
        [JsonIgnore]
        public string Data
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        [JsonIgnore]
        public string NodeName { set; get; }

        [JsonIgnore]
        public string Path { set; get; }

    }
}
