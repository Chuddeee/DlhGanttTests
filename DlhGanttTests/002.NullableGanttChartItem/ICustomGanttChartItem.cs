using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlhSoft.Windows.Controls;

namespace _002.NullableGanttChartItem
{
    /// <summary>
    /// Represents an item within an <see cref="T:DlhSoft.Windows.Controls.IGanttChartView">IGanttChartView</see> control.
    /// </summary>
    public interface ICustomGanttChartItem
    {
        /// <summary>
        /// Gets the content of the current item, displayed as a tool tip within the <see cref="T:DlhSoft.Windows.Controls.IGanttChartView">IGanttChartView</see> control.
        /// </summary>
        object Content
        {
            get;
        }

        /// <summary>
        /// Gets the hierarchical indentation level of the current item in the containing collection.
        /// </summary>
        int Indentation
        {
            get;
        }

        /// <summary>
        /// Gets the start date and time of the current task item.
        /// </summary>
        DateTime? Start
        {
            get;
        }

        /// <summary>
        /// Gets the finish date and time of the current task item.
        /// </summary>
        DateTime? Finish
        {
            get;
        }

        /// <summary>
        /// Gets the effort value of the current task item (identical to duration for standard tasks).
        /// </summary>
        TimeSpan Effort
        {
            get;
        }

        /// <summary>
        /// Gets the duration value of the current task item.
        /// </summary>
        TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Gets the completed effort value of the current task item.
        /// </summary>
        TimeSpan CompletedEffort
        {
            get;
        }

        /// <summary>
        /// Gets a value that indicates whether the current task item is to be displayed as a milestone.
        /// </summary>
        bool IsMilestone
        {
            get;
        }

        /// <summary>
        /// Gets the predecessor collection of the current task item used to define dependencies (to be displayed as polyline arrows between dependent task bars in the view).
        /// </summary>
        IEnumerable<ICustomPredecessorItem> Predecessors
        {
            get;
        }

        /// <summary>
        /// Gets the assignments of the current task item as a (usually string) value to be displayed in the view.
        /// </summary>
        object AssignmentsContent
        {
            get;
        }

        /// <summary>
        /// Gets the baseline (estimated) start date and time of the current task item.
        /// </summary>
        DateTime? BaselineStart
        {
            get;
        }

        /// <summary>
        /// Gets the baseline (estimated) finish date and time of the current task item.
        /// </summary>
        DateTime? BaselineFinish
        {
            get;
        }

        /// <summary>
        /// Gets the index string of the current task item.
        /// </summary>
        string IndexString
        {
            get;
        }

        /// <summary>
        /// Gets the predecessors string of the current task item.
        /// </summary>
        string PredecessorsString
        {
            get;
        }

        /// <summary>
        /// Gets the WBS index string of the current task item.
        /// </summary>
        string WbsIndexString
        {
            get;
        }

        /// <summary>
        /// Gets the total cost of the current task item.
        /// </summary>
        double Cost
        {
            get;
        }
    }
}
