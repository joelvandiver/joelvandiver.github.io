+++
title = "GitFlow"
date = 2021-06-08
description = "Git It Done & Keep It Clean"
slug = "2021-06-08-gitflow"
taxonomies = { tags = ["DX", "DevOps", "Git"] }
+++

First off, you're doing great! You are doing the best you can to keep track of the changes to your system.  The people on your team know what's going on ... right?  Code conflicts only turn into actual conflicts sporadically.  But, hey, that's just part of the job, isn't it?  You only run `git blame` sometimes - no, not to *shame* anyone, just *blame*. I've only reached for the non-existent `git shame` command a few times in my career 😀!

But, we can do better!

---

Git is a powerful (if not all-consuming) source control system.  I've been using it since 2012, and my particular workflow with Git has changed pretty dramatically over time. 

## Git Push-Pull Flow

In my early days of learning and using Git, I opted for the simple push-pull workflow.  The workflow was simply:

1. Fetch other developers' code from `main`.
2. Merge the changes from the remote `main` to your local `main`.
3. Resolve any conflicts.
4. Commit the changes.
5. Push the new commit.

The commands may look like:

```bash
# Get the latest commits from the remote.
git pull

# Make changes locally.
# Add all changes (or individually, if you prefer.)
git add -A

# Commit your changes with a meaningful message.
git commit -m "Sample Message"

# Fetch and merge changes.
git pull

# Fix any merge conflicts.

# Push the changes.
git push
```

This works fine for small systems but starts to fail in larger systems.

### Noisy Commits

The difficulty comes from providing *meaningful* commit messages of the changes over time. We are always (at least we should be) learning new things and discovering better ways to do things. I don't want to see a chain of my commits following this pattern:

- Do thing
- Undo thing
- Redo thing with new idea
- Start over

> **Anger-Danger** Side Note: I've had other developers call out a particular commit of mine as the smoking gun to their problem only for me to show them my fix in a later commit.

## Pull/Merge Request Flow

A feature branch is the perfect place to figure things out.

Now, the flow is:

1. Make a feature branch (aka `feature/{name}`)
   1. Push commits
   2. Figure out the correct solution
   3. Iterate.
2. Pull changes from other developers.
3. Create a pull/merge request
4. Squash commits into `main`. A squash commit summarizes all commits in the feature branch.

Let's upgrade the commands above to:

```bash
# Get the latest commits from the remote.
git pull

# Checkout a feature branch.
# Rename "sample_branch" to something meaningful
# that you are trying to achieve.
git checkout -b feature/sample_branch

# Push the branch with tracking to the remote.
git push -u origin feature/sample_branch

# Make changes locally.
# Add all changes (or individually, if you prefer.)
git add -A

# Commit your changes with a meaningful message.
git commit -m "Sample Message"

# Push the changes to your tracking branch.
git push

# Fetch and merge changes.
git pull

# Fix any merge conflicts.

# Push the merged changes.
git push

# Follow the Merge Request (GitLab)
# or Pull Request (GitHub)
# Be sure to squash the commits on merge!
```

With this flow, developers rummaging around in the commit log will see the *official and vetted* changes. The intermediate stages to discovery are thrown away.

## Reset-Rebase Flow

CI stands for "continuous integration". By definition, any branch of code is not continuously integrated. This can be a bit aggressive, but there is merit to this idea.

Let's try out a new flow:

1. Make commits locally to figure out the *correct* solution.
2. Reset the changes to `main`.
3. Fetch the remote changes.
4. Rebase your final changes on the remote changes.
5. Push your changes.

The commands can be upgraded further:

```bash
# Get the latest commits from the remote.
git pull

# Make changes locally.
# Add all changes (or individually, if you prefer.)
git add -A

# Commit your changes with a meaningful message.
git commit -m "Sample Message"

# Repeat your commits until your are satisfied.

# Reset your local changes to the remote branch.
git reset --soft origin/main

# Create a single, official commit with a meaningful message.
git commit -m "Official Sample Message"

# Fetch the changes.
git fetch

# Rebase your commit on all fetched commits.
git rebase

# Fix any rebase conflicts.

# Push the rebased changes.
git push
```

## Still Branch?

The purist in me prefers the reset-rebase flow, but the pragmatist in me realizes that we work in very complex systems that need more eyes on the work. The reset-rebase flow is great for getting things done and pushing the changes. The pull/merge request flow, however, is great for fostering team collaboration and ensuring greater quality. We all struggle with producer's bias; I mean I built it, so of course it's right!

I use a judgement call per codebase and for a particular problem if I am going to follow the reset-rebase flow or the pull/merge flow. If I follow the pull/merge flow, then I try to keep my branch short-lived (under a day or two).

## Update Log

> Update 2021-12

My original post was focused on the pull/merge flow. I realized that I need to leverage the reset-rebase flow to integrate continuously (CI) and to move more nimbly.

