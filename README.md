# GymManager

**GymManager** is a web application designed to streamline the management of gym memberships, classes, trainers, and payments. Built with Blazor and ASP.NET Core (.NET 8), it provides a modern, interactive interface for gym staff and administrators.

## Features

- **Member Management**
  - Add, update, view, and delete gym members.
  - Track member details: name, contact info, membership type, join date.
  - View the number of classes each member is registered for.

- **Class Management**
  - Create and manage gym classes.
  - Assign trainers to classes.
  - Set class schedules and maximum capacity.
  - View class rosters and registrations.

- **Trainer Management**
  - Add and manage trainers.
  - Assign trainers to classes.
  - Track trainer specializations and contact information.

- **Class Registration**
  - Register members for classes.
  - View and manage class registrations.
  - Prevent overbooking based on class capacity.

- **Payment Tracking**
  - Record and view member payments.
  - Track payment amounts, methods, and dates.

## Technology Stack

- **Frontend:** Blazor (Server or WebAssembly, as configured)
- **Backend:** ASP.NET Core Web API (.NET 8)
- **Database:** Entity Framework Core (with Identity for authentication)
- **ORM:** Entity Framework Core
- **Authentication:** ASP.NET Core Identity

## Project Structure

- `Controllers/` – API controllers for Members, Trainers, GymClasses, ClassRegistrations, and Payments.
- `Models/` – Entity and DTO classes for all core objects.
- `Data/` – ApplicationDbContext for EF Core.
- `Pages/` – Blazor components for UI (if using Blazor Server/WASM).

## Key Entities

- **Member:** Represents a gym member, including personal and membership details.
- **Trainer:** Represents a gym trainer, including specialization and contact info.
- **GymClass:** Represents a class, its schedule, assigned trainer, and capacity.
- **ClassRegistration:** Many-to-many relationship between members and classes.
- **Payment:** Tracks payments made by members.

## API Endpoints

- `/api/members` – CRUD operations for members.
- `/api/trainers` – CRUD operations for trainers.
- `/api/gymclasses` – CRUD operations for classes.
- `/api/classregistrations` – Manage class registrations.
- `/api/payments` – Manage payments.

## Getting Started

1. **Clone the repository:**

2. **Configure the database:**
   - Update the connection string in `appsettings.json`.
   - Run EF Core migrations:

3. **Run the application:**
   

4. **Access the app:**
   - Open your browser at `https://localhost:5001` (or the configured port).

## Customization

- Add new fields to models as needed.
- Extend Blazor components for custom UI/UX.
- Integrate additional authentication or reporting features.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.

---

*For questions or support, please open an issue on GitHub.*
