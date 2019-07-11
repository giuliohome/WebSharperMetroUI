namespace ClientServerTable

open System.Web

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

    member __.Application_BeginRequest(sender: obj, args: System.EventArgs) =
        HttpContext.Current
        |> function
        | null -> ()
        | ctx ->
            match ctx.Request with
            | null -> ()
            | req ->
                match req.Cookies.Item "csrftoken", req.Headers.Item "x-csrftoken" with
                | null, null -> ()
                | cookie, null ->
                    // fix for IE11, which does not always set the HTTP Header "x-csrftoken"
                    try req.Headers.Item "x-csrftoken" <- cookie.Value
                    with _ -> ()       // ignore possible errors
                | null, _ ->
                    // if header is set but cookie is not, there's nothing we can do (cookie collection is read-only)
                    ()
                | cookie, csrfHeader when cookie.Value <> csrfHeader ->
                    try req.Headers.Item "x-csrftoken" <- cookie.Value
                    with _ -> ()       // ignore possible errors
                | _ ->
                    ()      // all is fine, the default: cookie "csfrtoken" and header "x-csfrtoken" are equal