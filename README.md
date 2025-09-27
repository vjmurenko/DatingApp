# DatingApp

DatingApp - это современное веб-приложение для знакомств, разработанное с использованием передовых технологий и архитектурных подходов. Приложение предоставляет пользователям возможность создавать профили, просматривать другие профили, ставить лайки и общаться через встроенную систему сообщений.

## Технологии

### Backend
- **ASP.NET Core** 
- **C#** 
- **Entity Framework Core** 
- **PostgreSQL** 
- **SignalR** 
- **AutoMapper**
- **JWT (JSON Web Tokens)** 
- **Cloudinary** 

### Frontend
- **Angular** 
- **TypeScript** 
- **RxJS** 
- **Angular CLI** 

## Функциональность

- Регистрация и аутентификация пользователей
- Создание и редактирование профилей
- Загрузка и управление фотографиями (с использованием Cloudinary)
- Поиск и просмотр других пользователей
- Система лайков и взаимных интересов
- Обмен сообщениями между пользователями
- Административная панель для управления пользователями и контентом
- Отслеживание онлайн-статуса пользователей

## Запуск приложения

### Предварительные требования

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/) (для фронтенда)
- [Angular CLI](https://angular.io/cli)
- [PostgreSQL](https://www.postgresql.org/)
- [Git](https://git-scm.com/)

### Установка и запуск

1. Клонируйте репозиторий:
   ```bash
   git clone <URL_репозитория>
   cd DatingApp
   ```

2. Настройте конфигурацию базы данных в файле `API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=datingapp;Username=postgres;Password=yourpassword"
     }
   }
   ```

3. Установите зависимости для фронтенда:
   ```bash
   cd client
   npm install
   ```

4. Вернитесь в корневую директорию и восстановите зависимости для бэкенда:
   ```bash
   cd ..
   dotnet restore
   ```

5. Выполните миграции базы данных:
   ```bash
   dotnet ef database update --project API
   ```

6. Запустите API:
   ```bash
   cd API
   dotnet run
   ```
   API будет доступен по адресу `https://localhost:5001`

7. В новом терминале запустите Angular приложение:
   ```bash
   cd client
   ng serve
   ```
   Фронтенд будет доступен по адресу `http://localhost:4200`

8. Приложение будет доступно в браузере по адресу `http://localhost:4200`

### Настройка Cloudinary

Для загрузки изображений необходимо настроить интеграцию с Cloudinary:

1. Зарегистрируйтесь на [cloudinary.com](https://cloudinary.com/)
2. Получите учетные данные API
3. Добавьте их в `API/appsettings.Development.json`:
   ```json
   {
     "CloudinarySettings": {
       "CloudName": "your_cloud_name",
       "ApiKey": "your_api_key",
       "ApiSecret": "your_api_secret"
     }
   }
   ```

## Структура проекта

```
DatingApp/
├── API/                    # ASP.NET Core Web API
│   ├── Controllers/        # API контроллеры
│   ├── Data/              # Работа с базой данных
│   ├── DTO/               # Объекты передачи данных
│   ├── Entities/          # Сущности базы данных
│   ├── Extensions/        # Расширения
│   ├── Helpers/           # Вспомогательные классы
│   ├── Interfaces/        # Интерфейсы
│   ├── Middleware/        # ПО промежуточного слоя
│   ├── Services/          # Бизнес-логика
│   └── SinglaR/           # Реализация SignalR
└── client/                # Angular приложение
    ├── src/
    │   ├── app/
    │   │   ├── _models/   # Модели данных
    │   │   ├── _services/ # Сервисы
    │   ├── _guards/   # Защитники маршрутов
    │   │   ├── _interceptors/ # Перехватчики HTTP
    │   ├── home/      # Компонент главной страницы
    │   ├── members/   # Компоненты участников
    │   │   ├── messages/  # Компоненты сообщений
    │   │   └── nav/       # Навигационный компонент
    │   └── environments/  # Конфигурации окружения
```
