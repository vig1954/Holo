using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.WorkspacePanel
{
    public abstract class WorkspacePanelGroupableItemControllerBase : WorkspacePanelItemControllerBase
    {
        private FolderWorkspacePanelItemController _parent;

        public FolderWorkspacePanelItemController Parent
        {
            get => _parent;
            set
            {
                _parent = value;

                var marginLeft = _parent == null ? 0 : 20;
                View.Margin = new Padding(marginLeft, 0, 0, 0);
            }
        }

        public bool Visible => Parent?.Expanded ?? true;

        protected WorkspacePanelGroupableItemControllerBase(WorkspacePanelItem view) : base(view)
        {
            view.IsShowInEditorButtonVisible = false;
        }
    }
}