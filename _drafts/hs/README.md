```
docker volume create hs
docker run -it haskell:8 -v hs
docker run -i -t haskell:8
docker run --rm --interactive --volume $PWD:/src haskell:8
docker run --rm --interactive --volume /src haskell:8
docker run --rm --interactive --volume /src:/src haskell:8
docker run --rm --interactive --volume C:\git\joelvandiver.github.io\_drafts\hs\src:/src haskell:8

docker run --rm --interactive -v C:\git\joelvandiver.github.io\_drafts\hs\src:/src haskell:8
```