module EquipmentMapper

open EquipmentFormatter.Fsharp.External

type Operation =
    | Exchange of other: string
    | Replacement of part: string * by: string
    | Supplement of suffix: string

let proceed (label: string) operation =
    match operation with
    | Exchange other -> other
    | Replacement (part, by) -> label.Replace(part, by)
    | Supplement suffix -> label + suffix

let tryMapToExchange (variation: Variation) =
    match variation.Schema with
    | 7407  -> Some (Exchange "Nombre de cylindres")
    | 15304 -> Some (Exchange "Puissance (ch)")
    | 15305 -> Some (Exchange "Régime de puissance maxi (tr/mn)")
    | _ -> None

let tryMapToReplacementBySchema (variation: Variation) =
    match variation.Schema with
    | 23502 | 24002 -> Some (Replacement("an(s) / km", ": durée (ans)"))
    | 23503 | 24003 -> Some (Replacement("an(s) / km", ": kilométrage"))
    | 7403 -> Some (Replacement("litres / cm3", "litres"))
    | 7402 -> Some (Replacement("litres / cm3", "cm3"))
    | _ -> None

let tryMapToReplacementByLocation (variation: Variation) =
    match variation.Schema, variation.Location with
    | 17811, 'D' | 17818, 'D' -> Some (Replacement("conducteur / passager", "conducteur"))
    | 17811, 'P' | 17818, 'P' -> Some (Replacement("conducteur / passager", "passager"))
    | 53405, 'D' -> Some (Replacement("recharge (rapide) A / V / h", "recharge : ampérage (A)"))
    | 53405, 'F' -> Some (Replacement("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)"))
    | 53404, 'D' -> Some (Replacement("recharge (rapide) A / V / h", "recharge : voltage (V)"))
    | 53404, 'F' -> Some (Replacement("recharge (rapide) A / V / h", "recharge rapide : voltage (V)"))
    | 53403, 'D' -> Some (Replacement("recharge (rapide) A / V / h", "recharge : durée (heures)"))
    | 53403, 'F' -> Some (Replacement("recharge (rapide) A / V / h", "recharge rapide : durée (heures)"))
    | 23301, 'F' -> Some (Replacement("AV / AR", "AV"))
    | 23301, 'R' -> Some (Replacement("AV / AR", "AR"))
    | _ -> None

let tryMapToSupplement (variation: Variation) =
    match variation.Schema with
    | 14103 -> Some (Supplement " : largeur")
    | 14104 -> Some (Supplement " : profil")
    | _ -> None

let (<||>) fa fb x =
    match fa x with
    | Some v -> Some v
    | None -> fb x

let mapToOperation =
    tryMapToExchange
    <||> tryMapToReplacementBySchema
    <||> tryMapToReplacementByLocation
    <||> tryMapToSupplement

let computeLabel (equipment: Equipment) (variation: Variation) =
    let label = equipment.Label.Trim()

    variation
    |> mapToOperation
    |> Option.map (proceed label)
    |> (fun x -> defaultArg x label)
