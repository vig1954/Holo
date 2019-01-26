using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.WorkspacePanel
{
    public abstract class WorkspacePanelItemControllerBase
    {
        public WorkspacePanelItem View { get; }
        
        public virtual void UpdateView()
        {
        }
        protected WorkspacePanelItemControllerBase(WorkspacePanelItem view)
        {
            View = view;
        }
    }
}