﻿async function fetchItems(source, controller) {
    try {
        const response = await fetch(source, {
            method: "POST",
            headers:
            {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
            },
            body: formData
            }); // Укажите правильный путь к вашему API
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const items = await response.json();
        displayItems(items, controller)

    } catch (error) {
        console.error('Ошибка при получении данных:', error);
    }
}

function displayItems(items, controller) {
    const container = document.getElementById('blobparent');

    items.forEach(item => {
        const blob = document.createElement('a');
        blob.setAttribute('class', 'blob');
        blob.setAttribute('href', `../../${controller}/Detail/${item.id}`);
        h1 = document.createElement('h3');
        h1.setAttribute('class', 'blobtext');
        h1.textContent = item.name;
        h5 = document.createElement('h5');
        h5.setAttribute('class', 'blobdesc');
        h5.textContent = item.description;
        blob.appendChild(h1);
        blob.appendChild(h5);
        container.appendChild(blob);
    });
}
// Вызываем функцию для получения данных при загрузке страницы
