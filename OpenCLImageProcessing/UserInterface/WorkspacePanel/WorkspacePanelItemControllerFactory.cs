using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processing;
using UserInterface.DataProcessorViews;

namespace UserInterface.WorkspacePanel
{
    public static class WorkspacePanelItemControllerFactory
    {
        public static WorkspacePanelGroupableItemControllerBase Get(object data, WorkspacePanelItem view)
        {
            if (data is ImageHandler imageHandler)
                return new ImageWorkspacePanelItemController(imageHandler, view);
            if (data is IDataProcessorView dataProcessorView)
                return new DataProcessorViewWorkspacePanelItemController(dataProcessorView, view);

            throw new NotSupportedException($"{data.GetType()} is not supported by {nameof(WorkspacePanelItemControllerFactory)}");

        }
    }
}
