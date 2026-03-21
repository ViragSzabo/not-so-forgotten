# 🛡️ Security Policy: Not-So-Forgotten Cemetery

Ensuring that even in a digital graveyard, your data remains yours and your secrets are kept safe.

---

## 🔒 Secrets Management
The application interacts with external services (Spotify, YouTube). We follow strict guidelines to protect your credentials:
- **SecureStorage**: All API keys and Client IDs are stored using .NET MAUI `SecureStorage`, which utilizes platform-level secure APIs (e.g., Keychain on iOS/macOS, Keystore on Android).
- **No Plaintext**: No API keys or interactive secrets should ever be hardcoded or committed to version control.
- **Client-Side Only**: All data processing and authentication happen on your device. We do not transmit your keys to any intermediate server.

## 🛡️ Input Validation
To prevent "malicious memories" or database corruption:
- **Sanitization**: All user-provided titles and descriptions are trimmed and validated before being saved.
- **Async Safety**: Database operations are strictly asynchronous to prevent UI hanging during large writes.

## 🕰️ Reporting Vulnerabilities
If you discover a security hole or a ghostly bug that shouldn't be there, please contact us quietly:
- **Email**: Virág Szabó via 📧 virag.szabo@student.nhlstenden.com
- Please include a detailed description of the issue and steps to reproduce it.

---

_“Treat every secret like a whisper in the mist — with infinite care.”_
