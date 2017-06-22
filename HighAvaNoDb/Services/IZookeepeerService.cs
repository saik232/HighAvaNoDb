using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighAvaNoDb.Services
{
    public interface IZookeepeerService
    {
        void RegZk(Guid id);
        void UnRegZk(Guid id);
    }
}
