# MobileRecharge

# MobileRechargeAPI

## Overview

MobileRechargeAPI is a comprehensive solution for managing mobile recharge services, payment integrations, and user operations. The solution includes 6 projects, each handling different aspects of the system, from API management to domain logic, infrastructure, and unit testing.

## Projects

1. **MobileRecharge.API**
   - **Purpose**: Exposes APIs for mobile recharges, user authentication, and beneficiary management.
   - **Key Components**:
     - `Controllers`:
       - `AuthController.cs`: Handles user authentication.
       - `BeneficiaryController.cs`: Manages beneficiaries.
       - `MobileRechargeController.cs`: Handles mobile recharges.
       - `UserController.cs`: Manages user profiles.
     - **Middleware**:
       - `ErrorHandlingMiddleware.cs`: Global error handler for API exceptions.
     - **App Settings**:
       - `appsettings.Development.json` & `appsettings.json`: Environment configurations.

2. **MobileRecharge.Application**
   - **Purpose**: Contains business logic, service implementations, and data transfer objects (DTOs).
   - **Key Components**:
     - `DTOs`:
       - `BeneficiaryDto.cs`
       - `TopUpOptionDto.cs`
       - `UserDto.cs`
     - `Interfaces`:
       - `IBeneficiaryService.cs`: Interface for managing beneficiary services.
       - `IMobileRechargeService.cs`: Interface for mobile recharge services.
       - `IUserService.cs`: Interface for user-related services.
     - `Services`:
       - `BeneficiaryService.cs`: Implementation of beneficiary operations.
       - `MobileRechargeService.cs`: Handles mobile recharge transactions.
       - `UserService.cs`: Manages user-related business logic.

3. **MobileRecharge.Domain**
   - **Purpose**: Contains domain entities and interfaces for repositories.
   - **Key Components**:
     - `Models`:
       - `Beneficiary.cs`: Represents a beneficiary entity.
       - `TopUpOption.cs`: Defines various top-up options for mobile recharges.
       - `User.cs`: Represents user entity in the system.
     - `Interfaces`:
       - `IBeneficiaryRepository.cs`: Repository for managing beneficiaries.
       - `IMobileRechargeRepository.cs`: Repository for mobile recharges.
       - `IUserRepository.cs`: Repository for managing user data.
       - `IUnitOfWork.cs`: Manages transactions across repositories.
     - `DataAccess`:
       - `TopUpDbContext.cs`: Entity Framework context for top-up operations.
     - `Repositories`:
       - `BeneficiaryRepository.cs`: Implements beneficiary data access logic.
       - `MobileRechargeRepository.cs`: Implements mobile recharge data access.
       - `UserRepository.cs`: Implements user data access.

4. **MobileRecharge.Infrastructure**
   - **Purpose**: Manages external services, database context, and infrastructure-specific implementations.

5. **MobileRecharge.UnitTests**
   - **Purpose**: Provides unit tests to ensure the correctness of the business logic and API layers.
   - **Key Components**:
     - `Controllers`:
       - `BeneficiaryControllerTests.cs`
       - `MobileRechargeControllerTests.cs`
       - `UserControllerTests.cs`
     - `Services`:
       - `BeneficiaryServiceTests.cs`
       - `MobileRechargeServiceTests.cs`
       - `UserServiceTests.cs`

6. **Payment.Api**
   - **Purpose**: Handles payment integration, balance checks, and payment requests.
   - **Key Components**:
     - `Controllers`:
       - `BalanceController.cs`: Handles balance-related operations.
     - `Services`:
       - `BalanceService.cs`: Handles balance operations and payment processing.
       - `IBalanceService.cs`: Interface for balance services.
     - `DataAccess`:
       - `BalanceDbContext.cs`: Context for balance-related data.
     - `Models`:
       - `Balance.cs`: Represents balance data.
       - `PaymentRequest.cs`: Represents a payment request.
   
## Folder Structure

```plaintext
MobileRechargeAPI/
│
├── MobileRecharge.API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── BeneficiaryController.cs
│   │   ├── MobileRechargeController.cs
│   │   └── UserController.cs
│   ├── Middleware/
│   │   └── ErrorHandlingMiddleware.cs
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   └── Program.cs
│
├── MobileRecharge.Application/
│   ├── DTO/
│   │   ├── BeneficiaryDto.cs
│   │   ├── TopUpOptionDto.cs
│   │   └── UserDto.cs
│   ├── Interfaces/
│   │   ├── IBeneficiaryService.cs
│   │   ├── IMobileRechargeService.cs
│   │   └── IUserService.cs
│   └── Services/
│       ├── BeneficiaryService.cs
│       ├── MobileRechargeService.cs
│       └── UserService.cs
│
├── MobileRecharge.Domain/
│   ├── Models/
│   │   ├── Beneficiary.cs
│   │   ├── TopUpOption.cs
│   │   └── User.cs
│   ├── Interfaces/
│   │   ├── IBeneficiaryRepository.cs
│   │   ├── IMobileRechargeRepository.cs
│   │   ├── IUserRepository.cs
│   │   └── IUnitOfWork.cs
│   ├── DataAccess/
│   │   └── TopUpDbContext.cs
│   ├── Repositories/
│   │   ├── BeneficiaryRepository.cs
│   │   ├── MobileRechargeRepository.cs
│   │   └── UserRepository.cs
│
├── MobileRecharge.Infrastructure/
│
├── MobileRecharge.UnitTests/
│   ├── Controllers/
│   │   ├── BeneficiaryControllerTests.cs
│   │   ├── MobileRechargeControllerTests.cs
│   │   └── UserControllerTests.cs
│   ├── Services/
│       ├── BeneficiaryServiceTests.cs
│       ├── MobileRechargeServiceTests.cs
│       └── UserServiceTests.cs
│
├── Payment.Api/
│   ├── Controllers/
│   │   └── BalanceController.cs
│   ├── Services/
│   │   ├── BalanceService.cs
│   │   └── IBalanceService.cs
│   ├── Models/
│   │   ├── Balance.cs
│   │   └── PaymentRequest.cs
│   └── DataAccess/
│       └── BalanceDbContext.cs
