using Roomify.GP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Entities
{
    public class RoomImage
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public RoomType RoomType { get; set; }    // Enum

        // Dimensions
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        // Navigation Properties
        public ICollection<Description> Descriptions { get; set; }
    }
}
