module EquipmentMapper

open EquipmentFormatter.Fsharp.External

type Operation =
    | Exchange of other: string
    | Replacement of part: string * by: string
    | Supplement of suffix: string
    | NoOp

let proceed (label: string) operation =
    match operation with
    | Exchange other -> other
    | Replacement(part, by) -> label.Replace(part, by)
    | Supplement suffix -> label + suffix
    | NoOp -> label

let tryMapToExchange (variation: Variation) =
    match variation.Schema with
    | 7407  -> Exchange "Nombre de cylindres"
    | 15304 -> Exchange "Puissance (ch)"
    | 15305 -> Exchange "Régime de puissance maxi (tr/mn)"
    | _ -> NoOp

let tryMapToReplacementBySchema (variation: Variation) =
    match variation.Schema with
    | 23502 | 24002 -> Replacement("an(s) / km", ": durée (ans)")
    | 23503 | 24003 -> Replacement("an(s) / km", ": kilométrage")
    | 7403 -> Replacement("litres / cm3", "litres")
    | 7402 -> Replacement("litres / cm3", "cm3")
    | _ -> NoOp

let tryMapToReplacementByLocation (variation: Variation) =
    match variation.Schema, variation.Location with
    | 17811, 'D' | 17818, 'D' -> Replacement("conducteur / passager", "conducteur")
    | 17811, 'P' | 17818, 'P' -> Replacement("conducteur / passager", "passager")
    | 53405, 'D' -> Replacement("recharge (rapide) A / V / h", "recharge : ampérage (A)")
    | 53405, 'F' -> Replacement("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)")
    | 53404, 'D' -> Replacement("recharge (rapide) A / V / h", "recharge : voltage (V)")
    | 53404, 'F' -> Replacement("recharge (rapide) A / V / h", "recharge rapide : voltage (V)")
    | 53403, 'D' -> Replacement("recharge (rapide) A / V / h", "recharge : durée (heures)")
    | 53403, 'F' -> Replacement("recharge (rapide) A / V / h", "recharge rapide : durée (heures)")
    | 23301, 'F' -> Replacement("AV / AR", "AV")
    | 23301, 'R' -> Replacement("AV / AR", "AR")
    | _ -> NoOp

let tryMapToSupplement (variation: Variation) =
    match variation.Schema with
    | 14103 -> Supplement " : largeur"
    | 14104 -> Supplement " : profil"
    | _ -> NoOp

let (<||>) fa fb x =
    match fa x with
    | NoOp -> fb x
    | op -> op

let mapToOperation =
    tryMapToExchange
    <||> tryMapToReplacementBySchema
    <||> tryMapToReplacementByLocation
    <||> tryMapToSupplement

let computeLabel (equipment: Equipment) (variation: Variation) =
    variation
    |> mapToOperation
    |> proceed (equipment.Label.Trim())
