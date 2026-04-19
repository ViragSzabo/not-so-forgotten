# Test Specification: Not-So-Forgotten Cemetery

This document outlines the testing strategy and quality assurance measures implemented to ensure the reliability and performance of the application.

---

## 🧪 Testing Overview
The project employs a robust testing suite using **xUnit** and **Moq** to validate core logic, service layers, and ViewModel state transitions.

### Key Metrics
- **Total Unit Tests**: 54
- **Pass Rate**: 100%
- **Framework**: xUnit
- **Mocking Library**: Moq

---

## 📋 Test Suites

### 1. ViewModel Logic (`NotSoForgottenCemetery.Tests`)
- **HomeViewModel Tests**: Validates that background threads don't block the UI and that properties (Clock, CurrentSong) update correctly.
- **SettingsViewModel Tests**: Ensures that API keys are correctly retrieved and saved via the `ISettingsStore` abstraction.
- **MemoryBoardViewModel Tests**: Validates the loading and filtering of memory records from the database.

### 2. Service Layer (Mocked)
- **Database Service**: Verified via interface-driven testing to ensure CRUD operations are called with correct parameters.
- **External Services**: Spotify and YouTube integrations are mocked to ensure the application handles API responses (and failures) gracefully without requiring live credentials.

---

## 🏗️ Technical Implementation
Testing of multi-threaded code is often difficult; however, we have implemented:
- **Task Synchronization**: Using `Task.Delay` and `CancellationTokenSource` in tests to verify background loop behaviors.
- **Dependency Inversion**: By injecting interfaces (`IDatabase`, `ISpotifyService`), we can swap real implementations for mocks during test runs, ensuring 100% isolation.

## 🚀 Future Testing Goals
- **UI Integration Tests**: Leveraging MAUI's `Appium` support to automate screen transitions.
- **Load Testing**: Simulating a database with thousands of memory records to ensure high-performance scrolling and querying.
