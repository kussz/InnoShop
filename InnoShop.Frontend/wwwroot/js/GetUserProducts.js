﻿async function fetchItems(source, controller) {
    try {
        const response = await fetch(source); // Укажите правильный путь к вашему API
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
        h1 = document.createElement('h1');
        h1.setAttribute('class', 'blobtext');
        h1.textContent = item.name;
        h5 = document.createElement('h5');
        h5.setAttribute('class', 'blobdesc');
        h5.textContent = item.description;
        blob.appendChild(h1);
        blob.appendChild(h5);
        container.appendChild(blob);
    });
    document.getElementById('loading').remove();
    document.getElementById('loadedItems').style.display = "block";
}
// Вызываем функцию для получения данных при загрузке страницы
