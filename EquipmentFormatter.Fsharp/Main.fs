module EquipmentMapper

open EquipmentFormatter.Fsharp.External

let computeLabelTotallyNew (variation: Variation) =
    match variation.Schema with
    | 7407 -> Some "Nombre de cylindres"
    | 15304 -> Some "Puissance (ch)"
    | 15305 -> Some "Régime de puissance maxi (tr/mn)"
    | _ -> None

let computeLabelWithReplacementBySchema (label: string) (variation: Variation) =
    match variation.Schema with
    | 23502
    | 24002 -> Some(label.Replace("an(s) / km", ": durée (ans)"))
    | 23503
    | 24003 -> Some(label.Replace("an(s) / km", ": kilométrage"))
    | 7403 -> Some(label.Replace("litres / cm3", "litres"))
    | 7402 -> Some(label.Replace("litres / cm3", "cm3"))
    | _ -> None

let computeLabelWithReplacementByLocation (label: string) (variation: Variation) =
    match variation.Schema, variation.Location with
    | 23301, 'F' -> Some(label.Replace("AV / AR", "AV"))
    | 23301, 'R' -> Some(label.Replace("AV / AR", "AR"))
    | 17811, 'D'
    | 17818, 'D' -> Some(label.Replace("conducteur / passager", "conducteur"))
    | 17811, 'P'
    | 17818, 'P' -> Some(label.Replace("conducteur / passager", "passager"))
    | 53405, 'D' -> Some(label.Replace("recharge (rapide) A / V / h", "recharge : ampérage (A)"))
    | 53405, 'F' -> Some(label.Replace("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)"))
    | 53404, 'D' -> Some(label.Replace("recharge (rapide) A / V / h", "recharge : voltage (V)"))
    | 53404, 'F' -> Some(label.Replace("recharge (rapide) A / V / h", "recharge rapide : voltage (V)"))
    | 53403, 'D' -> Some(label.Replace("recharge (rapide) A / V / h", "recharge : durée (heures)"))
    | 53403, 'F' -> Some(label.Replace("recharge (rapide) A / V / h", "recharge rapide : durée (heures)"))
    | _ -> None

let computeLabelWithComplementaryInfo (label: string) (variation: Variation) =
    match variation.Schema with
    | 14103 -> Some(label + " : largeur")
    | 14104 -> Some(label + " : profil")
    | _ -> None

let (<||>) fa fb =
    fun x ->
        match fa x with
        | Some v -> Some v
        | None -> fb x

let computeLabel (equipment: Equipment) (variation: Variation) =
    let label = equipment.Label.Trim()
    let computeLabel' =
        computeLabelTotallyNew
        <||> computeLabelWithReplacementBySchema label
        <||> computeLabelWithReplacementByLocation label
        <||> computeLabelWithComplementaryInfo label
    variation
    |> computeLabel'
    |> (fun x -> defaultArg x label)
