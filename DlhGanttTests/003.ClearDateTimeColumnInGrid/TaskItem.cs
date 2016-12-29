using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _003.ClearDateTimeColumnInGrid
{
    public class TaskItem
    {
        public string Name { get; set; }
        public int IndentLevel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime CompletionCurrentDate { get; set; }
        public string AssignmentsString { get; set; }
        public string Description { get; set; }
    }

}
