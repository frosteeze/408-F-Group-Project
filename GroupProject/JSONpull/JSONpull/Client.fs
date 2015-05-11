namespace JSONpull

open WebSharper
open WebSharper.JavaScript
open QuoteField



[<JavaScript>]     
module Client =

    let Main =
        Console.Log("Running JavaScript Entry Point..")
        Console.Log(QuoteField.Quote.value)
        