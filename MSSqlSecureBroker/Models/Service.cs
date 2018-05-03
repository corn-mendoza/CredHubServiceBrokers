using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSSqlSecureBroker.Models
{
    public class Service
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Bindable { get; set; } = true;
        public List<Plan> Plans { get; set; } = new List<Plan>();
    }
}
