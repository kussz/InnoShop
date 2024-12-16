﻿document.getElementById("btn").addEventListener("click", async function (e) {
    e.preventDefault();
    const user = {
        email: document.getElementById("emailInput").value,
        username: document.getElementById("usernameInput").value,
        userTypeId: document.getElementById("userTypeInput").value,
        localityId: document.getElementById("localityInput").value,
        password: document.getElementById("passwordInput").value,
        passwordConfirm: document.getElementById("passwordConfirmInput").value
    }
    await registerUser(user);
});
async function registerUser(user) {
    const userString = JSON.stringify(user);

    try {
        const response = await fetch(`${userHost}/User/Register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                // Добавьте другие заголовки, если необходимо, например, авторизацию
            },
            body: userString
        });

        if (response.ok) {
            const token = await response.text();

            // Сохраняем токен в localStorage
            localStorage.setItem('jwtToken', token);

            // Перенаправление после успешного редактирования
            window.location.href = "/Product"; // Или используйте подходящий метод для вашего фреймворка
        } else {
            const errorResponse = await response.json();
            displayErrors(errorResponse.errors);
        }
    } catch (error) {
        console.error("Ошибка при регистрации:", error);
        // Обработка ошибок во время запроса
    }
}

function updateUIWithLocalitiesAndTypes(localities, userTypes) {
    // Обновите ваш интерфейс с полученными данными
    console.log(localities, userTypes);
    // Например, можно заполнить выпадающие списки или другие элементы
}
