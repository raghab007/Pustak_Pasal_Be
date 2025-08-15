# Pustak Ghar Backend API

**Pustak Ghar** is a web-based book retail and library system. This backend API is a **RESTful service** built using the **.NET Framework** to handle all server-side operations. It enables secure, efficient, and scalable interaction between the frontend and the database.

---

## Key Features & Endpoints

### 1. User Management
#### Registration & Authentication
- `POST /api/users/register` → Create a new member account
- `POST /api/users/login` → Authenticate users and return JWT token

#### Profile Management
- `GET /api/users/{id}` → Fetch user profile
- `PUT /api/users/{id}` → Update profile details
- `DELETE /api/users/{id}` → Delete account

---

### 2. Book Catalog
#### Book Retrieval
- `GET /api/books` → Fetch all books (supports filters: genre, author, language, availability)
- `GET /api/books/{id}` → Fetch book details by ID

#### Admin Operations
- `POST /api/books` → Add a new book
- `PUT /api/books/{id}` → Update book details
- `DELETE /api/books/{id}` → Remove a book

---

### 3. Cart & Orders
#### Cart Management
- `GET /api/cart` → View user cart
- `POST /api/cart` → Add book to cart
- `PUT /api/cart/{id}` → Update quantity in cart
- `DELETE /api/cart/{id}` → Remove book from cart

#### Order Processing
- `POST /api/orders` → Place a new order (claim code generated)
- `GET /api/orders/{id}` → Get order details
- `PUT /api/orders/{id}/cancel` → Cancel an order

---

### 4. Discounts
- **Bulk & Loyalty Discounts** are automatically applied during order creation:
  - 5% discount for orders with 5+ books
  - 10% loyalty discount after 10 successful purchases

---

### 5. Reviews & Ratings
- `POST /api/reviews` → Add review and rating for purchased books
- `GET /api/reviews/{bookId}` → Fetch reviews for a specific book

---

### 6. Admin Management
#### Inventory & Announcements
- `GET /api/admin/inventory` → View all books in inventory
- `PUT /api/admin/discounts` → Update discount rules
- `POST /api/admin/announcements` → Create site-wide announcements

---

### 7. Notifications
- Email notifications are sent for order confirmations, claim codes, and announcements using a secure mail service integration.

---

## Technical Features
- **Authentication & Authorization:** JWT-based authentication with role-based access for admins
- **Database:** SQL Server or any .NET-supported relational database
- **Error Handling:** Standardized responses with proper HTTP status codes
- **Data Validation:** Strong input validation on all endpoints
- **Security:** HTTPS support, secure token management
- **Scalability:** Modular RESTful design for easy expansion

---

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/pustak-ghar-backend.git

2.	Install dependencies and build the project using Visual Studio.
3.	Configure the database connection in appsettings.json.
4.	Run the API locally:
     dotnet run


