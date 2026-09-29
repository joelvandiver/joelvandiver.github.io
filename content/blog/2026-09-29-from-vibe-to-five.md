+++
title = "From Vibe to Five"
date = 2026-09-29
description = "What is engineering anyways?"
slug = "2026-09-29-from-vibe-to-five"
taxonomies = { tags = ["Agentic"] }
+++

Back in a post in May, [Stop Vibe to Five](@/blog/2026-05-25-stop-vibe-to-five.md), I had a gut response to the vibing all day long. In the months that followed, I have had time to process the change.

> How the power of AI put me on the path to rigor

## To Vibe or Not To Vibe

When gaining access to agentic AI for software engineering, how can you not see their immediate value? Vibes are awesome! With a short prompt the agent can build an amazing looking site. Why would you not embrace this technology?

But there’s a catch: if there wasn’t this problem of engineering. I mean what is “engineering” anyways? I would summarize engineering as the discipline of tradeoffs. Every decision is a tradeoff. You will decide to tradeoff between:

- performance and maintenance
- operations and development
- abstraction and concreteness
- delivery and correctness

to name a few. So, who gets to decide between the tradeoffs? Aren't the engineers ultimately the responsible party? If an agent produces vulnerable code, isn’t the engineer to blame for releasing it? If the agent produces a non-viable solution economically, isn’t the engineering team held accountable? 

Don’t get me wrong; vibes are great! They produce proof-of-concepts (POCs) worthy of review. But, you can’t stop there; you must engineer the solution.  Every POC is a promise of the possible, while engineering makes the possible a reality. 

Vibing all day everyday only incurs promise-debt. The promise-debt is a tradeoff like any other tradeoff. We can spend time producing the POCs to sell our ideas to stakeholders, or we can actually build the system. We should vibe to imagine the future we want to build, but let’s not work **from vibe to five**.

## Rigor in Vibes?

This may sound shocking, but I’m convinced that engineers can do their part to provide a more rigorous vibe. After the boom in agentic software development, I received 20+ vibes from my non-software-engineer colleagues in just days time. The apps looked great! The ideas from my colleagues were great! But, the code was terrible.

Here’s just a few of the problems I saw:
- **Mixture of ecosystems:** One vibe has a mixture of Java, Python, and NodeJS.
- **Insecure systems:** Cert for TLS transport were pinned directly into the software. 
- **Massive repos:** Another vibe we had took ~10 mins to download because of a malformed .gitignore.

That’s when I realized that we should build a platform for our colleagues to vibe in. The platform is fairly simple:
- Build the base OS and tools. (We built a pre-packaged distro for WSL.)
- Build template repositories with all of the beginning files. Agents transform the files they see, so the template repositories can provide the starter artifacts to build a better POC.
- Provide a tool to very quickly fork the templates into their systems. Most of our colleagues didn’t have the git expertise to manage the templates.

We placed rigorous starting points in our template repositories:
- We preferred languages with strong signals to agents (Rust & TypeScript).
- We advised the agents through AGENTS.md that the system they’re building is a POC, so they’re encouraged to mock all IO to simulate real traffic.
- We instructed the agents to perform git operations on behalf of the user but to follow good practices.

This gave the engineering team a little bit more control of the chaos of the vibes from everyone else. Of course, these are still POCs, but they provide a much better foundation to review.
