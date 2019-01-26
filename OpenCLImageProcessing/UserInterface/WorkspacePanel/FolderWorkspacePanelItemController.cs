using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.WorkspacePanel
{
    public class FolderWorkspacePanelItemController : WorkspacePanelItemControllerBase
    {
        private bool _expanded;
        public bool Expanded
        {
            get => _expanded;
            set
            {
                _expanded = value;
                ToggleImage();
            }
        }

        public string Title => View.Title;

        public FolderWorkspacePanelItemController(string title, WorkspacePanelItem view) : base(view)
        {
            view.SetTitle(title);
            view.SetInfo("");
            ToggleImage();
        }

        private void ToggleImage()
        {
            View.SetImage(_expanded ? Properties.Resources.icons8_opened_folder_30 : Properties.Resources.icons8_folder_30);
        }
    }
}
