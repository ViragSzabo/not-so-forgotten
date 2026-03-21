# 🏛️ Architecture Guide: Not-So-Forgotten Cemetery

This document provides a technical deep dive into the engineering principles and design patterns used in the cemetery.

---

## 🧩 Architectural Pattern: MVVM
The project strictly follows the **Model-View-ViewModel (MVVM)** pattern to ensure a clean separation of concerns:
- **Models**: Simple data structures (e.g., `MemoryDb`, `Song`) decorated with SQLite attributes.
- **Views (XAML)**: Declarative UI using MAUI controls, entirely decoupled from business logic.
- **ViewModels**: Manage application state and handle user interactions via `RelayCommand`. 

## 🏗️ Service Layer & Inversion of Control
Every major technical component (Database, Spotify, YouTube) is abstracted behind an **Interface**. This allows for:
- **Testability**: Services can be easily mocked using tools like `Moq`.
- **Flexibility**: The underlying implementation can change (e.g., switching from SQLite to a cloud DB) without affecting the UI.

### Dependency Injection (DI)
Services and ViewModels are registered in the MAUI container in `MauiProgram.cs`:
```csharp
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddSingleton<ISpotifyService, SpotifyService>();
builder.Services.AddTransient<HomeViewModel>();
```

## 💾 Database Schema
The app uses **SQLite** for lightweight, async local storage.
- `MemoryDb`: Stores graves (title, description, date).
- `WhisperDb`: Stores ghostly messages revealed by background threads.
- `UserProfileDb`: Stores cemetery visitor preferences.
- `PlaylistDb`: Stores curated collections of songs.

## 🧵 Concurrency & Threading
The project demonstrates advanced threading handling:
- **Background Tasks**: The "Radio Station" and "Memory Whispers" run on separate threads to keep the UI responsive.
- **Cancellation**: All long-running tasks use `CancellationTokenSource` to ensure proper cleanup when a page is closed.

---

_“The structure is the skeleton that keeps the memories upright.”_
