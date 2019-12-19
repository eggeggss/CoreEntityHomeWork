using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace webapi01.Models
{

    public  class MetaCourse
    {
        [Required]
        [Range(2, 10)]
        public int Credits { get; set; }
    }

    [ModelMetadataType(typeof(MetaCourse))]
    public partial class Course
    {
        public Course()
        {
            CourseInstructor = new HashSet<CourseInstructor>();
            Enrollment = new HashSet<Enrollment>();
        }

        public int CourseId { get; set; }
        public string Title { get; set; }

        
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? DateModified { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructor { get; set; }
        public virtual ICollection<Enrollment> Enrollment { get; set; }
    }
}
