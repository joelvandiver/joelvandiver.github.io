(**
F# Logo Generator
=================

[Addapted from FSSnip](http://www.fssnip.net/tH/title/F-logo-generator)
*)

(**
![fslogo.png](/posts/daily/2019/01/05/fs-logo/fs-logo.png)
*)

#if INTERACTIVE
#r "System.Drawing.dll"
#endif

open System.Drawing

let drawRect (xPos,yPos,height,direction,col) scale (bm:Bitmap) =
  let rec loop x y d i =
    for z = min (xPos*scale) x to (max (xPos*scale) x) - 1 do
      bm.SetPixel(z,y,Color.FromArgb(col))
    if i = 0 then bm
    elif i = ((height*scale) / 2) + 1
    then loop (x + d) (y + 1) (0 - d) (i - 1)
    else loop (x + d) (y + 1) d (i - 1)
  loop (xPos*scale) (yPos*scale) direction (height*scale)

let scale = 18
let size = 29 * scale
let back,dark,light = 0x0,0xff378bba,0xff30b9db
let image =
  [(14,0,28,-1,dark);(14,7,14,-1,back);(14,9,10,-1,dark);(15,0,28,1,light);(15,7,14,1,back)]
  |> List.fold (fun bm c -> drawRect c scale bm) (new Bitmap(size, size))

image.Save(__SOURCE_DIRECTORY__ + @"\fs-logo.png", Imaging.ImageFormat.Png)
