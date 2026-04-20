# 🕯️ Not-So-Forgotten Cemetery

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white&style=flat-square)
![MAUI](https://img.shields.io/badge/Framework-MAUI-512BD4?logo=dotnet&logoColor=white&style=flat-square)
![Build](https://img.shields.io/github/actions/workflow/status/ViragSzabo/not-so-forgotten/maui_build.yml?branch=main&style=flat-square)
![Tests](https://img.shields.io/badge/tests-54%20passing-brightgreen?style=flat-square)
![Dependabot](https://img.shields.io/badge/dependabot-enabled-025E8C?logo=dependabot&logoColor=white&style=flat-square)
![SQLite](https://img.shields.io/badge/Database-SQLite-07405E?logo=sqlite&logoColor=white&style=flat-square)
![License](https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey.svg?style=flat-square)

_A gothic autumn radio that whispers the memories of the not-so-forgotten._

---

## 🍁 Overview

**Not-So-Forgotten Cemetery** is a C# .NET MAUI application that blends technical skills with poetic storytelling. It is set in a misty digital graveyard with a **radio station** and a **memory keeper** — where every tombstone holds a story, and every whisper is a fragment of someone never forgotten.
The project demonstrates **multi-threading**, **async/await**, and **SQLite database operations** in a visually atmospheric interface.

---

_“Some memories never rest — they hum, they whisper, they wait.”_

---

## 🧩 Features

- **🎙️ Radio Station** – Background threads simulate music and message broadcasts, with real-time signal strength simulation.
- **🪦 Digital Cemetery** – Each grave represents a stored memory in a local SQLite database with async CRUD operations.
- **🌫️ Memory Whispers** – Periodic background scanning reveals random memories as ghostly “whispers” on screen.
- **📊 Technical Monitor** – A built-in dashboard for lecturers to observe thread activity and signal heartbeats in real-time.
- **🎨 Atmospheric UI** – Gothic visuals with flickering candle animations and a premium noir-inspired color palette.

---

| Component | Technology |
|------------|-------------|
| Framework | .NET MAUI (.NET 8) |
| Async Logic | Task Parallel Library (TPL) + CancellationToken |
| Database | SQLite-net-pcl (Asynchronous) |
| Architecture| MVVM + Dependency Injection (Microsoft.Extensions.DependencyInjection) |
| Design System| Custom Noir Palette (Gold, Charcoal, Mist) |
| Testing | xUnit + Moq |

---

## Architecture & Philosophy

**Not-So-Forgotten** is designed as a professional-grade demonstration of C# multi-threading:
- **Thread Isolation**: Background tasks are decoupled from the UI thread using the Task Parallel Library and `MainThread.BeginInvokeOnMainThread`.
- **Resource Management**: Uses `CancellationTokenSource` for clean disposal of background timers and tasks.
- **Diagnostic Transparency**: The "System Monitor" provides a window into the application's internal state, ideal for academic presentations.

---

## 📚 Guides
- [User Guide](USER_GUIDE.md) - How to use the app.
- [Architecture Guide](ARCHITECTURE.md) - Technical deep dive.
- [Security Policy](SECURITY.md) - Security and secrets management.
- [Test Specification](TEST_SPECIFICATION.md) - Testing strategy and coverage.
- [Presentation](Presentation.pptx) - Academic project presentation slides.

---

## ⚙️ Installation

### 🪶 Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **.NET MAUI workload** installed  
- .NET 8 SDK  
- Spotify Developer credentials if you plan to enable live music data 

--- 

### 🪄 Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/ViragSzabo/not-so-forgotten.git
   cd not-so-forgotten
   ```
2. Open `NotSoForgottenCemetery/NotSoForgottenCemetery.sln` in Visual Studio 2022.
3. Restore NuGet dependencies: **Tools → NuGet Package Manager → Restore**.
4. Select your target platform (Windows recommended for easiest setup) and press **Run**.
5. _(Optional)_ Add your Spotify Client ID and YouTube API Key in the **Configuration** page for full media integration.

## 🐳 Running Tests (Docker)

To verify the project's integrity in an isolated Linux environment without needing a MAUI setup:
1. Ensure Docker Desktop is running.
2. Run the following command in the terminal:
   ```bash
   docker compose up --build
   ```
3. The 54 unit tests will execute and test results will be output to the local `test-results/` folder.

---

## 🧡 Contributing
If you’d like to contribute to the Not-So-Forgotten Cemetery, please read the CONTRIBUTING.md file. Whether it’s fixing a bug, improving the visuals, or adding new “memories,” all contributions are welcome — as long as they respect the project’s tone and purpose.

---

## ⚖️ License & Credit
© 2026 Virág Szabó
Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.

---

## ✨Author
Virág Szabó
Senior ICT Student @ NHL Stenden
Project: Threading in C#
📧 virag.szabo@student.nhlstenden.com
