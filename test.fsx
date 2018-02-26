
type FOperation =
    | Add
    | Subtract
    | Multiply
    | Divide
    | SurplusShortfall




let calcSurplusShortfall current next =
    current + next + 99

let calc(operation, current, next) = 
    match operation with
    | Add -> current + next
    | Subtract -> current - next
    | Multiply -> current * next
    | Divide -> current / next
    | SurplusShortfall -> calcSurplusShortfall current next


calc (SurplusShortfall, 10, 20)
