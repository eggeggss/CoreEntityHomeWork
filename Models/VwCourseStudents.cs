﻿using System;
using System.Collections.Generic;

namespace webapi01.Models
{
    public partial class VwCourseStudents
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int? StudentId { get; set; }
        public string StudentName { get; set; }
    }
}
