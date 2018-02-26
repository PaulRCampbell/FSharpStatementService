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
    | Multiple


type StatementTemplate =
    | Unigro
    | GroCaptial

type StatementItem =
    { Name : string
      Type : string }

type TemplateItem = 
    {   StatementItem : StatementItem
        StatementTemplate : StatementTemplate
        DisplayOrder : int }

 type TemplateItemFormulas =
    {   Children: TemplateItem list
        Parent: TemplateItem 
        Operation: FormulaOperation
        OperationOrder: int }

type StatementValues = 
    {   Item: TemplateItem 
        Amount: int }


let livestock = { Name = "Livestock"; Type = "Balancesheet"}
let crop = { Name = "Crop"; Type = "Balancesheet"}
let currentAssets = { Name = "CurrentAssets"; Type = "Balancesheet"}
let totalAssets = { Name = "TotalAssets"; Type = "Balancesheet"}


let u_livestockTemplateItem = { StatementItem = livestock; StatementTemplate = Unigro; DisplayOrder = 1 }
let u_cropTemplateItem = { StatementItem = crop; StatementTemplate = Unigro; DisplayOrder = 2 }
let u_currentAssetsTemplateItem = { StatementItem = currentAssets; StatementTemplate = Unigro; DisplayOrder = 3 }
let u_totalAssetsTemplateItem = { StatementItem = totalAssets; StatementTemplate = Unigro; DisplayOrder = 4 }

let cropValue = { Item = u_cropTemplateItem; Amount = 5000 }
let livestockValue = { Item = u_cropTemplateItem; Amount = 15000 }


let nonCurrentAssetsFormula = { Parent = u_currentAssetsTemplateItem;
                                Children = [ u_livestockTemplateItem; u_cropTemplateItem; ];
                                Operation = Add;
                                OperationOrder = 1 }

let totalAssetsFormula = { Parent = u_totalAssetsTemplateItem;
                                Children = [ u_currentAssetsTemplateItem; ];
                                Operation = Add;
                                OperationOrder = 1 }



let statementValues = [ cropValue; livestockValue ]
    

    


let calcItemValue FormulaOperation prev curr =
                    match FormulaOperation with
                    | Add -> prev + curr
                    | Subtract -> prev - curr
                    | Multiply -> prev * curr
                    | Divide -> prev / curr