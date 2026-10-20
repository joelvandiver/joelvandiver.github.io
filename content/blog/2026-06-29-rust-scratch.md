+++
title = "Scratch: Images"
date = 2026-06-29
description = "Build Rust images FROM scratch"
slug = "2026-06-29-rust-scratch"
taxonomies = { tags = ["Rust", "Docker", "Kubernetes"] }
+++

There's a little fact about Rust that has transformed the way I view other tech stacks in containerized environments (Kubernetes):

> You can build Rust container images [`FROM scratch`](https://hub.docker.com/_/scratch/)!

Yes, you heard me right. Just drop a statically linked Rust binary in a scratch image, and voila!, the application is deployable to Kubernetes. At first glance, this might seem like an insignificant idea, but its impact is amazing! Most images are built with a distro base, and there has been much debate on my team whether to deploy Python images to a Debian or Alpine based distro. I mean just look at the supported tags for the Python images on [Docker Hub](https://hub.docker.com/_/python/).

## Scratch: Secure

All software should be safe and secure, but what does that mean, exactly? I mean Rust is secure by default, right? Well, technically more secure than most programming languages, but the environment you deploy it to will have a significant impact on the security posture, as well. Deploying a Rust application to any *nix-based distro will have the user-space programs to contend with for security.

So, why deploy any extra user-space programs at all? I mean, your statically linked Rust binary can just work with the kernel without shelling out to other tools. Why drop a shell such as bash or ash? Does your app really need to shell out in production to implement the feature? Or, even worse, does your app really need to update packages in the running container with `apt`, `apk`, ...? Could you imagine if a hacker said, "Nah, use my super-safe-apt-registry for your application needs" and all the while swapping out the expected binaries for theirs? I mean, this is crazy!

Building Rust binaries into scratch images takes just a few steps, and all of the security debt through ongoing vulnerability remediation for OS packages is removed!

## Scratch: Build

You get a few other benefits of switching to `scratch` image builds.

1. You can use the `x86_64-unknown-linux-musl` target to statically link your binary to avoid copying over the `glibc` dynamic libraries.
2. Your image builds simply copy the final binary and any relevant config files.
3. Your images will become super small as they now only contain your app after all!

## Scratch: Drawbacks

1. There isn't a shell for debugging. You will have to create extra debugging utilities beyond what you build into the binary itself (e.g. [OpenTelemetry](https://opentelemetry.io/)). I've worked around this problem by deploying debug sidecars to the pod. Note that I've used the [Kubernetes Ephemeral Containers](https://kubernetes.io/docs/concepts/workloads/pods/ephemeral-containers/) before, but I prefer to spin up a new deployment with a sidecar so that I can inspect the startup.
2. Host configuration files are missing in the `scratch` image. I've worked around this issue by building a `minimal` image through a multi-stage build to contain just a few things more than `scratch`:
   1. The CA bundle to establish trust
   2. The `USER` `Dockerfile` statement (which removes the need for `/etc/passwd`)

## Scratch: Example

Let's end with an example to get you going. I built a sample GitLab CI pipeline to show the `Build` and `Publish` jobs.

```yaml
stages:
  - build
  - publish

variables:
  TARGET: x86_64-unknown-linux-musl
  BINARY: my_app
  IMAGE: $CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA

# Compile a statically linked musl binary and hand it off as an artifact.
Build:
  stage: build
  image: rust:1-bookworm
  before_script:
    - rustup target add $TARGET
    - apt-get update && apt-get install -y musl-tools
  script:
    - cargo build --release --target $TARGET
  artifacts:
    paths:
      - target/$TARGET/release/$BINARY
    expire_in: 1 hour

# Build a scratch image around that binary, then tag and push it.
Publish:
  stage: publish
  needs:
    - job: Build
      artifacts: true
  before_script:
    - echo "$CI_REGISTRY_PASSWORD" | docker login -u "$CI_REGISTRY_USER" --password-stdin "$CI_REGISTRY"
  script:
    # Your CI needs docker!
    - docker build -t "$IMAGE" .
    - docker tag "$IMAGE" "$CI_REGISTRY_IMAGE:latest"
    - docker push "$IMAGE"
    - docker push "$CI_REGISTRY_IMAGE:latest"
```

> Here's my dirt-simple `Dockerfile`:

```Dockerfile
FROM alpine:3.24 AS certs
RUN apk add --no-cache ca-certificates

FROM scratch
COPY --from=certs /etc/ssl/certs/ca-certificates.crt /etc/ssl/certs/ca-certificates.crt
COPY target/x86_64-unknown-linux-musl/release/my_app /app/my_app
USER 10001:10001
ENTRYPOINT ["/app/my_app"]
```
