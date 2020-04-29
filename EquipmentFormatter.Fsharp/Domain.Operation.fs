module Vehicle.Domain.Operation

open Vehicle.Domain.Ports

type Operation =
    | Exchange of other: string
    | Replacement of part: string * by: string
    | Supplement of suffix: string

let proceed label operation =
    let (Label content) = label
    let newContent =
        match operation with
        | Exchange other -> other
        | Replacement(part, by) -> content.Replace(part, by)
        | Supplement suffix -> content + suffix
    Label newContent
