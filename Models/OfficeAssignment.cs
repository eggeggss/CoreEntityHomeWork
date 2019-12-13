using System;
using System.Collections.Generic;

namespace webapi01.Models
{
    public partial class OfficeAssignment
    {
        public int InstructorId { get; set; }
        public string Location { get; set; }
        public DateTime? DateModified { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Person Instructor { get; set; }
    }
}
