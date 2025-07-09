🎓 SchoolManagement.API

Complete School Management REST API built with ASP.NET Core 8

Manage students, teachers, schedules, attendance, grades, and more  all from a centralized backend.


📌 Overview


SchoolManagement.API is a modular and extensible RESTful backend for running and managing day-to-day operations of educational institutions. From user authentication to attendance tracking, the API is structured to support large-scale academic environments.

🚀 Features
🔐 Authentication with JWT & Token Refresh

🧑‍🏫 Role-based access: Admins, Teachers, Students

📚 Class & Subject Management

🗓 Schedule creation for classes, teachers, and students

✅ Attendance tracking per class & per day

📝 Grade assignment and subject-wise evaluation

📊 Pagination for Users, Classes, Subjects, and Grades


🌐 Swagger UI Ready
### 🔐 Authentication

All protected endpoints require a valid `Authorization` header:

```http
Authorization: Bearer {access_token}
```

---

## 🧭 API Directory

---

### 🧑‍🎓 Student Management

| METHOD | ENDPOINT           | DESCRIPTION               |
| ------ | ------------------ | ------------------------- |
| `GET`  | `/api/Student/all` | Get full list of students |

---

### 🧑‍🏫 Teacher Management

| METHOD | ENDPOINT           | DESCRIPTION      |
| ------ | ------------------ | ---------------- |
| `GET`  | `/api/Teacher/all` | Get all teachers |

---

### 📘 Subject Management

| METHOD   | ENDPOINT              | DESCRIPTION            |
| -------- | --------------------- | ---------------------- |
| `POST`   | `/api/Subject/add`    | Add new subject        |
| `GET`    | `/api/Subject/{id}`   | Get subject by ID      |
| `GET`    | `/api/Subject/paged`  | Get paginated subjects |
| `PUT`    | `/api/Subject/update` | Update subject         |
| `DELETE` | `/api/Subject/{id}`   | Delete subject         |

---

### 🏫 Class Management

| METHOD   | ENDPOINT                 | DESCRIPTION       |
| -------- | ------------------------ | ----------------- |
| `POST`   | `/api/Class/add`         | Add new class     |
| `GET`    | `/api/Class/all`         | List all classes  |
| `GET`    | `/api/Class/get/{id}`    | Get class details |
| `PUT`    | `/api/Class/update`      | Update class info |
| `DELETE` | `/api/Class/delete/{id}` | Delete class      |

---

### 🗓 Schedule Management

| METHOD | ENDPOINT                                 | DESCRIPTION                 |
| ------ | ---------------------------------------- | --------------------------- |
| `POST` | `/api/Schedule/add-entry`                | Add a schedule slot         |
| `GET`  | `/api/Schedule/classes-with-schedule`    | All scheduled classes       |
| `GET`  | `/api/Schedule/class/{classId}`          | Schedule for specific class |
| `GET`  | `/api/Schedule/class/{classId}/subjects` | Subjects of a class         |
| `GET`  | `/api/Schedule/teacher/{userId}`         | Teacher schedule            |
| `GET`  | `/api/Schedule/student`                  | Student schedule (authed)   |

---

### 📊 Grades & Evaluation

| METHOD | ENDPOINT                                    | DESCRIPTION                 |
| ------ | ------------------------------------------- | --------------------------- |
| `POST` | `/api/Grade/add`                            | Add grade                   |
| `GET`  | `/api/Grade/student/paged`                  | Paginated student grades    |
| `GET`  | `/api/Grade/teacher/paged`                  | Paginated grades by teacher |
| `GET`  | `/api/Grade/students/{subjectId}/{classId}` | Grades for subject/class    |
| `GET`  | `/api/Grade/subjects`                       | List subjects with grades   |

---

### 🕒 Attendance

| METHOD | ENDPOINT                                | DESCRIPTION                   |
| ------ | --------------------------------------- | ----------------------------- |
| `GET`  | `/api/Attendance/students/{scheduleId}` | Students assigned to a lesson |
| `GET`  | `/api/Attendance/today-lessons`         | Lessons scheduled for today   |
| `POST` | `/api/Attendance/save/{scheduleId}`     | Save daily attendance         |

---

### 👥 User Management

| METHOD   | ENDPOINT                  | DESCRIPTION             |
| -------- | ------------------------- | ----------------------- |
| `POST`   | `/api/User/add`           | Add new user            |
| `GET`    | `/api/User/logged-user`   | Get logged-in user info |
| `GET`    | `/api/User/get-user/{id}` | Get user by ID          |
| `GET`    | `/api/User/paged`         | Paginated users         |
| `GET`    | `/api/User/roles`         | Available user roles    |
| `PUT`    | `/api/User/update`        | Update user info        |
| `DELETE` | `/api/User/delete/{id}`   | Delete user             |

---

### 🔐 Auth

| METHOD | ENDPOINT             | DESCRIPTION                   |
| ------ | -------------------- | ----------------------------- |
| `POST` | `/api/Auth/login`    | Login and get JWT             |
| `POST` | `/api/Auth/logout`   | Logout and invalidate session |
| `GET`  | `/api/Token/refresh` | Refresh access token          |

---


