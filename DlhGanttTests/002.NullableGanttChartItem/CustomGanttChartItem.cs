using DlhSoft.Windows.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DlhSoft.Windows.Controls;

namespace _002.NullableGanttChartItem
{
    /// <summary>
    /// Represents an item within a <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> or <see cref="T:DlhSoft.Windows.Controls.GanttChartDataGrid">GanttChartDataGrid</see> control.
    /// </summary>
    public class CustomGanttChartItem : DataTreeGridItem, ICustomGanttChartItem, INotifyPropertyChanged
    {
        private IGanttChartView ganttChartView;

        private ScheduleChartItem scheduleChartItem;

        /// <summary>
        /// Identifies the ExpansionVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpansionVisibilityProperty = DependencyProperty.Register("ExpansionVisibility", typeof(Visibility), typeof(CustomGanttChartItem), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the IsSummaryEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSummaryEnabledProperty = DependencyProperty.Register("IsSummaryEnabled", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(true, new PropertyChangedCallback(CustomGanttChartItem.OnIsSummaryEnabledChanged)));

        /// <summary>
        /// Identifies the IsParentSummarizationEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsParentSummarizationEnabledProperty = DependencyProperty.Register("IsParentSummarizationEnabled", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(true, new PropertyChangedCallback(CustomGanttChartItem.OnIsParentSummarizationEnabledChanged)));

        /// <summary>
        /// Identifies the Start dependency property.
        /// </summary>
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(CustomGanttChartItem), new PropertyMetadata(DateTime.Today, new PropertyChangedCallback(CustomGanttChartItem.OnStartChanged)));

        /// <summary>
        /// Identifies the PreferredStart dependency property.
        /// </summary>
        public static readonly DependencyProperty PreferredStartProperty = DependencyProperty.Register("PreferredStart", typeof(DateTime), typeof(CustomGanttChartItem), new PropertyMetadata(DateTime.Today, new PropertyChangedCallback(CustomGanttChartItem.OnPreferredStartChanged)));

        /// <summary>
        /// Identifies the Finish dependency property.
        /// </summary>
        public static readonly DependencyProperty FinishProperty = DependencyProperty.Register("Finish", typeof(DateTime), typeof(CustomGanttChartItem), new PropertyMetadata(DateTime.Today, new PropertyChangedCallback(CustomGanttChartItem.OnFinishChanged)));

        /// <summary>
        /// Identifies the CompletedFinish dependency property.
        /// </summary>
        public static readonly DependencyProperty CompletedFinishProperty = DependencyProperty.Register("CompletedFinish", typeof(DateTime), typeof(CustomGanttChartItem), new PropertyMetadata(DateTime.Today, new PropertyChangedCallback(CustomGanttChartItem.OnCompletedFinishChanged)));

        /// <summary>
        /// Identifies the IsCompleted dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCompletedProperty = DependencyProperty.Register("IsCompleted", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(false, new PropertyChangedCallback(CustomGanttChartItem.OnIsCompletedChanged)));

        /// <summary>
        /// Identifies the IsMilestone dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMilestoneProperty = DependencyProperty.Register("IsMilestone", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(false, new PropertyChangedCallback(CustomGanttChartItem.OnIsMilestoneChanged)));

        private Schedule schedule;

        private bool isDuringUpdateTimingExtras;

        private bool isDuringCoerceTiming;

        /// <summary>
        /// Identifies the Predecessors dependency property.
        /// </summary>
        public static readonly DependencyProperty PredecessorsProperty = DependencyProperty.Register("Predecessors", typeof(PredecessorItemCollection), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnPredecessorsChanged)));

        private PredecessorItemCollection managedPredecessorItems;

        private string customWbsIndexString;

        /// <summary>
        /// Identifies the AssignmentsContent dependency property.
        /// </summary>
        public static readonly DependencyProperty AssignmentsContentProperty = DependencyProperty.Register("AssignmentsContent", typeof(object), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnAssignmentsContentChanged)));

        /// <summary>
        /// Identifies the BaselineStart dependency property.
        /// </summary>
        public static readonly DependencyProperty BaselineStartProperty = DependencyProperty.Register("BaselineStart", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnBaselineStartChanged)));

        /// <summary>
        /// Identifies the BaselineFinish dependency property.
        /// </summary>
        public static readonly DependencyProperty BaselineFinishProperty = DependencyProperty.Register("BaselineFinish", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnBaselineFinishChanged)));

        /// <summary>
        /// Identifies the MinStart dependency property.
        /// </summary>
        public static readonly DependencyProperty MinStartProperty = DependencyProperty.Register("MinStart", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnMinStartChanged)));

        /// <summary>
        /// Identifies the MaxStart dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxStartProperty = DependencyProperty.Register("MaxStart", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnMaxStartChanged)));

        /// <summary>
        /// Identifies the MinFinish dependency property.
        /// </summary>
        public static readonly DependencyProperty MinFinishProperty = DependencyProperty.Register("MinFinish", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnMinFinishChanged)));

        /// <summary>
        /// Identifies the MaxFinish dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxFinishProperty = DependencyProperty.Register("MaxFinish", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnMaxFinishChanged)));

        private double executionCost;

        private double? assignmentsCost = null;

        private double? extraCost = null;

        /// <summary>
        /// Identifies the DisplayRowIndex property value.
        /// </summary>
        public static readonly DependencyProperty DisplayRowIndexProperty = DependencyProperty.Register("DisplayRowIndex", typeof(int?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnDisplayRowIndexChanged)));

        /// <summary>
        /// Indicates the ActualDisplayRowIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualDisplayRowIndexProperty = DependencyProperty.Register("ActualDisplayRowIndex", typeof(int), typeof(CustomGanttChartItem), new PropertyMetadata(0, new PropertyChangedCallback(CustomGanttChartItem.OnActualDisplayRowIndexChanged)));

        /// <summary>
        /// Identifies the DisplayLineIndex property value.
        /// </summary>
        public static readonly DependencyProperty DisplayLineIndexProperty = DependencyProperty.Register("DisplayLineIndex", typeof(int?), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnDisplayLineIndexChanged)));

        /// <summary>
        /// Indicates the ActualDisplayLineIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualDisplayLineIndexProperty = DependencyProperty.Register("ActualDisplayLineIndex", typeof(int), typeof(CustomGanttChartItem), new PropertyMetadata(0, new PropertyChangedCallback(CustomGanttChartItem.OnActualDisplayLineIndexChanged)));

        /// <summary>
        /// Indicates the ComputedItemTop dependency property.
        /// </summary>
        public static readonly DependencyProperty ComputedItemTopProperty = DependencyProperty.Register("ComputedItemTop", typeof(double), typeof(CustomGanttChartItem), new PropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ItemHeight property value.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(CustomGanttChartItem), new PropertyMetadata(double.NaN, new PropertyChangedCallback(CustomGanttChartItem.OnItemHeightChanged)));

        /// <summary>
        /// Identifies the ItemBackground dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register("ItemBackground", typeof(Brush), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnItemBackgroundChanged)));

        /// <summary>
        /// Identifies the ItemForeground dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register("ItemForeground", typeof(Brush), typeof(CustomGanttChartItem), new PropertyMetadata(null, new PropertyChangedCallback(CustomGanttChartItem.OnItemForegroundChanged)));

        /// <summary>
        /// Indicates the ComputedTaskTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty ComputedTaskTemplateProperty = DependencyProperty.Register("ComputedTaskTemplate", typeof(DataTemplate), typeof(CustomGanttChartItem), new PropertyMetadata(null));

        /// <summary>
        /// Indicates the ComputedBarLeft dependency property.
        /// </summary>
        public static readonly DependencyProperty ComputedBarLeftProperty = DependencyProperty.Register("ComputedBarLeft", typeof(double), typeof(CustomGanttChartItem), new PropertyMetadata(0.0));

        /// <summary>
        /// Indicates the ComputedBarWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ComputedBarWidthProperty = DependencyProperty.Register("ComputedBarWidth", typeof(double), typeof(CustomGanttChartItem), new PropertyMetadata(0.0));

        /// <summary>
        /// Indicates the ComputedBarHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ComputedBarHeightProperty = DependencyProperty.Register("ComputedBarHeight", typeof(double), typeof(CustomGanttChartItem), new PropertyMetadata(0.0));

        /// <summary>
        /// Indicates the ComputedCompletedBarWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ComputedCompletedBarWidthProperty = DependencyProperty.Register("ComputedCompletedBarWidth", typeof(double), typeof(CustomGanttChartItem), new PropertyMetadata(0.0));

        /// <summary>
        /// Identifies the BarVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty BarVisibilityProperty = DependencyProperty.Register("BarVisibility", typeof(Visibility), typeof(CustomGanttChartItem), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(CustomGanttChartItem.OnBarVisibilityChanged)));

        /// <summary>
        /// Identifies the IsBarReadOnly dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBarReadOnlyProperty = DependencyProperty.Register("IsBarReadOnly", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(false, new PropertyChangedCallback(CustomGanttChartItem.OnIsBarReadOnlyChanged)));

        /// <summary>
        /// Indicates the IsBarThumbHitTestVisible dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBarThumbHitTestVisibleProperty = DependencyProperty.Register("IsBarThumbHitTestVisible", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(true));

        /// <summary>
        /// Indicates the IsVirtuallyVisible dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVirtuallyVisibleProperty = DependencyProperty.Register("IsVirtuallyVisible", typeof(bool), typeof(CustomGanttChartItem), new PropertyMetadata(false, new PropertyChangedCallback(CustomGanttChartItem.OnIsVirtuallyVisibleChanged)));

        /// <summary>
        /// Identifies the VirtualVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty VirtualVisibilityProperty = DependencyProperty.Register("VirtualVisibility", typeof(Visibility), typeof(CustomGanttChartItem), new PropertyMetadata(Visibility.Collapsed));

        private List<DependencyProperty> isPropertyChangeNotificationPaused = new List<DependencyProperty>();

        /// <summary>
        /// Occurs when a timing related property of the current task item changes.
        /// </summary>
        public event EventHandler TimingChanged;

        /// <summary>
        /// Occurs when predecessors changes for the current task item.
        /// </summary>
        public event EventHandler PredecessorsChanged;

        /// <summary>
        /// Occurs when the predecessor collection changes or any predecessor changes for the current task item.
        /// </summary>
        public event EventHandler DependenciesChanged;

        /// <summary>
        /// Occurs when assignments change for the current task item.
        /// </summary>
        public event EventHandler AssignmentsChanged;

        /// <summary>
        /// Occurs when the DisplayRowIndex property value of the current item changes.
        /// </summary>
        public event EventHandler DisplayRowIndexChanged;

        /// <summary>
        /// Occurs when the ActualDisplayRowIndex property value of the current item changes.
        /// </summary>
        public event EventHandler ActualDisplayRowIndexChanged;

        /// <summary>
        /// Occurs when the DisplayLineIndex property value of the current item changes.
        /// </summary>
        public event EventHandler DisplayLineIndexChanged;

        /// <summary>
        /// Occurs when the ActualDisplayLineIndex property value of the current item changes.
        /// </summary>
        public event EventHandler ActualDisplayLineIndexChanged;

        /// <summary>
        /// Occurs when the ItemHeight property value of the current item changes.
        /// </summary>
        public event EventHandler ItemHeightChanged;

        /// <summary>
        /// Occurs when any elements of the task bar change.
        /// </summary>
        public event EventHandler BarChanged;

        /// <summary>
        /// Gets the <see cref="T:DlhSoft.Windows.Controls.IGanttChartView">IGanttChartView</see> control instance of the current item.
        /// </summary>
        public IGanttChartView GanttChartView
        {
            get
            {
                return this.ganttChartView;
            }
            internal set
            {
                this.ganttChartView = value;
                this.CoerceTiming();
                this.CoerceBaselineTiming();
                this.OnPropertyChanged("ComputedBaselineBarLeft");
                this.OnPropertyChanged("ComputedBaselineBarWidth");
            }
        }

        /// <summary>
        /// Gets the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ScheduleChartItem">ScheduleChartItem</see> that hosts the current Gantt Chart item, when used within a Schedule Chart view.
        /// </summary>
        public ScheduleChartItem ScheduleChartItem
        {
            get
            {
                return this.scheduleChartItem;
            }
            internal set
            {
                this.scheduleChartItem = value;
                if (this.scheduleChartItem != null)
                {
                    this.scheduleChartItem.UpdateActualDisplayLineCount();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:DlhSoft.Windows.Controls.IScheduleChartView">IScheduleChartView</see> control instance of the current item, when used within a Schedule Chart view.
        /// </summary>
        public IScheduleChartView ScheduleChartView
        {
            get
            {
                if (this.scheduleChartItem == null)
                {
                    return null;
                }
                return this.scheduleChartItem.ScheduleChartView;
            }
        }

        internal object ScheduleChartTag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a Visibility value that indicates whether the current item is collapsed in order to display its expansion state in the view.
        /// This is a dependency value.
        /// </summary>
        public Visibility ExpansionVisibility
        {
            get
            {
                return (Visibility)base.GetValue(CustomGanttChartItem.ExpansionVisibilityProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ExpansionVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets whether the current item has children within the containing hierarchical collection, except that it always returns false when IsSummaryEnabled is set to false (in such case, HasHierarchicalChildren property may be used instead).
        /// This is based on a dependency property.
        /// </summary>
        public new bool HasChildren
        {
            get
            {
                return (base.HasChildren || base.IsVirtualSummary) && this.IsSummaryEnabled;
            }
            set
            {
                base.HasChildren = value;
            }
        }

        /// <summary>
        /// Gets whether the current item has children within the containing hierarchical collection.
        /// This is based on a dependency property.
        /// </summary>
        public bool HasHierarchicalChildren
        {
            get
            {
                return base.HasChildren;
            }
            set
            {
                base.HasChildren = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current item is considered as a leaf within the containing hierarchical collection.
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                return !this.HasChildren;
            }
        }

        /// <summary>
        /// Gets the direct parent item of the current item in the hierarchical collection.
        /// </summary>
        /// <returns>The parent item of the current item.</returns>
        public new CustomGanttChartItem Parent
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return null;
                }
                return this.ganttChartView.GetParent(this);
            }
        }

        /// <summary>
        /// Iterates all level parent items of the current item up in the hierarchical collection.
        /// </summary>
        /// <returns>All level parent items of the current item.</returns>
        public override IEnumerable<DataTreeGridItem> AllParents
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return null;
                }
                return from i in this.ganttChartView.GetAllParents(this)
                       select i;
            }
        }

        /// <summary>
        /// Iterates the direct child items of the current item in the hierarchical collection.
        /// </summary>
        /// <returns>The child items of the current item.</returns>
        public new IEnumerable<CustomGanttChartItem> Children
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return null;
                }
                return this.ganttChartView.GetChildren(this);
            }
        }

        /// <summary>
        /// Iterates all level child items of the current item down in the hierarchical collection.
        /// </summary>
        /// <returns>All level child items of the current item.</returns>
        public override IEnumerable<DataTreeGridItem> AllChildren
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return null;
                }
                return from i in this.ganttChartView.GetAllChildren(this)
                       select i;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the current task item is to be considered as a summary task and aggregate child item values.
        /// This is a dependency property.
        /// </summary>
        public bool IsSummaryEnabled
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsSummaryEnabledProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsSummaryEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the current task item is to be considered when aggregating child items and computing parent summary task values.
        /// This is a dependency property.
        /// </summary>
        public bool IsParentSummarizationEnabled
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsParentSummarizationEnabledProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsParentSummarizationEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the start date and time of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime Start
        {
            get
            {
                return (DateTime)base.GetValue(CustomGanttChartItem.StartProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.StartProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the preferred start date and time of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime PreferredStart
        {
            get
            {
                return (DateTime)base.GetValue(CustomGanttChartItem.PreferredStartProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.PreferredStartProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the finish date and time of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime Finish
        {
            get
            {
                return (DateTime)base.GetValue(CustomGanttChartItem.FinishProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.FinishProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the finish date and time of the completion time interval of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime CompletedFinish
        {
            get
            {
                return (DateTime)base.GetValue(CustomGanttChartItem.CompletedFinishProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.CompletedFinishProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the current task item is completed (triggering CompletedFinish property value to be equal to Finish property value).
        /// This is a dependency property.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsCompletedProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsCompletedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the current task item is to be displayed as a milestone (and triggering Finish property value to be equal to Start property value).
        /// This is a dependency property.
        /// </summary>
        public bool IsMilestone
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsMilestoneProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsMilestoneProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets an object that indicates the working and nonworking time intervals that should apply to this item in the Gantt Chart.
        /// </summary>
        public Schedule Schedule
        {
            get
            {
                return this.schedule;
            }
            set
            {
                if (value != this.schedule)
                {
                    this.schedule = value;
                }
                this.OnPropertyChanged("Schedule");
                this.OnTimingChanged();
                GanttChartView ganttChartView = this.GanttChartView as GanttChartView;
                if (ganttChartView != null && ganttChartView.IsIndividualItemNonworkingTimeHighlighted)
                {
                    ganttChartView.OnItemNonworkingHighlightsSourceChanged();
                }
            }
        }

        /// <summary>
        /// Gets the predecessor collection of the current task item used to define dependencies (to be displayed as polyline arrows between dependent task bars in the view).
        /// This is a dependency property.
        /// </summary>
        public PredecessorItemCollection Predecessors
        {
            get
            {
                return (PredecessorItemCollection)base.GetValue(CustomGanttChartItem.PredecessorsProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.PredecessorsProperty, value);
            }
        }

        /// <summary>
        /// Gets the predecessor collection of the current task item used to define dependencies (to be displayed as polyline arrows between dependent task bars in the view).
        /// </summary>
        IEnumerable<IPredecessorItem> ICustomGanttChartItem.Predecessors
        {
            get
            {
                if (this.Predecessors == null)
                {
                    return null;
                }
                return from p in this.Predecessors
                       select p;
            }
        }

        /// <summary>
        /// Gets a string value representing the index of the current task item within the GanttChartView host.
        /// </summary>
        public string IndexString
        {
            get
            {
                if (this.ganttChartView == null || this.ganttChartView.ManagedItems == null)
                {
                    return null;
                }
                return (this.ganttChartView.ManagedItems.IndexOf(this) + 1).ToString();
            }
        }

        /// <summary>
        /// Gets or sets a string value representing the work breakdown structure index of the current task item within the GanttChartView host.
        /// </summary>
        public string WbsIndexString
        {
            get
            {
                string arg_3D_0;
                if ((arg_3D_0 = this.customWbsIndexString) == null)
                {
                    if (this.ganttChartView == null || this.ganttChartView.ManagedItems == null)
                    {
                        return null;
                    }
                    arg_3D_0 = this.ganttChartView.ManagedItems.GetWbsIndexString(this, this.ganttChartView.WbsIndexStringSeparator);
                }
                return arg_3D_0;
            }
            set
            {
                if (value == string.Empty)
                {
                    value = null;
                }
                if (value == this.customWbsIndexString)
                {
                    return;
                }
                this.customWbsIndexString = value;
                this.OnPropertyChanged("WbsIndexString");
                if (this.Children != null)
                {
                    foreach (CustomGanttChartItem current in this.Children)
                    {
                        current.UpdateWbsIndexString();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.WbsIndexString" /> has been set to a custom value for this task item in the GanttChartView host.
        /// </summary>
        public bool IsCustomWbsIndexString
        {
            get
            {
                return this.customWbsIndexString != null;
            }
        }

        /// <summary>
        /// Gets or sets a string value representing the predecessors of the current task item and including dependency type abbreviations and/or lag times (in hours) within the GanttChartView host, based on or updating the Predecessors property value accordingly.
        /// </summary>
        public string PredecessorsString
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return null;
                }
                return this.ganttChartView.GetPredecessorsString(this);
            }
            set
            {
                if (this.ganttChartView == null)
                {
                    return;
                }
                this.ganttChartView.UpdatePredecessors(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the assignments of the current task item as a (usually string) value to be displayed in the view.
        /// This is a dependency property.
        /// </summary>
        public object AssignmentsContent
        {
            get
            {
                return base.GetValue(CustomGanttChartItem.AssignmentsContentProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.AssignmentsContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the effort value of the current task item (identical to duration for standard tasks), based on or updating the Finish property value accordingly.
        /// </summary>
        public TimeSpan Effort
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return TimeSpan.Zero;
                }
                if (this.HasChildren)
                {
                    TimeSpan timeSpan = TimeSpan.Zero;
                    IEnumerable<CustomGanttChartItem> children = this.ganttChartView.GetChildren(this);
                    if (children != null)
                    {
                        foreach (CustomGanttChartItem current in children)
                        {
                            timeSpan += current.Effort;
                        }
                        return timeSpan;
                    }
                }
                return this.ganttChartView.GetEffort(this.Start, this.Finish, this.Schedule);
            }
            set
            {
                if (this.ganttChartView == null)
                {
                    return;
                }
                this.Finish = this.ganttChartView.GetFinish(this.Start, value, this.Schedule);
            }
        }

        /// <summary>
        /// Gets or sets the total effort of the task, multiplying its duration by its total assignment allocation units.
        /// </summary>
        public TimeSpan TotalEffort
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return this.Effort;
                }
                if (this.HasChildren)
                {
                    TimeSpan timeSpan = TimeSpan.Zero;
                    IEnumerable<CustomGanttChartItem> children = this.ganttChartView.GetChildren(this);
                    if (children != null)
                    {
                        foreach (CustomGanttChartItem current in children)
                        {
                            timeSpan += current.TotalEffort;
                        }
                        return timeSpan;
                    }
                }
                double arg_BB_0 = this.Effort.TotalHours;
                double arg_BB_1;
                if (!this.ganttChartView.GetAssignments(this).Any<KeyValuePair<string, double>>())
                {
                    arg_BB_1 = 1.0;
                }
                else
                {
                    arg_BB_1 = this.ganttChartView.GetAssignments(this).Sum((KeyValuePair<string, double> a) => a.Value);
                }
                return TimeSpan.FromHours(arg_BB_0 * arg_BB_1);
            }
            set
            {
                if (this.ganttChartView == null)
                {
                    return;
                }
                double num = this.ganttChartView.GetAssignments(this).Sum((KeyValuePair<string, double> a) => a.Value);
                this.Effort = TimeSpan.FromHours(value.TotalHours / ((num > 0.0) ? num : 1.0));
            }
        }

        /// <summary>
        /// Gets or sets the duration value of the current task item, based on or updating the Finish property value accordingly.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return TimeSpan.Zero;
                }
                return this.ganttChartView.GetEffort(this.Start, this.Finish, this.Schedule);
            }
            set
            {
                if (this.ganttChartView == null)
                {
                    return;
                }
                this.Finish = this.ganttChartView.GetFinish(this.Start, value, this.Schedule);
            }
        }

        /// <summary>
        /// Gets or sets the completed effort value of the current task item, based on or updating the CompletedFinish property value accordingly.
        /// </summary>
        public TimeSpan CompletedEffort
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return TimeSpan.Zero;
                }
                if (this.HasChildren)
                {
                    TimeSpan timeSpan = TimeSpan.Zero;
                    IEnumerable<CustomGanttChartItem> children = this.ganttChartView.GetChildren(this);
                    if (children != null)
                    {
                        foreach (CustomGanttChartItem current in children)
                        {
                            timeSpan += current.CompletedEffort;
                        }
                        return timeSpan;
                    }
                }
                return this.ganttChartView.GetEffort(this.Start, this.CompletedFinish, this.Schedule);
            }
            set
            {
                if (this.ganttChartView == null)
                {
                    return;
                }
                this.CompletedFinish = this.ganttChartView.GetFinish(this.Start, value, this.Schedule);
            }
        }

        /// <summary>
        /// Gets the total completed effort of the task, multiplying its completed duration by its total assignment allocation units.
        /// </summary>
        public TimeSpan TotalCompletedEffort
        {
            get
            {
                if (this.ganttChartView == null)
                {
                    return this.CompletedEffort;
                }
                double arg_69_0 = this.CompletedEffort.TotalHours;
                double arg_69_1;
                if (!this.ganttChartView.GetAssignments(this).Any<KeyValuePair<string, double>>())
                {
                    arg_69_1 = 1.0;
                }
                else
                {
                    arg_69_1 = this.ganttChartView.GetAssignments(this).Sum((KeyValuePair<string, double> a) => a.Value);
                }
                return TimeSpan.FromHours(arg_69_0 * arg_69_1);
            }
        }

        /// <summary>
        /// Gets or sets the completion rate value of the current task item (between 0 and 1), based on or updating the CompletedEffort property value accordingly.
        /// </summary>
        public double Completion
        {
            get
            {
                if (!(this.Effort != TimeSpan.Zero))
                {
                    return (double)(this.IsCompleted ? 1 : 0);
                }
                return this.CompletedEffort.TotalHours / this.Effort.TotalHours;
            }
            set
            {
                this.CompletedEffort = TimeSpan.FromHours(this.Effort.TotalHours * Math.Max(0.0, Math.Min(1.0, value)));
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current task item has its work started, based on the Completion property value.
        /// </summary>
        public bool HasStarted
        {
            get
            {
                return this.CompletedFinish > this.Start;
            }
        }

        /// <summary>
        /// Gets or sets the baseline (estimated) start date and time of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime? BaselineStart
        {
            get
            {
                return (DateTime?)base.GetValue(CustomGanttChartItem.BaselineStartProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.BaselineStartProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the baseline (estimated) finish date and time of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime? BaselineFinish
        {
            get
            {
                return (DateTime?)base.GetValue(CustomGanttChartItem.BaselineFinishProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.BaselineFinishProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed left coordinate of the baseline task bar of the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (based on Start and BaselineStart values).
        /// </summary>
        public double ComputedBaselineBarLeft
        {
            get
            {
                if (!this.BaselineStart.HasValue || this.GanttChartView == null)
                {
                    return -3.4028234663852886E+38;
                }
                double num = this.GanttChartView.GetPosition(this.BaselineStart.Value) - this.GanttChartView.GetPosition(this.Start);
                if (num < DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent)
                {
                    num = DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent;
                }
                return num;
            }
        }

        /// <summary>
        /// Gets the computed width of the baseline task bar of the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (based on BaselineStart and BaselineFinish values).
        /// </summary>
        public double ComputedBaselineBarWidth
        {
            get
            {
                if (!this.BaselineStart.HasValue || this.GanttChartView == null)
                {
                    return 0.0;
                }
                if (!this.BaselineFinish.HasValue)
                {
                    return 4.0;
                }
                double num = Math.Max(4.0, this.GanttChartView.GetPosition(this.BaselineFinish.Value) - this.GanttChartView.GetPosition(this.BaselineStart.Value));
                if (num > DlhSoft.Windows.Controls.GanttChartView.MaxSizeExtent && this.ComputedBaselineBarLeft < DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent)
                {
                    num -= DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent - this.ComputedBaselineBarLeft;
                }
                return Math.Min(DlhSoft.Windows.Controls.GanttChartView.MaxSizeExtent, num);
            }
        }

        /// <summary>
        /// Gets or sets the baseline (estimated) duration value of the current task item, based on the BaselineStart and BaselineFinish property values.
        /// </summary>
        public TimeSpan? BaselineEffort
        {
            get
            {
                if (this.GanttChartView == null)
                {
                    return null;
                }
                if (!this.BaselineStart.HasValue || !this.BaselineFinish.HasValue)
                {
                    return null;
                }
                return new TimeSpan?(this.GanttChartView.GetEffort(this.BaselineStart.Value, this.BaselineFinish.Value, this.Schedule));
            }
            set
            {
                if (this.GanttChartView == null)
                {
                    return;
                }
                if (!this.BaselineStart.HasValue)
                {
                    this.BaselineStart = new DateTime?(this.Start);
                }
                this.BaselineFinish = (value.HasValue ? new DateTime?(this.GanttChartView.GetFinish(this.BaselineStart.Value, value.Value, this.Schedule)) : null);
            }
        }

        /// <summary>
        /// Gets the start delay time span value of the current task item, based on the BaselineStart property value.
        /// </summary>
        public TimeSpan StartDelay
        {
            get
            {
                if (this.GanttChartView == null)
                {
                    return TimeSpan.Zero;
                }
                if (!this.BaselineStart.HasValue)
                {
                    return TimeSpan.Zero;
                }
                return this.GanttChartView.GetEffort(this.BaselineStart.Value, this.Start, this.Schedule);
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is positive start delay on the current task item, based on the BaselineStart property value.
        /// </summary>
        public bool IsStartDelayed
        {
            get
            {
                return this.StartDelay > TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets the finish delay time span value of the current task item, based on the BaselineFinish property value.
        /// </summary>
        public TimeSpan FinishDelay
        {
            get
            {
                if (this.GanttChartView == null)
                {
                    return TimeSpan.Zero;
                }
                if (!this.BaselineFinish.HasValue)
                {
                    return TimeSpan.Zero;
                }
                return this.GanttChartView.GetEffort(this.BaselineFinish.Value, this.Finish, this.Schedule);
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is positive finish delay on the current task item, based on the BaselineFinish property value.
        /// </summary>
        public bool IsFinishDelayed
        {
            get
            {
                return this.FinishDelay > TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets or sets the minimum start date and time of the current task item.
        /// This is a dependency property.
        /// </summary>
        public DateTime? MinStart
        {
            get
            {
                return (DateTime?)base.GetValue(CustomGanttChartItem.MinStartProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.MinStartProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum start date and time of the current task item (applied when possible).
        /// This is a dependency property.
        /// </summary>
        public DateTime? MaxStart
        {
            get
            {
                return (DateTime?)base.GetValue(CustomGanttChartItem.MaxStartProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.MaxStartProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum finish date and time of the current task item (applied when possible).
        /// This is a dependency property.
        /// </summary>
        public DateTime? MinFinish
        {
            get
            {
                return (DateTime?)base.GetValue(CustomGanttChartItem.MinFinishProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.MinFinishProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum finish date and time of the current task item (applied when possible).
        /// This is a dependency property.
        /// </summary>
        public DateTime? MaxFinish
        {
            get
            {
                return (DateTime?)base.GetValue(CustomGanttChartItem.MaxFinishProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.MaxFinishProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the execution cost of this task.
        /// </summary>
        public double ExecutionCost
        {
            get
            {
                return this.executionCost;
            }
            set
            {
                this.executionCost = value;
                this.OnPropertyChanged("ExecutionCost");
                this.UpdateCost();
            }
        }

        /// <summary>
        /// Gets the assignments cost of the task, summing up <see cref="P:DlhSoft.Windows.Controls.GanttChartView.DefaultResourceUsageCost" /> or <see cref="P:DlhSoft.Windows.Controls.GanttChartView.SpecificResourceUsageCosts" />, and <see cref="P:DlhSoft.Windows.Controls.GanttChartView.DefaultResourceHourCost" /> or <see cref="P:DlhSoft.Windows.Controls.GanttChartView.SpecificResourceHourCosts" /> multiplied by work effort hours (for standard tasks).
        /// The value is eventually multiplied by assignment allocation units (if set).
        /// </summary>
        public double AssignmentsCost
        {
            get
            {
                if (!this.assignmentsCost.HasValue)
                {
                    double arg_45_0;
                    if (this.ganttChartView == null)
                    {
                        arg_45_0 = 0.0;
                    }
                    else
                    {
                        arg_45_0 = this.ganttChartView.GetAssignments(this).Sum((KeyValuePair<string, double> a) => (((this.ganttChartView.SpecificResourceUsageCosts != null && this.ganttChartView.SpecificResourceUsageCosts.ContainsKey(a.Key)) ? this.ganttChartView.SpecificResourceUsageCosts[a.Key] : this.ganttChartView.DefaultResourceUsageCost) + ((!this.HasChildren) ? (((this.ganttChartView.SpecificResourceHourCosts != null && this.ganttChartView.SpecificResourceHourCosts.ContainsKey(a.Key)) ? this.ganttChartView.SpecificResourceHourCosts[a.Key] : this.ganttChartView.DefaultResourceHourCost) * this.Effort.TotalHours) : 0.0)) * a.Value);
                    }
                    this.assignmentsCost = new double?(arg_45_0);
                }
                return this.assignmentsCost.Value;
            }
        }

        /// <summary>
        /// Gets the extra cost of the task, summing up <see cref="P:DlhSoft.Windows.Controls.GanttChartView.TaskInitiationCost" />, <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.AssignmentsCost" />, and child task costs (for summary tasks).
        /// </summary>
        public double ExtraCost
        {
            get
            {
                if (!this.extraCost.HasValue)
                {
                    double arg_76_0 = ((this.ganttChartView != null) ? this.ganttChartView.TaskInitiationCost : 0.0) + this.AssignmentsCost;
                    double arg_76_1;
                    if (!this.HasChildren || this.Children == null)
                    {
                        arg_76_1 = 0.0;
                    }
                    else
                    {
                        arg_76_1 = this.Children.Sum((CustomGanttChartItem i) => i.Cost);
                    }
                    this.extraCost = new double?(arg_76_0 + arg_76_1);
                }
                return this.extraCost.Value;
            }
        }

        /// <summary>
        /// Gets or sets the total cost of the task, summing up <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ExecutionCost" /> and <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ExtraCost" />.
        /// </summary>
        public double Cost
        {
            get
            {
                return this.ExecutionCost + this.ExtraCost;
            }
            set
            {
                this.ExecutionCost = value - this.ExtraCost;
            }
        }

        /// <summary>
        /// Gets or sets the index of the row that the current item should be displayed at in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (null by default, displaying the item at the containing collection index).
        /// This is a dependency property.
        /// </summary>
        public int? DisplayRowIndex
        {
            get
            {
                return (int?)base.GetValue(CustomGanttChartItem.DisplayRowIndexProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.DisplayRowIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets the actual index of the row that the current item is displayed at in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (if DisplayRowIndex is null, it returns the containing collection index of the current item).
        /// This is a dependency property.
        /// </summary>
        public int ActualDisplayRowIndex
        {
            get
            {
                return (int)base.GetValue(CustomGanttChartItem.ActualDisplayRowIndexProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ActualDisplayRowIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the index of the line that the current item should be displayed at within its parent <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ScheduleChartItem">ScheduleChartItem</see> in a <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ScheduleChartView">ScheduleChartView</see> control (null by default, displaying the item at the containing collection index).
        /// This is a dependency property.
        /// </summary>
        public int? DisplayLineIndex
        {
            get
            {
                return (int?)base.GetValue(CustomGanttChartItem.DisplayLineIndexProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.DisplayLineIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets the actual index of the line that the current item is displayed at within its parent <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ScheduleChartItem">ScheduleChartItem</see> in a <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.ScheduleChartView">ScheduleChartView</see> control (if DisplayLineIndex is null, it returns 0).
        /// This is a dependency property.
        /// </summary>
        public int ActualDisplayLineIndex
        {
            get
            {
                return (int)base.GetValue(CustomGanttChartItem.ActualDisplayLineIndexProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ActualDisplayLineIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed Canvas.Top value to be used for displaying the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control.
        /// This is a dependency property.
        /// </summary>
        public double ComputedItemTop
        {
            get
            {
                return (double)base.GetValue(CustomGanttChartItem.ComputedItemTopProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ComputedItemTopProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the specific height that the current item should use to be displayed in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control when AreIndividualItemHeightsApplied property of the containing control is set to true (by default Auto, i.e. NaN).
        /// This is a dependency property.
        /// </summary>
        public double ItemHeight
        {
            get
            {
                return (double)base.GetValue(CustomGanttChartItem.ItemHeightProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ItemHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the background of the item row in the grid and/or chart view.
        /// This is a dependency property.
        /// </summary>
        public Brush ItemBackground
        {
            get
            {
                return (Brush)base.GetValue(CustomGanttChartItem.ItemBackgroundProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ItemBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the foreground of the item row content in the grid and/or chart view.
        /// This is a dependency property.
        /// </summary>
        public Brush ItemForeground
        {
            get
            {
                return (Brush)base.GetValue(CustomGanttChartItem.ItemForegroundProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ItemForegroundProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed DataTemplate to be used for displaying the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (standard task, summary task, or milestone task), based on the current task item properties and position in the containing hierarchical collection.
        /// This is a dependency property.
        /// </summary>
        public DataTemplate ComputedTaskTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(CustomGanttChartItem.ComputedTaskTemplateProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ComputedTaskTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed Canvas.Left value to be used for displaying the task bar of the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (based on Start property value).
        /// This is a dependency property.
        /// </summary>
        public double ComputedBarLeft
        {
            get
            {
                return (double)base.GetValue(CustomGanttChartItem.ComputedBarLeftProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ComputedBarLeftProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed width value to be used for displaying the task bar of the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (based on Finish and Start property values).
        /// This is a dependency property.
        /// </summary>
        public double ComputedBarWidth
        {
            get
            {
                return (double)base.GetValue(CustomGanttChartItem.ComputedBarWidthProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ComputedBarWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed height value to be used for displaying the task bar of the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (based on GanttChartView.ItemHeight property value).
        /// This is a dependency property.
        /// </summary>
        public double ComputedBarHeight
        {
            get
            {
                return (double)base.GetValue(CustomGanttChartItem.ComputedBarHeightProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ComputedBarHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets the computed width value to be used for displaying the completion interval bar within the task bar of the current item in the <see cref="P:DlhSoft.Windows.Controls.CustomGanttChartItem.GanttChartView">GanttChartView</see> control (based on CompletedFinish and Start property values).
        /// This is a dependency property.
        /// </summary>
        public double ComputedCompletedBarWidth
        {
            get
            {
                return (double)base.GetValue(CustomGanttChartItem.ComputedCompletedBarWidthProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.ComputedCompletedBarWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether and how the bar of the current task item is going to be displayed in the chart view.
        /// This is a dependency property.
        /// </summary>
        public Visibility BarVisibility
        {
            get
            {
                return (Visibility)base.GetValue(CustomGanttChartItem.BarVisibilityProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.BarVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the user can edit the timing values of the current task item in the control by drag and drop operations applied on its associated display bar.
        /// This is a dependency property.
        /// </summary>
        public bool IsBarReadOnly
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsBarReadOnlyProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsBarReadOnlyProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the Thumb controls of the task item bar should enable the user to edit the timing values of task items in the control by drag and drop operations (the Thumb objects would be enabled if GanttChartView.IsBarReadOnly attached property is set to false on the item).
        /// This is a dependency property.
        /// </summary>
        public bool IsBarThumbHitTestVisible
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsBarThumbHitTestVisibleProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsBarThumbHitTestVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets (or sets) a value that indicates whether the current item is (or was) visible inside the scrolling view box when using virtualization mode (when GanttChartView.IsVirtualizing or GanttChartDataGrid.IsVirtualizing is set to true).
        /// This is a dependency property.
        /// </summary>
        public bool IsVirtuallyVisible
        {
            get
            {
                return (bool)base.GetValue(CustomGanttChartItem.IsVirtuallyVisibleProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.IsVirtuallyVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets a Visibility value that indicates whether the current item is (or was) visible inside the scrolling view box when using virtualization mode (when GanttChartView.IsVirtualizing or GanttChartDataGrid.IsVirtualizing is set to true).
        /// This is a dependency value.
        /// </summary>
        public Visibility VirtualVisibility
        {
            get
            {
                return (Visibility)base.GetValue(CustomGanttChartItem.VirtualVisibilityProperty);
            }
            set
            {
                base.SetValue(CustomGanttChartItem.VirtualVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="T:DlhSoft.Windows.Controls.CustomGanttChartItem">CustomGanttChartItem</see> instance.
        /// </summary>
        public CustomGanttChartItem()
        {
            this.Predecessors = new PredecessorItemCollection();
        }

        /// <summary>
        /// Raises the ExpansionChanged event and updates appropriate internal properties of the current item.
        /// </summary>
        protected override void OnExpansionChanged()
        {
            base.OnExpansionChanged();
            this.ExpansionVisibility = (base.IsExpanded ? Visibility.Collapsed : Visibility.Visible);
        }

        /// <summary>
        /// Provides a way for inheriters to specify the managed items collection that will store the new child items specified to be loaded during <see cref="E:DlhSoft.Windows.Controls.DataTreeGridItem.VirtualSummaryInitiallyExpanded" /> event handling.
        /// </summary>
        protected override IList GetVirtualSummaryInitializatingCollection()
        {
            GanttChartView ganttChartView = this.GanttChartView as GanttChartView;
            IGanttChartView ganttChartView2 = (ganttChartView != null) ? ganttChartView.MainGanttChartView : null;
            if (ganttChartView2 == null)
            {
                return null;
            }
            return ganttChartView2.Items;
        }

        /// <summary>
        /// Raises the HasChildrenChanged event.
        /// </summary>
        protected override void OnHasChildrenChanged()
        {
            base.OnHasChildrenChanged();
            this.CoerceTiming();
            this.OnPropertyChanged("HasHierarchicalChildren");
            this.OnPropertyChanged("IsLeaf");
        }

        /// <summary>
        /// Gets a value that indicates whether the current item has the specified item as a direct or indirect, and an implicit or explicit predecessor in the hierarchy.
        /// </summary>
        /// <param name="predecessorItem">The item to check whether it is a predecessor for the current item.</param>
        /// <returns>A value that indicates whether the current item has the specified item as a predecessor.</returns>
        public bool DependsOf(CustomGanttChartItem predecessorItem)
        {
            return this.ganttChartView != null && this.ganttChartView.DependsOf(this, predecessorItem);
        }

        private static void OnIsSummaryEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("IsSummaryEnabled");
            CustomGanttChartItem.OnPropertyChanged("HasChildren");
            CustomGanttChartItem.UpdateComputedItemTop();
            CustomGanttChartItem.UpdateBar();
            CustomGanttChartItem.UpdateDependencyLines();
            CustomGanttChartItem.UpdateComputedTaskTemplate();
            CustomGanttChartItem.OnTimingChanged();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.IsSummaryEnabled);
        }

        private static void OnIsParentSummarizationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("IsParentSummarizationEnabled");
            CustomGanttChartItem.OnTimingChanged();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.IsParentSummarizationEnabled);
        }

        private static void OnStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            DateTime initialCompletedFinish = CustomGanttChartItem.CompletedFinish;
            GanttChartView ganttChartView = CustomGanttChartItem.ganttChartView as GanttChartView;
            if (ganttChartView != null && ganttChartView.AreTaskDependencyConstraintsEnabled)
            {
                ganttChartView.Dispatcher.BeginInvoke(new Action(delegate
                {
                    bool isDuringEnsureDependencyConstraintsOperation = ganttChartView.isDuringEnsureDependencyConstraintsOperation;
                    ganttChartView.isDuringEnsureDependencyConstraintsOperation = true;
                    CustomGanttChartItem CustomGanttChartItem;
                    for (CustomGanttChartItem = CustomGanttChartItem; CustomGanttChartItem != null; CustomGanttChartItem = CustomGanttChartItem.Parent)
                    {
                        if (CustomGanttChartItem.Predecessors != null)
                        {
                            foreach (PredecessorItem current in CustomGanttChartItem.Predecessors)
                            {
                                if (current.Item != null)
                                {
                                    switch (current.DependencyType)
                                    {
                                        case DependencyType.FinishStart:
                                            {
                                                DateTime finish = ganttChartView.GetFinish(current.Item.Finish, current.Lag);
                                                if (finish > CustomGanttChartItem.Start)
                                                {
                                                    CustomGanttChartItem.Start = finish;
                                                    if (initialCompletedFinish <= (DateTime)e.OldValue)
                                                    {
                                                        CustomGanttChartItem.CompletedFinish = CustomGanttChartItem.Start;
                                                    }
                                                    return;
                                                }
                                                break;
                                            }
                                        case DependencyType.StartStart:
                                            {
                                                DateTime finish = ganttChartView.GetFinish(current.Item.Start, current.Lag);
                                                if (finish > CustomGanttChartItem.Start)
                                                {
                                                    CustomGanttChartItem.Start = finish;
                                                    if (initialCompletedFinish <= (DateTime)e.OldValue)
                                                    {
                                                        CustomGanttChartItem.CompletedFinish = CustomGanttChartItem.Start;
                                                    }
                                                    return;
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                    ganttChartView.isDuringEnsureDependencyConstraintsOperation = isDuringEnsureDependencyConstraintsOperation;
                }), new object[0]);
            }
            CustomGanttChartItem.OnPropertyChanged("Start");
            if (ganttChartView == null || (!ganttChartView.isDuringEnsureDependencyConstraintsOperation && !ganttChartView.isDuringUndoRedoOperation))
            {
                CustomGanttChartItem.PreferredStart = CustomGanttChartItem.Start;
            }
            bool flag = CustomGanttChartItem.RequestPropertyChangeNotificationPause(CustomGanttChartItem.StartProperty);
            CustomGanttChartItem.OnTimingChanged();
            CustomGanttChartItem.UpdateDependencyLines();
            if (flag)
            {
                CustomGanttChartItem.ResumePropertyChangedNotifications(CustomGanttChartItem.StartProperty);
            }
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.Start);
            CustomGanttChartItem.OnPropertyChanged("ComputedBaselineBarLeft");
            CustomGanttChartItem.OnPropertyChanged("ComputedBaselineBarWidth");
            CustomGanttChartItem.OnPropertyChanged("StartDelay");
            CustomGanttChartItem.OnPropertyChanged("IsStartDelayed");
            if (initialCompletedFinish <= (DateTime)e.OldValue)
            {
                CustomGanttChartItem.CompletedFinish = CustomGanttChartItem.Start;
            }
        }

        private static void OnPreferredStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("PreferredStart");
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.PreferredStart);
        }

        private static void OnFinishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("Finish");
            bool flag = CustomGanttChartItem.RequestPropertyChangeNotificationPause(CustomGanttChartItem.FinishProperty);
            CustomGanttChartItem.CoerceFinish();
            CustomGanttChartItem.OnTimingChanged();
            CustomGanttChartItem.UpdateDependencyLines();
            CustomGanttChartItem.UpdateIsCompleted();
            if (flag)
            {
                CustomGanttChartItem.ResumePropertyChangedNotifications(CustomGanttChartItem.FinishProperty);
            }
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.Finish);
            CustomGanttChartItem.OnPropertyChanged("FinishDelay");
            CustomGanttChartItem.OnPropertyChanged("IsFinishDelayed");
        }

        private void CoerceFinish()
        {
            if (this.Finish > this.Start && this.IsMilestone && !this.HasChildren)
            {
                this.IsMilestone = false;
            }
        }

        private static void OnCompletedFinishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("CompletedFinish");
            bool flag = CustomGanttChartItem.RequestPropertyChangeNotificationPause(CustomGanttChartItem.CompletedFinishProperty);
            CustomGanttChartItem.OnTimingChanged();
            CustomGanttChartItem.UpdateIsCompleted();
            if (flag)
            {
                CustomGanttChartItem.ResumePropertyChangedNotifications(CustomGanttChartItem.CompletedFinishProperty);
            }
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.CompletedFinish);
        }

        private static void OnIsCompletedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("IsCompleted");
            CustomGanttChartItem.OnPropertyChanged("Completion");
            CustomGanttChartItem.CoerceCompletion();
        }

        internal void UpdateIsCompleted()
        {
            if (!this.HasChildren)
            {
                if (this.Finish > this.Start)
                {
                    this.IsCompleted = (this.CompletedFinish == this.Finish);
                    return;
                }
            }
            else if (this.Children != null && this.Children.Any<CustomGanttChartItem>())
            {
                this.IsCompleted = this.Children.All((CustomGanttChartItem c) => c.IsCompleted);
            }
        }

        private void CoerceCompletion()
        {
            if (this.IsCompleted)
            {
                this.CompletedFinish = this.Finish;
            }
            else if (this.CompletedFinish == this.Finish)
            {
                this.CompletedFinish = this.Start;
            }
            if (this.Parent != null)
            {
                this.Parent.UpdateIsCompleted();
            }
        }

        private static void OnIsMilestoneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("IsMilestone");
            bool flag = CustomGanttChartItem.RequestPropertyChangeNotificationPause(CustomGanttChartItem.IsMilestoneProperty);
            CustomGanttChartItem.UpdateComputedTaskTemplate();
            CustomGanttChartItem.OnTimingChanged();
            CustomGanttChartItem.UpdateDependencyLines();
            CustomGanttChartItem.UpdateIsCompleted();
            if (flag)
            {
                CustomGanttChartItem.ResumePropertyChangedNotifications(CustomGanttChartItem.IsMilestoneProperty);
            }
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.IsMilestone);
        }

        /// <summary>
        /// Updates appropriate internal properties of the current task item, and raises the TimingChanged event.
        /// </summary>
        protected virtual void OnTimingChanged()
        {
            this.CoerceTiming();
            this.CoerceBaselineTiming();
            this.UpdateBar();
            if (this.TimingChanged != null)
            {
                this.TimingChanged(this, EventArgs.Empty);
            }
            this.UpdateTimingExtras();
            if (this.ganttChartView != null)
            {
                for (CustomGanttChartItem parent = this.ganttChartView.GetParent(this); parent != null; parent = this.ganttChartView.GetParent(parent))
                {
                    parent.UpdateTimingExtras();
                }
            }
            if (this.GanttChartView != null && this.GanttChartView.AreTaskDependencyConstraintsEnabled)
            {
                this.GanttChartView.EnsureDependencyConstraints();
            }
            GanttChartView ganttChartView = this.GanttChartView as GanttChartView;
            if (ganttChartView != null && ganttChartView.AreTaskInterruptionsHighlighted)
            {
                ganttChartView.OnTaskInterruptionHighlightsSourceChanged();
            }
        }

        internal void UpdateTimingExtras()
        {
            if (this.isDuringUpdateTimingExtras)
            {
                return;
            }
            this.isDuringUpdateTimingExtras = true;
            base.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                this.OnPropertyChanged("Effort");
                this.OnPropertyChanged("Duration");
                this.OnPropertyChanged("CompletedEffort");
                this.OnPropertyChanged("TotalEffort");
                this.OnPropertyChanged("TotalCompletedEffort");
                this.OnPropertyChanged("Completion");
                this.OnPropertyChanged("IsCompleted");
                this.OnPropertyChanged("HasStarted");
                this.isDuringUpdateTimingExtras = false;
                this.UpdateCost();
            }), DispatcherPriority.Background, new object[0]);
        }

        internal void CoerceTiming()
        {
            if (!this.isDuringCoerceTiming && !this.HasChildren && this.GanttChartView != null)
            {
                this.isDuringCoerceTiming = true;
                DateTime dateTime = this.Start;
                if (this.MinStart.HasValue && dateTime < this.MinStart)
                {
                    dateTime = this.MinStart.Value;
                }
                else if (this.MaxStart.HasValue && dateTime > this.MaxStart)
                {
                    dateTime = this.MaxStart.Value;
                }
                if (!(this is AllocationItem))
                {
                    dateTime = this.GanttChartView.GetNextWorkingTime(dateTime, this.Schedule);
                }
                if (!this.IsMilestone)
                {
                    DateTime dateTime2 = this.Finish;
                    if (this.MaxFinish.HasValue && dateTime2 > this.MaxFinish)
                    {
                        dateTime2 = this.MaxFinish.Value;
                    }
                    else if (this.MinFinish.HasValue && dateTime2 < this.MinFinish)
                    {
                        dateTime2 = this.MinFinish.Value;
                    }
                    this.Start = dateTime;
                    if (!(this is AllocationItem))
                    {
                        this.Finish = this.GanttChartView.GetPreviousWorkingTime(dateTime2, this.Schedule);
                    }
                    else
                    {
                        this.Finish = dateTime2;
                    }
                    DateTime dateTime3 = this.CompletedFinish;
                    if (dateTime3 < dateTime)
                    {
                        dateTime3 = dateTime;
                    }
                    if (dateTime3 > dateTime2)
                    {
                        dateTime3 = dateTime2;
                    }
                    if (!(this is AllocationItem))
                    {
                        this.CompletedFinish = this.GanttChartView.GetPreviousWorkingTime(dateTime3, this.Schedule);
                    }
                    else
                    {
                        this.CompletedFinish = dateTime3;
                    }
                }
                else if (this.GanttChartView.AreMilestonesFinishTimes == false || this is AllocationItem)
                {
                    this.Start = dateTime;
                }
                else
                {
                    DateTime previousWorkingTime = this.GanttChartView.GetPreviousWorkingTime(this.Start, this.Schedule);
                    if (this.GanttChartView.AreMilestonesFinishTimes == true)
                    {
                        this.Start = previousWorkingTime;
                    }
                    else if (dateTime - this.Start <= this.Start - previousWorkingTime)
                    {
                        this.Start = dateTime;
                    }
                    else
                    {
                        this.Start = previousWorkingTime;
                    }
                }
                this.isDuringCoerceTiming = false;
            }
            if (this.IsMilestone && !this.HasChildren)
            {
                this.Finish = this.Start;
            }
            if (this.Finish < this.Start)
            {
                this.Finish = this.Start;
            }
            if (this.CompletedFinish < this.Start)
            {
                this.CompletedFinish = this.Start;
            }
            if (this.CompletedFinish > this.Finish)
            {
                this.CompletedFinish = this.Finish;
            }
            if (this.Finish > this.Start && !this.HasChildren)
            {
                this.IsMilestone = false;
            }
        }

        private static void OnPredecessorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("Predecessors");
            CustomGanttChartItem.OnPropertyChanged("PredecessorsString");
            CustomGanttChartItem.OnPredecessorsChanged();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e);
        }

        /// <summary>
        /// Executes internal predecessor related data management, updates appropriate internal properties of the current task item, and raises the PredecessorsChanged and DependenciesChanged events.
        /// </summary>
        protected virtual void OnPredecessorsChanged()
        {
            if (this.managedPredecessorItems != null)
            {
                this.managedPredecessorItems.Changed -= new EventHandler(this.ManagedPredecessorItems_Changed);
                this.managedPredecessorItems.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ManagedPredecessorItems_CollectionChanged);
            }
            this.managedPredecessorItems = this.Predecessors;
            if (this.managedPredecessorItems != null)
            {
                this.SetManagedPredecessorItems(this.managedPredecessorItems);
                this.managedPredecessorItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ManagedPredecessorItems_CollectionChanged);
                this.managedPredecessorItems.Changed += new EventHandler(this.ManagedPredecessorItems_Changed);
            }
            this.OnDependenciesChanged();
            if (this.PredecessorsChanged != null)
            {
                this.PredecessorsChanged(this, EventArgs.Empty);
            }
        }

        private void ManagedPredecessorItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("Predecessors");
            this.OnPropertyChanged("PredecessorsString");
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.SetManagedPredecessorItems(e.NewItems);
                    return;
                case NotifyCollectionChangedAction.Remove:
                    return;
            }
            this.SetManagedPredecessorItems(this.managedPredecessorItems);
        }

        private void SetManagedPredecessorItems(IList predecessorItems)
        {
            foreach (PredecessorItem predecessorItem in predecessorItems)
            {
                predecessorItem.DependentItem = this;
            }
        }

        private void ManagedPredecessorItems_Changed(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Predecessors");
            this.OnPropertyChanged("PredecessorsString");
            this.OnDependenciesChanged();
        }

        /// <summary>
        /// Updates appropriate internal properties of the current task item, and raises the DependenciesChanged event.
        /// </summary>
        protected virtual void OnDependenciesChanged()
        {
            if (this.ganttChartView != null)
            {
                if (!this.ganttChartView.AreCircularDependenciesAllowed)
                {
                    this.ganttChartView.RemoveCircularDependencies();
                }
                if (this.ganttChartView.AreTaskDependencyConstraintsEnabled)
                {
                    this.ganttChartView.EnsureDependencyConstraints();
                }
            }
            this.UpdateDependencyLines();
            if (this.DependenciesChanged != null)
            {
                this.DependenciesChanged(this, EventArgs.Empty);
            }
        }

        internal void UpdateIndexString()
        {
            this.OnPropertyChanged("IndexString");
        }

        internal void UpdateWbsIndexString()
        {
            this.OnPropertyChanged("WbsIndexString");
        }

        private static void OnAssignmentsContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            if (e.OldValue is string && e.NewValue is string && e.OldValue as string == e.NewValue as string)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("AssignmentsContent");
            CustomGanttChartItem.OnAssignmentsChanged();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e);
            CustomGanttChartItem.UpdateCost();
        }

        /// <summary>
        /// Raises the AssignmentsChanged event.
        /// </summary>
        protected virtual void OnAssignmentsChanged()
        {
            if (this.AssignmentsChanged != null)
            {
                this.AssignmentsChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Reschedules the current item to start on the specified date and time.
        /// </summary>
        public void RescheduleToStart(DateTime start)
        {
            TimeSpan effort = this.Effort;
            TimeSpan completedEffort = this.CompletedEffort;
            this.Start = start;
            this.Effort = effort;
            this.CompletedEffort = completedEffort;
        }

        /// <summary>
        /// Reschedules the current item to finish on the specified date and time.
        /// </summary>
        public void RescheduleToFinish(DateTime finish)
        {
            if (this.ganttChartView == null)
            {
                return;
            }
            TimeSpan effort = this.Effort;
            TimeSpan completedEffort = this.CompletedEffort;
            this.Start = this.ganttChartView.GetStart(effort, finish, this.Schedule);
            this.Effort = effort;
            this.CompletedEffort = completedEffort;
        }

        /// <summary>
        /// Gets a value indicating whether the task item is currently on schedule, so that its completed effort value reaches the current date and time.
        /// </summary>
        /// <returns>A value indicating whether the task item is currently on schedule.</returns>
        public bool IsOnSchedule()
        {
            return this.IsOnSchedule(DateTime.Now);
        }

        /// <summary>
        /// Gets a value indicating whether the task item would be on schedule at the specified date and time, so that its completed effort value reaches the specified date and time.
        /// </summary>
        /// <returns>A value indicating whether the task item would be on schedule at the specified date and time.</returns>
        public bool IsOnSchedule(DateTime dateTime)
        {
            return this.CompletedFinish >= dateTime;
        }

        private static void OnBaselineStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("BaselineStart");
            bool flag = CustomGanttChartItem.RequestPropertyChangeNotificationPause(CustomGanttChartItem.BaselineStartProperty);
            CustomGanttChartItem.CoerceBaselineTiming();
            if (flag)
            {
                CustomGanttChartItem.ResumePropertyChangedNotifications(CustomGanttChartItem.BaselineStartProperty);
            }
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.BaselineStart);
            CustomGanttChartItem.OnPropertyChanged("ComputedBaselineBarLeft");
            CustomGanttChartItem.OnPropertyChanged("ComputedBaselineBarWidth");
            CustomGanttChartItem.OnPropertyChanged("BaselineEffort");
            CustomGanttChartItem.OnPropertyChanged("StartDelay");
            CustomGanttChartItem.OnPropertyChanged("IsStartDelayed");
        }

        private static void OnBaselineFinishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("BaselineFinish");
            bool flag = CustomGanttChartItem.RequestPropertyChangeNotificationPause(CustomGanttChartItem.BaselineFinishProperty);
            CustomGanttChartItem.CoerceBaselineTiming();
            if (flag)
            {
                CustomGanttChartItem.ResumePropertyChangedNotifications(CustomGanttChartItem.BaselineFinishProperty);
            }
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.BaselineFinish);
            CustomGanttChartItem.OnPropertyChanged("ComputedBaselineBarWidth");
            CustomGanttChartItem.OnPropertyChanged("BaselineEffort");
            CustomGanttChartItem.OnPropertyChanged("FinishDelay");
            CustomGanttChartItem.OnPropertyChanged("IsFinishDelayed");
        }

        internal void CoerceBaselineTiming()
        {
            if (!this.isDuringCoerceTiming && this.GanttChartView != null)
            {
                this.isDuringCoerceTiming = true;
                if (this.BaselineStart.HasValue)
                {
                    this.BaselineStart = new DateTime?(this.GanttChartView.GetNextWorkingTime(this.BaselineStart.Value, this.Schedule));
                }
                if (this.BaselineFinish.HasValue)
                {
                    this.BaselineFinish = new DateTime?(this.GanttChartView.GetPreviousWorkingTime(this.BaselineFinish.Value, this.Schedule));
                }
                this.isDuringCoerceTiming = false;
            }
            if (this.BaselineFinish < this.BaselineStart)
            {
                this.BaselineFinish = this.BaselineStart;
            }
        }

        private static void OnMinStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("MinStart");
            CustomGanttChartItem.CoerceTiming();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.MinStart);
        }

        private static void OnMaxStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("MaxStart");
            CustomGanttChartItem.CoerceTiming();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.MaxStart);
        }

        private static void OnMinFinishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("MinFinish");
            CustomGanttChartItem.CoerceTiming();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.MinFinish);
        }

        private static void OnMaxFinishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("MaxFinish");
            CustomGanttChartItem.CoerceTiming();
            CustomGanttChartItem.OnManagedDependencyPropertyChanged(e, CustomGanttChartItem.MaxFinish);
        }

        internal void UpdateCost()
        {
            this.assignmentsCost = null;
            this.OnPropertyChanged("AssignmentsCost");
            this.UpdateExtraCost();
        }

        internal void UpdateExtraCost()
        {
            this.extraCost = null;
            this.OnPropertyChanged("ExtraCost");
            this.OnPropertyChanged("Cost");
            if (this.Parent != null)
            {
                this.Parent.UpdateExtraCost();
            }
        }

        private static void OnDisplayRowIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("DisplayRowIndex");
            CustomGanttChartItem.OnDisplayRowIndexChanged();
        }

        /// <summary>
        /// Raises the DisplayRowIndexChanged event.
        /// </summary>
        protected virtual void OnDisplayRowIndexChanged()
        {
            if (this.DisplayRowIndexChanged != null)
            {
                this.DisplayRowIndexChanged(this, EventArgs.Empty);
            }
        }

        private static void OnActualDisplayRowIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("ActualDisplayRowIndex");
            CustomGanttChartItem.OnPropertyChanged("IndexString");
            CustomGanttChartItem.OnActualDisplayRowIndexChanged();
            CustomGanttChartItem.UpdateWbsIndexString();
            CustomGanttChartItem.UpdateComputedItemTop();
            CustomGanttChartItem.UpdateBar();
            CustomGanttChartItem.UpdateDependencyLines();
            GanttChartView ganttChartView = CustomGanttChartItem.GanttChartView as GanttChartView;
            if (ganttChartView != null)
            {
                IEnumerable<CustomGanttChartItem> successors = ganttChartView.GetSuccessors(CustomGanttChartItem);
                if (successors != null)
                {
                    foreach (CustomGanttChartItem current in successors)
                    {
                        current.OnPropertyChanged("PredecessorsString");
                    }
                }
                if (ganttChartView.IsIndividualItemNonworkingTimeHighlighted)
                {
                    ganttChartView.OnItemNonworkingHighlightsSourceChanged();
                }
                if (ganttChartView.AreTaskInterruptionsHighlighted)
                {
                    ganttChartView.OnTaskInterruptionHighlightsSourceChanged();
                }
                ganttChartView.OnItemAdornersSourceChanged(false);
            }
        }

        /// <summary>
        /// Raises the ActualDisplayRowIndexChanged.
        /// </summary>
        protected virtual void OnActualDisplayRowIndexChanged()
        {
            if (this.ActualDisplayRowIndexChanged != null)
            {
                this.ActualDisplayRowIndexChanged(this, EventArgs.Empty);
            }
        }

        private static void OnDisplayLineIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("DisplayLineIndex");
            CustomGanttChartItem.OnDisplayLineIndexChanged();
            ScheduleChartItem scheduleChartItem = CustomGanttChartItem.ScheduleChartItem;
            if (CustomGanttChartItem.DisplayLineIndex.HasValue)
            {
                CustomGanttChartItem.ActualDisplayLineIndex = CustomGanttChartItem.DisplayLineIndex.Value;
                return;
            }
            if (scheduleChartItem != null)
            {
                scheduleChartItem.UpdateActualDisplayLineIndexes();
            }
        }

        /// <summary>
        /// Raises the DisplayLineIndexChanged event.
        /// </summary>
        protected virtual void OnDisplayLineIndexChanged()
        {
            if (this.DisplayLineIndexChanged != null)
            {
                this.DisplayLineIndexChanged(this, EventArgs.Empty);
            }
        }

        private static void OnActualDisplayLineIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            if (CustomGanttChartItem.ScheduleChartView != null && CustomGanttChartItem.ActualDisplayLineIndex >= CustomGanttChartItem.ScheduleChartView.MaxLineCountPerRow)
            {
                CustomGanttChartItem.ActualDisplayLineIndex = CustomGanttChartItem.ScheduleChartView.MaxLineCountPerRow - 1;
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("ActualDisplayLineIndex");
            CustomGanttChartItem.OnActualDisplayLineIndexChanged();
            CustomGanttChartItem.UpdateComputedItemTop();
            CustomGanttChartItem.UpdateBar();
            CustomGanttChartItem.UpdateDependencyLines();
            ScheduleChartItem scheduleChartItem = CustomGanttChartItem.ScheduleChartItem;
            if (scheduleChartItem != null)
            {
                scheduleChartItem.UpdateActualDisplayLineCount();
            }
        }

        /// <summary>
        /// Raises the ActualDisplayLineIndexChanged.
        /// </summary>
        protected virtual void OnActualDisplayLineIndexChanged()
        {
            if (this.ActualDisplayLineIndexChanged != null)
            {
                this.ActualDisplayLineIndexChanged(this, EventArgs.Empty);
            }
        }

        internal void UpdateComputedItemTop()
        {
            if (this.GanttChartView == null || double.IsNaN(this.GanttChartView.ItemHeight))
            {
                return;
            }
            GanttChartView ganttChartView = this.GanttChartView as GanttChartView;
            if (!ganttChartView.AreIndividualItemHeightsApplied)
            {
                this.ComputedItemTop = (double)this.ActualDisplayRowIndex * this.GanttChartView.ItemHeight;
                return;
            }
            if (ganttChartView.MainScheduleChartView == null)
            {
                if (ganttChartView.MainGanttChartView.ManagedItems == null)
                {
                    return;
                }
                double num = 0.0;
                int i;
                for (i = 0; i < this.ActualDisplayRowIndex; i++)
                {
                    IEnumerable<double> source = (from it in ganttChartView.MainGanttChartView.ManagedItems
                                                  where it.IsVisible && it.ActualDisplayRowIndex == i
                                                  select it).Select(delegate (CustomGanttChartItem it)
                                                  {
                                                      if (!double.IsNaN(it.ItemHeight))
                                                      {
                                                          return it.ItemHeight;
                                                      }
                                                      return this.GanttChartView.ItemHeight;
                                                  });
                    num += (source.Any<double>() ? source.Max() : this.GanttChartView.ItemHeight);
                }
                if (this.ScheduleChartItem != null)
                {
                    int line;
                    for (line = 0; line < this.ActualDisplayLineIndex; line++)
                    {
                        IEnumerable<CustomGanttChartItem> source2 = from i in this.ScheduleChartItem.CustomGanttChartItems
                                                              where i.IsVisible && i.ActualDisplayLineIndex == line
                                                              select i;
                        double arg_1B3_0 = num;
                        double arg_1B3_1;
                        if (!source2.Any<CustomGanttChartItem>())
                        {
                            arg_1B3_1 = 0.0;
                        }
                        else
                        {
                            arg_1B3_1 = source2.Max(delegate (CustomGanttChartItem i)
                            {
                                if (!double.IsNaN(i.ItemHeight))
                                {
                                    return i.ItemHeight;
                                }
                                return ganttChartView.ItemHeight;
                            });
                        }
                        num = arg_1B3_0 + arg_1B3_1;
                    }
                }
                this.ComputedItemTop = num;
                return;
            }
            else
            {
                if (ganttChartView.MainScheduleChartView.ManagedItems == null)
                {
                    return;
                }
                double num2 = 0.0;
                int i;
                for (i = 0; i < this.ActualDisplayRowIndex; i++)
                {
                    IEnumerable<double> source3 = (from it in ganttChartView.MainScheduleChartView.ManagedItems
                                                   where it.IsVisible && it.ActualDisplayRowIndex == i
                                                   select it).Select(delegate (ScheduleChartItem it)
                                                   {
                                                       if (!double.IsNaN(it.ItemHeight))
                                                       {
                                                           return it.ItemHeight;
                                                       }
                                                       return this.GanttChartView.ItemHeight;
                                                   });
                    num2 += (source3.Any<double>() ? source3.Max() : this.GanttChartView.ItemHeight);
                }
                if (this.ScheduleChartItem != null)
                {
                    int line;
                    for (line = 0; line < this.ActualDisplayLineIndex; line++)
                    {
                        IEnumerable<CustomGanttChartItem> source4 = from i in this.ScheduleChartItem.CustomGanttChartItems
                                                              where i.IsVisible && i.ActualDisplayLineIndex == line
                                                              select i;
                        double arg_324_0 = num2;
                        double arg_324_1;
                        if (!source4.Any<CustomGanttChartItem>())
                        {
                            arg_324_1 = ganttChartView.ItemHeight;
                        }
                        else
                        {
                            arg_324_1 = source4.Max(delegate (CustomGanttChartItem i)
                            {
                                if (!double.IsNaN(i.ItemHeight))
                                {
                                    return i.ItemHeight;
                                }
                                return ganttChartView.ItemHeight;
                            });
                        }
                        num2 = arg_324_0 + arg_324_1;
                    }
                }
                this.ComputedItemTop = num2;
                return;
            }
        }

        private static void OnItemHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("ItemHeight");
            CustomGanttChartItem.OnItemHeightChanged();
            GanttChartView ganttChartView = CustomGanttChartItem.GanttChartView as GanttChartView;
            if (ganttChartView != null)
            {
                ganttChartView.UpdateComputedHeight();
                ganttChartView.UpdateComputedItemTops();
                ganttChartView.UpdateBars();
                ganttChartView.UpdateVirtualizationVisibility();
                ganttChartView.OnItemAdornersSourceChanged(false);
            }
        }

        /// <summary>
        /// Raises the ItemHeightChanged event.
        /// </summary>
        protected virtual void OnItemHeightChanged()
        {
            if (this.ItemHeightChanged != null)
            {
                this.ItemHeightChanged(this, EventArgs.Empty);
            }
        }

        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("ItemBackground");
            GanttChartView ganttChartView = CustomGanttChartItem.GanttChartView as GanttChartView;
            if (ganttChartView != null && ganttChartView.AreIndividualItemAppearanceSettingsApplied)
            {
                ganttChartView.OnItemAdornersSourceChanged(false);
            }
        }

        private static void OnItemForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("ItemForeground");
            GanttChartView ganttChartView = CustomGanttChartItem.GanttChartView as GanttChartView;
            if (ganttChartView != null && ganttChartView.AreIndividualItemAppearanceSettingsApplied)
            {
                ganttChartView.OnItemAdornersSourceChanged(false);
            }
        }

        internal void UpdateComputedTaskTemplate()
        {
            if (this.GanttChartView == null)
            {
                return;
            }
            this.ComputedTaskTemplate = (this.HasChildren ? this.GanttChartView.SummaryTaskTemplate : ((!this.IsMilestone) ? this.GanttChartView.StandardTaskTemplate : this.GanttChartView.MilestoneTaskTemplate));
        }

        internal void UpdateBar()
        {
            if (this.GanttChartView == null)
            {
                return;
            }
            double barHeight = this.GanttChartView.BarHeight;
            double num = barHeight;
            double num2 = this.GanttChartView.GetPosition(this.Start);
            double num3 = this.GanttChartView.GetPosition(this.Finish) - num2;
            double val = this.GanttChartView.GetPosition(this.CompletedFinish) - num2;
            if (this.HasChildren || this.IsMilestone)
            {
                num2 -= barHeight / 2.0;
                num3 += barHeight;
                if (this.HasChildren)
                {
                    if (num3 < 2.0 * barHeight)
                    {
                        num3 = 2.0 * barHeight;
                    }
                    num += barHeight / 3.0;
                }
            }
            if (num3 > DlhSoft.Windows.Controls.GanttChartView.MaxSizeExtent && num2 < DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent)
            {
                num3 -= DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent - num2;
                num2 = DlhSoft.Windows.Controls.GanttChartView.MinPositionExtent;
            }
            num3 = Math.Min(DlhSoft.Windows.Controls.GanttChartView.MaxSizeExtent, num3);
            this.ComputedBarHeight = num;
            this.ComputedBarLeft = num2;
            this.ComputedBarWidth = Math.Max(4.0, num3);
            this.ComputedCompletedBarWidth = Math.Min(DlhSoft.Windows.Controls.GanttChartView.MaxSizeExtent, val);
            this.OnPropertyChanged("ComputedBaselineBarLeft");
            this.OnPropertyChanged("ComputedBaselineBarWidth");
            this.OnBarChanged();
        }

        /// <summary>
        /// Raises the <see cref="E:DlhSoft.Windows.Controls.CustomGanttChartItem.BarChanged" /> event.
        /// </summary>
        protected virtual void OnBarChanged()
        {
            if (this.BarChanged != null)
            {
                this.BarChanged(this, EventArgs.Empty);
            }
        }

        private static void OnBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("BarVisibility");
            ScheduleChartItem scheduleChartItem = CustomGanttChartItem.ScheduleChartItem;
            if (scheduleChartItem == null)
            {
                return;
            }
            scheduleChartItem.UpdateActualDisplayLineIndexes();
            scheduleChartItem.UpdateActualDisplayLineCount();
            scheduleChartItem.UpdateItemHeight();
        }

        private static void OnIsBarReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.OnPropertyChanged("IsBarReadOnly");
            CustomGanttChartItem.IsBarThumbHitTestVisible = !CustomGanttChartItem.IsBarReadOnly;
        }

        internal void UpdateDependencyLines()
        {
            if (this.GanttChartView == null || this.Predecessors == null)
            {
                return;
            }
            foreach (PredecessorItem current in this.Predecessors)
            {
                current.Update();
            }
        }

        private static void OnIsVirtuallyVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomGanttChartItem CustomGanttChartItem = d as CustomGanttChartItem;
            if (CustomGanttChartItem == null)
            {
                return;
            }
            CustomGanttChartItem.VirtualVisibility = (CustomGanttChartItem.IsVirtuallyVisible ? Visibility.Visible : Visibility.Collapsed);
        }

        /// <summary>
        /// Allows inheriters to submit custom item property changes to the undo queue.
        /// </summary>
        protected internal void AddPropertyChangeToUndoQueue(DependencyProperty property, object oldValue, object newValue)
        {
            if (this.isPropertyChangeNotificationPaused.Contains(property))
            {
                return;
            }
            GanttChartView ganttChartView = this.GanttChartView as GanttChartView;
            if (ganttChartView == null || !ganttChartView.IsUndoEnabled)
            {
                return;
            }
            ganttChartView.PushUndoOperation(new GanttChartView.ItemPropertyChange
            {
                Item = this,
                Property = property,
                OldValue = oldValue,
                NewValue = newValue
            });
        }

        /// <summary>
        /// Allows inheriters to pause propagation of property changes to the undo queue for a specific property.
        /// </summary>
        protected internal bool RequestPropertyChangeNotificationPause(DependencyProperty property)
        {
            if (this.isPropertyChangeNotificationPaused.Contains(property))
            {
                return false;
            }
            this.isPropertyChangeNotificationPaused.Add(property);
            return true;
        }

        /// <summary>
        /// Allows inheriters to resume propagation of property changes to the undo queue for a specific property.
        /// </summary>
        protected internal void ResumePropertyChangedNotifications(DependencyProperty property)
        {
            this.isPropertyChangeNotificationPaused.Remove(property);
        }

        /// <summary>
        /// Raises the ManagedDependencyPropertyChanged event.
        /// </summary>
        public override void OnManagedDependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            this.OnManagedDependencyPropertyChanged(e, e.NewValue);
        }

        private void OnManagedDependencyPropertyChanged(DependencyPropertyChangedEventArgs e, object actualNewValue)
        {
            base.OnManagedDependencyPropertyChanged(e);
            this.AddPropertyChangeToUndoQueue(e.Property, e.OldValue, actualNewValue);
        }

        object ICustomGanttChartItem.get_Content()
        {
            return base.Content;
        }

        int ICustomGanttChartItem.get_Indentation()
        {
            return base.Indentation;
        }
    }
}
