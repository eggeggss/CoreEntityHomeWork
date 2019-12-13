using System;
using System.Collections.Generic;

namespace webapi01.Models
{
    public partial class CourseInstructor
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? DateModified { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Course Course { get; set; }
        public virtual Person Instructor { get; set; }
    }
}
