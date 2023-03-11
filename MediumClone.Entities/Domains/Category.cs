using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Entities.Domains
{
	public class Category : BaseEntity
	{
        public string Title { get; set; }

        public List<BlogCategory> BlogCategories { get; set; }
    }
}
