+++
title = "Scratch: Images"
date = 2026-06-29
description = "Build Rust images FROM scratch"
slug = "2026-06-29-rust-scratch"
taxonomies = { tags = ["Rust", "Docker", "Kubernetes"] }
+++

There's a little fact about Rust that obliterated all other tech stacks I work within containerized environments (Kubernetes):

> You can build Rust container images [`FROM scratch`](https://hub.docker.com/_/scratch/)!

Yes, you heard me right. Just drop a statically linked Rust binary in a scratch image, and voila!, the application is deployable to Kubernetes. At first glance, this might seem like an insignificant idea, but it's impact is priceless.

## Scratch: Secure

All software should be safe and secure, but what does that mean, exactly? I mean Rust is secure by default, right? Well, technically more secure than most programming languages, but the environment you deploy it to will have a greater impact on the security posture. Deploying a Rust application to any *nix-based distro will have the user-space programs to contend with for security.

So, why deploy the user-space programs at all? I mean, your statically linked Rust binary can just work directly with the kernel. Why drop a shell such as bash or ash or _____? Does your app really need to shell-out in production to implement the feature? Or, even worse, does your app really need to update packages in the running container with `apt`, `apk`, ...? Could you imagine if a hacker said, "Na, use my super-safe-apt-registry for your application needs"? I mean, this is insanity!

## Scratch: Build

You get a few other benefits with switching to `scratch` image builds.

1. You can build your binary once in you CI/CD pipelines. Then, you can deploy the statically linked binary to different hosts.
2. Your image builds can be focused on just copying the final binary and any relevant config files.
3. Your images are super small; they contain only your app after all!

The primary drawback is libary code (`*.so` on Linux) can't be shared when deployed to the same host. But, this isn't usually relevant in containerized workloads.

## Scratch: Example

Here's an example to get you going. I built a sample GitLab CI/CD to show the `Build` is separated from the `Publish` jobs.

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
  image: rust:latest
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
  image: docker:latest
  services:
    - docker:dind
  needs:
    - job: Build
      artifacts: true
  before_script:
    - echo "$CI_REGISTRY_PASSWORD" | docker login -u "$CI_REGISTRY_USER" --password-stdin "$CI_REGISTRY"
  script:
    - docker build -t "$IMAGE" .
    - docker tag "$IMAGE" "$CI_REGISTRY_IMAGE:latest"
    - docker push "$IMAGE"
    - docker push "$CI_REGISTRY_IMAGE:latest"
```

> Dirt-simple Dockerfile:

```Dockerfile
FROM scratch
COPY target/x86_64-unknown-linux-musl/release/my_app /app/my_app
ENTRYPOINT ["/app/my_app"]
```
