#load "FinancialStatementLib.fs"
open System.Security.Policy
open System
open System.Numerics
open System.Numerics
open FinancialStatementLib

// Define your library scripting code here/usr/bin/ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"

type FormulaOperation =
    | Add
    | Subtract
    | Divide
    | Multiply
    | SurplusShortfall


type StatementTemplate =
    | BobTheBuilders
    | MrBens

type StatementItem =
    { Name : string
      Type : string }

type TemplateItem = 
    {   StatementItem : StatementItem
        StatementTemplate : StatementTemplate
        DisplayOrder : int }

 type TemplateItemFormula =
    {   ChildNode: TemplateItem
        ParentNode: TemplateItem 
        Operation: FormulaOperation
        OperationOrder: int }

type StatementValue = 
    {   Item: TemplateItem 
        Amount: decimal }

type StatementValueOperation =
    {   Name: string
        Operation: FormulaOperation
        OperationOrder: int
        Amount: decimal}


let ppe = { Name = "Property Plant and Equipment"; Type = "Balancesheet"}
let longTermInvestments = { Name = "Long Term Investments"; Type = "Balancesheet"}
let intangibleAssets = { Name = "Intangible Assets"; Type = "Balancesheet"}

let nonCurrentAssets = { Name = "Non Current Assets"; Type = "Balancesheet"}


let deferredTax = { Name = "Deferred Tax"; Type = "Balancesheet"}
let inventories = { Name = "Inventories"; Type = "Balancesheet"}
let cash = { Name = "Cash"; Type = "Balancesheet"}

let currentAssets = { Name = "Current Assets"; Type = "Balancesheet"}

let totalAssets = { Name = "Total Assets"; Type = "Balancesheet"}


let u_propertyTemplateItem = { StatementItem = ppe; StatementTemplate = BobTheBuilders; DisplayOrder = 1 }
let u_longTermInvestmentsTemplateItem = { StatementItem = longTermInvestments; StatementTemplate = BobTheBuilders; DisplayOrder = 2 }
let u_intangibleAssetsTemplateItem = { StatementItem = intangibleAssets; StatementTemplate = BobTheBuilders; DisplayOrder = 3 }
let u_nonCurrentAssetsTemplateItem = { StatementItem = nonCurrentAssets; StatementTemplate = BobTheBuilders; DisplayOrder = 4 }


let u_defferedTaxTemplateItem = { StatementItem = deferredTax; StatementTemplate = BobTheBuilders; DisplayOrder = 5 }
let u_inventoriesTemplateItem = { StatementItem = inventories; StatementTemplate = BobTheBuilders; DisplayOrder = 6 }
let u_cashTemplateItem = { StatementItem = cash; StatementTemplate = BobTheBuilders; DisplayOrder = 7 }
let u_currentAssetsTemplateItem = { StatementItem = currentAssets; StatementTemplate = BobTheBuilders; DisplayOrder = 8 }


let u_totalAssetsTemplateItem = { StatementItem = totalAssets; StatementTemplate = BobTheBuilders; DisplayOrder = 9 }




let nonCurrentAssetsFormula = [{ ParentNode = u_nonCurrentAssetsTemplateItem;
                                ChildNode =  u_propertyTemplateItem;
                                Operation = Add;
                                OperationOrder = 1 };

                                { ParentNode = u_nonCurrentAssetsTemplateItem;
                                ChildNode =  u_longTermInvestmentsTemplateItem;
                                Operation = Add;
                                OperationOrder = 2 };
                                
                                { ParentNode = u_nonCurrentAssetsTemplateItem;
                                ChildNode =  u_intangibleAssetsTemplateItem;
                                Operation = Subtract;
                                OperationOrder = 3 };]

let currentAssetsFormula = [{ ParentNode = u_currentAssetsTemplateItem;
                                ChildNode =  u_inventoriesTemplateItem;
                                Operation = Add;
                                OperationOrder = 1 };

                                { ParentNode = u_currentAssetsTemplateItem;
                                ChildNode =  u_cashTemplateItem;
                                Operation = Add;
                                OperationOrder = 2 };
                                
                                { ParentNode = u_currentAssetsTemplateItem;
                                ChildNode =  u_defferedTaxTemplateItem;
                                Operation = Subtract;
                                OperationOrder = 3 };]

                                


let totalAssetsFormula = [{ ParentNode = u_totalAssetsTemplateItem;
                                ChildNode = u_currentAssetsTemplateItem;
                                Operation = Add;
                                OperationOrder = 1 };
                                { ParentNode = u_totalAssetsTemplateItem;
                                ChildNode = u_nonCurrentAssetsTemplateItem;
                                Operation = Add;
                                OperationOrder = 2 }]



let propValue = { Item = u_propertyTemplateItem; Amount = 5000m }
let longTermInvestmentValue = { Item = u_longTermInvestmentsTemplateItem; Amount = 1000m }
let intagibleAssetsValue = { Item = u_intangibleAssetsTemplateItem; Amount = 1000m }

let statementValues = [ propValue; longTermInvestmentValue; intagibleAssetsValue ]


let findStatementValue (item:TemplateItemFormula, statementValueList: list<StatementValue>) =
        printfn "item to find %s" item.ChildNode.StatementItem.Name
        let res = statementValueList
                |> List.find(fun x -> x.Item.StatementItem.Name = item.ChildNode.StatementItem.Name)

        { Amount = res.Amount; OperationOrder = item.OperationOrder; Operation = item.Operation; Name = res.Item.StatementItem.Name }

let calcSurplusShortfall current next =
    current + next + 99m

let calc(operation, current:decimal, next:decimal) = 
    match operation with
    | Add -> current + next
    | Subtract -> current - next
    | Multiply -> current * next
    | Divide -> current / next
    | SurplusShortfall -> calcSurplusShortfall current next

let calcStatementValues (totalAssetsFormula:list<TemplateItemFormula>, statementValueList:list<StatementValue>) =
    totalAssetsFormula
    |> List.map(fun elem -> findStatementValue (elem, statementValueList))
    |> List.sortBy(fun elem -> elem.OperationOrder)
    |> List.fold(fun acc elem -> calc(elem.Operation, acc, elem.Amount)) 0m



calcStatementValues (nonCurrentAssetsFormula, statementValues)
//calc (SurplusShortfall, 10m, 20m)
