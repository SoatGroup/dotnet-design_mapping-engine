using EquipmentFormatter.External;

namespace EquipmentFormatter.Models
{
  public sealed class RuleChain
  {
    private Rule Rule { get; }

    private RuleChain Next { get; }

    public RuleChain(Rule rule, RuleChain next)
    {
      Rule = rule;
      Next = next;
    }

    public string Apply(Variation variation, string label) =>
      Rule.IsSatisfiedBy(variation)
        ? Rule.ApplyOn(label)
        : Next.Apply(variation, label);
  }
}