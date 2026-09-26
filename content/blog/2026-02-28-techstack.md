+++
title = "Tech Stack 2026"
date = 2026-02-28
description = "My 2026 tech stack"
slug = "2026-02-28-techstack"
taxonomies = { tags = ["Dev"] }
+++

Even with the depth and breadth of technology today, it's import to take stock of your preferred technologies. Here's my reflection of my go-to techstack. [Checkout my [about page](@/about.md) to see my journey through technology through the years.]

## Language Principles

Before considering the techstack, I first should layout some guiding principles in a programming language choice.

### Principle 1. I prefer open source.

To be honest, I have a hard time even thinking of software that I could work in *that is not open source.* Though I had a short stint with the Microsoft proprietary .NET Framework, it was soon open-sourced as `dotnet`. There have been just a few times over the years that I have collaborated with teams that use proprietary software for their development, and I have cringed every time. The quality of both the software used and produced were subpar.

### Principle 2. I prefer clarity and simplicity.

At face value, I think most people would assume that Python is the logical answer for a language that promotes clarity and simplicity. However, I would claim that the language should be evaluated on both its simplicity in reading and writing. I would agree that Python is easier to write (at least initially in project), but I have had nightmares reading Python including my own! 

To me, a statically typed language is the only way to promote clarity and simplicity. Dynamically typed language, such as Python, may support annotations, but they don't provide the guarantees like a statically typed language.

### Principle 3. I prefer Linux friendly technologies.

Windows is great, at least for a season, but I have run into problems setting up build servers—not to mention my own development system. I made the switch to Linux-*only* for development about 10 years ago, and I haven't looked back.

### Principle 4. I prefer rigorous thinking.

I got my programming start in my high school Math class programming my TI-83 back in the 90s. Later, I fostered my love for Mathematics and programming while working as a high school Math teacher in 2000s. I enjoyed proving things and starting from first principles, and I enjoyed teaching others how to prove things as well. When I became a software engineer, I sought out technology that reflected the Mathematical rigor I was accustomed to.

---

## Final Language Considerations

- JavaScript is ubiquitous, but it's riddled with issues. (How is this even `true`? `false != !""`)
- Haskell is great for its strictness, but it hasn't seen the industry support like a myriad of other choices.
- Python is great for communicating basic ideas, but it cannot lead you to the strictness required by rigorous thinking.
- Other garbage collected languages may be great at static typing (e.g. C#, F#, and Java), however, they don't force you to consider how a computer processes memory.
- Some languages require programming memory (e.g. C/C++) but don't guide you down the rigorous path. [You're free to call `free` whenever (or not) you want to, after all.]

> The *only* logical answer for me is Rust.

- Rust has broad industry support. It's now in the [Linux kernel](https://docs.kernel.org/rust/index.html), and even Windows has [embraced it](https://techcommunity.microsoft.com/blog/windowsdriverdev/towards-rust-in-windows-drivers/4449718).
- Rust foregoes the non-determinism of the garbage collector by introducing the idea of [Ownership and Borrowing](https://doc.rust-lang.org/book/ch04-01-what-is-ownership.html) to manage memory.
- Rust's rigor is its simplicity. Reading Rust means you don't have to consider a whole host of issues.

---

## Rust Techstack

Great! Now that the language is settled, I can consider the techstack for Rust. 

### 1. Web API: [Rocket](https://rocket.rs/)

I really like the simplicity of Rocket's API. The macros are straightforward:

```rust
// See https://rocket.rs/overview/
#[post("/user", data = "<new_user>")]
fn new_user(admin: AdminUser, new_user: Form<User>) -> T {
    ...
}
```

You get type-safety out of the box.  See `admin: AdminUser` in the example above.

Other Rust web frameworks fit the bill as well ([Axum](https://docs.rs/axum/latest/axum/) and [Actix](https://actix.rs/)), so I think I'll take them out for a spin this year.

### 2. Database: PostgreSQL

PostgreSQL seems to be the one of the most well-supported, open-source databases available. It pairs nicely with the [`sqlx` crate](https://docs.rs/sqlx/latest/sqlx/). SQLx [provides static typing](https://github.com/launchbadge/sqlx?tab=readme-ov-file#compile-time-verification) in Rust for SQL. I think it's insane! It's worth copying here because of its beauty:

```rust
// See https://github.com/launchbadge/sqlx?tab=readme-ov-file#compile-time-verification
struct Country { country: String, count: i64 }

let countries = sqlx::query_as!(Country,
        "
SELECT country, COUNT(*) as count
FROM users
GROUP BY country
WHERE organization = ?
        ",
        organization
    )
    .fetch_all(&pool) // -> Vec<Country>
    .await?;

// countries[0].country
// countries[0].count
```

I have dealt with the pain of ORMs and the looseness of SQL-as-strings over the years, and I find this capability **THE** reason to switch to Rust.

### 3. Browser Frontend: TypeScript & ReactJS

I know, I know, in my discourse above, it would seem clear that my language of choice in all things would be Rust. But, in my opinion, putting Rust in the browser via Web Assembly (WASM) has not come of age. I have dabbled with several of the WASM Rust frameworks (e.g. [Yew](https://yew.rs/) and [Percy](https://chinedufn.github.io/percy/)), but none of them seems to have garnered the industry support I'd prefer. So, if I am to develop a web app in the browser, I will lean heavily on TypeScript to bring some of the rigor to JavaScript. 

I choose [ReactJS](https://react.dev/) for the web framework. I have worked with several other JS frameworks over the years, and I haven't found any of them to be as simple as React.

### 4. Containers: Docker, Buildah, & K3S

Is there any choice to build containers besides Docker? Okay, just kidding. 

1. I use Docker for its awesome cli to develop container images, though I'm considering taking [Podman](https://podman.io/) out for a spin this year.
2. I use [Buildah](https://buildah.io/) for building images in a CI primarily because it avoids the need for a daemon running as root.
3. I use [CRI-O](https://cri-o.io/) in [K3S](https://k3s.io/) for Kubernetes. That was a mouthful! But, I like the simplicity of K3S as a ready-built Kubernetes platform, and you get CRI-O for free-*ish*.

### 5. CLI Frontend: Clap

[Clap](https://docs.rs/clap/latest/clap/) seems to be obvious choice for developing CLIs in Rust. But, I think I'd like to level-up my game this year with [RATATUI](https://ratatui.rs/). [Who says you can't make a UI in a terminal?]


<!-- 
Design Your Personal Preferred Tech Stack
Identify and describe your preferred tools, languages, and frameworks for building software projects. Justify your choices based on experience or curiosity. Turn this into a post and publish it on Substack, Medium, DEV.to, or similar.

What to submit: A link to your published post and a short summary of one tool or language you plan to explore next.
-->