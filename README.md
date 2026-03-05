# Combination Generator API

A REST API that generates valid combinations of items and stores them in a database.

The API receives a list of item counts and a required combination length. Each item belongs to a type identified by a prefix letter (A, B, C, ...). The system generates all valid combinations while enforcing a rule: **items with the same prefix letter cannot appear in the same combination**.

All generated combinations are stored in the database and returned to the client.

---

# Problem Description

You are given an array of numbers where each number represents the **count of items of a specific type**.

Each item type is represented by a prefix letter:

| Prefix | Item Examples |
| ------ | ------------- |
| A      | A1            |
| B      | B1, B2        |
| C      | C1            |

Example input:

```
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

# Example

For:

```
items = [1, 2, 1]
length = 2
```

Valid combinations:

```
["A1", "B1"]
["A1", "B2"]
["A1", "C1"]
["B1", "C1"]
["B2", "C1"]
```

Invalid combinations:

```
["B1", "B2"]  (same prefix)
```