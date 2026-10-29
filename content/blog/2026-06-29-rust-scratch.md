+++
title = "Scratch: Images"
date = 2026-06-29
description = "Build Rust images FROM scratch"
slug = "2026-06-29-rust-scratch"
taxonomies = { tags = ["Rust", "Docker", "Kubernetes"] }
+++

There's a little fact about Rust that has transformed the way I view other tech stacks in containerized environments (Kubernetes):

> You can build Rust container images [`FROM scratch`](https://hub.docker.com/_/scratch/)!

Yes, you heard me right. Just drop a statically linked Rust binary in a scratch image, and voila!, the application is deployable to Kubernetes with minimal dependencies. To be clear, other languages can perform this same feat, but when compared to languages like Python and NodeJS, this feels like magic! This impact of building `FROM scratch` is far reaching! Most images are built with a distro base to provide some OS utilities. For example, in my team we debated for years whether we should deploy Python images based on Debian or Alpine. We finally landed on Alpine for all images for security reasons, but I mean just look at the options for the Python images on [Docker Hub](https://hub.docker.com/_/python/).

## Scratch: Secure

All software should be safe and secure, but what does that mean, exactly? 

The language choice matters for security, of course, but what about the environment where the solution runs? It’s easy to get focused on the language features of Rust that make it more secure than other languages, but that neglects the point that the OS utilities sitting right next to the solution could have vulnerabilities themselves! Deploying a Rust application to any *nix-based distro will have the OS utilities to contend with for security.

So, why deploy any extra utilities at all? I mean, your statically linked Rust binary can work directly with the kernel without shelling out to other tools. Why drop in a shell such as bash or ash? Does your app really need to shell out in production to implement the feature? Or, even worse, does your app really need to update packages in the running container with a package manager? Could you imagine if a hacker said, "Nah, use my super-safe-apt-registry for your application needs" and all the while swapping out the expected binaries for theirs? I mean, this is crazy!

Building Rust binaries into scratch images takes just a few steps, and all of the security debt related to the OS utilities is removed because the utilities are gone!

## Scratch: Build

You get a few other benefits of switching to scratch image builds.

By statically linking your binary you avoid copying over the `glibc` dynamic libraries.
The image builds are simpler because they just need to copy the final binary and any relevant config files.
Your images will become super small as they now only contain your app after all!

## Scratch: Drawbacks

1. There isn't a shell for debugging. You will have to create extra debugging utilities beyond what you build into the binary itself (e.g. OpenTelemetry). I've worked around this problem by deploying debug sidecars to the pod. You could try [Kubernetes Ephemeral Containers](https://kubernetes.io/docs/concepts/workloads/pods/ephemeral-containers/) which are built for the purpose, but I prefer to spin up a new deployment with a sidecar so that I have more control over the container’s lifecycle.
2. Host configuration files are missing in the scratch image. I've worked around this issue by building a minimal image through a multi-stage build to contain just a few things more than scratch:
   1. The CA bundle to establish trust
   2. The `USER` Dockerfile statement (which removes the need for `/etc/passwd`)

## Conclusion

Aside from the few drawbacks with easy workarounds, I have found building docker images `FROM scratch` to have transformed my work and the work of my team. Our container images are faster to build, smaller in size, and are more secure!
