using System;

namespace EquipmentFormatter
{
  public static class Operation
  {
    public static Func<string, string> Exchange(string value) =>
      _ => value;

    public static Func<string, string> Replace(string part, string by) =>
      label => label.Replace(part, by);

    public static Func<string, string> Supplement(string value) =>
      label => label + value;
  }
}