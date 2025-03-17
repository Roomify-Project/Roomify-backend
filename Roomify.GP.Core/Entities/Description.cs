using Roomify.GP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Entities
{
    public class Description
    {
        public Guid Id { get; set; }
        public string Box { get; set; }
        public RoomStyle Style { get; set; }     // Enum

        // FK
        public Guid RoomImageId { get; set; }
        public RoomImage RoomImage { get; set; }

    }
}
