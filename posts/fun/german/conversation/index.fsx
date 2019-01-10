(**
# F# Deutsche <|> English

*)

type English =
| Hello
| ``My name is `` of name:string

type Deutsche =
| Hallo
| ``Mein Name ist `` of name:string

let translate (text: English) : Deutsche = 
   match text with 
   | Hello -> Hallo
   | ``My name is `` name -> ``Mein Name ist `` name

let übersetzen (text: Deutsche) : English =
   match text with 
   | Hallo -> Hello
   | ``Mein Name ist `` name -> ``My name is `` name

let greet = übersetzen Hallo
let introduce = übersetzen (``Mein Name ist `` "Joel Vandiver")

let grüßen = translate Hello
let vorstellen = translate (``My name is `` "Joel Vandiver")

