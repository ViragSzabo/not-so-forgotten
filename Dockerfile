# ─────────────────────────────────────────────────────────────────
# Dockerfile — Not-So-Forgotten Cemetery (Unit Tests)
#
# .NET MAUI apps cannot run headlessly in Docker, but the test project
# targets plain net8.0 (with MAUI stubs) and runs perfectly in a
# standard SDK container.
# ─────────────────────────────────────────────────────────────────

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test

WORKDIR /app

# ── Copy project files first for layer caching ──────────────────
COPY NotSoForgottenCemetery/NotSoForgottenCemetery.csproj ./NotSoForgottenCemetery/
COPY NotSoForgottenCemetery.Tests/NotSoForgottenCemetery.Tests.csproj ./NotSoForgottenCemetery.Tests/

# ── Restore NuGet packages ───────────────────────────────────────
RUN dotnet restore NotSoForgottenCemetery.Tests/NotSoForgottenCemetery.Tests.csproj

# ── Copy source that the test project directly compiles ─────────
COPY NotSoForgottenCemetery/Models/        ./NotSoForgottenCemetery/Models/
COPY NotSoForgottenCemetery/ViewModels/    ./NotSoForgottenCemetery/ViewModels/
COPY NotSoForgottenCemetery/Services/      ./NotSoForgottenCemetery/Services/

# ── Copy the full test project ───────────────────────────────────
COPY NotSoForgottenCemetery.Tests/         ./NotSoForgottenCemetery.Tests/

# ── Run tests ────────────────────────────────────────────────────
# Use --no-restore because we already restored above.
# Results are written to /app/test-results/ for artifact collection.
RUN dotnet test NotSoForgottenCemetery.Tests/NotSoForgottenCemetery.Tests.csproj \
        --no-restore \
        --logger "trx;LogFileName=results.trx" \
        --results-directory /app/test-results

# ── Default command (re-run tests if container is started) ───────
CMD ["dotnet", "test", "NotSoForgottenCemetery.Tests/NotSoForgottenCemetery.Tests.csproj", \
        "--no-restore", "--logger", "console;verbosity=normal"]
