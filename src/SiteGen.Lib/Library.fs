namespace SiteGen.Lib

module Site =
    type Inline =
    | Text of string
    | P of Inline
    | Span of Inline
    | Strong of Inline
    | Anchor of href:string * Inline
    | Script of src:string

    type Block =
    | Div of Block
    | Ul of Block list
    | Ol of Block list

    type Header =
    | Link of href:string * rel:string
    | MetaCharset of string
    | MetaName of name:string * content:string
    | MetaHttp of httpEquiv:string * content:string
    | Title of string

    type Html = 
        {   Head: Header
            Body: Block }

    let print (html: Html) : string = 
        ""