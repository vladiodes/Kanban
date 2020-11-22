using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class TaskViewModel
    {
        //contains all properties to show task details

        private TaskModel _task;

        public string Title{get { return _task.Title; } }
        public string Description { get { return _task.Description; } }
        public int Id { get { return _task.Id; } }
        public DateTime CreationTime { get { return _task.CreationTime; } }
        public DateTime DueDate { get { return _task.DueDate; } }
        public string Assignee { get { return _task.Assignee; } }

        public TaskViewModel(TaskModel _task)
        {
            this._task = _task;
        }
    }
}
