namespace EquipmentFormatter.Fsharp

module External =
    type Equipment = { Schema: int; Label: string; Variations: Variation[] }
    and  Variation = { Schema: int; Location: char; Value: string }
