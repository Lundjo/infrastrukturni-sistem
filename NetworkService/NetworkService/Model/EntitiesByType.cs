using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class EntitiesByType
    {
        public string TypeName { get; set; }
        public List<Entity> Entities { get; set; }

        public EntitiesByType()
        {
            Entities = new List<Entity>();
        }
    }
}
