# ASP.NET Core Windows Impersonation

```fsharp	
let getResponse () =
    let req = System.Net.WebRequest.CreateHttp(@"http://localhost:80/3.1.4/api/profile")
    req.Method <- "GET"
    //System.Security.Principal.WindowsIdentity.RunImpersonated()
    req.UseDefaultCredentials <- true;
    req.UserAgent <- "GCS API";
    try
        let resp = req.GetResponse() :?> System.Net.HttpWebResponse
        resp |> box
    with
        | :? System.Net.WebException as webEx ->
            webEx |> box

[<HttpGet>]
member this.Get() =

    let response =
        try
            use wi =
                this.HttpContext.User.Identity
                    :?> WindowsIdentity

            let work = getResponse

            System.Security.Principal.WindowsIdentity.RunImpersonated(wi.AccessToken, Func<obj> work)
        with | ex -> ex |> box

    ActionResult<obj> response

```