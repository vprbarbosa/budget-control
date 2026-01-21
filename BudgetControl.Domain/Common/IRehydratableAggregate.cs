using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Domain.Common
{
    public interface IRehydratableAggregate
    {
        void AfterRehydration();
    }
}
