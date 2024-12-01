async function fetchItem(source,id,func) {
    try {
        const response = await fetch(`${source}/Details/${id}`, {
            headers:
            {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                }
        }); // Укажите правильный путь к вашему API
        if (!response.ok) {
                if (response.status == 401)
                    window.location.href = '/Account/Unauthorized'
                if (response.status == 403)
                    window.location.href = '/Account/Forbidden'
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



