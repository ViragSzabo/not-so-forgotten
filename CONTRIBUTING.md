# 🕯️ Contributing to Not-So-Forgotten Cemetery

_A gothic autumn radio that whispers the memories of the not-so-forgotten._

---

## 🌿 Introduction

Thank you for stopping by the Not-So-Forgotten Cemetery — a place where code meets memory, and creativity meets logic. 
Every contribution keeps the whispers alive. Whether you’re fixing a thread that broke, adding a new “memory,” 
or enhancing the misty UI — your effort helps this digital graveyard grow.

---

## ⚙️ How to Contribute
### 🪦 1. Fork & Clone

Start by forking the repository and cloning it to your local environment:

```bash
git clone https://github.com/<your-username>/not-so-forgotten-cemetery.git
cd not-so-forgotten-cemetery
```

### 🌫️ 2. Create a New Branch

Please use descriptive branch names:

```bash
git checkout -b feature/add-memory-whispers
```
#### or
```bash
git checkout -b fix/radio-thread-issue
```

### 🔮 3. Make Your Changes

Follow the project’s tone and design guidelines:
* **Keep code clean**, readable, and async-safe.
* **Use MVVM structure** and **async/await** for database and UI interactions.
* **Preserve the autumn–gothic atmosphere** in visuals and naming _(e.g., prefer WhisperService over MessageHandler)_.
* **Be respectful with creative additions** — everything should feel like part of the same story.

### 💾 4. Test Before You Commit

- **Unit Tests**: Ensure all tests in `NotSoForgottenCemetery.Tests` pass.
- **Manual Verification**: Make sure the app builds and runs successfully on Windows.
- **Concurrency**: Check thread synchronisation and database operations (no deadlocks or blocking UI threads).

### 🤖 5. CI/CD Pipeline

The project uses **GitHub Actions** to verify builds. Ensure your branch builds successfully in the CI environment before requesting a review.

### ✍️ 6. Commit Convention

Write clear and meaningful commit messages:

* **feat:** add background whisper thread for random memory playback
* **fix:** prevent async deadlock in radio broadcast service
* **style:** adjust candle animation glow

### 🕯️ 6. Push & Pull Request

Push your branch and open a pull request:

```bash
git push origin feature/add-memory-whispers
```

Then go to the main repository and open a Pull Request.
Describe your changes briefly, and — if you wish — add a poetic line to accompany your update (optional but encouraged).

## 🧩 Areas to Contribute

You can contribute in many ways:

* **💀 Code:** Bug fixes, features, async improvements, performance tweaks
* **🎨 Design:** Gothic/atmospheric UI, animation effects, new icons
* **🧠 Story:** Add new “memories,” whispers, or character fragments to the database
* **📜 Documentation:** Improve or translate README, wiki pages, or tutorials

## 🧘 Code Style & Guidelines

- Follow C# conventions and use async/await correctly.
- Use dependency injection and services where possible.
- Keep functions small and readable.
- Comments should enhance clarity, not clutter.
- Avoid blocking calls (no .Result or .Wait() in async code).

## 🕰️ Reporting Issues

If you find a bug, please open an issue with:
1. Clear title
2. Steps to reproduce
3. Expected vs. actual behaviour
4. Optional: screenshots or log snippets

Example issue title: _“🕯️ Whisper threads freeze when switching radio channels”_

## 💌 A Note from the Keeper

_“Every contribution is a candle — a light in the mist that keeps the memories alive.”_
Thank you for helping this project grow with your time, creativity, and code.

## ⚖️ License

This project is licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
You’re free to fork, remix, and expand — as long as it remains non-commercial and properly credited.
