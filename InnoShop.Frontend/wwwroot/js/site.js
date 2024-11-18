function getUsernameFromToken(token) {
    // Разделяем токен на части
    const tokenParts = token.split('.');
    if (tokenParts.length !== 3) {
        throw new Error('Неверный формат токена');
    }

    // Декодируем полезную нагрузку (payload) из base64
    const payload = JSON.parse(atob(tokenParts[1]));

    // Предполагаем, что имя пользователя хранится в свойстве `sub`
    return payload.sub;
}
async function updateAuthButtons() {
    authButtons = document.getElementById("authButtons");
    authButtons.innerHTML = '';
    //await loadNavigation();
    if (localStorage.getItem("jwtToken") != null)
    {
        appendButtonWithIcon("fas fa-user",getUsernameFromToken(localStorage.getItem('jwtToken')), redirectToProfile)
        appendButton('Выйти',logout)
    } else {
        // Если токена нет, показываем кнопки "Зарегистрироваться" и "Войти"

        appendButton('Зарегистрироваться', redirectToRegister)
        appendButton('Войти',redirectToLogin)
    }
}

// Функция для выхода пользователя
function logout() {
    localStorage.removeItem('jwtToken'); // Удаляем токен
    location.reload(); // Обновляем кнопки
}

// Пример функций для перехода на страницы регистрации и входа
function redirectToRegister() {
    window.location.href = '/Account/Register'; // Замените на правильный URL
}
function redirectToProfile() {
    window.location.href = '/Account';
}
function redirectToLogin() {
    window.location.href = '/Account/Login'; // Замените на правильный URL
}
function getLi() {
    li = document.createElement("li");
    //li.setAttribute("class", "rightli");
    return li;
}
function getA() {
    a = document.createElement("a");
    //a.setAttribute("class", "nav-link text-dark");
    return a;
}
function appendButton(text,func) {
    a = getA();
    a.innerText = text;
    a.onclick = func;
    li = getLi();
    li.appendChild(a);
    authButtons.appendChild(li);
}

function appendButtonWithIcon(icon,text, func) {
    a = getA();
    i = document.createElement("i");
    i.setAttribute("class", icon);
    a.appendChild(i);
    var textNode = document.createTextNode(` `+text);
    a.appendChild(textNode);
    //a.innerText += text;
    a.onclick = func;
    li = getLi();
    li.appendChild(a);
    authButtons.appendChild(li);
}
async function loadNavigation() {
    await fetch('Home/NavPartial') // Укажите правильный путь к действию контроллера
        .then(response => {
            if (!response.ok) {
                throw new Error('Сеть не в порядке: ' + response.statusText);
            }
            return response.text(); // Получаем HTML как текст
        })
        .then(html => {
            document.getElementById('authButtons').innerHTML = html; // Вставляем HTML в контейнер
        })
        .catch(error => {
            console.error('Ошибка:', error);
        });
}

// При загрузке страницы обновляем кнопки
document.addEventListener('DOMContentLoaded', updateAuthButtons);