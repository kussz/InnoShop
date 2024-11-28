document.getElementById('loginForm').addEventListener('submit', async function (e) {
    e.preventDefault(); // Предотвращаем стандартное поведение формы

    const username = document.getElementById('UserName').value;
    const password = document.getElementById('Password').value;
    try {
        // Выполняем POST-запрос
        const response = await fetch(`${userHost}/User/Login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                UserName: username,
                Password: password
            })
        })
        if (!response.ok) {
            throw new Error('Ошибка входа: ' + response.statusText);
        }
        const token = await response.text(); // Предполагаем, что сервер возвращает JSON с токеном
        // Сохраняем токен в localStorage или sessionStorage
        localStorage.setItem('jwtToken', token);
        updateNav();
        window.location.href = '/Account';
    } catch (error) {
        document.getElementById("result").textContent = "Неверный логин или пароль"; // Отображение ошибки
    }
});