+++
title = "Joel Vandiver"
description = ""
+++

## About Me

My journey towards software engineering took a non-traditional path. As a Physics & Mathematics major, I had exposure to programming in college, but I had never connected the rigor of Mathematics and Science to programming even though "Science" is right there in the name "Computer Science". Science has been a part of my daily experience since childhood.

With my thirst for learning and my love for Mathematics, I became a High School Mathematics teacher upon graduating with my bachelor's degree. After a few years in, I began programming a small website for my students. I figured I could post content to the website for students to review on their own time. [See my ancient youtube account: https://www.youtube.com/joelvandiver.] What started as a simple website posting static content quickly turned into programming math lessons alongside video content. I used JavaScript to generate problems where the core idea can be expressed in modern TypeScript:

```typescript
/// Represents a problem for students.
interface Problem {
    equation: string,
    x: number,
    answer: number
}

/// Return the linear problem for this lesson.
function linear(m: number, b: number, x: number): Problem {
    return {
        x,
        equation: `y = ${m}x + ${b}`,
        answer: (m * x) + b
    }
}

/// Generate a random integer up to the max number.
function generateNumber(max: number) {
    return Math.floor(Math.random() * max + 1);
}

/// Create a linear problem with random m, b, and x values from 1 to 10.
function createProblem(): Problem {
    const m = generateNumber(10);
    const b = generateNumber(10);
    const x = generateNumber(10);
    return linear(m, b, x);
}
```

The code above can generate problems for web apps, tests, quizzes, etc.

```text
Provide the values of y for the following equations:

1.) y = 8x + 3 when x = 7

2.) y = 9x + 5 when x = 2

3.) y = 8x + 4 when x = 5

4.) y = 6x + 3 when x = 7

Answers: 59, 23, 44, 45
```

With that one idea, my programming career was born! I began generating individualized tests with the exact same logic but completely different numerical values and answers per student. Then, I programmed the automated assessments via Excel and numerical scantrons. The more I programmed, the more I was able to focus on student learning and their achievements.

Surprisingly, the act of programming the math lessons led me to more rigorously define the lesson itself. What started as a pragmatic approach to removing the toil of problem generation led to the start of my Computer Science journey. I was forced to formalize the lessons through Algorithms everyday. Instead of generating problems on the fly as students needed help, I would generate the generator. That was crazy to me!

After several years of this positive feedback loop, I was offered an opportunity to work at a small software company nearby. I started my programming career in .NET. Day after day, I would slog through raw SQL queries as I learned to express my questions with code. I was shocked by the impurities of imperative languages such as C#. Coming from a Math background, I had exclusively programmed in an immutable, functional style. It wasn't until years later that I learned just how impure JS actually is.

Later, I had the opportunity to move to a bigger company with a larger space to grow. Over time, I have worked in various positions in the company, and I've had the opportunity to learn from other languages and frameworks.

With every technical skill I acquired, I learned new things:

- **SQL**: Taught me how to reason about my questions using set-theory.
- **C#.NET**: Taught me object-oriented programming, dependency injection, and a whole host of design patterns.
- **F#.NET**: Taught me to rely on immutability by default, use functions as first-class values, and the benefits of minimizing side-effects.
- **TypeScript**: Taught me that types can be computable as well.
- **Python**: Taught me to get things done and to be expressive in my ideas.
- **Rust**: Taught me to prove memory and data structures in a system.
- **Docker**: Taught me to formally define my application's environment and to explore other technologies.
- **Kubernetes**: Taught me to distribute my services to simplify development at scale.
- **GenAI**: Taught me to formalize my questions to accelerate my learning.
- **AgenticAI**: Taught me to press into rigor and verification as the system being built rapidly evolves.

The list could go on forever, it seems. My career has been a wild ride of constant learning, and I've got so much more to explore!

---

> Find me on [LinkedIn](https://www.linkedin.com/in/joelvandiver/) / [GitHub](https://github.com/joelvandiver)
