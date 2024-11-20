async function fetchItem(source,id,func) {
    try {
        const response = await fetch(`${source}/Details/${id}`); // Укажите правильный путь к вашему API
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const item = await response.json();
        func(item);
        document.getElementById('loading').remove();
        document.getElementById('loadedItems').style.display = "block";
    } catch (error) {
        console.error('Ошибка при получении данных:', error);
    }
}



