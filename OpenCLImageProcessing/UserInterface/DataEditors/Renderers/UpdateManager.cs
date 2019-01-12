using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.Renderers
{
    public static class UpdateManager
    {
        private static object _currentInvokingObject;
        private static bool _isUnlockingInProgress = false;
        private static object _lockObject = new object();
        private static List<UpdateInvokationInfo> _updateInvokationInfos = new List<UpdateInvokationInfo>();
        private static List<UpdateInvokationInfo> _updateInvokationInfos2 = new List<UpdateInvokationInfo>(); // todo: совсем скатился. Нужно переосмыслить этот класс полностью.
        private static bool _locked;

        public static bool IsLockedFor(object obj)
        {
            var free = !_locked || obj == _currentInvokingObject;
            return !free;
        }

        public static void Lock()
        {
            _locked = true;
        }

        public static void Unlock()
        {
            lock (_lockObject)
            {
                _isUnlockingInProgress = true;

                foreach (var updateInvokationInfo in _updateInvokationInfos)
                {
                    _currentInvokingObject = updateInvokationInfo.Object;

                    updateInvokationInfo.UpdateAction.Invoke();

                    _currentInvokingObject = null;
                }

                _updateInvokationInfos.Clear();

                _locked = false;
                _isUnlockingInProgress = false;

                foreach (var updateInvokationInfo in _updateInvokationInfos2)
                {
                    updateInvokationInfo.UpdateAction.Invoke();
                }

                _updateInvokationInfos2.Clear();
            }
        }

        public static void DelayUpdateUntilUnlocked(object obj, Action updateAction)
        {
            lock (_lockObject)
            {
                if (!_locked || _currentInvokingObject == obj)
                {
                    updateAction.Invoke();
                    return;
                }

                if (_updateInvokationInfos.Any(i => i.Object == obj))
                    return;

                if (!_isUnlockingInProgress)
                {
                    _updateInvokationInfos.Add(new UpdateInvokationInfo
                    {
                        Object = obj,
                        UpdateAction = updateAction
                    });
                    return;
                }

                if (_updateInvokationInfos2.Any(i => i.Object == obj))
                    return;

                _updateInvokationInfos2.Add(new UpdateInvokationInfo
                {
                    Object = obj,
                    UpdateAction = updateAction
                });
            }
        }

        private class UpdateInvokationInfo
        {
            public object Object { get; set; }
            public Action UpdateAction { get; set; }
        }
    }
}