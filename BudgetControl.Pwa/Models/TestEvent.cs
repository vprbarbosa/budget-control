using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Models
{
    public sealed record TestEvent(string Message, DateTimeOffset At);
}
