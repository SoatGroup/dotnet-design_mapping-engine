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

    public string Apply(Variation variation, string label) =>
      Rule.IsSatisfiedBy(variation)
        ? Rule.ApplyOn(label)
        : Next.Match(
            Some: rule => rule.Apply(variation, label),
            None: () => label);
  }
}