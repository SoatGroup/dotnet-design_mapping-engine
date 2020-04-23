namespace EquipmentFormatter.Fsharp

open EquipmentFormatter.Fsharp.External

module Main =
  let computeLabel (equipment: Equipment) (variation: Variation) =
    let label = equipment.Label.Trim()
    match variation.Schema, variation.Location with
    | 7407, _ -> "Nombre de cylindres"
    | 15304, _ -> "Puissance (ch)"
    | 15305, _ -> "Régime de puissance maxi (tr/mn)"
    | 23502, _ | 24002, _ -> label.Replace("an(s) / km", ": durée (ans)");
    | 23503, _ | 24003, _ -> label.Replace("an(s) / km", ": kilométrage");
    | 7403, _ -> label.Replace("litres / cm3", "litres");
    | 7402, _ -> label.Replace("litres / cm3", "cm3")
    | 23301, 'F' -> label.Replace("AV / AR", "AV");
    | 23301, 'R' -> label.Replace("AV / AR", "AR");
    | 17811, 'D' | 17818, 'D' -> label.Replace("conducteur / passager", "conducteur");
    | 17811, 'P' | 17818, 'P' -> label.Replace("conducteur / passager", "passager")
    | 53405, 'D' -> label.Replace("recharge (rapide) A / V / h", "recharge : ampérage (A)");
    | 53405, 'F' -> label.Replace("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)");
    | 53404, 'D' -> label.Replace("recharge (rapide) A / V / h", "recharge : voltage (V)");
    | 53404, 'F' -> label.Replace("recharge (rapide) A / V / h", "recharge rapide : voltage (V)");
    | 53403, 'D' -> label.Replace("recharge (rapide) A / V / h", "recharge : durée (heures)");
    | 53403, 'F' -> label.Replace("recharge (rapide) A / V / h", "recharge rapide : durée (heures)");
    | 14103, _ -> label + " : largeur"
    | 14104, _ -> label + " : profil"
    | _ -> label

