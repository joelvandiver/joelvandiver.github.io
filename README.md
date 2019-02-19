# Joel Vandiver's Blog

The url for this Blog is here:
[https://joelvandiver.github.io/](https://joelvandiver.github.io/)

## Troubleshooting

FSharp.Formatting may throw an error about not finding the .optdata and .sigdata in .fake/.  To resolve, copy the files from:
1. C:\git\joelvandiver.github.io\packages\FSharp.Core\lib\net45\FSharp.Core.optdata
2. C:\git\joelvandiver.github.io\packages\FSharp.Core\lib\net45\FSharp.Core.sigdata

to:
C:\git\joelvandiver.github.io\.fake\.store\fake-cli\5.12.1\fake-cli\5.12.1\tools\netcoreapp2.1\any