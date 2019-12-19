using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapi01.Meta
{

    public class PatchCourse
    {
        public string Title { get; set; }
        public int Credits { get; set; }

    }

    public class InsertObj
    {

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public String StartDate { get; set; }

        public int InstructorID { get; set; }


    }

    public class UpdateObj
    {

        public int DepartmentID { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public String StartDate { get; set; }

        public int InstructorID { get; set; }

        public String RowVersion { get; set; }


    }

    public class DeleteObj
    {
        public int DepartmentID { get; set; }

        public String RowVersion_Original { get; set; }
    }
}
