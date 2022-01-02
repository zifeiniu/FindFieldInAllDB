using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFieldInAllDB
{
    internal class LogHelper
    {
        public static Action<string> LogAction;

        public static void AddLog(string log) 
        {
            LogAction?.Invoke(log);
        }
    }
}
