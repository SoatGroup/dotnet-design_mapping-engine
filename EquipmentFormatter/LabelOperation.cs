using System;

namespace EquipmentFormatter
{
  public static class LabelOperation
  {
    public static Func<string, string> ExchangeWith(string value) =>
      _ => value;

    public static Func<string, string> Replace(string part, string by) =>
      label => label.Replace(part, by);

    public static Func<string, string> Append(string value) =>
      label => label + value;
  }
}