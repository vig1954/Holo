using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Debounce
    {
        private static Dictionary<string, Task> _scheduledTasks = new Dictionary<string, Task>();

        public static void Invoke(string key, Action action, TimeSpan delay)
        {
            // todo:
            action();
        }
    }
}
