namespace ClientServerTable

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server

type MetroStyle() =
    inherit Resources.BaseResource("https://cdn.metroui.org.ua",
        "v4/css/metro-all.min.css",
        "v4/css/metro.min.css",
        "v4/css/metro-colors.min.css",
        "v4/css/metro-rtl.min.css",
        "v4/css/metro-icons.min.css")


[<Require(typeof<JQuery.Resources.JQuery>)>]
type MetroScript() =
    inherit Resources.BaseResource("https://cdn.metroui.org.ua/v4/js/metro.min.js")

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About
    | [<EndPoint "/table">] Table

module Templating =
    open WebSharper.UI.Html

    type MainTemplate = Templating.Template<"Main.html">

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             li [if endpoint = act then yield attr.``class`` "active"] [
                a [attr.href (ctx.Link act)] [text txt]
             ]
        [
            "Home" => EndPoint.Home
            "About" => EndPoint.About
            "Table" => EndPoint.Table
        ]

    let Main ctx action (title: string) (body: Doc list) =
        Content.Page(
            MainTemplate()
                .Title(title)
                .MenuBar(MenuBar ctx action)
                .Body(body)
                .Doc()
        )

module ServerModel =
    let Entries = 
        [ ("test", "go"); ("again", "ping") ] 
        |> List.append <| List.map(fun i -> (string i,string i)) [1..20] 
        |> List.toSeq 
    let EntryMap = Map<string, string>( Entries )

module Site =
    open WebSharper.UI.Html

    let HomePage ctx =
        Templating.Main ctx EndPoint.Home "Home" [
            h1 [] [text "Say Hi to the server!"]
            div [] [client <@ Client.Main() @>]
        ]

    let AboutPage ctx =
        Templating.Main ctx EndPoint.About "About" [
            h1 [] [text "About"]
            p [] [text "This is a template WebSharper client-server application."]
        ]
    
    let TablePage ctx =
        Templating.Main ctx EndPoint.Table "My Table Page" [
            Doc.WebControl (new Web.Require<MetroStyle>())
            h1 [] [text "My cool Table"]
            p [] [text "This is a test for using WebSharper with Metro-Ui-Css"]
            div [] [client <@ Client.Inspector() @>]
            table [attr.``class`` "table"; attr.``data-`` "role" "table"; attr.id "demo-table"] [
                yield thead [] [
                    tr [] [
                        th [attr.``class`` "sortable-column sort-asc"] [text "Col 1"]
                        th [attr.``class`` "sortable-column"] [text "Col 2"]
                    ] ]
                for KeyValue(key, value) in ServerModel.EntryMap do
                    yield tr [] [
                        td [] [text key]
                        td [] [text value]
                    ] 
            ]
            Doc.WebControl (new Web.Require<MetroScript>())
        ]

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
            | EndPoint.Table -> TablePage ctx
        )
