using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Entities
{
    public class PortfolioPost
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }   // the saved file path/URL
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // FK
        public Guid UserId { get; set; }
        public required User User { get; set; }
    }
}