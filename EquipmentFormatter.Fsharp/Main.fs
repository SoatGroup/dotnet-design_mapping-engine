module EquipmentMapper

open EquipmentFormatter.Fsharp.External

// Modélisation du domaine par le typage des données et des fonctions
type Operation =
    | Exchange of other: string
    | Replacement of part: string * by: string
    | Supplement of suffix: string

type Label = Label of string

type ComputeLabel = Equipment -> Variation -> Label
type MapToOperation = Variation -> Operation option
type Proceed = Label -> Operation -> Label

// Implémentation
let proceed : Proceed = fun label operation ->
    let (Label content) = label
    let newContent =
        match operation with
        | Exchange other -> other
        | Replacement (part, by) -> content.Replace(part, by)
        | Supplement suffix -> content + suffix
    Label newContent

let mapToExchange : MapToOperation = fun variation ->
    match variation.Schema with
    | 7407  -> Some (Exchange "Nombre de cylindres")
    | 15304 -> Some (Exchange "Puissance (ch)")
    | 15305 -> Some (Exchange "Régime de puissance maxi (tr/mn)")
    | _ -> None

let mapToReplacementBySchema : MapToOperation = fun variation ->
    match variation.Schema with
    | 23502 | 24002 -> Some (Replacement("an(s) / km", ": durée (ans)"))
    | 23503 | 24003 -> Some (Replacement("an(s) / km", ": kilométrage"))
    | 7403 -> Some (Replacement("litres / cm3", "litres"))
    | 7402 -> Some (Replacement("litres / cm3", "cm3"))
    | _ -> None

let mapToReplacementByLocation : MapToOperation = fun variation ->
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

let mapToSupplement : MapToOperation = fun variation ->
    match variation.Schema with
    | 14103 -> Some (Supplement " : largeur")
    | 14104 -> Some (Supplement " : profil")
    | _ -> None

let (<||>) fa fb x =
    match fa x with
    | Some v -> Some v
    | None -> fb x

let mapToOperation : MapToOperation =
    mapToExchange
    <||> mapToReplacementBySchema
    <||> mapToReplacementByLocation
    <||> mapToSupplement

let computeLabel : ComputeLabel = fun equipment variation ->
    let label = Label (equipment.Label.Trim())
    let result = variation |> mapToOperation |> Option.map (proceed label)
    match result with
    | Some x -> x
    | None -> label
