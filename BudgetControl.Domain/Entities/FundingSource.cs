using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.Entities
{
    public sealed class FundingSource : Entity
    {
        public string Name { get; }

        public FundingSource(string name, Guid? id = null)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Funding source name is required.");

            Name = name;
        }
    }
}
