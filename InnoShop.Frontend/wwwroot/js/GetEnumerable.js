async function fetchItems(source) {
    try {
        const response = await fetch(source); // Укажите правильный путь к вашему API
if (!response.ok) {
            throw new Error('Network response was not ok');
        }
const items = await response.json();
        return items;

    } catch (error) {
    console.error('Ошибка при получении данных:', error);
    }
}

function displayItems(items,source) {
    const container = document.getElementById('itemsContainer');
container.innerHTML = ''; // Очищаем контейнер

    items.forEach(item => {
        const row = document.createElement('tr');
        const a = document.createElement('a');
        a.setAttribute('class', 'blober');
        a.setAttribute('href', `${source}/Details/${item.id}`);
        const id = document.createElement('td');
        id.textContent = item.id;
        const name = document.createElement('td');
        a.textContent = item.name;
        row.appendChild(id);
        name.appendChild(a);
        row.appendChild(name);
        container.appendChild(row);
    });
    document.getElementById('loading').style.display = "none";
    document.getElementById('loadedItems').style.display = "block";
}
// Вызываем функцию для получения данных при загрузке страницы
