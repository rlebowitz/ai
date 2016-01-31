using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMLbot.Utils
{
    public static class TaskExtensions
    {
        /// <summary>
        /// A Task extension designed to timeout a given task after the specified number of milliseconds.
        /// </summary>
        /// <param name="task">The specified task.</param>
        /// <param name="millisecondsTimeout">The specified number of milliseconds to wait till timing out.</param>
        public static async Task TimeoutAfter(this Task task, int millisecondsTimeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
                await task;
            else
                throw new TimeoutException();
        }
    }
}
