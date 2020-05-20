using LanguageExt;
using static LanguageExt.Prelude;

namespace EquipmentFormatter.Models
{
  public sealed class RuleChain<TCriteria, TData>
  {
    private Rule<TCriteria, TData> Rule { get; }

    private Option<RuleChain<TCriteria, TData>> Next { get; }

    public RuleChain(Rule<TCriteria, TData> rule, RuleChain<TCriteria, TData> next)
    {
      Rule = rule;
      Next = Optional(next);
    }

    public IOperation<TData> SelectOperationAdaptedTo(TCriteria criteria) =>
      Rule.IsSatisfiedBy(criteria)
        ? (IOperation<TData>) Rule
        : Next.Match(
            Some: chain => chain.SelectOperationAdaptedTo(criteria),
            None: () => Rule<TCriteria, TData>.Default(label => label));
  }
}