# Testing Procedures for National Library Information System

## Overview
This document outlines the testing procedures for three .NET 8 projects in a library management system:

1. **Library Catalogue Service** (MongoDB on localhost)
2. **Staff Management Service** (MS SQL with JWT Authentication)
3. **Membership and Lending Service** (MS SQL on localhost)

---

## 1. Library Catalogue Service (MongoDB on localhost)

### Setup
- Ensure MongoDB is running locally (`localhost:27017`).
- Create a MongoDB database named `NaLib_Catalogue`.

### Testing Steps

#### Database Connectivity
- Validate the connection to MongoDB during application startup.

#### CRUD Operations
- **Create**: Test adding new library resources and authors.
- **Read**: Retrieve resources by ID.
- **Update**: Update library resources.

---

## 2. Staff Management Service (MS SQL and JWT Authentication)

### Setup
- Ensure MS SQL is running locally.
- Populate the database with sample staff data.
- Import the database into SQL Server using the `NaLib_Staff_ManagementDB.bacpac` file found in the `database` folder.

### Testing Steps

#### Authentication
- Test the login endpoint with valid and invalid credentials.
- Validate issued JWT tokens.

#### Authorization
- Access secure endpoints without a token (should return `401 Unauthorized`).

#### CRUD Operations
- **Create**: Add new staff members.
- **Read**: Fetch staff details using ID or search filters.
- **Update**: Update staff records (e.g., position or department).
- **Delete**: Remove a staff record and validate it is removed from the database.

#### Error Handling
- Attempt unauthorized actions (e.g., accessing staff data without logging in).
- Test invalid or missing input for API requests.

---

## 3. Membership and Lending Service (MS SQL on localhost)

### Setup
- Ensure MS SQL is running locally.
- Populate the database with sample membership and lending data.
- Import the database into SQL Server using the `Member_and_LendingBD.bacpac` file found in the `database` folder.

### Testing Steps

#### Database Connectivity
- Validate database connectivity during startup.

#### Membership Management
- **Register**: Add new members.
- **View**: Fetch member details by ID.
- **Update**: Modify member details (e.g., address or contact info).

---

## Testing Tools
- Use **Swagger** for manual API testing and documentation.

