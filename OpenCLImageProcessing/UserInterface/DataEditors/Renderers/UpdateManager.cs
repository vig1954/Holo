using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.Renderers
{
    public static class UpdateManager
    {
        private static List<UpdateInvokationInfo> _updateInvokationInfos = new List<UpdateInvokationInfo>();
        public static bool Locked { get; private set; }

        public static void Lock()
        {
            Locked = true;
        }

        public static void Unlock()
        {
            Locked = false;

            foreach (var updateInvokationInfo in _updateInvokationInfos)
            {
                updateInvokationInfo.UpdateAction.Invoke();
            }

            _updateInvokationInfos.Clear();
        }

        public static void DelayUpdateUntilUnlocked(object obj, Action updateAction)
        {
            if (!Locked)
            {
                updateAction.Invoke();
                return;
            }

            if (_updateInvokationInfos.Any(i => i.Object == obj))
                return;

            _updateInvokationInfos.Add(new UpdateInvokationInfo
            {
                Object = obj,
                UpdateAction = updateAction
                    
            });
        }

        private class UpdateInvokationInfo
        {
            public object Object { get; set; }
            public Action UpdateAction { get; set; }
        }
    }
}
