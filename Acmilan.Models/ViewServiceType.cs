using System.Collections.Generic;
using System.Linq;

namespace Acmilan.Models
{
    public class ViewServiceType
    {
        public int Category_id { get; set; }
        public string Category { get; set; }
        public List<ViewServiceItem> ListB { get; set; }
        
    }
}
