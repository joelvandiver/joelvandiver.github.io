+++
title = "Resilient Gateway"
date = 2024-04-23
description = "Build resilient Nginx reverse proxies."
slug = "2024-04-23-resilient-gateway"
taxonomies = { tags = ["Nginx"] }
+++

Creating a "Resilient Gateway" using Nginx as a reverse proxy is a great way to ensure that your web infrastructure can handle failures gracefully. Let's explore how to configure Nginx to start up and remain operational even if the backend services are down. This setup enhances the resilience and availability of your web applications, making them more robust against backend instabilities.

## Understand: The Challenge

Typically, when Nginx is configured to proxy to a backend service and that service is unavailable at startup, Nginx will not come up!

Let's take this `nginx.conf` as an example:

```conf
events {
    worker_connections 1024;
}

http {
    server {
        listen 80;

        location /flaky {
            proxy_pass http://flaky:8000;
        }
    }
}
```

If the `http://flaky:8000` service is not running, then Nginx will not start. By default, Nginx requires all backend services to be up and running when it comes up.

## The Solution: Resilient Gateway

To make our Nginx gateway resilient, we can use a combination of configuration strategies that include service discovery and DNS resolution. This setup ensures that Nginx can start up without needing the backend services to be available and can continue to operate if they go down.

> Step 1: Defining a Variable

Nginx evaluates variables lazily. This means that the resolution of the hostname happens at the time of the request, allowing for dynamic changes in the backend's status.

`set $backend "flaky:8000";`

Here, `$backend` is a variable, and its resolution is deferred until a request is made. This setup allows for dynamic reconfiguration if the status of flaky changes.

> Step 2: Add the Internal DNS Resolver

`resolver 127.0.0.11 valid=30s;  # Docker's internal DNS resolver`

The `resolver` directive defines the DNS server to use for name resolution since the service is discovered at the time of the request.

```conf
events {
    worker_connections 1024;
}

http {
    server {
        listen 80;

        set $backend "flaky:8000";

        location /flaky {
            proxy_pass http://$backend;
        }
    }
}
```

In this configuration, if the flaky service is down, users will encounter an error when trying to access it through the gateway. The web server is free to respond with appropriate errors pages (typically 502 Bad Gateway). Serving error pages is by far a better experience than not responding at all. Also, other services may be up and running and can respond to requests even if a particular backend service is down.

## Conclusion

By implementing the dns resolver, your Nginx reverse proxy becomes more resilient and enhances the availability and stability of your applications. This configuration is especially useful in a microservices architectures where individual service availability can vary.
