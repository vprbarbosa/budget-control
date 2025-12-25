namespace BudgetControl.Domain.Entities
{
    public sealed class FundingSource
    {
        public Guid Id { get; }
        public string Name { get; }

        public FundingSource(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Funding source name is required.");

            Id = id;
            Name = name;
        }
    }
}
