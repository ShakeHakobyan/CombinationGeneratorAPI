# Combination Generator API

A REST API that generates **valid combinations of items** and stores them in a database.

The API receives a list of item counts and a required combination length. Each item belongs to a type identified by a **prefix letter** (A, B, C, …).

**Rule:** Items with the **same prefix letter cannot appear in the same combination**.

All generated combinations are stored in the database and returned to the client.

---

## Problem Description

You are given an array of numbers where each number represents the **count of items of a specific type**.

| Prefix | Item Examples |
| ------ | ------------- |
| A      | A1            |
| B      | B1, B2        |
| C      | C1            |

**Example input:**

```json
[1, 2, 1]
```

This corresponds to the following items:

```
A1
B1, B2
C1
```

When generating combinations, **two items with the same prefix cannot appear together**.

---

## Example

**Input:**

```json
{
  "items": [1, 2, 1],
  "length": 2
}
```

**Valid combinations:**

```
["A1", "B1"]
["A1", "B2"]
["A1", "C1"]
["B1", "C1"]
["B2", "C1"]
```

**Invalid combinations:**

```
["B1", "B2"]  (same prefix)
```

---

## Features

* Generate all valid combinations according to the prefix rule.
* Store **requests, generated combinations, and individual items** in a database.
* Use **transactions** to ensure consistent database insertions.
* Return generated combinations with **unique IDs**.

---

## Validation & Restrictions

1. **Input constraints:**

   * Maximum of **26 item types** (A-Z).
   * Item counts **cannot be negative**.
   * Combination length **cannot be negative**.

2. **Output limits:**

   * If the number of generated combinations exceeds the internal threshold (e.g., `MAX_COMBINATIONS = 100,000`), the API will **return an error** to prevent performance issues.

## Requirements

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* PostgreSQL
* Optional: Visual Studio 2022 or VS Code

**Notes:**

* NuGet dependencies (EF Core, FluentValidation, Npgsql) are restored automatically via `.csproj`.
* CLI tools like `dotnet-ef` may need to be installed globally:

```bash
dotnet tool install --global dotnet-ef
```

---

## Setup & Running

1. **Clone the repository:**

```bash
git clone https://github.com/ShakeHakobyan/CombinationGeneratorAPI.git
cd CombinationGeneratorAPI
```

2. **Configure database:**
   Edit `appsettings.json` and set your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=CombinationDB;Username=postgres;Password=yourpassword"
  }
}
```

3. **Restore dependencies:**

```bash
dotnet restore
```

4. **Apply EF Core migrations:**

```bash
dotnet ef database update --project CombinationGeneratorAPI
```

5. **Run the API:**

```bash
dotnet run --project CombinationGeneratorAPI
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

---

## API Usage

**POST** `/generate`

**Request body:**

```json
{
  "items": [1, 2, 1],
  "length": 2
}
```

**Response:**

```json
{
  "id": 1,
  "combination": [
    ["A1", "B1"],
    ["A1", "B2"],
    ["A1", "C1"],
    ["B1", "C1"],
    ["B2", "C1"]
  ]
}
```

---

## Testing

Run **unit and integration tests**:

```bash
dotnet test
```

Test files are located in:

* `CombinationGeneratorAPI.UnitTests/CombinationGeneratorTests.cs`
* `CombinationGeneratorAPI.UnitTests/Integration/`


## TODO / Roadmap

Future improvements and features planned for **CombinationGeneratorAPI**:

1. **Optimize combination generation**

   * Change the algorithm to **generate each item only once**.
   * Reduce memory usage.

2. **CI/CD & Testing**

   * Set up a **GitHub Actions pipeline** to run **unit and integration tests** automatically on push/pull requests.
   * Add **integration tests** for `LazyRequestCleanupService` to ensure proper request cleanup.
   * Output test results in a **readable format** for easier debugging.

3. **Logging & Monitoring**

   * Add structured **logging** throughout the system (generation, caching, DB transactions, errors).

4. **User Interface**

   * Improve **API documentation / UI** to make it **more user-friendly and visually appealing**.

5. **Database & Versioning**

   * Implement **versioning in the database** to prevent returning **stale results**.

6. **Request Management**

   * Add **cancellation support** for long-running combination generation requests.
   * Consider **different strategies for cache cleanup**, e.g., time-based, size-based, or lazy cleanup.

7. **Other Enhancements**

   * Allow **configurable MAX_COMBINATIONS** and other parameters via settings.
