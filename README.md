# Customer Management System (C# Console App)

A **C# console application** to manage regular and premium customers. It demonstrates **Object-Oriented Programming (OOP)** concepts such as **abstraction, inheritance, encapsulation, and polymorphism**, while providing features to **add, view, search, update, delete, 
and sort customers**.

---

## Features

- Added **regular** and **premium customers**.
- Store customer details:  
  - `ID`  
  - `Name`  
  - `Address`  
  - `Code`  
  - `Product`  
  - `Category`  
  - `Order Date`  
  - `Received Date`  
  - `Reward Points` (for premium customers)
- **Search** customers by `ID`, `Name`, or `Code`.
- **Update** customer information.
- **Delete** customers.
- **Sort** customers by:
  - Name
  - ID
  - **Code (alphanumeric)**
- Handles **input validation** for Name (max 50 characters) and Address (max 200 characters).
- Demonstrates **abstract classes and derived classes** for customer management.

---

## OOP Concepts Used

- **Abstraction:** `CustomerManager` is an abstract class that defines the operations but cannot be instantiated directly.
- **Inheritance:** `PremiumCustomer` inherits from `Customer`.
- **Encapsulation:** `Name` and `Address` are private fields with public getters/setters for validation.
- **Polymorphism:** `Display()` method is overridden in `PremiumCustomer` to include reward points.

---

### How to Run

1. dotnet restore ( downloads all required NuGet packages )
2. dotnet build ( Builds the project )
3. **dotnet run** ( runs the project )
