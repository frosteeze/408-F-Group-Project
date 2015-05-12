namespace Client_Server

open WebSharper
open WebSharper.JavaScript
open WebSharper.Html.Client

[<JavaScript>]
module Client =

    let Start input k =
        async {
            let! data = Remoting.Process_quote(input)
            return k data
        }
        |> Async.Start
    
    let auto input k = 
        async {
            let! data = Remoting.Process_quote_FData(input)
            return k data
        }
        |> Async.Start
    let Main () =
        let input = Input [Attr.Value ""]
        let label = Div [Text ""]
        let label3 = Div[Text "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22%2C%22AAPL%22%2C%22GOOG%22%2C%22MSFT%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json"]
        let label4 = Div[Text ""]
        Div [

            label3
            input
            label
            
            Button [Text "Click"]
            |>! OnClick (fun _ _ ->
                Start input.Value (fun out ->
                    label.Text <- out))



            Button [Text "Click auto"]
            |>! OnClick (fun _ _ ->
                auto input.Value (fun out ->
                    label4.Text <- out))
            
            label4


        ]
