using EquipmentFormatter.External;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EquipmentFormatter.Models
{
  public sealed class RuleChain
  {
    private Rule Rule { get; }

    private Option<RuleChain> Next { get; }

    public RuleChain(Rule rule, RuleChain next)
    {
      Rule = rule;
      Next = Optional(next);
    }

    public IOperation SelectOperationAdaptedTo(Variation variation) =>
      Rule.IsSatisfiedBy(variation)
        ? (IOperation) Rule
        : Next.Match(
            Some: chain => chain.SelectOperationAdaptedTo(variation),
            None: () => Rule.Default(label => label));
  }
}