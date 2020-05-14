using EquipmentFormatter.External;

namespace EquipmentFormatter.Models
{
  public class RuleChain
  {
    public static RuleChain End(string label) =>
      new RuleChain(new Rule(_ => true, _ => label), null);

    private Rule Current { get; }
    private RuleChain Next { get; }

    public RuleChain(Rule current, RuleChain next)
    {
      Current = current;
      Next    = next;
    }

    public string Apply(Variation variation, string label) =>
      Current.IsSatisfiedBy(variation)
        ? Current.ApplyOn(label)
        : Next.Apply(variation, label);
  }
}