using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.Entities
{
    public sealed class FundingSource : Entity
    {
        public string Name { get; private set; }

        private FundingSource(string name, Guid? id = null)
            : base(id)
        {
            Name = name;
        }

        public static FundingSource Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            return new FundingSource(name);
        }
    }
}
